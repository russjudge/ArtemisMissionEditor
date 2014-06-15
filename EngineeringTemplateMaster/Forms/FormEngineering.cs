using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace EngineeringTemplateMaster
{
    public partial class FormEngineering : Form
    {
        #region Assembly Title and Version

        public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.Major.ToString("00")
					+ Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString("00")
					+ Assembly.GetExecutingAssembly().GetName().Version.Build.ToString("00")
					+ "." + Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString("0");

			}
		}
		
#endregion

        private static string _mainFormName = AssemblyTitle + " v" + AssemblyVersion;

		//Must contain invalid symbols like * or < or / or | etc, so that no file name matches this name
		public static string EngineeringSettingsCurrentName = "\t< Artemis Setting >";

		public FormEngineering()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Text = _mainFormName;

			UpdateFileList();

			settingsControl1.UpdateDisplayedData();
		}

		private void UpdateFileList()
		{
			if (!Directory.Exists("Presets"))
				Directory.CreateDirectory("Presets");
			string[] files = Directory.GetFiles("Presets", "*.dat");
			fileList.Items.Clear();
			settingsControl1.CurrentSetting = null;

			EngineeringSettings es = new EngineeringSettings();
            if (!File.Exists(Program.ArtemisPath + @"engineeringSettings.dat") || !es.LoadFromFile(Program.ArtemisPath + @"engineeringSettings.dat"))
                es.SaveToFile(Program.ArtemisPath + @"engineeringSettings.dat");
			es.ListBox = fileList;
            if (es.LoadFromFile(Program.ArtemisPath + @"engineeringSettings.dat"))
			{
				es.Name = EngineeringSettingsCurrentName;
				fileList.Items.Add(es);
				fileList.SelectedIndex = 0;
			}

			foreach (string file in files)
			{
				es = new EngineeringSettings();
				es.ListBox = fileList;
				if (es.LoadFromFile(file))
					fileList.Items.Add(es);
			}
		}

		private void fileList_SelectedValueChanged(object sender, EventArgs e)
		{
			settingsControl1.CurrentSetting = (EngineeringSettings)fileList.SelectedItem;
		}

		private void resetCurrentPresetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (settingsControl1.CurrentSetting == null)
				return;
			for (int i = 0; i < 10; i++)
			{
				settingsControl1.CurrentSetting.Presets[i].ResetCoolant();
				settingsControl1.CurrentSetting.Presets[i].ResetEnergy();
			}
			settingsControl1.UpdateDisplayedData();
		}

		private void reloadListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateFileList();
		}

		private void applySelectedPresetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ApplySelected();
		}


		private void applyStartArtemisToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ApplySelectedAndGo();
		}

		private bool ApplySelected(bool showErrorMessages = true)
		{
			if (settingsControl1.CurrentSetting == null)
				return false;

            if (!settingsControl1.CurrentSetting.SaveToFile(Program.ArtemisPath + @"engineeringSettings.dat"))
			{
				if (showErrorMessages) 
					MessageBox.Show("Failed to save engineeringSettings.dat!");
				return false;
			}

			EngineeringSettings es = new EngineeringSettings();
			es.ListBox = fileList;
            es.LoadFromFile(Program.ArtemisPath + @"engineeringSettings.dat");
			es.Name = EngineeringSettingsCurrentName;

			if (((EngineeringSettings)fileList.Items[0]).Name == EngineeringSettingsCurrentName)
				fileList.Items[0] = es;
			else
				fileList.Items.Insert(0, es);

			fileList.SelectedIndex = 0;

			return true;
		}

		private void ApplySelectedAndGo()
		{
			if (ApplySelected(false))
			{
				Program.StartArtemis = true;
				Close();
			}
		}

		private void storeAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StoreAs();
		}

		private void applyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ApplySelected();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveCurrent();
		}

		private void applyGoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ApplySelectedAndGo();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormEngineering_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys.S | Keys.Control))
			{
				e.SuppressKeyPress = true;
				SaveCurrent();
			}
			if (e.KeyData == (Keys.S | Keys.Control | Keys.Alt))
			{
				e.SuppressKeyPress = true;
				StoreAs();
			}
			if (e.KeyData == (Keys.Enter | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				ApplySelected();
			}
			if (e.KeyData == (Keys.Enter | Keys.Control | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				ApplySelectedAndGo();
			}
		}

		private void SaveCurrent()
		{
			if (settingsControl1.CurrentSetting == null)
				return;
			if (!settingsControl1.CurrentSetting.ChangesPending)
				return;
            if (settingsControl1.CurrentSetting.SaveToFile(settingsControl1.CurrentSetting.Name == EngineeringSettingsCurrentName ? Program.ArtemisPath + @"engineeringSettings.dat" : "Presets\\" + settingsControl1.CurrentSetting.Name + ".dat"))
				settingsControl1.CurrentSetting.ChangesPending = false;
		}

		private void StoreAs()
		{
			if (settingsControl1.CurrentSetting == null)
				return;
			FormStoreDialog FSD = new FormStoreDialog();
			FSD.input.Text = settingsControl1.CurrentSetting.Name == EngineeringSettingsCurrentName ? "" : settingsControl1.CurrentSetting.Name;
			FSD.ShowDialog();
			if (FSD.DialogResult != DialogResult.OK)
				return;

			settingsControl1.CurrentSetting.SaveToFile("Presets\\" + FSD.input.Text + ".dat");

			EngineeringSettings es = new EngineeringSettings();
			es.ListBox = fileList;
			es.LoadFromFile("Presets\\" + FSD.input.Text + ".dat");

			int found = -1;

			for (int i = 0; i < fileList.Items.Count; i++)
			{
				if (((EngineeringSettings)fileList.Items[i]).Name == FSD.input.Text)
				{
					fileList.Items[i] = es;
					found = i;
				}
			}
			if (found == -1)
			{
				fileList.Items.Add(es);
				found = fileList.Items.Count - 1;
			}

			fileList.SelectedIndex = found;
		}
	}
}
