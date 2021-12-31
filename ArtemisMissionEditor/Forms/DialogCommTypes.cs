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
    public partial class DialogCommTypes : Form
    {
        private DialogCommTypes()
        {
            InitializeComponent();
        }

        public static KeyValuePair<bool, string> Show(string name, string initialValue)
        {
            string result = "";
            using (DialogCommTypes form = new DialogCommTypes())
            {
                string caption = name;
                form.Text = caption;

                string value = (initialValue ?? "").ToLower();

                form.cbAlert.Checked = value.Contains("alert");
                form.cbSide.Checked = value.Contains("side");
                form.cbStatus.Checked = value.Contains("status");
                form.cbPlayer.Checked = value.Contains("player");
                form.cbStation.Checked = value.Contains("station");
                form.cbEnemy.Checked = value.Contains("enemy");
                form.cbFriend.Checked = value.Contains("friend");

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                
                result += form.cbAlert.Checked ? " ALERT" : "";
                result += form.cbSide.Checked ? " SIDE" : "";
                result += form.cbStatus.Checked ? " STATUS" : "";
                result += form.cbPlayer.Checked ? " PLAYER" : "";
                result += form.cbStation.Checked ? " STATION" : "";
                result += form.cbEnemy.Checked ? " ENEMY" : "";
                result += form.cbFriend.Checked ? " FRIEND" : "";
                if (result.Length > 0)
                    result = result.Substring(1);
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
            Close();
        }

        private void DialogCommTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.D1) cbAlert.Checked = !cbAlert.Checked;
            if (e.KeyData == Keys.D2) cbSide.Checked = !cbSide.Checked;
            if (e.KeyData == Keys.D3) cbStatus.Checked = !cbStatus.Checked;
            if (e.KeyData == Keys.D4) cbPlayer.Checked = !cbPlayer.Checked;
            if (e.KeyData == Keys.D5) cbStation.Checked = !cbStation.Checked;
            if (e.KeyData == Keys.D6) cbEnemy.Checked = !cbEnemy.Checked;
            if (e.KeyData == Keys.D7) cbFriend.Checked = !cbFriend.Checked;

            if (e.KeyData == (Keys.D1 | Keys.Shift)) cbAlert.Checked = true;
            if (e.KeyData == (Keys.D2 | Keys.Shift)) cbSide.Checked = true;
            if (e.KeyData == (Keys.D3 | Keys.Shift)) cbStatus.Checked = true;
            if (e.KeyData == (Keys.D4 | Keys.Shift)) cbPlayer.Checked = true;
            if (e.KeyData == (Keys.D5 | Keys.Shift)) cbStation.Checked = true;
            if (e.KeyData == (Keys.D6 | Keys.Shift)) cbEnemy.Checked = true;
            if (e.KeyData == (Keys.D7 | Keys.Shift)) cbFriend.Checked = true;

            if (e.KeyData == (Keys.D1 | Keys.Alt)) cbAlert.Checked = false;
            if (e.KeyData == (Keys.D2 | Keys.Alt)) cbSide.Checked = false;
            if (e.KeyData == (Keys.D3 | Keys.Alt)) cbStatus.Checked = false;
            if (e.KeyData == (Keys.D4 | Keys.Alt)) cbPlayer.Checked = false;
            if (e.KeyData == (Keys.D5 | Keys.Alt)) cbStation.Checked = false;
            if (e.KeyData == (Keys.D6 | Keys.Alt)) cbEnemy.Checked = false;
            if (e.KeyData == (Keys.D7 | Keys.Alt)) cbFriend.Checked = false;

            if (e.KeyData == (Keys.A | Keys.Shift) || e.KeyData == (Keys.Oemtilde | Keys.Shift))
            {
                cbAlert.Checked = true;
                cbSide.Checked = true;
                cbStatus.Checked = true;
                cbPlayer.Checked = true;
                cbStation.Checked = true;
                cbEnemy.Checked = true;
                cbFriend.Checked = true;
            }

            if (e.KeyData == (Keys.A | Keys.Alt) || e.KeyData == (Keys.Oemtilde | Keys.Alt))
            {
                cbAlert.Checked = false;
                cbSide.Checked = false;
                cbStatus.Checked = false;
                cbPlayer.Checked = false;
                cbStation.Checked = false;
                cbEnemy.Checked = false;
                cbFriend.Checked = false;
            }

            if (e.KeyData == (Keys.A | Keys.Control) || e.KeyData == Keys.A
                 || e.KeyData == (Keys.Oemtilde | Keys.Control) || e.KeyData == Keys.Oemtilde)
            {
                if (cbAlert.Checked)
                {
                    cbAlert.Checked = false;
                    cbSide.Checked = false;
                    cbStatus.Checked = false;
                    cbPlayer.Checked = false;
                    cbStation.Checked = false;
                    cbEnemy.Checked = false;
                    cbFriend.Checked = false;
                }
                else
                {
                    cbAlert.Checked = true;
                    cbSide.Checked = true;
                    cbStatus.Checked = true;
                    cbPlayer.Checked = true;
                    cbStation.Checked = true;
                    cbEnemy.Checked = true;
                    cbFriend.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use hotkeys to edit the list of types:\r\n"+
                "Use 1234567 keys on keyboard to toggle the state.\r\n"+
                " + Shift to set the state to ON.\r\n"+
                " + Alt to set the state to OFF.\r\n"+
                "Use A (optionally with Ctrl) to toggle the state of all types.\r\n"+
                " + Shift to set the state of all types to ON.\r\n"+
                " + Alt to set the state of all types to OFF.", 
                "How to use");
        } 
    }
}
