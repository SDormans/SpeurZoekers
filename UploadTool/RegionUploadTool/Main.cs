using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.UploadTools;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Region = Speurzoekers.Common.Domain.Region;

namespace RegionUploadTool
{
    public partial class Main : Form
    {
        private const string UrlBase = "www.keene.edu/campus/maps/tool/",
            UrlStart = "https://" + UrlBase + "?coordinates=";

        private readonly List<EnumDetail<Biotope>> Biotopes;

        private SpeurzoekersDbConnection _speurzoekersDbCon;
        private List<RegionSummary> _regions;
        private bool _loading, _valueChanged, _isClosing, _parentChanged;
        private TreeNode _currentNode;
        private Region _shadowRegion;

        public Main()
        {
            AutoScaleDimensions = new SizeF(6F, 13F);
            InitializeComponent();

            Biotopes = Enum.GetValues(typeof(Biotope))
                .Cast<Biotope>()
                .Select((value, index) =>
                    new EnumDetail<Biotope>(value, value.Description()))
                .ToList();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            _loading = true;
            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _speurzoekersDbCon = new SpeurzoekersDbConnection(basePath, new RegionEntityMapper());

            try
            {
                _speurzoekersDbCon.CheckSqlConnection();
                _speurzoekersDbCon.CheckValiditySqlScheme();
            }
            catch (SpeurzoekersDbConnection.SpeurzoekersDbConException ex)
            {
#if DEBUG
                MessageBox.Show(string.Format(Messages.DebugStartError, ex.Message),
                    Messages.CriticalErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                MessageBox.Show(string.Format(Messages.CriticalStartError, ex.Message),
                    Messages.CriticalErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
                return;
#endif
            }

            _regions = await _speurzoekersDbCon.RegionRepository.GetRegionsSummary();
            _regions.ForEach(x => x.Name ??= "[ONBEKEND]");
            await RebuildList(false);

            BiotopeSelection.Items.AddRange(Biotopes.ToArray());

            UrlLabel.Text = UrlBase;

            LoadRegionData();
        }

        private async Task RebuildList(bool keepIndex)
        {
            RegionList.Nodes.Clear();

            List<TreeNode> nodes = _regions
                .Select(x => new TreeNode
                {
                    Text = x.Name,
                    Tag = x.Id
                })
                .ToList();
            RegionSummary[] childeren = _regions
                .Where(x => x.ParentId.HasValue)
                .ToArray();

            foreach (RegionSummary child in childeren)
            {
                TreeNode childNode = nodes.First(x => ((Guid)x.Tag) == child.Id),
                    parentNode = nodes.First(x => ((Guid)x.Tag) == child.ParentId);

                parentNode.Nodes.Add(childNode);
            }

            RegionList.Nodes.AddRange(nodes
                .Where(x => x.Parent == null)
                .ToArray());
            RegionList.Nodes.Add(new TreeNode
            { 
                Text = "[Nieuw]",
                Tag = Guid.Empty
            });

            if (keepIndex)
            {
                RegionList.SelectedNode = _currentNode;
            }
            else
            {
                TreeNode first = RegionList.Nodes[0];
                RegionList.SelectedNode = first;
                _currentNode = first;
                _shadowRegion = await _speurzoekersDbCon.RegionRepository
                    .FetchRegion((Guid)first.Tag);
            }
        }

        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isClosing)
            {
                return;
            }

            e.Cancel = true;

            if (await CanLeave())
            {
                _isClosing = true;
                Close();
            }
        }

        private async void RegionList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_loading)
            {
                return;
            }

            _loading = true;

            bool? isNew = null;
            Coordinate[] newCors = null;

            if (await CanLeave())
            {
                if (((Guid)RegionList.SelectedNode.Tag) != Guid.Empty)
                {
                    isNew = false;
                }
                else
                {
                    newCors = GetStartCoordinates();
                    isNew = newCors != null ? true : (bool?)null;
                }
            }

