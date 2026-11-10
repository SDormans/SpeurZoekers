using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.UploadTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadToolGui
{
    public class PictureTab
    {
        public static readonly string[] SupportedImageExtensions = new[]
        {
            "BMP",
            "JPG",
            "JPEG",
            "GIF",
            "PNG",
            "TIFF"
        };

        private bool SingleTagName => 
            ((IAppMode)_baseStatic).ControlOptions.HasFlag(ControlOption.SingleTagName) 
            || _baseStatic.PictureNames.Length == 1;

        private string TagName
        {
            get
            {
                if (SingleTagName)
                {
                    return _item.TagNames[0];
                }
                // else
                return _item.TagNames[_mainForm.PictureSize.SelectedIndex];
            }
            set
            {
                if (SingleTagName)
                {
                    _item.TagNames[0] = value;
                }
                else
                {
                    _item.TagNames[_mainForm.PictureSize.SelectedIndex] = value;
                }
            }
        }

        private readonly MainForm _mainForm;

        private IPicture _baseStatic;
        private Image[] _images;
        private bool[] _picsLoaded, _imgsChanged, _imgsOverridden;
        private string[] _orgPictureNames;
        private IPictureDto _item;
        private string _picPath;
        private Point _imgMouse;

        public PictureTab(MainForm mainForm)
        {
            _mainForm = mainForm;

            _mainForm.PictureName.TextChanged += PictureName_TextChanged;
            _mainForm.PictureBox.MouseDown += PictureBox_MouseDown;
            _mainForm.PictureBox.MouseUp += PictureBox_MouseUp;
            _mainForm.PictureSize.SelectedIndexChanged += PictureSize_SelectedIndexChanged;
            _mainForm.OpenPath.Click += OpenPath_Click;
        }

        public void SetupPictureTab(IPicture baseStatic)
        {
            _baseStatic = baseStatic;

            _images = new Image[baseStatic.PictureNames.Length];
            _imgsChanged = new bool[baseStatic.PictureNames.Length];
            _imgsOverridden = new bool[baseStatic.PictureNames.Length];

            _mainForm.Tabs.Controls.Add(_mainForm.TabPicture);

            _mainForm.PictureSize.Items.Clear();
            _mainForm.PictureSize.Items.AddRange(baseStatic.PictureNames);
            _mainForm.PictureSize.SelectedIndex = 0;

            _mainForm.PictureName.MaxLength = _baseStatic.TagNamesMaxLength.Max();

            if (baseStatic.PictureNames.Length == 1)
            {
                _mainForm.PictureSize.Visible = false;
                _mainForm.PicturePanel.SetColumn(_mainForm.PictureName, 0);
                _mainForm.PicturePanel.SetColumnSpan(_mainForm.PictureName, 2);
            }
            else
            {
                _mainForm.PictureSize.Visible = true;
                _mainForm.PicturePanel.SetColumn(_mainForm.PictureName, 1);
                _mainForm.PicturePanel.SetColumnSpan(_mainForm.PictureName, 1);
            }
        }

        public async Task LoadPictureTab(IPictureDto item)
        {
            _item = item;

            _mainForm.PictureBox.Image = null;

            for (int i = 0; i < _baseStatic.PictureNames.Length; i++)
            {
                if (_images[i] != null)
                {
                    _images[i].Dispose();
                }

                _images[i] = null;
                _imgsChanged[i] = false;
                _imgsOverridden[i] = false;
            }

            _orgPictureNames = (string[])_item.TagNames.Clone();
            _mainForm.PictureName.Text = TagName;

            await GetPicturesFromBlob();
            SetImage();
        }

        public bool CanLeave()
        {
            return _imgsChanged.All(x => !x);
        }

        public async void PictureName_TextChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                TagName = _mainForm.PictureName.Text == string.Empty ? null : _mainForm.PictureName.Text;
                _imgsOverridden[_mainForm.PictureSize.SelectedIndex] = false;
                await CheckPicName();
                _mainForm.ValueChanged();
            }
        }

        public void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _mainForm.Cursor = Cursors.SizeAll;
            _imgMouse = e.Location;
        }

        public void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mainForm.PictureBox.Location = new Point(_mainForm.PictureBox.Location.X + (e.Location.X - _imgMouse.X),
                _mainForm.PictureBox.Location.Y + (e.Location.Y - _imgMouse.Y));
            _mainForm.Cursor = Cursors.Default;
        }

        public void PictureSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _mainForm.IsLoading = true;

                if (!SingleTagName)
                {
                    _mainForm.PictureName.Text = TagName;
                }

                _mainForm.IsLoading = false;

                SetImage();
            }
        }

        public async void OpenPath_Click(object sender, EventArgs e)
        {
            string imgFilter = string.Join(';', SupportedImageExtensions.Select(x => "*." + x));

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Bitmap|*.BMP|" +
                "JP(E)G-afbeelding|*.JPG;*.JPEG|" +
                "GIF-afbeelding|*.GIF|" +
                "PNG-afbeelding|*.PNG|" +
                "TIFF-afbeelding|*.TIFF|" +
                $"Alle afbeeldingsbestanden|{imgFilter}",
                FilterIndex = 6
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _mainForm.IsLoading = true;
                _picPath = ofd.FileName;
                await GetPictureFromFile();
                _mainForm.IsLoading = false;
            }
        }

        public async Task<Dictionary<string, DataConsistency.ItemStatus>> ValidateData()
        {
            var items = new Dictionary<string, DataConsistency.ItemStatus>();
            bool namesOk = true;
            int[] nameLengths = _baseStatic.TagNamesMaxLength;
            string imgName(int i)
            {
                if (_baseStatic.PictureNames[0] == string.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    return $" '{_baseStatic.PictureNames[i]}'";
                }
            }
            void checkName(string msg)
            {
                if (string.IsNullOrWhiteSpace(_item.TagNames[0]))
                {
                    items.Add(msg, DataConsistency.ItemStatus.Critical);
                    namesOk = false;
                }
                else if (_item.TagNames[0].Length > nameLengths[0])
                {
                    items.Add($"Naam lengte ({_item.TagNames[0].Length} > {nameLengths[0]})", DataConsistency.ItemStatus.Critical);
                    namesOk = false;
                }
            }

            if (SingleTagName)
            {
                checkName("Naam afbeelding");
            }
            else
            {
                checkName($"Naam voor afbeelding{imgName(0)}");

                if (!((IAppMode)_baseStatic).ControlOptions.HasFlag(ControlOption.MultiplePicturesOptional))
                {
                    for (int i = 1; i < _baseStatic.PictureNames.Length; i++)
                    {
                        checkName($"Naam voor afbeelding{imgName(i)}");
                    }
                }
            }

            if (namesOk)
            {
                try
                {
                    GetPictureFormats();

                    for (int i = 0; i < _images.Length; i++)
                    {
                        if (_images[i] == null)
                        {
                            continue;
                        }

                        if (_imgsOverridden[i])
                        {
                            items.Add($"Afbeelding(en) genaamd '{_item.TagNames[i]}' wordt hernoemd/vervangen",
                                DataConsistency.ItemStatus.Action);
                        }

                        if ((_baseStatic.RequiredWidth.HasValue && _images[i].Width != _baseStatic.RequiredWidth) 
                            || (_baseStatic.RequiredHeight.HasValue && _images[i].Height != _baseStatic.RequiredHeight)) // > NULL = false
                        {
                            items.Add($"Afbeelding{imgName(i)} is verkeerd formaat, formaat is {_baseStatic.RequiredWidth}X{_baseStatic.RequiredHeight}",
                                DataConsistency.ItemStatus.Critical);
                        }
                        else if (_images[i].Width > _baseStatic.MaxWidth || _images[i].Height > _baseStatic.MaxHeight) // > NULL = false
                        {
                            items.Add($"Afbeelding{imgName(i)} is te groot, maximum is {_baseStatic.MaxWidth}X{_baseStatic.MaxHeight}",
                                DataConsistency.ItemStatus.Critical);
                        }

                        if (_baseStatic.PreferLandscape && _images[i].Width < _images[i].Height)
                        {
                            items.Add($"Afbeelding{imgName(i)} is staand, liggend is gewenst voor de opmaak van de app",
                                DataConsistency.ItemStatus.Optional);
                        }
                    }

                    if (((IAppMode)_baseStatic).ControlOptions.HasFlag(ControlOption.MultiplePicturesOptional))
                    {
                        if (_images[0] == null && !await _item.PictureExist(0))
                        {
                            items.Add($"Tenminste afbeelding voor{imgName(0)}",
                                DataConsistency.ItemStatus.Critical);
                        }

                        for (int i = 1; i < _images.Length; i++)
                        {
                            if (_orgPictureNames[i] != null && string.IsNullOrWhiteSpace(_item.TagNames[i]))
                            {
                                items.Add($"Afbeelding voor{imgName(i)} wordt verwijderd",
                                    DataConsistency.ItemStatus.Action);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _images.Length; i++)
                        {
                            if (_images[i] == null && !await _item.PictureExist(i))
                            {
                                items.Add($"Afbeelding voor{imgName(i)}",
                                    DataConsistency.ItemStatus.Critical);
                            }
                        }
                    }
                }
                catch (NotSupportedException)
                {
                    items.Add("Onbekende/ontbrekende afbeeldingsextensie", DataConsistency.ItemStatus.Critical);
                }
            }

            return items;
        }

        public async Task Save()
        {
            ImageFormat[] formats = GetPictureFormats();

            await _item.SavePictures(_images, _imgsChanged, _orgPictureNames, formats);

            for (int i = 0; i < _baseStatic.PictureNames.Length; i++)
            {
                _mainForm.PictureSize.Items[i] = _baseStatic.PictureNames[i];
                _picsLoaded[i] = true;
                _imgsChanged[i] = false;
                _imgsOverridden[i] = false;

                if (!SingleTagName && _item.TagNames[i] == null && _images[i] != null)
                {
                    _images[i].Dispose();
                    _images[i] = null;

                    if (_mainForm.PictureSize.SelectedIndex == i)
                    {
                        SetImage();
                    }
                }
            }

            _orgPictureNames = (string[])_item.TagNames.Clone();
        }

        private ImageFormat[] GetPictureFormats()
        {
            ImageFormat[] formats = new ImageFormat[_item.TagNames.Length];

            for (int i = 0; i < formats.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(_item.TagNames[i]))
                {
                    formats[i] = null;
                }
                else
                {
                    formats[i] = (Path.GetExtension(_item.TagNames[i]).ToUpper()) switch
                    {
                        ".BMP" => ImageFormat.Bmp,
                        ".JPG" => ImageFormat.Jpeg,
                        ".JPEG" => ImageFormat.Jpeg,
                        ".GIF" => ImageFormat.Gif,
                        ".PNG" => ImageFormat.Png,
                        ".TIFF" => ImageFormat.Tiff,
                        _ => throw new NotSupportedException("Onbekend afbeeldingstype."),
                    };
                }
            }

            return formats;
        }

        private async Task GetPicturesFromBlob()
        {
            _mainForm.Cursor = Cursors.WaitCursor;
            _mainForm.PlaceHolder.Text = "Bezig laden...";
            _mainForm.PlaceHolder.Visible = true;

            _picsLoaded = await _item.LoadPictures(_images);

            _mainForm.Cursor = Cursors.Default;
        }

        private async Task GetPictureFromFile()
        {
            string imgFilter = string.Join(", ", SupportedImageExtensions[0..^2]) +
                $" of {SupportedImageExtensions[^1]}";

            if (!SupportedImageExtensions.Contains(
                    Path.GetExtension(_picPath).TrimStart('.').ToUpper()))
            {
                MessageBox.Show($"Het bestandstype wordt niet ondersteund gebruik:\n{imgFilter}",
                    "Bestandtype niet ondersteund", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _mainForm.PlaceHolder.Text = "Bezig laden...";
            _mainForm.PlaceHolder.Visible = true;
            _mainForm.Cursor = Cursors.WaitCursor;

            Image img = Image.FromFile(_picPath);

            if (CheckPicSize(img))
            {
                _mainForm.PictureBox.Image = null;

                if (_images[_mainForm.PictureSize.SelectedIndex] != null)
                {
                    _images[_mainForm.PictureSize.SelectedIndex].Dispose();
                }

                _images[_mainForm.PictureSize.SelectedIndex] = img;
                _imgsChanged[_mainForm.PictureSize.SelectedIndex] = true;
                _picsLoaded[_mainForm.PictureSize.SelectedIndex] = true;
                _mainForm.PictureSize.Items[_mainForm.PictureSize.SelectedIndex] =
                    _baseStatic.PictureNames[_mainForm.PictureSize.SelectedIndex] + " *";

                if (string.IsNullOrWhiteSpace(TagName))
                {
                    TagName = _mainForm.PictureName.Text = Path.GetFileName(_picPath);
                }

                _imgsOverridden[_mainForm.PictureSize.SelectedIndex] = false;
                await CheckPicName();

                _mainForm.ValueChanged();

                SetImage();
            }

            _mainForm.PlaceHolder.Visible = false;
            _mainForm.Cursor = Cursors.Default;
        }

        private async Task CheckPicName()
        {
            if (TagName != null)
            {
                bool dupName;

                if (SingleTagName)
                {
                    dupName = false;
                }
                else
                {
                    dupName = _orgPictureNames.Take(_mainForm.PictureSize.SelectedIndex).Contains(TagName) ||
                      _orgPictureNames.Skip(_mainForm.PictureSize.SelectedIndex + 1).Contains(TagName);
                }

                if (dupName || await _item.TagNameExistOutsideId(TagName))
                {
                    _imgsOverridden[_mainForm.PictureSize.SelectedIndex] = true;
                    MessageBox.Show("Deze bestandsnaam is al in gebruikt, huidige afbeelding(en) wordt hernoemd/vervangen bij verdergaan.",
                        "Duplicaat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool CheckPicSize(Image image)
        {
            if (image != null 
                && ((_baseStatic.RequiredWidth.HasValue && image.Width != _baseStatic.RequiredWidth)
                 || (_baseStatic.RequiredHeight.HasValue && image.Height != _baseStatic.RequiredHeight)))
            {
                MessageBox.Show($"Afbeelding is verkeerd formaat, formaat is {_baseStatic.RequiredWidth}X{_baseStatic.RequiredHeight}",
                    "Verkeerd formaat", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            /*else*/ if (image != null && (image.Width > _baseStatic.MaxWidth || image.Height > _baseStatic.MaxHeight))
            {
                MessageBox.Show($"Afbeelding is te groot, maximum is {_baseStatic.MaxWidth}X{_baseStatic.MaxHeight}",
                    "Te groot", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void SetImage()
        {
            _mainForm.PictureBox.Image = _images[_mainForm.PictureSize.SelectedIndex];

            if (_images[_mainForm.PictureSize.SelectedIndex] != null)
            {
                _mainForm.PlaceHolder.Visible = false;
                _mainForm.PictureBox.Location = new Point((_mainForm.Tabs.SelectedTab.Width - _mainForm.PictureBox.Width) / 2,
                    (_mainForm.PictureInnerPanel.Height - _mainForm.PictureBox.Height) / 2);
            }
            else
            {
                _mainForm.PlaceHolder.Visible = true;
                _mainForm.PlaceHolder.Text = _picsLoaded[_mainForm.PictureSize.SelectedIndex] ?
                    "Geen afbeelding" : "Fout: afbeelding niet gevonden";
            }
        }
    }
}