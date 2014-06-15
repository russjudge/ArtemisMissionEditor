using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public partial class _FormHelp : ArtemisMissionEditor.FormSerializeableToRegistry
	{
		public _FormHelp()
		{
			InitializeComponent();
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
			if (!Program.IsClosing)
			{
				e.Cancel = true;
				this.Hide();
			}

			SaveToRegistry();
		}

		private void _FormHelp_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}
	}
}
