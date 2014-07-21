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
            if (e.KeyData == Keys.M || e.KeyData == Keys.D1) cbM.Checked = !cbM.Checked;
            if (e.KeyData == Keys.H || e.KeyData == Keys.D2) cbH.Checked = !cbH.Checked;
            if (e.KeyData == Keys.W || e.KeyData == Keys.D3) cbW.Checked = !cbW.Checked;
            if (e.KeyData == Keys.E || e.KeyData == Keys.D4) cbE.Checked = !cbE.Checked;
            if (e.KeyData == Keys.S || e.KeyData == Keys.D5) cbS.Checked = !cbS.Checked;
            if (e.KeyData == Keys.C || e.KeyData == Keys.D6) cbC.Checked = !cbC.Checked;
            if (e.KeyData == Keys.O || e.KeyData == Keys.D7) cbO.Checked = !cbO.Checked;

            if (e.KeyData == (Keys.M | Keys.Shift) || e.KeyData == (Keys.D1 | Keys.Shift)) cbM.Checked = true;
            if (e.KeyData == (Keys.H | Keys.Shift) || e.KeyData == (Keys.D2 | Keys.Shift)) cbH.Checked = true;
            if (e.KeyData == (Keys.W | Keys.Shift) || e.KeyData == (Keys.D3 | Keys.Shift)) cbW.Checked = true;
            if (e.KeyData == (Keys.E | Keys.Shift) || e.KeyData == (Keys.D4 | Keys.Shift)) cbE.Checked = true;
            if (e.KeyData == (Keys.S | Keys.Shift) || e.KeyData == (Keys.D5 | Keys.Shift)) cbS.Checked = true;
            if (e.KeyData == (Keys.C | Keys.Shift) || e.KeyData == (Keys.D6 | Keys.Shift)) cbC.Checked = true;
            if (e.KeyData == (Keys.O | Keys.Shift) || e.KeyData == (Keys.D7 | Keys.Shift)) cbO.Checked = true;

            if (e.KeyData == (Keys.M | Keys.Alt) || e.KeyData == (Keys.D1 | Keys.Alt)) cbM.Checked = false;
            if (e.KeyData == (Keys.H | Keys.Alt) || e.KeyData == (Keys.D2 | Keys.Alt)) cbH.Checked = false;
            if (e.KeyData == (Keys.W | Keys.Alt) || e.KeyData == (Keys.D3 | Keys.Alt)) cbW.Checked = false;
            if (e.KeyData == (Keys.E | Keys.Alt) || e.KeyData == (Keys.D4 | Keys.Alt)) cbE.Checked = false;
            if (e.KeyData == (Keys.S | Keys.Alt) || e.KeyData == (Keys.D5 | Keys.Alt)) cbS.Checked = false;
            if (e.KeyData == (Keys.C | Keys.Alt) || e.KeyData == (Keys.D6 | Keys.Alt)) cbC.Checked = false;
            if (e.KeyData == (Keys.O | Keys.Alt) || e.KeyData == (Keys.D7 | Keys.Alt)) cbO.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift) || e.KeyData == (Keys.Oemtilde | Keys.Shift))
            {
                cbM.Checked = true;
                cbH.Checked = true;
                cbW.Checked = true;
                cbE.Checked = true;
                cbS.Checked = true;
                cbC.Checked = true;
                cbO.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt) || e.KeyData == (Keys.Oemtilde | Keys.Alt))
            {
                cbM.Checked = false;
                cbH.Checked = false;
                cbW.Checked = false;
                cbE.Checked = false;
                cbS.Checked = false;
                cbC.Checked = false;
                cbO.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control) || e.KeyData == Keys.A
                 || e.KeyData == (Keys.Oemtilde | Keys.Control) || e.KeyData == Keys.Oemtilde)
            {
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use hotkeys to edit the list of consoles:\r\n"+
                "Use MHWESCO or 1234567 keys on keyboard to toggle the state.\r\n"+
                " + Shift to set the state to ON.\r\n"+
                " + Alt to set the state to OFF.\r\n"+
                "Use A (optionally with Ctrl) to toggle the state of all consoles.\r\n"+
                " + Shift to set the state of all consoles to ON.\r\n"+
                " + Alt to set the state of all consoles to OFF.", 
                "How to use");
        } 
	}
}
