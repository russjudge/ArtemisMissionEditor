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
	public partial class DialogAbilityBits : Form
	{
        private DialogAbilityBits()
		{
			InitializeComponent();
		}

        public static KeyValuePair<bool, string> Show(string name, string initialValue)
		{

            int result = 0;
            using (DialogAbilityBits form = new DialogAbilityBits())
            {
                string caption = name;
                form.Text = caption;

                int value = Helper.IntTryParse(initialValue) ? Helper.StringToInt(initialValue) : 0;

                form.cbM.Checked = (value & 1) == 1;
                form.cbS.Checked = (value & 2) == 2;
                form.cbL.Checked = (value & 4) == 4;
                form.cbC.Checked = (value & 8) == 8;
                form.cbH.Checked = (value & 16) == 16;
                form.cbW.Checked = (value & 32) == 32;
                form.cbT.Checked = (value & 64) == 64;

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                result += form.cbM.Checked ? 1 : 0;
                result += form.cbS.Checked ? 2 : 0;
                result += form.cbL.Checked ? 4 : 0;
                result += form.cbC.Checked ? 8 : 0;
                result += form.cbH.Checked ? 16 : 0;
                result += form.cbW.Checked ? 32 : 0;
                result += form.cbT.Checked ? 64 : 0;
            }
			return new KeyValuePair<bool, string>(true, result.ToString());
		}

		private void ckButton_Click(object sender, EventArgs e)
		{
			Close_OK();
		}

		private void Close_OK()
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

        private void DialogConsoleList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.M) cbM.Checked = !cbM.Checked;
            if (e.KeyData == Keys.S) cbS.Checked = !cbS.Checked;
            if (e.KeyData == Keys.L) cbL.Checked = !cbL.Checked;
            if (e.KeyData == Keys.C) cbC.Checked = !cbC.Checked;
            if (e.KeyData == Keys.H) cbH.Checked = !cbH.Checked;
            if (e.KeyData == Keys.W) cbW.Checked = !cbW.Checked;
            if (e.KeyData == Keys.T) cbT.Checked = !cbT.Checked;

            if (e.KeyData == (Keys.M | Keys.Shift)) cbM.Checked = true;
            if (e.KeyData == (Keys.S | Keys.Shift)) cbS.Checked = true;
            if (e.KeyData == (Keys.L | Keys.Shift)) cbL.Checked = true;
            if (e.KeyData == (Keys.C | Keys.Shift)) cbC.Checked = true;
            if (e.KeyData == (Keys.H | Keys.Shift)) cbH.Checked = true;
            if (e.KeyData == (Keys.W | Keys.Shift)) cbW.Checked = true;
            if (e.KeyData == (Keys.T | Keys.Shift)) cbT.Checked = true;

            if (e.KeyData == (Keys.M | Keys.Alt)) cbM.Checked = false;
            if (e.KeyData == (Keys.S | Keys.Alt)) cbS.Checked = false;
            if (e.KeyData == (Keys.L | Keys.Alt)) cbL.Checked = false;
            if (e.KeyData == (Keys.C | Keys.Alt)) cbC.Checked = false;
            if (e.KeyData == (Keys.H | Keys.Alt)) cbH.Checked = false;
            if (e.KeyData == (Keys.W | Keys.Alt)) cbW.Checked = false;
            if (e.KeyData == (Keys.T | Keys.Alt)) cbT.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift))
            {
                cbM.Checked = true;
                cbS.Checked = true;
                cbL.Checked = true;
                cbC.Checked = true;
                cbH.Checked = true;
                cbW.Checked = true;
                cbT.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt))
            {
                cbM.Checked = false;
                cbS.Checked = false;
                cbL.Checked = false;
                cbC.Checked = false;
                cbH.Checked = false;
                cbW.Checked = false;
                cbT.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control))
            if (cbM.Checked)
            {
                cbM.Checked = false;
                cbS.Checked = false;
                cbL.Checked = false;
                cbC.Checked = false;
                cbH.Checked = false;
                cbW.Checked = false;
                cbT.Checked = false;
            }
            else
            {
                cbM.Checked = true;
                cbS.Checked = true;
                cbL.Checked = true;
                cbC.Checked = true;
                cbH.Checked = true;
                cbW.Checked = true;
                cbT.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use hotkeys to edit the list of abilities:\r\nUse MSLCHWT keys on keyboard to toggle the state.\r\nUse Shift + MSLCHWT keys to set the state on.\r\nUse Alt + MSLCHWT keys to set the state off.\r\nUse Ctrl + A to toggle the state of all abilities.\r\nUse Shift + A to set the state of all abilities on.\r\nUse Alt + A to set the state of all abilities to off.", "How to use");
        }
	}
}
