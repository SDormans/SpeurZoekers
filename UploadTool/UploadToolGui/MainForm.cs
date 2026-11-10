using AutoMapper;
using CollectibleUploadToolCon;
using UploadToolGui.Dto;
using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.UploadTools;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadToolGui
{
    public partial class MainForm : Form
    {
        public bool IsLoading { get; set; }

        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly MainTab _mainControl;
        private readonly QuestionTab _questionControl;
        private readonly AvailabilityTab _availabilityControl;
        private readonly PictureTab _pictureControl;

        private IAppMode _baseStatic;

        private FilterForm FilterDialog;
        private AppModeOptions _mode;
        private IBase _shadowItem;
        private bool _isClosing, _valueChanged, _checkFilter;
        private int _prevItemIndex;

        public MainForm(AppModeOptions appMode)
        {
            IsLoading = true;
            _mode = appMode;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);

            InitializeComponent();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DtoMapper());
                mc.AddProfile(new ActivityEntityMapper());
                mc.AddProfile(new AvatarEntityMapper());
                mc.AddProfile(new CollectibleEntityMapper());
            });

            _mapper = mappingConfig.CreateMapper();

            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _speurzoekersDbCon = new SpeurzoekersDbConnection(
                basePath,
                new ActivityEntityMapper(),
                new AvatarEntityMapper(),
                new CollectibleEntityMapper());

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

            MenuMode.Items.AddRange(Enum.GetValues(typeof(AppModeOptions))
                .Cast<AppModeOptions>()
                .Select(x => x.Description())
                .ToArray());
            MenuMode.SelectedIndex = (int)appMode;

            _mainControl = new MainTab(this);
            _questionControl = new QuestionTab(this);
            _availabilityControl = new AvailabilityTab(this);
            _pictureControl = new PictureTab(this);
        }

        public void ValueChanged()
        {
            _valueChanged = true;
            MenuSave.Text = "Sla op *";
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            await MainSetup();
        }

        private async void MenuMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && await CanLeave())
            {
                IsLoading = true;
                _mode = (AppModeOptions)MenuMode.SelectedIndex;
                await MainSetup();
            }
        }

        private async Task MainSetup()
        {
            Tabs.TabPages.Clear();

            switch (_mode)
            {
                case AppModeOptions.Activity:
                    _baseStatic = new ActivityAppMode(_speurzoekersDbCon, _mapper);

                    _mainControl.SetupMainTab(_baseStatic);
                    _availabilityControl.SetupAvailabilityTab((IAvailability)_baseStatic);
                    _pictureControl.SetupPictureTab((IPicture)_baseStatic);
                    break;
                case AppModeOptions.Avatars:
                    int defaultPrice = await _speurzoekersDbCon.ConfigurationRepository.GetAvatarPrice();
                    _baseStatic = new AvatarTypeAppMode(_speurzoekersDbCon, _mapper, defaultPrice);

                    _mainControl.SetupMainTab(_baseStatic);
                    _pictureControl.SetupPictureTab((IPicture)_baseStatic);
                    break;
                case AppModeOptions.Collectibles:
                    _baseStatic = new CollectibleTypeAppMode(_speurzoekersDbCon, _mapper);

                    _mainControl.SetupMainTab(_baseStatic);
                    _questionControl.SetupQuestionTab();
                    _availabilityControl.SetupAvailabilityTab((IAvailability)_baseStatic);
                    _pictureControl.SetupPictureTab((IPicture)_baseStatic);
                    break;
                default:
                    break;
            }

            bool filter = _baseStatic.ControlOptions.HasFlag(ControlOption.Filter);

            MenuFilter.Visible = filter;

            if (filter)
            {
                FilterDialog = new FilterForm(
                    ((IFilter)_baseStatic).FilterList,
                    ((IFilter)_baseStatic).SortingList
                );
            }

            await MainLoad(await _baseStatic.GetItems());
        }

        private async Task MainLoad(ListItem[] items)
        {
            IsLoading = true;

            ItemList.Items.Clear();
            ItemList.Items.AddRange(items);

            ItemList.SelectedIndex = 0;

            await FetchItem();
            await LoadItemData();

            IsLoading = false;
            _isClosing = false;
        }

        private async Task FetchItem()
        {
            if (ItemList.Items.Count == 1)
            {
                NewItem();
            }
            else
            {
                _shadowItem = await _baseStatic.GetItem(((ListItem)ItemList.SelectedItem).Id);
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private async void ItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && ItemList.SelectedIndex != _prevItemIndex)
            {
                IsLoading = true;
                
                if (await CanLeave())
                {
                    if (_shadowItem.IsNew)
                    {
                        ItemList.Items.RemoveAt(_prevItemIndex);
                    }

                    _prevItemIndex = ItemList.SelectedIndex;

                    if (ItemList.SelectedIndex != ItemList.Items.Count - 1)
                    {
                        await FetchItem();
                        await LoadItemData();
                    }
                    else
                    {
                        NewItem();
                        await LoadItemData();
                    }
                }
                else
                {
                    ItemList.SelectedIndex = _prevItemIndex;
                }

                IsLoading = false;
            }
        }

        private void NewItem()
        {
            _shadowItem = _baseStatic.NewItem();
            ItemList.Items[^1] = new ListItem(Guid.Empty, "[Bezig]");
            ItemList.Items.Add(new ListItem(Guid.Empty, "[Nieuw]"));
            _prevItemIndex = ItemList.SelectedIndex;
        }

        private async Task LoadItemData()
        {
            _checkFilter = _valueChanged = false;

            _mainControl.LoadMainTab((IMainDto)_shadowItem);

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadQuestion))
            {
                _questionControl.LoadQuestionTab((IQuestionDto)_shadowItem);
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadAvailability))
            {
                _availabilityControl.LoadAvailabilityTab((IAvailabilityDto)_shadowItem);
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadPicture))
            {
                await _pictureControl.LoadPictureTab((IPictureDto)_shadowItem);
            }

            MenuSave.Text = "Sla op";
        }

        private async Task<bool> CanLeave()
        {
            if (_valueChanged || !_pictureControl.CanLeave())
            {
                DataConsistency con = await ValidateData();

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

        private async Task<DataConsistency> ValidateData()
        {
            var items = new Dictionary<string, DataConsistency.ItemStatus>();

            foreach (KeyValuePair<string, DataConsistency.ItemStatus> pair in await _mainControl.ValidateData())
            {
                items.Add(pair.Key, pair.Value);
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadQuestion))
            {
                foreach (KeyValuePair<string, DataConsistency.ItemStatus> pair in await _questionControl.ValidateData())
                {
                    items.Add(pair.Key, pair.Value);
                }
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadAvailability))
            {
                foreach (KeyValuePair<string, DataConsistency.ItemStatus> pair in await _availabilityControl.ValidateData())
                {
                    items.Add(pair.Key, pair.Value);
                }
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadPicture))
            {
                foreach (KeyValuePair<string, DataConsistency.ItemStatus> pair in await _pictureControl.ValidateData())
                {
                    items.Add(pair.Key, pair.Value);
                }
            }

            if (items.Any())
            {
                DataConsistency.ValidationStatus con = items.Any(x => x.Value == DataConsistency.ItemStatus.Critical) ? 
                    DataConsistency.ValidationStatus.CriticalMissing : 
                    DataConsistency.ValidationStatus.OptionalMissing;

                return new DataConsistency
                {
                    Consistency = con,
                    MissingData = items
                        .OrderByDescending(x => x.Value)
                        .Select(x => x.Key + (x.Value switch
                        {
                            DataConsistency.ItemStatus.Critical => " [c]",
                            DataConsistency.ItemStatus.Optional => " [o]",
                            _ => string.Empty
                        }))
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

        private async void MenuRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Messages.Remove, Messages.RemoveCaption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (!_shadowItem.IsNew)
            {
                await _shadowItem.RemoveItem();
            }

            IsLoading = true;
            ItemList.Items.RemoveAt(ItemList.SelectedIndex);
            _prevItemIndex = 0;
            ItemList.SelectedIndex = 0;

            await FetchItem();
            await LoadItemData();
            IsLoading = false;
        }

        private async void MenuSave_Click(object sender, EventArgs e)
        {
            IsLoading = true;
            DataConsistency con = await ValidateData();

            if (con.Consistency == DataConsistency.ValidationStatus.OptionalMissing)
            {
                string msg = string.Format(Messages.SaveAnywayQuestion, Messages.MissingOptional, con);
                DialogResult result = MessageBox.Show(msg, Messages.MissingCaption, 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    IsLoading = false;
                    return;
                }
            }
            else if (con.Consistency == DataConsistency.ValidationStatus.CriticalMissing)
            {
                string msg = string.Format(Messages.CantSaveMessage, Messages.MissingCritical, con);
                MessageBox.Show(msg, Messages.MissingCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                IsLoading = false;
                return;
            }

            if (_valueChanged)
            {
                if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadQuestion))
                {
                    _questionControl.Save();
                }

                if (_baseStatic.ControlOptions.HasFlag(ControlOption.LoadPicture))
                {
                    await _pictureControl.Save();
                }

                await _shadowItem.SaveDb();

                ItemList.Items[ItemList.SelectedIndex] = new ListItem(_shadowItem.Id, _shadowItem.Name);

                _valueChanged = false;
                _checkFilter = true;
                IsLoading = false;
                MenuSave.Text = "Sla op";

                MessageBox.Show(Messages.SaveSucces, Messages.SaveSuccesCaption,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void MenuFilter_Click(object sender, EventArgs e)
        {
            await OpenFilterDialog();
        }

        private async Task OpenFilterDialog()
        {
            ListItem[] items;
            var result = FilterDialog.ShowDialog(this);

            if (result == DialogResult.Yes || (result == DialogResult.No && _checkFilter))
            {
                if (FilterDialog.FilterCriteria.Any() || !FilterDialog.IsDefaultSorting)
                {
                    items = await _baseStatic.GetItems(FilterDialog.FilterCriteria, 
                        FilterDialog.OrConcatenation, FilterDialog.Sorting);

                    if (items.All(x => _baseStatic.IsNewItem(x.Id)))
                    {
                        MessageBox.Show("Dit filter heeft geen resultaten", string.Empty, 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _checkFilter = true;
                        _ = OpenFilterDialog();
                        return;
                    }
                }
                else
                {
                    items = await _baseStatic.GetItems();
                }

                _checkFilter = false;
                
                await MainLoad(items);
            }
        }
    }
}
