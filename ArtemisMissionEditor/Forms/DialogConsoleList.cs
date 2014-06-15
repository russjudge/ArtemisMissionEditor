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
	public partial class DialogConsoleList : Form
	{
        private DialogConsoleList()
		{
			InitializeComponent();
		}

        public static KeyValuePair<bool, string> Show(string name, string initialValue)
		{
            string result = "";
            using (DialogConsoleList form = new DialogConsoleList())
            {
                string caption = name;
                form.Text = caption;

                string value = (initialValue ?? "").ToLower();

                form.cbM.Checked = value.Contains('m');
                form.cbH.Checked = value.Contains('h');
                form.cbW.Checked = value.Contains('w');
                form.cbE.Checked = value.Contains('e');
                form.cbS.Checked = value.Contains('s');
                form.cbC.Checked = value.Contains('c');
                form.cbO.Checked = value.Contains('o');

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                
                result += form.cbM.Checked ? "M" : "";
                result += form.cbH.Checked ? "H" : "";
                result += form.cbW.Checked ? "W" : "";
                result += form.cbE.Checked ? "E" : "";
                result += form.cbS.Checked ? "S" : "";
                result += form.cbC.Checked ? "C" : "";
                result += form.cbO.Checked ? "O" : "";
            }
			return new KeyValuePair<bool, string>(true, result);
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
            if (e.KeyData == Keys.H) cbH.Checked = !cbH.Checked;
            if (e.KeyData == Keys.W) cbW.Checked = !cbW.Checked;
            if (e.KeyData == Keys.E) cbE.Checked = !cbE.Checked;
            if (e.KeyData == Keys.S) cbS.Checked = !cbS.Checked;
            if (e.KeyData == Keys.C) cbC.Checked = !cbC.Checked;
            if (e.KeyData == Keys.O) cbO.Checked = !cbO.Checked;

            if (e.KeyData == (Keys.M | Keys.Shift)) cbM.Checked = true;
            if (e.KeyData == (Keys.H | Keys.Shift)) cbH.Checked = true;
            if (e.KeyData == (Keys.W | Keys.Shift)) cbW.Checked = true;
            if (e.KeyData == (Keys.E | Keys.Shift)) cbE.Checked = true;
            if (e.KeyData == (Keys.S | Keys.Shift)) cbS.Checked = true;
            if (e.KeyData == (Keys.C | Keys.Shift)) cbC.Checked = true;
            if (e.KeyData == (Keys.O | Keys.Shift)) cbO.Checked = true;

            if (e.KeyData == (Keys.M | Keys.Alt)) cbM.Checked = false;
            if (e.KeyData == (Keys.H | Keys.Alt)) cbH.Checked = false;
            if (e.KeyData == (Keys.W | Keys.Alt)) cbW.Checked = false;
            if (e.KeyData == (Keys.E | Keys.Alt)) cbE.Checked = false;
            if (e.KeyData == (Keys.S | Keys.Alt)) cbS.Checked = false;
            if (e.KeyData == (Keys.C | Keys.Alt)) cbC.Checked = false;
            if (e.KeyData == (Keys.O | Keys.Alt)) cbO.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift))
            {
                cbM.Checked = true;
                cbH.Checked = true;
                cbW.Checked = true;
                cbE.Checked = true;
                cbS.Checked = true;
                cbC.Checked = true;
                cbO.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt))
            {
                cbM.Checked = false;
                cbH.Checked = false;
                cbW.Checked = false;
                cbE.Checked = false;
                cbS.Checked = false;
                cbC.Checked = false;
                cbO.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control))
            if (cbM.Checked)
            {
                cbM.Checked = false;
                cbH.Checked = false;
                cbW.Checked = false;
                cbE.Checked = false;
                cbS.Checked = false;
                cbC.Checked = false;
                cbO.Checked = false;
            }
            else
            {
                cbM.Checked = true;
                cbH.Checked = true;
                cbW.Checked = true;
                cbE.Checked = true;
                cbS.Checked = true;
                cbC.Checked = true;
                cbO.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use hotkeys to edit the list of consoles:\r\nUse MHWESCO keys on keyboard to toggle the state.\r\nUse Shift + MHWESCO keys to set the state on.\r\nUse Alt + MHWESCO keys to set the state off.\r\nUse Ctrl + A to toggle the state of all consoles.\r\nUse Shift + A to set the state of all consoles on.\r\nUse Alt + A to set the state of all consoles to off.", "How to use");
        }
	}
}
