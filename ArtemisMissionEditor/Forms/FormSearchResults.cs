using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public partial class _FormSearchResults : FormSerializeableToRegistry
    {
        public bool Empty;

		private MissionSearchStructure _mss;

        public _FormSearchResults()
        {
            InitializeComponent();
			
			UpdateState();
        }

		public void SetSubtitle(MissionSearchStructure mss)
		{
			_mss = mss;

			UpdateState();
		}

        public void UpdateState()
        {
			string text = (_mss.replacement == null ? "Looked for \"" + _mss.input + "\" in " : "Replaced \"" + _mss.input + "\" with \"" + _mss.replacement + "\" in ")
				+ (_mss.xmlAttName ? "[Xml attribute name], " : "")
				+ (_mss.xmlAttValue ? "[Xml attribute value" + (string.IsNullOrWhiteSpace(_mss.attName) ? "" : " (in att name \"" + _mss.attName + "\")") + "], " : "")
				+ (_mss.nodeNames ? "[Mission node name], " : "")
				+ (_mss.commentaries ? "[Commentary], " : "")
				+ (_mss.statementText ? "[Displayed text], " : "")
				+ (_mss.matchCase ? "Matching case and " : "")
				+ (_mss.matchExact ? "Matching value exactly and " : "")
				+ (_mss.onlyInCurrentNode ? "Only in selected node and " : "");

			if (text.Substring(text.Length - 2, 2) == ", ")
				text = text.Substring(0, text.Length - 2);
			if (text.Substring(text.Length - 5, 5) == " and ")
				text = text.Substring(0, text.Length - 5);

			_FSR_ss_Main_l_Main.Text = text;

            _FSR_ss_Main_tsb_Clear.Enabled = _FSR_lb_Main.Items.Count > 0;
			Text = _mss.replacement == null ? "Find Results: Total " + _FSR_lb_Main.Items.Count.ToString() + " matches" : "Replace Results: Total " + _FSR_lb_Main.Items.Count.ToString() + " items";
            Empty = !_FSR_ss_Main_tsb_Clear.Enabled;

			if (Empty)
				Hide();
        }

		public void ClearList()
		{
			_FSR_lb_Main.Items.Clear();

			Mission.Current.SetSelection();

			UpdateState();
		}

		public void SetList(List<MissionSearchResult> list)
		{
			_FSR_lb_Main.Items.Clear();

			foreach (MissionSearchResult item in list)
				_FSR_lb_Main.Items.Add(item);

			Mission.Current.SetSelection(list);

			UpdateState();
		}

        private void _E_FSR_Load(object sender, EventArgs e)
        {
            _mainSC = null;
            _rightSC = null;
            ID = "FormSearchResults";
            LoadFromRegistry();

            UpdateState();
        }

        private void _E_FSR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.IsClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void _E_FSR_ss_Main_tsb_Clear_ButtonClick(object sender, EventArgs e)
        {
			ClearList();
        }

        private void _E_FSR_lb_Main_DoubleClick(object sender, EventArgs e)
        {
			if (_FSR_lb_Main.SelectedItem != null)
			{
				((MissionSearchResult)_FSR_lb_Main.SelectedItem).Activate();
				Program.FM.BringToFront();
				Program.FSR.BringToFront();
			}
        }

		private void _E_FSR_lb_Main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				_E_FSR_lb_Main_DoubleClick(sender, e);
		}

		private void _E_FSR_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				Close();
			}

			if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
			{
				e.SuppressKeyPress = true;
				Mission.Current.ShowFindForm();
			}
			if (e.KeyCode == Keys.H && Control.ModifierKeys == Keys.Control)
			{
				e.SuppressKeyPress = true;
				Mission.Current.ShowReplaceForm();
			}
		}
    }
}
