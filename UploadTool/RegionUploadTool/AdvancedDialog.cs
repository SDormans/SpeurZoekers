using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegionUploadTool
{
    public partial class AdvancedDialog : Form
    {
        [Flags]
        public enum AdvancedDialogResult
        {
            CoordinatesChanged = 1 << 0,
            ParentChanged = 1 << 1,
            NewChild = 1 << 2
        }

        public AdvancedDialogResult Result { get; private set; }

        private readonly IEnumerable<RegionSummary> _regions;
        private readonly Region _shadowRegion;

        private bool _requestedNew;

        public AdvancedDialog(IEnumerable<RegionSummary> regionsSummary, Region shadowRegion)
        {
            InitializeComponent();

            Result = 0;
            _regions = regionsSummary;
            _shadowRegion = shadowRegion;
            _requestedNew = false;
        }

        private void AdvancedDialog_Load(object sender, EventArgs e)
        {
            Coordinates.Text = string.Join<Coordinate>("\r\n", _shadowRegion.PolygonVertices);

            List<RegionSummary> selectRegions = new List<RegionSummary>(_regions);
            selectRegions.RemoveAll(x => x.Id == _shadowRegion.Id);
            RemoveAllChilderen(selectRegions, _shadowRegion.Id);
            selectRegions.Insert(0, new RegionSummary
                {
                    Id = Guid.Empty,
                    Name = string.Empty
                });

            ParentSelector.Items.AddRange(selectRegions.ToArray());

            if (_shadowRegion.ParentId.HasValue)
            {
                ParentSelector.SelectedIndex = selectRegions.FindIndex(x => x.Id == _shadowRegion.ParentId);
            }

            CoordinatesOrder.SelectedIndex = 0;
        }

        private void RemoveAllChilderen(List<RegionSummary> regions, Guid id)
        {
            RegionSummary[] childeren = regions.Where(x => x.ParentId == id).ToArray();

            foreach (RegionSummary child in childeren)
            {
                RemoveAllChilderen(regions, child.Id);
                regions.Remove(child);
            }
        }

        private void AdvancedDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Result = 0;

            if (((RegionSummary)ParentSelector.SelectedItem)?.Id != _shadowRegion.ParentId)
            {
                Guid id = ((RegionSummary)ParentSelector.SelectedItem).Id;

                if (id != Guid.Empty)
                {
                    _shadowRegion.ParentId = ((RegionSummary)ParentSelector.SelectedItem).Id;
                }
                else
                {
                    _shadowRegion.ParentId = null;
                }

                Result |= AdvancedDialogResult.ParentChanged;
            }

            Coordinate[] cors = GetCoordinates()?.ToArray();

            if (cors == null)
            {
                DialogResult result = MessageBox.Show("Geen correct gebied, verder gaan zonder te bewaren?",
                    Messages.NoSaveCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (!cors.SequenceEqual(_shadowRegion.PolygonVertices))
            {
                _shadowRegion.PolygonVertices = cors;
                Result |= AdvancedDialogResult.CoordinatesChanged;
            }

            if (_requestedNew)
            {
                Result |= AdvancedDialogResult.NewChild;
                return;
            }
        }

        private void NewChild_Click(object sender, EventArgs e)
        {
            _requestedNew = true;
            Close();
        }

        private void DialogBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormatCoordinates_Click(object sender, EventArgs e)
        {
            IEnumerable<Coordinate> cors = GetCoordinates();

            if (cors != null)
            {
                Coordinates.Text = string.Join<Coordinate>("\r\n", cors);
            }
        }

        private IEnumerable<Coordinate> GetCoordinates()
        {
            try
            {
                MatchCollection matches = Regex.Matches(Coordinates.Text,
                    @"(?<val0>\d+(?<sep>[\.,])\d+)[ ;,\[\]]+(?<val1>\d+[\.,]\d+)\W*");

                if (matches.Count < 3)
                {
                    throw new Exception();
                }

                string lon = CoordinatesOrder.SelectedIndex == 0 ? "val0" : "val1",
                    lat = CoordinatesOrder.SelectedIndex == 0 ? "val1" : "val0";
                NumberFormatInfo format = new NumberFormatInfo
                {
                    NumberDecimalSeparator = matches[0].Groups["sep"].Value,
                    NumberDecimalDigits = 6
                };

                IEnumerable<Coordinate> cors = matches.Select(x =>
                    new Coordinate(float.Parse(x.Groups[lon].Value, format),
                    float.Parse(x.Groups[lat].Value, format)));

                return cors;
            }
            catch
            {
                return null;
            }
        }
    }
}
