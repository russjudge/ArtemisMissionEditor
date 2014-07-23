using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public partial class FormSearchResults : FormSerializeableToRegistry
    {
        public bool Empty;

		private MissionSearchStructure MissionSearchStructureInstance;

        public FormSearchResults()
        {
            InitializeComponent();
			
			UpdateState();
        }

		public void SetSubtitle(MissionSearchStructure mss)
		{
			MissionSearchStructureInstance = mss;

			UpdateState();
		}

        public void UpdateState()
        {
			string text = (MissionSearchStructureInstance.replacement == null ? "Looked for \"" + MissionSearchStructureInstance.input + "\" in " : "Replaced \"" + MissionSearchStructureInstance.input + "\" with \"" + MissionSearchStructureInstance.replacement + "\" in ")
				+ (MissionSearchStructureInstance.xmlAttName ? "[Xml attribute name], " : "")
				+ (MissionSearchStructureInstance.xmlAttValue ? "[Xml attribute value" + (string.IsNullOrWhiteSpace(MissionSearchStructureInstance.attName) ? "" : " (in att name \"" + MissionSearchStructureInstance.attName + "\")") + "], " : "")
				+ (MissionSearchStructureInstance.nodeNames ? "[Mission node name], " : "")
				+ (MissionSearchStructureInstance.commentaries ? "[Commentary], " : "")
				+ (MissionSearchStructureInstance.statementText ? "[Displayed text], " : "")
				+ (MissionSearchStructureInstance.matchCase ? "Matching case and " : "")
				+ (MissionSearchStructureInstance.matchExact ? "Matching value exactly and " : "")
				+ (MissionSearchStructureInstance.onlyInCurrentNode ? "Only in selected node and " : "");

			if (text.Substring(text.Length - 2, 2) == ", ")
				text = text.Substring(0, text.Length - 2);
			if (text.Substring(text.Length - 5, 5) == " and ")
				text = text.Substring(0, text.Length - 5);

			_FSR_ss_Main_l_Main.Text = text;

            _FSR_ss_Main_tsb_Clear.Enabled = _FSR_lb_Main.Items.Count > 0;
			Text = MissionSearchStructureInstance.replacement == null ? "Find Results: Total " + _FSR_lb_Main.Items.Count.ToString() + " matches" : "Replace Results: Total " + _FSR_lb_Main.Items.Count.ToString() + " items";
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
				Program.FormMainInstance.BringToFront();
				Program.FormSearchResultsInstance.BringToFront();
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
