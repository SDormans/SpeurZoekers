using UploadToolGui.Dto;
using UploadToolGui.Dto.Enums;
using Microsoft.EntityFrameworkCore.Internal;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Helper.Enums;
using Speurzoekers.Common.UploadTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UploadToolGui
{
    public partial class FilterForm : Form
    {
        private enum ControlNames
        {
            Panel,
            PropertySelector,
            ComparisonSelector,
            Input,
            InputNum,
            InputSelector,
            Delete
        }

        public List<FilterCriterion> FilterCriteria { get; private set; }
        public Sorting Sorting { get; private set; }
        public bool IsDefaultSorting { get; private set; }
        public bool OrConcatenation { get; private set; }

        private readonly FilterItem[] _items;
        private readonly SortItem[] _sortingList;
        private Dictionary<string, ComparisonTypes> EnumComparisons =>
            new Dictionary<string, ComparisonTypes>
            {
                { "=", ComparisonTypes.EnumEqual },
                { "<>", ComparisonTypes.EnumNotEqual },
                { "(..)", ComparisonTypes.EnumHasFlag },
                { "<> (..)", ComparisonTypes.EnumNoFlag }
            };
        private Dictionary<string, ComparisonTypes> EnumCountComparisons =>
            new Dictionary<string, ComparisonTypes>
            {
                { "=", ComparisonTypes.EnumCountEqual },
                { "<>", ComparisonTypes.EnumCountNotEqual },
                { ">", ComparisonTypes.EnumCountLarger },
                { "=>", ComparisonTypes.EnumCountLargerOrEqual },
                { "<", ComparisonTypes.EnumCountSmaller },
                { "<=", ComparisonTypes.EnumCountSmallerOrEqual }
            };
        private Dictionary<string, ComparisonTypes> NumberComparisons =>
            new Dictionary<string, ComparisonTypes>
            {
                { "=", ComparisonTypes.NumEqual },
                { "<>", ComparisonTypes.NumNotEqual },
                { ">", ComparisonTypes.NumLarger },
                { "=>", ComparisonTypes.NumLargerOrEqual },
                { "<", ComparisonTypes.NumSmaller },
                { "<=", ComparisonTypes.NumSmallerOrEqual }
            };
        private Dictionary<string, ComparisonTypes> StringComparisons =>
            new Dictionary<string, ComparisonTypes>
            {
                { "=", ComparisonTypes.StringEqual },
                { "<>", ComparisonTypes.StringNotEqual },
                { "(..)", ComparisonTypes.StringContains },
                { "<> (..)", ComparisonTypes.StringNotContains }
            };

        private bool _changed;

        public FilterForm(FilterItem[] items, SortItem[] sortingList)
        {
            _items = items;
            _sortingList = sortingList;
            FilterCriteria = new List<FilterCriterion>();

            InitializeComponent();

            MenuSortProp.Items.AddRange(_sortingList);
            MenuSortProp.SelectedIndex = 0;
            MenuConcatenation.SelectedIndex = 0;
            MenuSortDir.SelectedIndex = 0;
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            _changed = false;

            if (!MainPanel.Controls.Any())
            {
                AddRow();
            }
        }

        private void FilterForm_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in MainPanel.Controls)
            {
                control.Width = MainPanel.Width - 6;
            }
        }

        private void MenuOk_Click(object sender, EventArgs e)
        {
            if (!_changed)
            {
                DialogResult = DialogResult.No;
                return;
            }

            bool correctFilter = true;

            FilterCriteria = new List<FilterCriterion>(MainPanel.Controls.Count);

            foreach(TableLayoutPanel panel in MainPanel.Controls)
            {
                ComboBox prop = panel.Controls.Find(
                    panel.Name.Replace(ControlNames.Panel.ToString(), ControlNames.PropertySelector.ToString()),
                        false)[0] as ComboBox;

                if (prop.SelectedIndex < 0)
                {
                    continue;
                }

                ComboBox comp = panel.Controls.Find(
                    panel.Name.Replace(ControlNames.Panel.ToString(), ControlNames.ComparisonSelector.ToString()),
                        false)[0] as ComboBox,
                    inputSelect = panel.Controls.Find(
                    panel.Name.Replace(ControlNames.Panel.ToString(), ControlNames.InputSelector.ToString()),
                        false)[0] as ComboBox;
                TextBox input = panel.Controls.Find(
                    panel.Name.Replace(ControlNames.Panel.ToString(), ControlNames.Input.ToString()),
                        false)[0] as TextBox;
                NumericUpDown inputNum = panel.Controls.Find(
                    panel.Name.Replace(ControlNames.Panel.ToString(), ControlNames.InputNum.ToString()),
                        false)[0] as NumericUpDown;

                if (comp.SelectedIndex < 0)
                {
                    correctFilter = false;
                    break;
                }

                FilterItem filter = (FilterItem)prop.SelectedItem;
                FilterCriterion criterion = new FilterCriterion
                {
                    PropertyName = filter.PropertyName,
                    Type = filter.Type
                };

                switch (filter.FilterType)
                {
                    case FilterType.Enum:
                        if (inputSelect.SelectedIndex < 0)
                        {
                            correctFilter = false;
                            break;
                        }

                        criterion.Comparison = EnumComparisons[(string)comp.SelectedItem];
                        criterion.Value = ((EnumDetail)inputSelect.SelectedItem).EnumValue;
                        break;
                    case FilterType.EnumCount:
                        criterion.Comparison = EnumCountComparisons[(string)comp.SelectedItem];
                        criterion.Value = inputNum.Value;
                        break;
                    case FilterType.Number:
                        criterion.Comparison = NumberComparisons[(string)comp.SelectedItem];
                        criterion.Value = inputNum.Value;
                        break;
                    case FilterType.String:
                        if (string.IsNullOrWhiteSpace(input.Text))
                        {
                            correctFilter = false;
                            break;
                        }

                        criterion.Comparison = StringComparisons[(string)comp.SelectedItem];
                        criterion.Value = input.Text;
                        break;
                }
                
                FilterCriteria.Add(criterion);
            }

            if (correctFilter)
            {
                SortItem sort = (SortItem)MenuSortProp.SelectedItem;

                Sorting = new Sorting
                {
                    PropertyName = sort.PropertyName,
                    Ascending = MenuSortDir.SelectedIndex == 0,
                    SortingMethod = sort.SortingMethod
                };
                IsDefaultSorting = MenuSortProp.SelectedIndex == 0 && MenuSortDir.SelectedIndex == 0;
                OrConcatenation = MenuConcatenation.SelectedIndex == 1;

                DialogResult = DialogResult.Yes;
            }
            else
            {
                MessageBox.Show("Ongeldige filter", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MenuReset_Click(object sender, EventArgs e)
        {
            ComboBox prop = ((TableLayoutPanel)MainPanel.Controls[0]).Controls
                .Find(ControlNames.PropertySelector + "0", false).First() as ComboBox;

            if (MainPanel.Controls.Count > 1 || prop?.SelectedIndex > 0 || 
                MenuSortProp.SelectedIndex > 0 || MenuSortDir.SelectedIndex > 0)
            {
                _changed = true;
            }

            MainPanel.Controls.Clear();
            MenuSortProp.SelectedIndex = 0;
            MenuSortDir.SelectedIndex = 0;

            AddRow();
        }

        private void MenuAdd_Click(object sender, EventArgs e)
        {
            AddRow();
        }

        private void Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changed = true;
        }

        private void AddRow()
        {
            ComboBox para = new ComboBox
            {
                Dock = DockStyle.Fill,
                Name = ControlNames.PropertySelector.ToString() + MainPanel.Controls.Count
            };
            para.Items.AddRange(_items);
            para.SelectedIndexChanged += PropertySelector_SelectedIndexChanged;

            ComboBox comp = new ComboBox
            {
                Dock = DockStyle.Fill,
                Enabled = false,
                Name = ControlNames.ComparisonSelector.ToString() + MainPanel.Controls.Count
            };
            comp.SelectedIndexChanged += Input_Changed;

            TextBox input = new TextBox
            {
                Dock = DockStyle.Fill,
                Enabled = false,
                Name = ControlNames.Input.ToString() + MainPanel.Controls.Count
            };
            input.TextChanged += Input_Changed;

            NumericUpDown inputNum = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Name = ControlNames.InputNum.ToString() + MainPanel.Controls.Count,
                Maximum = long.MaxValue,
                Minimum = long.MinValue,
                Visible = false
            };
            inputNum.ValueChanged += Input_Changed;

            ComboBox inputSelect = new ComboBox
            {
                Dock = DockStyle.Fill,
                Name = ControlNames.InputSelector.ToString() + MainPanel.Controls.Count,
                Visible = false
            };
            inputSelect.SelectedIndexChanged += Input_Changed;

            Button del = new Button
            {
                Font = new Font(Font, FontStyle.Bold),
                Margin = new Padding(1),
                Name = ControlNames.Delete.ToString() + MainPanel.Controls.Count,
                Size = new Size(31, 31),
                Text = "X"
            };
            del.Click += DeleteRow_Click;

            TableLayoutPanel row = new TableLayoutPanel
            {
                Name = ControlNames.Panel.ToString() + MainPanel.Controls.Count,
                Height = 35,
                Width = MainPanel.Width - 6
            };

            row.RowStyles.Add(new RowStyle());
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 35));

            row.Controls.Add(para);
            row.SetCellPosition(para, new TableLayoutPanelCellPosition(0, 0));
            row.Controls.Add(comp);
            row.SetCellPosition(comp, new TableLayoutPanelCellPosition(1, 0));
            row.Controls.Add(input);
            row.SetCellPosition(input, new TableLayoutPanelCellPosition(2, 0));
            row.Controls.Add(inputNum);
            row.SetCellPosition(inputNum, new TableLayoutPanelCellPosition(2, 0));
            row.Controls.Add(inputSelect);
            row.SetCellPosition(inputSelect, new TableLayoutPanelCellPosition(2, 0));
            row.Controls.Add(del);
            row.SetCellPosition(del, new TableLayoutPanelCellPosition(3, 0));

            MainPanel.Controls.Add(row);
        }

        private void Input_Changed(object sender, EventArgs e)
        {
            _changed = true;
        }

        private void PropertySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox prop = (ComboBox)sender;
            TableLayoutPanel panel = MainPanel.Controls.Find(
                prop.Name.Replace(ControlNames.PropertySelector.ToString(), ControlNames.Panel.ToString()),
                    false)[0] as TableLayoutPanel;
            ComboBox comp = panel.Controls.Find(
                prop.Name.Replace(ControlNames.PropertySelector.ToString(), ControlNames.ComparisonSelector.ToString()),
                    false)[0] as ComboBox,
                inputSelect = panel.Controls.Find(
                prop.Name.Replace(ControlNames.PropertySelector.ToString(), ControlNames.InputSelector.ToString()),
                    false)[0] as ComboBox;
            TextBox input = panel.Controls.Find(
                prop.Name.Replace(ControlNames.PropertySelector.ToString(), ControlNames.Input.ToString()),
                    false)[0] as TextBox;
            NumericUpDown inputNum = panel.Controls.Find(
                prop.Name.Replace(ControlNames.PropertySelector.ToString(), ControlNames.InputNum.ToString()),
                    false)[0] as NumericUpDown;
            FilterItem filter = (FilterItem)prop.SelectedItem;

            comp.SelectedIndex = -1;
            comp.Items.Clear();
            comp.Enabled = true;

            input.Enabled = true;
            input.Visible = false;
            input.Text = string.Empty;
            inputNum.Visible = false;
            inputNum.Value = 0;
            inputSelect.Visible = false;
            inputSelect.SelectedIndex = -1;

            switch (filter.FilterType)
            {
                case FilterType.Enum:
                    comp.Items.AddRange(EnumComparisons
                        .Select(x => x.Key)
                        .ToArray());

                    EnumDetail[] items = Enum.GetValues(filter.Type)
                        .Cast<Enum>()
                        .Select((value, index) =>
                            new EnumDetail(value, value.Description()))
                        .ToArray();

                    inputSelect.Items.Clear();
                    inputSelect.Items.AddRange(items);
                    inputSelect.Visible = true;
                    break;
                case FilterType.EnumCount:
                    comp.Items.AddRange(EnumCountComparisons
                        .Select(x => x.Key)
                        .ToArray());

                    inputNum.Value = 0;
                    inputNum.Visible = true;
                    break;
                case FilterType.Number:
                    comp.Items.AddRange(NumberComparisons
                        .Select(x => x.Key)
                        .ToArray());

                    inputNum.Value = 0;
                    inputNum.Visible = true;
                    break;
                case FilterType.String:
                    comp.Items.AddRange(StringComparisons
                        .Select(x => x.Key)
                        .ToArray());

                    input.Text = string.Empty;
                    input.Visible = true;
                    break;
            }

            _changed = true;
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name
                .Replace(ControlNames.Delete.ToString(), ControlNames.Panel.ToString());

            MainPanel.Controls.RemoveByKey(name);

            if (MainPanel.Controls.Count == 0)
            {
                AddRow();
            }

            _changed = true;
        }
    }
}
