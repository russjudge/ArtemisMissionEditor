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
	public partial class SettingsControl : UserControl
	{
		private EngineeringSettings _engineeringSettings;
		public EngineeringSettings CurrentSetting
		{
			get
			{
				return _engineeringSettings;
			}
			set
			{
				_engineeringSettings = value;
				UpdateDisplayedData();
			}
		}

		public SettingsControl()
		{
			InitializeComponent();

			presetControl1.PresetID = 1;
			presetControl1.SettingsControl = this;
			presetControl2.PresetID = 2;
			presetControl2.SettingsControl = this;
			presetControl3.PresetID = 3;
			presetControl3.SettingsControl = this;
			presetControl4.PresetID = 4;
			presetControl4.SettingsControl = this;
			presetControl5.PresetID = 5;
			presetControl5.SettingsControl = this;
			presetControl6.PresetID = 6;
			presetControl6.SettingsControl = this;
			presetControl7.PresetID = 7;
			presetControl7.SettingsControl = this;
			presetControl8.PresetID = 8;
			presetControl8.SettingsControl = this;
			presetControl9.PresetID = 9;
			presetControl9.SettingsControl = this;
			presetControl0.PresetID = 0;
			presetControl0.SettingsControl = this;
			
		}

		/// <summary>
		/// Force a refresh of all the data in the control
		/// </summary>
		public void UpdateDisplayedData()
		{
			if (Enabled != (CurrentSetting != null))
				Enabled = !Enabled;

			presetControl1.UpdateDisplayedData();
			presetControl2.UpdateDisplayedData();
			presetControl3.UpdateDisplayedData();
			presetControl4.UpdateDisplayedData();
			presetControl5.UpdateDisplayedData();
			presetControl6.UpdateDisplayedData();
			presetControl7.UpdateDisplayedData();
			presetControl8.UpdateDisplayedData();
			presetControl9.UpdateDisplayedData();
			presetControl0.UpdateDisplayedData();
		}


	}
}
