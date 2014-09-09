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
    public partial class FormNotepad : FormSerializeableToRegistry
    {
        public FormNotepad()
        {
            InitializeComponent();
        }

        public void ShowText(string text, string caption)
        {
            Text = caption;
            doubleBufferedTextBox1.Text = text;
            // Stupid fucking Miscrosoft. 
            // If I don't select different control before show, then even if I DeselectAll, it will still be selected!
            // Not to mention I have to DeselectAll in the first place, 
            // because the text gets selected for NO REASON if it's the focused control when the form is shown
            buttonOK.Select();
            Show();
            BringToFront();
            doubleBufferedTextBox1.Select();
            doubleBufferedTextBox1.DeselectAll();
            
        }

        private void FormNotepad_Load(object sender, EventArgs e)
        {
            _mainSC = null;
            _rightSC = null;
            ID = "FormNotepad";
            LoadFromRegistry();
        }

        private void FormNotepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
                SaveToRegistry();
        }

        private void FormNotepad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                Hide();
        }

        private void doubleBufferedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FormNotepad_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                SaveToRegistry();
                Program.ShowMainFormIfRequired();
            }
        }
    }
}
