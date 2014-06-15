using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EngineeringTemplateMaster
{
	public partial class FormStoreDialog : Form
	{
		public FormStoreDialog()
		{
			InitializeComponent();
		}
		
		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter && button1.Enabled)
			{
				e.SuppressKeyPress = true;
				DialogResult = DialogResult.OK;
			}
			if (e.KeyData == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				DialogResult = DialogResult.Cancel;
			}
		}

		private void input_TextChanged(object sender, EventArgs e)
		{
			if (button1.Enabled != Valid())
				button1.Enabled = !button1.Enabled;
		}

		private bool Valid()
		{
			return !(input.Text.Contains('\\')
				|| input.Text.Contains('/')
				|| input.Text.Contains(':')
				|| input.Text.Contains('*')
				|| input.Text.Contains('?')
				|| input.Text.Contains('"')
				|| input.Text.Contains('<')
				|| input.Text.Contains('>')
				|| input.Text.Contains('|')
				|| string.IsNullOrWhiteSpace(input.Text));
		}
	}
}
