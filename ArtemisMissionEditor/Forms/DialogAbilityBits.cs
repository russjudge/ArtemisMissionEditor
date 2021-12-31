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

                form.cbStealth.Checked   = (value & 1) == 1;
                form.cbLowVis.Checked    = (value & 2) == 2;
                form.cbCloak.Checked     = (value & 4) == 4;
                form.cbHET.Checked       = (value & 8) == 8;
                form.cbWarp.Checked      = (value & 16) == 16;
                form.cbTeleport.Checked  = (value & 32) == 32;
                form.cbTractor.Checked   = (value & 64) == 64;
                form.cbDrones.Checked    = (value & 128) == 128;
                form.cbAntiMine.Checked  = (value & 256) == 256;
                form.cbAntiTorp.Checked  = (value & 512) == 512;
                form.cbShldDrain.Checked = (value & 1024) == 1024;
                form.cbShldVamp.Checked  = (value & 2048) == 2048;
                form.cbTeleBack.Checked  = (value & 4096) == 4096;
                form.cbShldReset.Checked = (value & 8192) == 8192;

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                result += form.cbStealth.Checked ? 1 : 0;
                result += form.cbLowVis.Checked ? 2 : 0;
                result += form.cbCloak.Checked ? 4 : 0;
                result += form.cbHET.Checked ? 8 : 0;
                result += form.cbWarp.Checked ? 16 : 0;
                result += form.cbTeleport.Checked ? 32 : 0;
                result += form.cbTractor.Checked ? 64 : 0;
                result += form.cbDrones.Checked ? 128 : 0;
                result += form.cbAntiMine.Checked ? 256 : 0;
                result += form.cbAntiTorp.Checked ? 512 : 0;
                result += form.cbShldDrain.Checked ? 1024 : 0;
                result += form.cbShldVamp.Checked ? 2048 : 0;
                result += form.cbTeleBack.Checked ? 4096 : 0;
                result += form.cbShldReset.Checked ? 8192 : 0;
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
			Close();
		}

        private void DialogConsoleList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.M || e.KeyData == Keys.D1)       cbStealth.Checked = !cbStealth.Checked;
            if (e.KeyData == Keys.L || e.KeyData == Keys.D2)       cbLowVis.Checked = !cbLowVis.Checked;
            if (e.KeyData == Keys.C || e.KeyData == Keys.D3)       cbCloak.Checked = !cbCloak.Checked;
            if (e.KeyData == Keys.H || e.KeyData == Keys.D4)       cbHET.Checked = !cbHET.Checked;
            if (e.KeyData == Keys.W || e.KeyData == Keys.D5)       cbWarp.Checked = !cbWarp.Checked;
            if (e.KeyData == Keys.T || e.KeyData == Keys.D6)       cbTeleport.Checked = !cbTeleport.Checked;
            if (e.KeyData == Keys.B || e.KeyData == Keys.D7)       cbTractor.Checked = !cbTractor.Checked;
            if (e.KeyData == Keys.D || e.KeyData == Keys.D8)       cbDrones.Checked = !cbDrones.Checked;
            if (e.KeyData == Keys.I || e.KeyData == Keys.D9)       cbAntiMine.Checked = !cbAntiMine.Checked;
            if (e.KeyData == Keys.O || e.KeyData == Keys.D0)       cbAntiTorp.Checked = !cbAntiTorp.Checked;
            if (e.KeyData == Keys.S || e.KeyData == Keys.OemMinus) cbShldDrain.Checked = !cbShldDrain.Checked;
            if (e.KeyData == Keys.V || e.KeyData == Keys.Oemplus)  cbShldVamp.Checked = !cbShldVamp.Checked;
            if (e.KeyData == Keys.Y)                               cbTeleBack.Checked = !cbTeleBack.Checked;
            if (e.KeyData == Keys.R)                               cbShldReset.Checked = !cbShldReset.Checked;

            if (e.KeyData == (Keys.M | Keys.Shift) || e.KeyData == (Keys.D1 | Keys.Shift))       cbStealth.Checked = true;
            if (e.KeyData == (Keys.L | Keys.Shift) || e.KeyData == (Keys.D2 | Keys.Shift))       cbLowVis.Checked = true;
            if (e.KeyData == (Keys.C | Keys.Shift) || e.KeyData == (Keys.D3 | Keys.Shift))       cbCloak.Checked = true;
            if (e.KeyData == (Keys.H | Keys.Shift) || e.KeyData == (Keys.D4 | Keys.Shift))       cbHET.Checked = true;
            if (e.KeyData == (Keys.W | Keys.Shift) || e.KeyData == (Keys.D5 | Keys.Shift))       cbWarp.Checked = true;
            if (e.KeyData == (Keys.T | Keys.Shift) || e.KeyData == (Keys.D6 | Keys.Shift))       cbTeleport.Checked = true;
            if (e.KeyData == (Keys.B | Keys.Shift) || e.KeyData == (Keys.D7 | Keys.Shift))       cbTractor.Checked = true;
            if (e.KeyData == (Keys.D | Keys.Shift) || e.KeyData == (Keys.D8 | Keys.Shift))       cbDrones.Checked = true;
            if (e.KeyData == (Keys.I | Keys.Shift) || e.KeyData == (Keys.D9 | Keys.Shift))       cbAntiMine.Checked = true;
            if (e.KeyData == (Keys.O | Keys.Shift) || e.KeyData == (Keys.D0 | Keys.Shift))       cbAntiTorp.Checked = true;
            if (e.KeyData == (Keys.S | Keys.Shift) || e.KeyData == (Keys.OemMinus | Keys.Shift)) cbShldDrain.Checked = true;
            if (e.KeyData == (Keys.V | Keys.Shift) || e.KeyData == (Keys.Oemplus | Keys.Shift))  cbShldVamp.Checked = true;
            if (e.KeyData == (Keys.Y | Keys.Shift))                                              cbTeleBack.Checked = true;
            if (e.KeyData == (Keys.R | Keys.Shift))                                              cbShldReset.Checked = true;

            if (e.KeyData == (Keys.M | Keys.Alt) || e.KeyData == (Keys.D1 | Keys.Alt))       cbStealth.Checked = false;
            if (e.KeyData == (Keys.L | Keys.Alt) || e.KeyData == (Keys.D2 | Keys.Alt))       cbLowVis.Checked = false;
            if (e.KeyData == (Keys.C | Keys.Alt) || e.KeyData == (Keys.D3 | Keys.Alt))       cbCloak.Checked = false;
            if (e.KeyData == (Keys.H | Keys.Alt) || e.KeyData == (Keys.D4 | Keys.Alt))       cbHET.Checked = false;
            if (e.KeyData == (Keys.W | Keys.Alt) || e.KeyData == (Keys.D5 | Keys.Alt))       cbWarp.Checked = false;
            if (e.KeyData == (Keys.T | Keys.Alt) || e.KeyData == (Keys.D6 | Keys.Alt))       cbTeleport.Checked = false;
            if (e.KeyData == (Keys.B | Keys.Alt) || e.KeyData == (Keys.D7 | Keys.Alt))       cbTractor.Checked = false;
            if (e.KeyData == (Keys.D | Keys.Alt) || e.KeyData == (Keys.D8 | Keys.Alt))       cbDrones.Checked = false;
            if (e.KeyData == (Keys.I | Keys.Alt) || e.KeyData == (Keys.D9 | Keys.Alt))       cbAntiMine.Checked = false;
            if (e.KeyData == (Keys.O | Keys.Alt) || e.KeyData == (Keys.D0 | Keys.Alt))       cbAntiTorp.Checked = false;
            if (e.KeyData == (Keys.S | Keys.Alt) || e.KeyData == (Keys.OemMinus | Keys.Alt)) cbShldDrain.Checked = false;
            if (e.KeyData == (Keys.V | Keys.Alt) || e.KeyData == (Keys.Oemplus | Keys.Alt))  cbShldVamp.Checked = false;
            if (e.KeyData == (Keys.Y | Keys.Alt))                                            cbTeleBack.Checked = false;
            if (e.KeyData == (Keys.R | Keys.Alt))                                            cbShldReset.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift) || e.KeyData == (Keys.Oemtilde | Keys.Shift))
            {
                cbStealth.Checked = true;
                cbLowVis.Checked = true;
                cbCloak.Checked = true;
                cbHET.Checked = true;
                cbWarp.Checked = true;
                cbTeleport.Checked = true;
                cbTractor.Checked = true;
                cbDrones.Checked = true;
                cbAntiMine.Checked = true;
                cbAntiTorp.Checked = true;
                cbShldDrain.Checked = true;
                cbShldVamp.Checked = true;
                cbTeleBack.Checked = true;
                cbShldReset.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt) || e.KeyData == (Keys.Oemtilde | Keys.Alt))
            {
                cbStealth.Checked = false;
                cbLowVis.Checked = false;
                cbCloak.Checked = false;
                cbHET.Checked = false;
                cbWarp.Checked = false;
                cbTeleport.Checked = false;
                cbTractor.Checked = false;
                cbDrones.Checked = false;
                cbAntiMine.Checked = false;
                cbAntiTorp.Checked = false;
                cbShldDrain.Checked = false;
                cbShldVamp.Checked = false;
                cbTeleBack.Checked = false;
                cbShldReset.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control) || e.KeyData == Keys.A
                || e.KeyData == (Keys.Oemtilde | Keys.Control) || e.KeyData == Keys.Oemtilde)
            {
                if (cbStealth.Checked)
                {
                    cbStealth.Checked = false;
                    cbLowVis.Checked = false;
                    cbCloak.Checked = false;
                    cbHET.Checked = false;
                    cbWarp.Checked = false;
                    cbTeleport.Checked = false;
                    cbTractor.Checked = false;
                    cbDrones.Checked = false;
                    cbAntiMine.Checked = false;
                    cbAntiTorp.Checked = false;
                    cbShldDrain.Checked = false;
                    cbShldVamp.Checked = false;
                    cbTeleBack.Checked = false;
                    cbShldReset.Checked = false;
                }
                else
                {
                    cbStealth.Checked = true;
                    cbLowVis.Checked = true;
                    cbCloak.Checked = true;
                    cbHET.Checked = true;
                    cbWarp.Checked = true;
                    cbTeleport.Checked = true;
                    cbTractor.Checked = true;
                    cbDrones.Checked = true;
                    cbAntiMine.Checked = true;
                    cbAntiTorp.Checked = true;
                    cbShldDrain.Checked = true;
                    cbShldVamp.Checked = true;
                    cbTeleBack.Checked = true;
                    cbShldReset.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "You can use hotkeys to edit the list of abilities:\r\n" +
                "Use MSLCHWTBDIOSV or 1234567890-= keys on keyboard to toggle the state.\r\n" +
                " + Shift modifier to set the state to ON.\r\n" +
                " + Alt modifier to set the state to OFF.\r\n" +
                "Use A or ~  (optionally with Ctrl) to toggle the state of all abilities.\r\n" +
                " + Shift to set the state of all abilities to ON.\r\n" +
                " + Alt to set the state of all abilities to OFF.",
                "How to use");
        }
    }
}
