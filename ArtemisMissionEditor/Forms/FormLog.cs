using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Forms
{
	public partial class FormLog : FormSerializeableToRegistry
	{
		public FormLog()
		{
			InitializeComponent();

            foreach (string item in Log.LogDisplayLinesTemp)
                _FL_lb_Log.Items.Add(item);
            Log.LogDisplayLinesTemp.Clear();
		}

		private void _E_FormLog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
            else
			    SaveToRegistry();
		}

		private void _E_FL_Load(object sender, EventArgs e)
		{
			_mainSC = null;
			_rightSC = null;
			ID = "FormLog";
			LoadFromRegistry();

            
		}

		private void _FormLog_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.KeyData == Keys.Escape)
                Hide();
		}

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonOK_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                SaveToRegistry();
                Program.ShowMainFormIfRequired();
            }
        }
	}
}
