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

                form.cbITMS.Checked = (value & 1) == 1;
                form.cbITLT.Checked = (value & 2) == 2;
                form.cbC.Checked    = (value & 4) == 4;
                form.cbHET.Checked  = (value & 8) == 8;
                form.cbW.Checked    = (value & 16) == 16;
                form.cbT.Checked    = (value & 32) == 32;
                form.cbTB.Checked   = (value & 64) == 64;
                form.cbDL.Checked   = (value & 128) == 128;
                form.cbAMB.Checked  = (value & 256) == 256;
                form.cbATB.Checked  = (value & 512) == 512;
                form.cbAS.Checked   = (value & 1024) == 1024;

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                result += form.cbITMS.Checked ? 1 : 0;
                result += form.cbITLT.Checked ? 2 : 0;
                result += form.cbC.Checked ? 4 : 0;
                result += form.cbHET.Checked ? 8 : 0;
                result += form.cbW.Checked ? 16 : 0;
                result += form.cbT.Checked ? 32 : 0;
                result += form.cbTB.Checked ? 64 : 0;
                result += form.cbDL.Checked ? 128 : 0;
                result += form.cbAMB.Checked ? 256 : 0;
                result += form.cbATB.Checked ? 512 : 0;
                result += form.cbAS.Checked ? 1024 : 0;
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
            if (e.KeyData == Keys.M || e.KeyData == Keys.D1)       cbITMS.Checked = !cbITMS.Checked;
            if (e.KeyData == Keys.L || e.KeyData == Keys.D2)       cbITLT.Checked = !cbITLT.Checked;
            if (e.KeyData == Keys.C || e.KeyData == Keys.D3)       cbC.Checked = !cbC.Checked;
            if (e.KeyData == Keys.H || e.KeyData == Keys.D4)       cbHET.Checked = !cbHET.Checked;
            if (e.KeyData == Keys.W || e.KeyData == Keys.D5)       cbW.Checked = !cbW.Checked;
            if (e.KeyData == Keys.T || e.KeyData == Keys.D6)       cbT.Checked = !cbT.Checked;
            if (e.KeyData == Keys.B || e.KeyData == Keys.D7)       cbTB.Checked = !cbTB.Checked;
            if (e.KeyData == Keys.D || e.KeyData == Keys.D8)       cbDL.Checked = !cbDL.Checked;
            if (e.KeyData == Keys.I || e.KeyData == Keys.D9)       cbAMB.Checked = !cbAMB.Checked;
            if (e.KeyData == Keys.O || e.KeyData == Keys.D0)       cbATB.Checked = !cbATB.Checked;
            if (e.KeyData == Keys.S || e.KeyData == Keys.OemMinus) cbAS.Checked = !cbAS.Checked;

            if (e.KeyData == (Keys.M | Keys.Shift) || e.KeyData == (Keys.D1 | Keys.Shift))       cbITMS.Checked = true;
            if (e.KeyData == (Keys.L | Keys.Shift) || e.KeyData == (Keys.D2 | Keys.Shift))       cbITLT.Checked = true;
            if (e.KeyData == (Keys.C | Keys.Shift) || e.KeyData == (Keys.D3 | Keys.Shift))       cbC.Checked = true;
            if (e.KeyData == (Keys.H | Keys.Shift) || e.KeyData == (Keys.D4 | Keys.Shift))       cbHET.Checked = true;
            if (e.KeyData == (Keys.W | Keys.Shift) || e.KeyData == (Keys.D5 | Keys.Shift))       cbW.Checked = true;
            if (e.KeyData == (Keys.T | Keys.Shift) || e.KeyData == (Keys.D6 | Keys.Shift))       cbT.Checked = true;
            if (e.KeyData == (Keys.B | Keys.Shift) || e.KeyData == (Keys.D7 | Keys.Shift))       cbTB.Checked = true;
            if (e.KeyData == (Keys.D | Keys.Shift) || e.KeyData == (Keys.D8 | Keys.Shift))       cbDL.Checked = true;
            if (e.KeyData == (Keys.I | Keys.Shift) || e.KeyData == (Keys.D9 | Keys.Shift))       cbAMB.Checked = true;
            if (e.KeyData == (Keys.O | Keys.Shift) || e.KeyData == (Keys.D0 | Keys.Shift))       cbATB.Checked = true;
            if (e.KeyData == (Keys.S | Keys.Shift) || e.KeyData == (Keys.OemMinus | Keys.Shift)) cbAS.Checked = true;

            if (e.KeyData == (Keys.M | Keys.Alt) || e.KeyData == (Keys.D1 | Keys.Alt))       cbITMS.Checked = false;
            if (e.KeyData == (Keys.L | Keys.Alt) || e.KeyData == (Keys.D2 | Keys.Alt))       cbITLT.Checked = false;
            if (e.KeyData == (Keys.C | Keys.Alt) || e.KeyData == (Keys.D3 | Keys.Alt))       cbC.Checked = false;
            if (e.KeyData == (Keys.H | Keys.Alt) || e.KeyData == (Keys.D4 | Keys.Alt))       cbHET.Checked = false;
            if (e.KeyData == (Keys.W | Keys.Alt) || e.KeyData == (Keys.D5 | Keys.Alt))       cbW.Checked = false;
            if (e.KeyData == (Keys.T | Keys.Alt) || e.KeyData == (Keys.D6 | Keys.Alt))       cbT.Checked = false;
            if (e.KeyData == (Keys.B | Keys.Alt) || e.KeyData == (Keys.D7 | Keys.Alt))       cbTB.Checked = false;
            if (e.KeyData == (Keys.D | Keys.Alt) || e.KeyData == (Keys.D8 | Keys.Alt))       cbDL.Checked = false;
            if (e.KeyData == (Keys.I | Keys.Alt) || e.KeyData == (Keys.D9 | Keys.Alt))       cbAMB.Checked = false;
            if (e.KeyData == (Keys.O | Keys.Alt) || e.KeyData == (Keys.D0 | Keys.Alt))       cbATB.Checked = false;
            if (e.KeyData == (Keys.S | Keys.Alt) || e.KeyData == (Keys.OemMinus | Keys.Alt)) cbAS.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift) || e.KeyData == (Keys.Oemtilde | Keys.Shift))
            {
                cbITMS.Checked = true;
                cbITLT.Checked = true;
                cbC.Checked = true;
                cbHET.Checked = true;
                cbW.Checked = true;
                cbT.Checked = true;
                cbTB.Checked = true;
                cbDL.Checked = true;
                cbAMB.Checked = true;
                cbATB.Checked = true;
                cbAS.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt) || e.KeyData == (Keys.Oemtilde | Keys.Alt))
            {
                cbITMS.Checked = false;
                cbITLT.Checked = false;
                cbC.Checked = false;
                cbHET.Checked = false;
                cbW.Checked = false;
                cbT.Checked = false;
                cbTB.Checked = false;
                cbDL.Checked = false;
                cbAMB.Checked = false;
                cbATB.Checked = false;
                cbAS.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control) || e.KeyData == Keys.A
                || e.KeyData == (Keys.Oemtilde | Keys.Control) || e.KeyData == Keys.Oemtilde)
            {
                if (cbITMS.Checked)
                {
                    cbITMS.Checked = false;
                    cbITLT.Checked = false;
                    cbC.Checked = false;
                    cbHET.Checked = false;
                    cbW.Checked = false;
                    cbT.Checked = false;
                    cbTB.Checked = false;
                    cbDL.Checked = false;
                    cbAMB.Checked = false;
                    cbATB.Checked = false;
                    cbAS.Checked = false;
                }
                else
                {
                    cbITMS.Checked = true;
                    cbITLT.Checked = true;
                    cbC.Checked = true;
                    cbHET.Checked = true;
                    cbW.Checked = true;
                    cbT.Checked = true;
                    cbTB.Checked = true;
                    cbDL.Checked = true;
                    cbAMB.Checked = true;
                    cbATB.Checked = true;
                    cbAS.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "You can use hotkeys to edit the list of abilities:\r\n" +
                "Use MSLCHWTBDIOS or 1234567890- keys on keyboard to toggle the state.\r\n" +
                " + Shift modifier to set the state to ON.\r\n" +
                " + Alt modifier to set the state to OFF.\r\n" +
                "Use A or ~  (optionally with Ctrl) to toggle the state of all abilities.\r\n" +
                " + Shift to set the state of all abilities to ON.\r\n" +
                " + Alt to set the state of all abilities to OFF.",
                "How to use");
        }
	}
}
