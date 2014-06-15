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
	public partial class _FormFindReplace : FormSerializeableToRegistry
	{
		public _FormFindReplace()
		{
			InitializeComponent();

            UpdateComponents();
		}

        public void UpdateComponents()
        {
            _E_FFR_cb_XmlValues_CheckedChanged(null, null);
        }

        private void EnsureChecked()
		{
            _FFR_cb_NodeNamesFind.Checked = _FFR_cb_NodeNamesFind.Checked || !(_FFR_cb_XmlNames.Checked || _FFR_cb_XmlValuesFind.Checked || _FFR_cb_NodeNamesFind.Checked || _FFR_cb_CommentariesFind.Checked || _FFR_cb_DisplayedText.Checked);
		}

		public void FindNext()
		{
			if (_FFR_tb_Input.Text.Length == 0)
				return;

			EnsureChecked();

            List<MissionSearchResult> result = Mission.Current.FindAll(new MissionSearchStructure(_FFR_tb_Input.Text, null, _FFR_cb_XmlNames.Checked, _FFR_cb_XmlValuesFind.Checked, _FFR_cob_AttNameFind.Text, _FFR_cb_NodeNamesFind.Checked, _FFR_cb_CommentariesFind.Checked, _FFR_cb_DisplayedText.Checked, _FFR_cb_CaseFind.Checked, _FFR_cb_ExactFind.Checked, _FFR_cb_SelectedNodeFind.Checked), true, true);

            if (result.Count > 0)
                result[0].Activate();
            else
                SystemSounds.Beep.Play();
		}
        
		public void FindPrevious()
		{
			if (_FFR_tb_Input.Text.Length == 0)
				return; 
			
			EnsureChecked();

			List<MissionSearchResult> result = Mission.Current.FindAll(new MissionSearchStructure(_FFR_tb_Input.Text, null, _FFR_cb_XmlNames.Checked, _FFR_cb_XmlValuesFind.Checked, _FFR_cob_AttNameFind.Text, _FFR_cb_NodeNamesFind.Checked, _FFR_cb_CommentariesFind.Checked, _FFR_cb_DisplayedText.Checked, _FFR_cb_CaseFind.Checked, _FFR_cb_ExactFind.Checked, _FFR_cb_SelectedNodeFind.Checked), false, true);

            if (result.Count > 0)
                result[0].Activate();
            else
                SystemSounds.Beep.Play();
		}
        
        public void FindAll()
        {
			if (_FFR_tb_Input.Text.Length == 0)
				return;

			EnsureChecked();

			MissionSearchStructure mss = new MissionSearchStructure(_FFR_tb_Input.Text, null, _FFR_cb_XmlNames.Checked, _FFR_cb_XmlValuesFind.Checked, _FFR_cob_AttNameFind.Text, _FFR_cb_NodeNamesFind.Checked, _FFR_cb_CommentariesFind.Checked, _FFR_cb_DisplayedText.Checked, _FFR_cb_CaseFind.Checked, _FFR_cb_ExactFind.Checked, _FFR_cb_SelectedNodeFind.Checked);
			List<MissionSearchResult> result = Mission.Current.FindAll(mss);

			Program.FSR.SetSubtitle(mss);
			Program.FSR.SetList(result);

            ShowSearchResultsForm();
        }
        
        public void ReplaceFindNext()
        {
			if (_FFR_tb_ReplaceWhat.Text.Length == 0)
				return;

			List<MissionSearchResult> result = Mission.Current.FindAll(new MissionSearchStructure(_FFR_tb_ReplaceWhat.Text, null, false, _FFR_cb_XmlValuesReplace.Checked, _FFR_cob_AttNameReplace.Text, _FFR_cb_NodeNamesReplace.Checked, _FFR_cb_CommentariesReplace.Checked, false, _FFR_cb_CaseReplace.Checked, _FFR_cb_ExactReplace.Checked, _FFR_cb_SelectedNodeReplace.Checked), true, true);

            if (result.Count > 0)
                result[0].Activate();
            else
                SystemSounds.Beep.Play();
        }
        
		public void ReplaceFindPrevious()
		{
			if (_FFR_tb_ReplaceWhat.Text.Length == 0)
				return;

			List<MissionSearchResult> result = Mission.Current.FindAll(new MissionSearchStructure(_FFR_tb_ReplaceWhat.Text, null, false, _FFR_cb_XmlValuesReplace.Checked, _FFR_cob_AttNameReplace.Text, _FFR_cb_NodeNamesReplace.Checked, _FFR_cb_CommentariesReplace.Checked, false, _FFR_cb_CaseReplace.Checked, _FFR_cb_ExactReplace.Checked, _FFR_cb_SelectedNodeReplace.Checked), false, true);

			if (result.Count > 0)
				result[0].Activate();
			else
				SystemSounds.Beep.Play();
		}
        
        public void ReplaceNext()
        {
			if (_FFR_tb_ReplaceWhat.Text.Length == 0)
				return;

			MissionSearchStructure mss = new MissionSearchStructure(_FFR_tb_ReplaceWhat.Text, _FFR_tb_ReplaceWith.Text, false, _FFR_cb_XmlValuesReplace.Checked, _FFR_cob_AttNameReplace.Text, _FFR_cb_NodeNamesReplace.Checked, _FFR_cb_CommentariesReplace.Checked, false, _FFR_cb_CaseReplace.Checked, _FFR_cb_ExactReplace.Checked, _FFR_cb_SelectedNodeReplace.Checked);
            
            if (!Mission.Current.Find_DoesCurrentSelectionMatch(mss))
                ReplaceFindNext();

            int result = Mission.Current.ReplaceCurrent(mss);

            Log.Add("Replaced '" + mss.input + "' with '" + _FFR_tb_ReplaceWith.Text + "' " + result.ToString() + " time(s).");

            if (result == 0)
                SystemSounds.Beep.Play();
        }
        
		public void ReplacePrevious()
		{
			if (_FFR_tb_ReplaceWhat.Text.Length == 0)
				return;

			MissionSearchStructure mss = new MissionSearchStructure(_FFR_tb_ReplaceWhat.Text, _FFR_tb_ReplaceWith.Text, false, _FFR_cb_XmlValuesReplace.Checked, _FFR_cob_AttNameReplace.Text, _FFR_cb_NodeNamesReplace.Checked, _FFR_cb_CommentariesReplace.Checked, false, _FFR_cb_CaseReplace.Checked, _FFR_cb_ExactReplace.Checked, _FFR_cb_SelectedNodeReplace.Checked);

			if (!Mission.Current.Find_DoesCurrentSelectionMatch(mss))
				ReplaceFindPrevious();

			int result = Mission.Current.ReplaceCurrent(mss);

			Log.Add("Replaced '" + mss.input + "' with '" + _FFR_tb_ReplaceWith.Text + "' " + result.ToString() + " time(s).");

			if (result == 0)
				SystemSounds.Beep.Play();
		}
        
        public void ReplaceAll()
        {
			if (_FFR_tb_ReplaceWhat.Text.Length == 0)
				return;

			MissionSearchStructure mss = new MissionSearchStructure(_FFR_tb_ReplaceWhat.Text, _FFR_tb_ReplaceWith.Text, false, _FFR_cb_XmlValuesReplace.Checked, _FFR_cob_AttNameReplace.Text, _FFR_cb_NodeNamesReplace.Checked, _FFR_cb_CommentariesReplace.Checked, false, _FFR_cb_CaseReplace.Checked, _FFR_cb_ExactReplace.Checked, _FFR_cb_SelectedNodeReplace.Checked);

			List<MissionSearchResult> list = new List<MissionSearchResult>();
            int result = Mission.Current.ReplaceAll(list, mss);

            Log.Add("Replaced '" + mss.input + "' with '" + _FFR_tb_ReplaceWith.Text + "' " + result.ToString() + " time(s).");

			if (result > 0)
			{
				Program.FSR.SetSubtitle(mss);
				Program.FSR.SetList(list);

				ShowSearchResultsForm();

				SystemSounds.Asterisk.Play();
			}
			else
				SystemSounds.Beep.Play();
        }

        private void ShowSearchResultsForm()
        {
            if (!Program.FSR.Empty)
            {
                Program.FSR.Show();
                Program.FSR.BringToFront();
            }
        }

        private bool FindMode()
        {
            return _FFR_tc_Main.SelectedTab == Program.FFR._FFR_tc_Main.TabPages[0];
        }

        private bool ReplaceMode()
        {
            return _FFR_tc_Main.SelectedTab == Program.FFR._FFR_tc_Main.TabPages[1];
        }

        private void _E_FFR_Load_private_RecursivelyFindAttributeValues(ExpressionMember item, List<string> list)
        {
            foreach (KeyValuePair<string, List<ExpressionMember>> kvp in item.PossibleExpressions)
                foreach (ExpressionMember child in kvp.Value)
                    _E_FFR_Load_private_RecursivelyFindAttributeValues(child, list);

            if (!string.IsNullOrWhiteSpace(item.Name) && !list.Contains(item.Name))
                list.Add(item.Name);
        }

        private List<string> _E_FFR_Load_private_FindAttributeValues()
        {
            List<string> result = new List<string>();

            foreach (ExpressionMember item in Expressions.Root)
                _E_FFR_Load_private_RecursivelyFindAttributeValues(item, result);

            return result;
        }

        private void _E_FFR_Load(object sender, EventArgs e)
        {
            _mainSC = null;
            _rightSC = null;
            ID = "FormFindReplace";
            LoadFromRegistry();

            List<string> attNames = _E_FFR_Load_private_FindAttributeValues();
            attNames.Sort();

            _FFR_cob_AttNameFind.Items.Clear();
            _FFR_cob_AttNameReplace.Items.Clear();
            foreach (string item in attNames)
            {
                _FFR_cob_AttNameFind.Items.Add(item);
                _FFR_cob_AttNameReplace.Items.Add(item);
            }
        }

        private void _E_FFR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.IsClosing)
            {
                e.Cancel = true;
                this.Hide();
                Program.FSR.Hide();
            }

            SaveToRegistry();
        }

        private void _E_FFR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                Close();
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (e.KeyData == Keys.Enter)
                {
                    if (FindMode())
                        FindNext();
                    if (ReplaceMode())
                        ReplaceFindNext();
                }

                if (e.KeyData == (Keys.Enter | Keys.Shift))
                {
                    if (FindMode())
                        FindPrevious();
                    if (ReplaceMode())
                        ReplaceFindPrevious();
                }

                if (e.KeyData == (Keys.Enter | Keys.Control))
                {
                    if (FindMode())
                        FindAll();
                    if (ReplaceMode())
                        ReplaceAll();
                }

                if (e.KeyData == (Keys.Alt | Keys.Enter))
                {
                    if (ReplaceMode())
                        ReplaceNext();
                }

                if (e.KeyData == (Keys.Alt | Keys.Enter | Keys.Shift))
                {
                    if (ReplaceMode())
                        ReplacePrevious();
                }
            }


            if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
            {
                e.SuppressKeyPress = true;
                _FFR_tc_Main.SelectedTab = Program.FFR._FFR_tc_Main.TabPages[0];
            }

            if (e.KeyCode == Keys.H && Control.ModifierKeys == Keys.Control)
            {
                e.SuppressKeyPress = true;
                _FFR_tc_Main.SelectedTab = Program.FFR._FFR_tc_Main.TabPages[1];
            }
        }

        private void _E_FFR_b_Next_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void _E_FFR_b_Previous_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }

        private void _E_FFR_cb_XmlValues_CheckedChanged(object sender, EventArgs e)
        {
            _FFR_cob_AttNameFind.Enabled = _FFR_cb_XmlValuesFind.Checked;
        }

        private void _E_FFR_cob_AttName_Leave(object sender, EventArgs e)
        {
            ((ComboBox)sender).Invalidate();
        }

        private void _E_FFR_b_FindAll_Click(object sender, EventArgs e)
        {
            FindAll();
        }

        private void _E_FFR_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (FindMode())
                MessageBox.Show("You can look for the searched value in the following:\r\n - attribute names (ex. \"type\" in <create/>)\r\n - attribute values (ex. \"enemy\" in \"type\" in <create/>)\r\n    - you can optionally look in attributes with specific names\r\n - mission node names (ex. a name of event or folder)\r\n - commentaries\r\n - displayed text (ex. \"skybox\" in \"Set skybox index...\")\r\n\r\nYou can also:\r\n - match the whole value (ex. \"x\" will not match \"centerX\")\r\n - match the exact case (ex. \"centerx\" will not match \"centerX\")\r\n - look only in the selected event\r\n\r\nYou can use the following hotkeys:\r\n - Find Next: [Enter]\r\n - Find Previous: [Shift]+[Enter]\r\n - Find All: [Ctrl]+[Enter]", "Find");
            if (ReplaceMode())
				MessageBox.Show("You can look for the searched value in the following:\r\n - attribute values (ex. \"enemy\" in \"type\" in <create/>)\r\n    - you can optionally look in attributes with specific names\r\n - mission node names (ex. a name of event or folder)\r\n - commentariesr\n\r\nYou can also:\r\n - match the whole value (ex. \"x\" will not match \"centerX\")\r\n - match the exact case (ex. \"centerx\" will not match \"centerX\")\r\n - look only in the selected event\r\n\r\nYou can use the following hotkeys:\r\n - Find Next: [Enter]\r\n - Find Previous: [Shift]+[Enter]\r\n - Replace Next: [Alt]+[Enter]\r\n - Replace Previous: [Alt]+[Shift]+[Enter]\r\n - Replace All: [Ctrl]+[Enter]", "Replace");
        }

        private void _E_FFR_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                ShowSearchResultsForm();
        }

        private void _E_FFR_Activated(object sender, EventArgs e)
        {
            if (FindMode())
                _FFR_tb_Input.Focus();
            if (ReplaceMode())
                _FFR_tb_ReplaceWhat.Focus();
        }

        private void _E_FFR_b_FindNextInReplace_Click(object sender, EventArgs e)
        {
            ReplaceFindNext();
        }

        private void _E_FFR_b_ReplaceAll_Click(object sender, EventArgs e)
        {
            ReplaceAll();
        }

        private void _E_FFR_b_ReplaceNext_Click(object sender, EventArgs e)
        {
            ReplaceNext();
        }

        private void _E_FFR_tb_ReplaceWhat_TextChanged(object sender, EventArgs e)
        {
            if (_FFR_tb_Input.Text != _FFR_tb_ReplaceWhat.Text)
                _FFR_tb_Input.Text = _FFR_tb_ReplaceWhat.Text;
        }

        private void _E_FFR_tb_Input_TextChanged(object sender, EventArgs e)
        {
            if (_FFR_tb_ReplaceWhat.Text != _FFR_tb_Input.Text)
                _FFR_tb_ReplaceWhat.Text = _FFR_tb_Input.Text;
        }

        private void _E_FFR_tc_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FindMode())
                _FFR_tb_Input.Focus();
            if (ReplaceMode())
                _FFR_tb_ReplaceWhat.Focus();
        }

        private void _E_FFR_b_FindPreviousInReplace_Click(object sender, EventArgs e)
		{
			ReplaceFindPrevious();
		}

		private void _E_FFR_b_ReplacePrevious_Click(object sender, EventArgs e)
		{
			ReplacePrevious();
		}
	}
}
