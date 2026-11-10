using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.UploadTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadToolGui
{
    public class AvailabilityTab
    {
        private readonly MainForm _mainForm;

        private IAvailability _baseStatic;
        private IAvailabilityDto _item;
        private bool _hasList1;

        public AvailabilityTab(MainForm mainForm)
        {
            _mainForm = mainForm;

            _mainForm.AvList0.SelectedIndexChanged += AvList0_SelectedIndexChanged;
            _mainForm.ToggleAvList0.Click += ToggleAvList0_Click;
            _mainForm.AvList1.SelectedIndexChanged += AvList1_SelectedIndexChanged;
            _mainForm.ToggleAvList1.Click += ToggleAvList1_Click;
        }

        public void SetupAvailabilityTab(IAvailability baseStatic)
        {
            _baseStatic = baseStatic;
            _hasList1 = baseStatic.AvailabilityLists.GetLength(0) > 1;

            _mainForm.Tabs.Controls.Add(_mainForm.TabAvailability);

            _mainForm.TabAvailability.Text = _baseStatic.AvailabilityListNames[0];
            _mainForm.AvList0.Items.Clear();
            _mainForm.AvList1.Items.Clear();
            _mainForm.ToggleAvList1.Visible = _hasList1;
            _mainForm.AvList1.Visible = _hasList1;
            _mainForm.lbAvList0.Text = _baseStatic.AvailabilityListNames[0];

            _mainForm.AvList0.Items.AddRange(baseStatic.AvailabilityLists[0]);

            if (_hasList1)
            {
                _mainForm.AvailibiltyPanel.SetColumnSpan(_mainForm.lbAvList1, 1);
                _mainForm.TabAvailability.Text += "/" + _baseStatic.AvailabilityListNames[1];
                _mainForm.lbAvList1.Text = _baseStatic.AvailabilityListNames[1];

                _mainForm.AvList1.Items.AddRange(baseStatic.AvailabilityLists[1]);
            }
            else // keep the columns right from collapsing
            {
                _mainForm.AvailibiltyPanel.SetColumnSpan(_mainForm.lbAvList1, 2);
                _mainForm.lbAvList1.Text = string.Empty;
            }
        }

        public void LoadAvailabilityTab(IAvailabilityDto item)
        {
            _item = item;

            for (int i = 0; i < _mainForm.AvList0.Items.Count; i++)
            {
                _mainForm.AvList0.SetItemChecked(i, 
                    _item.AvailabilityListProps[0]().HasFlag(_baseStatic.AvailabilityLists[0][i].EnumValue));
            }

            for (int i = 0; i < _mainForm.AvList1.Items.Count; i++)
            {
                _mainForm.AvList1.SetItemChecked(i,
                    _item.AvailabilityListProps[1]().HasFlag(_baseStatic.AvailabilityLists[1][i].EnumValue));
            }
        }

        public Task<Dictionary<string, DataConsistency.ItemStatus>> ValidateData()
        {
            var items = new Dictionary<string, DataConsistency.ItemStatus>();

            if (_item.AvailabilityListProps[0].Equals(0))
            {
                items.Add(_baseStatic.AvailabilityListNames[0], DataConsistency.ItemStatus.Optional);
            }

            if (_hasList1 && _item.AvailabilityListProps[1].Equals(0))
            {
                items.Add(_baseStatic.AvailabilityListNames[1], DataConsistency.ItemStatus.Optional);
            }

            return Task.FromResult(items);
        }

        public void AvList0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                SetList0();
            }
        }

        public void ToggleAvList0_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _mainForm.AvList0.Items.Count; i++)
            {
                _mainForm.AvList0.SetItemChecked(i, !_mainForm.AvList0.GetItemChecked(i));
            }

            SetList0();
        }

        public void AvList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                SetList1();
            }
        }

        public void ToggleAvList1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _mainForm.AvList1.Items.Count; i++)
            {
                _mainForm.AvList1.SetItemChecked(i, !_mainForm.AvList1.GetItemChecked(i));
            }

            SetList1();
        }

        private void SetList0()
        {
            _item.SetAvailabilityListProp(0, _mainForm.AvList0.CheckedItems
                   .Cast<EnumDetail>()
                   .Select(x => x.EnumValue));

            _mainForm.ValueChanged();
        }

        private void SetList1()
        {
            _item.SetAvailabilityListProp(1, _mainForm.AvList1.CheckedItems
                   .Cast<EnumDetail>()
                   .Select(x => x.EnumValue));

            _mainForm.ValueChanged();
        }
    }
}