            if (!isNew.HasValue) // rollback
            {
                RegionList.SelectedNode = _currentNode;
                _loading = false;
            }
            else
            {
                if (_shadowRegion.Id == default)
                {
                    RegionList.Nodes.Remove(_currentNode);
                }

                _currentNode = RegionList.SelectedNode;

                if (isNew.Value)
                {
                    CreateNew(newCors);
                }
                else
                {
                    _shadowRegion = await _speurzoekersDbCon.RegionRepository
                        .FetchRegion((Guid)RegionList.SelectedNode.Tag);

                    LoadRegionData();
                }
            }
        }

        private void NameInput_TextChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                ValueChanged();
                _shadowRegion.Name = NameInput.Text;
            }
        }

        private void BiotopeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                ValueChanged();
                _shadowRegion.Biotope =
                    ((EnumDetail<Biotope>)BiotopeSelection.SelectedItem).EnumValue;
            }
        }

        private void MinPositions_ValueChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _shadowRegion.MinPositions = (int)MinPositions.Value;
                ValueChanged();
            }
        }

        private void MaxPositions_ValueChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _shadowRegion.MaxPositions = (int)MaxPositions.Value;
                ValueChanged();
            }
        }

        private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_loading)
            {
                _loading = false;
                return;
            }

            string corString = Uri.UnescapeDataString(e.Url.ToString())
                                    .Substring(UrlStart.Length);
            List<Coordinate> corsValues = corString.Split('\n')
                                    .Select(x => new Coordinate(x))
                                    .ToList();

            if (corsValues[0].Equals(corsValues[^1])) // 'closed shape' has equal start and end coordinates
            {
                corsValues.RemoveAt(corsValues.Count - 1);
            }

            if (corsValues.Count < 3)
            {
                MessageBox.Show("Incompleet gebied", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // else
            MessageBox.Show("Gebied ingelezen", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Coordinate center = GetCenter(corsValues);

            _shadowRegion.PolygonVertices = corsValues.ToArray();
            _shadowRegion.Longitude = center.Longitude;
            _shadowRegion.Latitude = center.Latitude;

            ValueChanged();
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            _loading = true;

            DataConsistency con = await CorrectData();

            if (con.Consistency == DataConsistency.ValidationStatus.OptionalMissing)
            {
                string msg = string.Format(Messages.SaveAnywayQuestion, Messages.MissingOptional, con);
                DialogResult result = MessageBox.Show(msg, Messages.MissingCaption, 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    _loading = false;
                    return;
                }
            }
            else if (con.Consistency == DataConsistency.ValidationStatus.CriticalMissing)
            {
                string msg = string.Format(Messages.CantSaveMessage, Messages.MissingCritical, con);
                MessageBox.Show(msg, Messages.MissingCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                _loading = false;
                return;
            }

            if (_valueChanged)
            {
                await _speurzoekersDbCon.RegionRepository.AddOrUpdateRegion(_shadowRegion);

                _currentNode.Text = _shadowRegion.Name;
                _currentNode.Tag = _shadowRegion.Id;
            }

            if (_parentChanged)
            {
                _regions.Find(x => x.Id == _shadowRegion.Id).ParentId = _shadowRegion.ParentId;
                await RebuildList(true);
            }

            _valueChanged = false;
            _loading = false;
            Save.Text = "Sla op";
            OpenAdvancedDialog.Enabled = true;

            MessageBox.Show(Messages.SaveSucces, Messages.SaveSuccesCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void RemoveRegion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Messages.Remove, Messages.RemoveCaption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _loading = true;
            _regions.RemoveAll(x => x.Id == _shadowRegion.Id);

            foreach (RegionSummary reg in _regions.Where(x => x.ParentId == _shadowRegion.Id))
            {
                reg.ParentId = null;
            }

            if (_shadowRegion.Id != Guid.Empty)
            {
                await _speurzoekersDbCon.RegionRepository.RemoveRegion(_shadowRegion.Id);
            }

            await RebuildList(false);
            
            LoadRegionData();
        }

        private async void OpenAdvancedDialog_Click(object sender, EventArgs e)
        {
            AdvancedDialog dialog = new AdvancedDialog(_regions, _shadowRegion);
            dialog.ShowDialog();

            if (dialog.Result.HasFlag(AdvancedDialog.AdvancedDialogResult.ParentChanged) ||
                dialog.Result.HasFlag(AdvancedDialog.AdvancedDialogResult.CoordinatesChanged))
            {
                ValueChanged();
            }

            if (dialog.Result == AdvancedDialog.AdvancedDialogResult.NewChild)
            {
                await CreateNewChild();
            }

            if (dialog.Result.HasFlag(AdvancedDialog.AdvancedDialogResult.CoordinatesChanged))
            {
                _loading = true;
                Browser.Navigate(GetUrl());
                _loading = false;
            }

            if (dialog.Result.HasFlag(AdvancedDialog.AdvancedDialogResult.ParentChanged))
            {
                _parentChanged = true;
            }
        }

        private void CreateNew(Coordinate[] cors)
        {
            _shadowRegion = new Region
            {
                PolygonVertices = cors,
                Biotope = Biotope.Stad
            };

            RegionList.Nodes[^1].Text = "[Bezig]";
            RegionList.Nodes.Add(new TreeNode
            {
                Text = "[Nieuw]",
                Tag = Guid.Empty
            });

            LoadRegionData(true);
        }

        private async Task CreateNewChild()
        {
            if (_shadowRegion.Id == Guid.Empty)
            {
                MessageBox.Show("Kan geen binnengebied aanmaken als het buitengebied nog niet opgeslagen is.",
                     Messages.CriticalErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!await CanLeave())
            {
                return;
            }

            _loading = true;

            _shadowRegion = new Region
            {
                PolygonVertices = GetMinMax(_shadowRegion.PolygonVertices),
                MinPositions = _shadowRegion.MinPositions,
                MaxPositions = _shadowRegion.MaxPositions,
                ParentId = _shadowRegion.Id,
                Biotope = _shadowRegion.Biotope
            };

            TreeNode child = new TreeNode
            {
                Text = "[Bezig]",
                Tag = Guid.Empty
            };

            _currentNode.Nodes.Add(child);
            _currentNode = child;
            RegionList.SelectedNode = child;

            LoadRegionData(true);
        }

        private Coordinate[] GetMinMax(Coordinate[] coordinates)
        {
            float minLat = coordinates.Min(p => p.Latitude),
                minLong = coordinates.Min(p => p.Longitude),
                maxLat = coordinates.Max(p => p.Latitude),
                maxLong = coordinates.Max(p => p.Longitude);

            return new[]
            {
                new Coordinate(minLong, minLat),
                new Coordinate(maxLong, maxLat)
            };
        }

        private void LoadRegionData(bool valueChanged = false)
        {
            _valueChanged = valueChanged;
            _parentChanged = false;

            NameInput.Text = _shadowRegion.Name;
            BiotopeSelection.SelectedIndex =
                Biotopes.FindIndex(x => x.EnumValue == _shadowRegion.Biotope);
            MinPositions.Value = _shadowRegion.MinPositions;
            MaxPositions.Value = _shadowRegion.MaxPositions;
            Save.Text = valueChanged ? "Sla op *" : "Sla op";
            Browser.Navigate(GetUrl());
        }

        private Coordinate[] GetStartCoordinates()
        {
            Coordinate[] cors;
            ProvinceListDialog dialog = new ProvinceListDialog();
            dialog.ShowDialog();

            if (dialog.Result == null)
            {
                return null;
            }
            /*else*/if (dialog.Result.Coordinates == null)
            {
                cors = GetMinMax(_shadowRegion.PolygonVertices);
            }
            else
            {
                cors = dialog.Result.Coordinates;
            }

            return cors;
        }

        private async Task<bool> CanLeave()
        {
            if (_valueChanged)
            {
                DataConsistency con = await CorrectData();

                string missingMessage = con.Consistency switch
                {
                    DataConsistency.ValidationStatus.Complete => Messages.NoSaveComplete,
                    DataConsistency.ValidationStatus.OptionalMissing => Messages.MissingOptional,
                    _ => Messages.MissingCritical,
                };
                MessageBoxIcon icon = con.Consistency == DataConsistency.ValidationStatus.CriticalMissing ?
                    MessageBoxIcon.Error : MessageBoxIcon.Warning;

                DialogResult result = MessageBox.Show(
                     string.Format(Messages.NoSaveQuestion, missingMessage, con),
                     Messages.NoSaveCaption, MessageBoxButtons.OKCancel, icon);

                return result == DialogResult.OK;
            }
            else
            {
                return true;
            }
        }

        private async Task<DataConsistency> CorrectData()
        {
            Dictionary<string, bool> missing = new Dictionary<string, bool>();

            if (string.IsNullOrWhiteSpace(_shadowRegion.Name))
            {
                missing.Add("Naam", true);
            }
            else if (await _speurzoekersDbCon.RegionRepository
                .NameExistOutsideId(_shadowRegion.Id, _shadowRegion.Name))
            {
                missing.Add("Unieke naam", true);
            }
            if (MinPositions.Value > MaxPositions.Value)
            {
                missing.Add("Minimale waarde kleiner dan maximale", true);
            }
            if (_shadowRegion.PolygonVertices.Length < 3)
            {
                missing.Add("Volledig gebied (niet ingeladen?)", true);
            }

            if (missing.Any())
            {
                DataConsistency.ValidationStatus con = missing.Any(x => x.Value) ?
                    DataConsistency.ValidationStatus.CriticalMissing :
                    DataConsistency.ValidationStatus.OptionalMissing;

                return new DataConsistency
                {
                    Consistency = con,
                    MissingData = missing
                        .OrderByDescending(x => x.Value)
                        .Select(x => x.Key + (x.Value ? string.Empty : " [o]"))
                };
            }
            else
            {
                return new DataConsistency
                {
                    Consistency = DataConsistency.ValidationStatus.Complete
                };
            }
        }

        private string GetUrl()
        {
            List<Coordinate> cors = _shadowRegion.PolygonVertices.ToList();

            if (!cors[0].Equals(cors[^1]))
            {
                cors.Add(cors[0]); // 'closed shape' has equal start and end coordinates
            }

            IEnumerable<string> points = cors.Select(x => x.ToString());
            return Uri.EscapeUriString($"{UrlStart}{string.Join('\n', points)}");
        }

        private Coordinate GetCenter(IEnumerable<Coordinate> points)
        {
            float minLat = points.Select(p => p.Latitude).Min(),
                minLong = points.Select(p => p.Longitude).Min(),
                maxLat = points.Select(p => p.Latitude).Max(),
                maxLong = points.Select(p => p.Longitude).Max(),
                midLat = (maxLat - minLat) / 2 + minLat, 
                midLong = (maxLong - minLong) / 2 + minLong;

            return new Coordinate(midLong, midLat);
        }

        private void ValueChanged()
        {
            _valueChanged = true;
            Save.Text = "Sla op *";
        }

        private void Drawregion(Image img, IEnumerable<Coordinate> points)
        {
            float size = 520, scale = 512;
            Bitmap bit = new Bitmap(img);

            using Graphics grap = Graphics.FromImage(bit);
            float minLat = points.Min(p => p.Latitude),
                minLong = points.Min(p => p.Longitude),
                maxLat = points.Max(p => p.Latitude),
                maxLong = points.Max(p => p.Longitude),
                lenghtLat = maxLat - minLat, lenghtLong = maxLong - minLong,
                factor = scale / Math.Max(lenghtLat, lenghtLong),
                offsetLat = (size - lenghtLat * factor) / 2 - minLat * factor,
                offsetLong = (size - lenghtLong * factor) / 2 - minLong * factor;
            Coordinate prevPoint = points.Last();
            Pen pen = new Pen(Color.Black, 2);

            grap.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);

            foreach (Coordinate point in points)
            {
                float lat1 = prevPoint.Latitude * factor + offsetLat,
                    long1 = prevPoint.Longitude * factor + offsetLong,
                    lat2 = point.Latitude * factor + offsetLat,
                    long2 = point.Longitude * factor + offsetLong;

                grap.DrawLine(pen, lat1, long1, lat2, long2);
                prevPoint = point;
            }

            grap.Flush();
        }
    }
}
