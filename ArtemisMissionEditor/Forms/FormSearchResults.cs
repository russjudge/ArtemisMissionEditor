using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace ArtemisMissionEditor
{
    public partial class FormSearchResults : FormSerializeableToRegistry
    {
        public bool Empty;

        public MissionSearchCommand MissionSearchCommandInstance { get; private set; }

        public FormSearchResults()
        {
            InitializeComponent();
			
			UpdateState();
        }

        public void ShowIfNotEmpty(bool playSoundOnFailure = false)
        {
            if (Empty)
            {
                if (playSoundOnFailure)
                    SystemSounds.Beep.Play();
                return;
            }
            Show();
            BringToFront();
        }

        /// <summary>  Set subtitle from a Mission Search Command class </summary>
		public void SetSubtitle(MissionSearchCommand msc)
		{
			MissionSearchCommandInstance = msc;
            UpdateState();
		}

        public void SetSubtitle(string text)
        {
            _FSR_ss_Main_l_Main.Text = text;
            Text = "Find problems";

            MissionSearchCommandInstance= null;
            UpdateState();
        }

        public void UpdateState()
        {
            if (MissionSearchCommandInstance != null)
            {
                string text = (MissionSearchCommandInstance.Replacement == null ? "Looked for \"" + MissionSearchCommandInstance.Input + "\" in " : "Replaced \"" + MissionSearchCommandInstance.Input + "\" with \"" + MissionSearchCommandInstance.Replacement + "\" in ")
                    + (MissionSearchCommandInstance.XmlAttName ? "[Xml attribute name], " : "")
                    + (MissionSearchCommandInstance.XmlAttValue ? "[Xml attribute value" + (string.IsNullOrWhiteSpace(MissionSearchCommandInstance.AttName) ? "" : " (in att name \"" + MissionSearchCommandInstance.AttName + "\")") + "], " : "")
                    + (MissionSearchCommandInstance.NodeNames ? "[Mission node name], " : "")
                    + (MissionSearchCommandInstance.Commentaries ? "[Commentary], " : "")
                    + (MissionSearchCommandInstance.StatementText ? "[Displayed text], " : "")
                    + (MissionSearchCommandInstance.MatchCase ? "Matching case and " : "")
                    + (MissionSearchCommandInstance.MatchExact ? "Matching value exactly and " : "")
                    + (MissionSearchCommandInstance.OnlyInCurrentNode ? "Only in selected node and " : "");

                if (text.Substring(text.Length - 2, 2) == ", ")
                    text = text.Substring(0, text.Length - 2);
                if (text.Substring(text.Length - 5, 5) == " and ")
                    text = text.Substring(0, text.Length - 5);

                _FSR_ss_Main_l_Main.Text = text;

                Text = MissionSearchCommandInstance.Replacement == null ? "Find Results: Total " + _FSR_lv_Main.Rows.Count.ToString() + " matches" : "Replace Results: Total " + _FSR_lv_Main.Rows.Count.ToString() + " items";
                _FSR_ss_Main_tsb_Update.Visible= false;
            }
            else
            {
                Text = "Find potential problems: " + _FSR_lv_Main.Rows.Count.ToString() + " matches";
                _FSR_ss_Main_tsb_Update.Visible = true;
            }

            _FSR_ss_Main_tsb_Clear.Enabled = _FSR_lv_Main.Rows.Count > 0;
            Empty = !_FSR_ss_Main_tsb_Clear.Enabled;

			if (Empty)
				Hide();
        }

		public void ClearList()
		{
			_FSR_lv_Main.Rows.Clear();

			Mission.Current.SetSelection();

			UpdateState();
		}

		public void SetList(List<MissionSearchResult> list)
		{
            _FSR_lv_Main.SuspendLayout();
            _FSR_lv_Main.Rows.Clear();
            foreach (MissionSearchResult item in list)
                AddItemToList(item);
            _FSR_lv_Main.ResumeLayout();

			Mission.Current.SetSelection(list);

			UpdateState();
		}

        private void AddItemToList(MissionSearchResult msr)
        {
            DataGridViewRow dgvr = _FSR_lv_Main.Rows[_FSR_lv_Main.Rows.Add()];
            dgvr.Tag = msr;
            dgvr.Cells[0].Value = msr.CurNode > 0 ? (int?)msr.CurNode : null;
            dgvr.Cells[1].Value = msr.CurStatement > 0 ? (int?)msr.CurStatement : null;
            dgvr.Cells[2].Value = msr.NodeText;
            dgvr.Cells[3].Value = msr.Text;
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
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                Program.ShowMainFormIfRequired();
            }

            SaveToRegistry();
        }

        private void _E_FSR_ss_Main_tsb_Clear_ButtonClick(object sender, EventArgs e)
        {
			ClearList();
        }

        private void _E_FSR_lb_Main_DoubleClick(object sender, EventArgs e)
        {
			if (_FSR_lv_Main.SelectedRows.Count != 0)
			{
                ((MissionSearchResult)_FSR_lv_Main.SelectedRows[0].Tag).Activate();
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

        public void FindProblems(bool hide = true)
        {
            if (hide) Hide();
            List<MissionSearchResult> result = Mission.Current.FindProblems();

            SetSubtitle("");
            SetList(result);
            ShowIfNotEmpty(true);
        }

        private void _FSR_ss_Main_tsb_Update_ButtonClick(object sender, EventArgs e)
        {
            FindProblems(false);
        }
    }
}
