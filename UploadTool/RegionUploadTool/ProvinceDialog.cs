using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RegionUploadTool
{
    public partial class ProvinceListDialog : Form
    {
        public Provinces.Province Result { get; private set; }

        private static readonly List<Provinces.Province> ProvinceSource;

        static ProvinceListDialog()
        {
            ProvinceSource = new List<Provinces.Province>(Provinces.Dutch.Length + 1)
            {
                new Provinces.Province("[Vorige item]", null)
            };
            ProvinceSource.AddRange(Provinces.Dutch);
        }

        public ProvinceListDialog()
        {
            InitializeComponent();            

            ProvinceList.DataSource = ProvinceSource;
            ProvinceList.DisplayMember = "Name";
            ProvinceList.SelectedIndex = -1;
            ProvinceList.SelectedIndexChanged += ProvinceList_SelectedIndexChanged;
        }

        private void ProvinceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Result = (Provinces.Province)ProvinceList.SelectedItem;
            Close();
        }
    }
}
