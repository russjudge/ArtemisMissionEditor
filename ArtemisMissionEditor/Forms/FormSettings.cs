using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public partial class FormSettings : FormSerializeableToRegistry
    {
        public FormSettings()
        {
            InitializeComponent();

            pgMisc.SelectedObject = Settings.Current;
			pgColor.SelectedObject = new DictionaryPropertyGridAdapter(Settings.Current._bindsBrushColor);
        }


		private void _E_FS_b_SaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Save();
        }

        private void _E_FS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                Program.ShowMainFormIfRequired();
            }

			SaveToRegistry(); 
        }

		private void _E_FL_Load(object sender, EventArgs e)
		{
			_mainSC = null;
			_rightSC = null;
			ID = "FormSettings";
			LoadFromRegistry();

			ReadSettings();
		}

		private void _E_FM_tb_VD_TextChanged(object sender, EventArgs e)
		{
			Settings.Current.DefaultVesselDataPath = _FM_tb_VD.Text;
		}

		private void _E_FM_b_OpenVesselData_Click(object sender, EventArgs e)
		{
            DialogResult res;
            string filename;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.AddExtension = true;
                ofd.Multiselect = false;
                ofd.Filter = "XML Files|*.xml|All Files|*.*";
                ofd.Title = "Select vesselData.xml file";

                res = ofd.ShowDialog();
                filename = ofd.FileName;
            }
			if (res != DialogResult.OK)
				return;

			_FM_tb_VD.Text = filename;
		}

		private void _E_FS_b_ResetDefaults_Click(object sender, EventArgs e)
		{
			Settings.Current = new Settings();
			ReadSettings();
		}

		private void _E_FM_b_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void ReadSettings()
		{
			_FM_tb_VD.Text = Settings.Current.DefaultVesselDataPath;
			_FM_tb_SN.Text = Settings.Current.NewMissionStartBlock;
            _FM_tb_PN.Lines = Settings.Current.PlayerShipNames;
		}

		private void _E_FM_tb_SN_TextChanged(object sender, EventArgs e)
		{
			Settings.Current.NewMissionStartBlock = _FM_tb_SN.Text;
		}

		private void _E_FS_b_LoadSettings_Click(object sender, EventArgs e)
		{
			Settings.Load();
			ReadSettings();
		}

		private void _FormSettings_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}

        private void _FM_tb_PN_TextChanged(object sender, EventArgs e)
        {
            Settings.Current.PlayerShipNames = _FM_tb_PN.Lines;
        }



    }
}
