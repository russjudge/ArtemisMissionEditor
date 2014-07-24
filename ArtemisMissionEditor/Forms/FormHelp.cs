using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public enum FormHelpPage
    {
        Miscellaneous,
        SpaceMapHotkeys
    }

	public partial class FormHelp : ArtemisMissionEditor.FormSerializeableToRegistry
	{
		public FormHelp()
		{
			InitializeComponent();
		}

        public void SetPage(FormHelpPage page)
        {
            switch (page)
            {
                case FormHelpPage.Miscellaneous:
                    tabControl1.SelectedTab = tabControl1.TabPages[0];
                    break;
                case FormHelpPage.SpaceMapHotkeys:
                    tabControl1.SelectedTab = tabControl1.TabPages[1];
                    break;        
                default:
                    throw new NotImplementedException("Unimplemented help form page "+page.ToString());
            }
            buttonOK.Select();
        }

		private void FormHelp_Load(object sender, EventArgs e)
		{
			_mainSC = null;
			_rightSC = null;
			ID = "FormHelp";
			LoadFromRegistry();
		}

		private void FormHelp_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!Program.IsClosing)
			{
				e.Cancel = true;
				this.Hide();
			}

			SaveToRegistry();
		}

		private void _FormHelp_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}

        private void _FM_b_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
	}
}
