using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Forms
{
    public enum FormHelpPage
    {
        Miscellaneous,
        SpaceMapHotkeys,
        Analysis
    }

	public partial class FormHelp : FormSerializeableToRegistry
	{
        private bool FirstShow;
		public FormHelp()
		{
			InitializeComponent();
            FirstShow = true;
		}

        public void ShowPage(FormHelpPage page)
        {
            switch (page)
            {
                case FormHelpPage.Miscellaneous:
                    tabControl1.SelectedTab = tabControl1.TabPages[0];
                    break;
                case FormHelpPage.SpaceMapHotkeys:
                    tabControl1.SelectedTab = tabControl1.TabPages[1];
                    break;        
                case FormHelpPage.Analysis:
                    tabControl1.SelectedTab = tabControl1.TabPages[2];
                    break;        
                default:
                    throw new NotImplementedException("Unimplemented help form page "+page.ToString());
            }
            if (FirstShow)
            {
                FirstShow = false;
                tabControl1_SelectedIndexChanged(null, null);
            }
            buttonOK.Select();
            Show();
            BringToFront();
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
            if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
            else
                SaveToRegistry();
		}

		private void _FormHelp_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.KeyData == Keys.Escape)
                Hide();
		}

        private void _FM_b_OK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            { 
                case 0:
                    doubleBufferedPanel1.Show();
                    doubleBufferedPanel2.Hide();
                    doubleBufferedPanel3.Hide();
                    break;
                case 1:
                    doubleBufferedPanel1.Hide();
                    doubleBufferedPanel2.Show();
                    doubleBufferedPanel3.Hide();
                    break;
                case 2:
                    doubleBufferedPanel1.Hide();
                    doubleBufferedPanel2.Hide();
                    doubleBufferedPanel3.Show();
                    break;
                default:
                    throw new NotImplementedException("Unimplemented tab panel #" + tabControl1.SelectedIndex);
            }
        }

        private void FormHelp_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                SaveToRegistry();
                Program.ShowMainFormIfRequired();
            }
        }
	}
}
