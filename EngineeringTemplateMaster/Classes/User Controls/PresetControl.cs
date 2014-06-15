using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EngineeringTemplateMaster
{
	public partial class PresetControl : UserControl
	{
		private int _presetID;
		[DisplayName("Preset ID"), Description("Keyboard key associated with this preset")]
		public int PresetID {
			get
			{
				return _presetID;
			}
			set
			{
				_presetID = value;
				label1.Text = value.ToString();
			}
		}

		public SettingsControl SettingsControl { get; set; }

		private bool _childHasFocus;
		public bool ChildHasFocus {
			get
			{
				return _childHasFocus;
			}
			set
			{
				_childHasFocus = value;
				UpdateLabel();
			}
		}

		public PresetControl()
		{
			InitializeComponent();

			shipSystemControl0.SystemID = 0;
			shipSystemControl0.PresetControl = this;
			shipSystemControl1.SystemID = 1;
			shipSystemControl1.PresetControl = this;
			shipSystemControl2.SystemID = 2;
			shipSystemControl2.PresetControl = this;
			shipSystemControl3.SystemID = 3;
			shipSystemControl3.PresetControl = this;
			shipSystemControl4.SystemID = 4;
			shipSystemControl4.PresetControl = this;
			shipSystemControl5.SystemID = 5;
			shipSystemControl5.PresetControl = this;
			shipSystemControl6.SystemID = 6;
			shipSystemControl6.PresetControl = this;
			shipSystemControl7.SystemID = 7;
			shipSystemControl7.PresetControl = this;

		}

		public void UpdateLabel()
		{
			if (ChildHasFocus)
			{
				label1.ForeColor = Color.Red;
			}
			else
			{
				label1.ForeColor = Color.Black;
			}
		}

        /// <summary>
        /// Force update of data displayed on the control
        /// </summary>
        public void UpdateDisplayedData()
		{
			shipSystemControl0.UpdateDisplayedData();
			shipSystemControl1.UpdateDisplayedData();
			shipSystemControl2.UpdateDisplayedData();
			shipSystemControl3.UpdateDisplayedData();
			shipSystemControl4.UpdateDisplayedData();
			shipSystemControl5.UpdateDisplayedData();
			shipSystemControl6.UpdateDisplayedData();
			shipSystemControl7.UpdateDisplayedData();
		}

		private void resetEnergyLevelsTo100ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetEnergy();
		}

		private void resetCoolantLevelsTo0ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetCoolant();
		}

		private void resetBothToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetEnergy();
			ResetCoolant();
		}

		public void ResetEnergy(int value = 100)
		{
			if (SettingsControl.CurrentSetting == null)
				return;

			SettingsControl.CurrentSetting.Presets[PresetID].ResetEnergy(value);
			SettingsControl.UpdateDisplayedData();
		}

		public void ResetCoolant()
		{
			if (SettingsControl.CurrentSetting == null)
				return;

			SettingsControl.CurrentSetting.Presets[PresetID].ResetCoolant();
			SettingsControl.UpdateDisplayedData();
		}

		private void label1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(PointToScreen(e.Location));
		}

		private void resetEnergyTo0ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetEnergy(0);
		}

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                label1.DoDragDrop(label1.Text, DragDropEffects.Copy);
        }

        private void label1_DragOver(object sender, DragEventArgs e)
        {
            int tmp;
            e.Effect = int.TryParse((string)e.Data.GetData(typeof(string)),out tmp) && (string)e.Data.GetData(typeof(string)) != label1.Text ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            int sourceID;
            if (!int.TryParse((string)e.Data.GetData(typeof(string)), out sourceID))
                return;

            SettingsControl.CurrentSetting.CopyPreset(sourceID, PresetID);
        }

        #region Preset Copying from CMS

        private void allPresetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i.ToString() == label1.Text)
                    continue;
                SettingsControl.CurrentSetting.CopyPreset(PresetID, i);
            }
        }

        private void CopyPreset(int target)
        {
            if (target.ToString() == label1.Text)
                return;
            SettingsControl.CurrentSetting.CopyPreset(PresetID, target);
        }

        private void preset1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(1);   
        }

        private void preset2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(2);
        }

        private void preset3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(3);
        }

        private void preset4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(4);
        }

        private void preset5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(5);
        }

        private void preset6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(6);
        }

        private void preset7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(7);
        }

        private void preset8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(8);
        }

        private void preset9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(9);
        }

        private void preset0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPreset(0);
        }

        #endregion

        private void copyPresetToToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            preset0ToolStripMenuItem.Enabled = 0.ToString() != label1.Text;
            preset1ToolStripMenuItem.Enabled = 1.ToString() != label1.Text;
            preset2ToolStripMenuItem.Enabled = 2.ToString() != label1.Text;
            preset3ToolStripMenuItem.Enabled = 3.ToString() != label1.Text;
            preset4ToolStripMenuItem.Enabled = 4.ToString() != label1.Text;
            preset5ToolStripMenuItem.Enabled = 5.ToString() != label1.Text;
            preset6ToolStripMenuItem.Enabled = 6.ToString() != label1.Text;
            preset7ToolStripMenuItem.Enabled = 7.ToString() != label1.Text;
            preset8ToolStripMenuItem.Enabled = 8.ToString() != label1.Text;
            preset9ToolStripMenuItem.Enabled = 9.ToString() != label1.Text;
        }

        private void AutoCoolant(int max)
        {
            SettingsControl.CurrentSetting.Presets[PresetID].AutoCoolant(max);
        }

        private void CapCoolant(int max)
        {
            SettingsControl.CurrentSetting.Presets[PresetID].CapCoolant(max);
        }

        private void automaticallyMax8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoCoolant(8);
        }

        private void automaticallyMax10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoCoolant(10);
        }

        private void automaticallyMax12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoCoolant(12);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CapCoolant(8);
        }

        private void leaveAtMost10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CapCoolant(10);
        }

        private void leaveAtMost12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CapCoolant(12);
        }

		
	}
}
