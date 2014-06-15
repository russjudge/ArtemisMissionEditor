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
	public partial class ShipSystemControl : UserControl
	{
		private int _systemID;
		[DisplayName("System ID"), Description("System ID of this control")]
		public int SystemID
		{
			get
			{
				return _systemID;
			}
			set
			{
				_systemID = value;
				switch (value)
				{
					case 0:
						myLabel1.NewText = "PRI. BEAM";
						break;
					case 1:
						myLabel1.NewText = "TORPEDO";
						break;
					case 2:
						myLabel1.NewText = "SENSORS";
						break;
					case 3:
						myLabel1.NewText = "MANEUVER";
						break;
					case 4:
						myLabel1.NewText = "IMPULSE";
						break;
					case 5:
						myLabel1.NewText = "WARP/JUMP";
						break;
					case 6:
						myLabel1.NewText = "FRONT SHLD";
						break;
					case 7:
						myLabel1.NewText = "REAR SHLD";
						break;
				}
			}
		}

		public PresetControl PresetControl;

		public ShipSystemControl()
		{
			InitializeComponent();

			energy.GotFocus		+= energy_GotFocus;
			energy.LostFocus	+= energy_LostFocus;
			coolant.GotFocus	+= coolant_GotFocus;
			coolant.LostFocus	+= coolant_LostFocus;

			_leftMouseDownCoolant = false;

            UpdateDisplayedData();
		}
		
		#region Coolant

		void coolant_GotFocus(object sender, EventArgs e)
		{
            OutputCoolantText("");
			coolant.SelectAll();
			SetChildGotFocus(true);
		}

		void coolant_LostFocus(object sender, EventArgs e)
		{
			WriteCoolantText(coolant.Text);
			SetChildGotFocus(false);
		}

        /// <summary>
        /// Outputs coolant text and bar to control
        /// </summary>
		public void OutputCoolant()
		{
            OutputCoolantText();
			coolantBar.Invalidate();
		}

        //public void OutputCoolantText(string appendix = "*")
        public void OutputCoolantText(string appendix = "")
        {
            coolant.Text = GetCoolantText() + appendix;
        }

		public string GetCoolantText()
		{
            return GetCoolantValue().ToString();
		}
        
        public byte GetCoolantValue()
        {
            if (PresetControl == null || PresetControl.SettingsControl == null || PresetControl.SettingsControl.CurrentSetting == null)
                return 0;
            return PresetControl.SettingsControl.CurrentSetting.Presets[PresetControl.PresetID].GetCoolantAmount(SystemID);
        }

		public void SetCoolantValue(byte ivalue)
		{
			if (PresetControl == null || PresetControl.SettingsControl == null || PresetControl.SettingsControl.CurrentSetting == null)
				return;
			PresetControl.SettingsControl.CurrentSetting.Presets[PresetControl.PresetID].SetCoolantAmount(SystemID, ivalue);
		}

        public void SetCoolantText(byte ivalue)
        {
            coolant.Text = ivalue.ToString();
            coolant.SelectAll();
        }

		public void WriteCoolantText(string text)
		{
			string svalue = text.Replace("*", "");
			byte ivalue;
			if (byte.TryParse(svalue, out ivalue))
				SetCoolantValue(ivalue);
			OutputCoolant();
		}

		#endregion

		#region Energy

		void energy_GotFocus(object sender, EventArgs e)
		{
            OutputEnergyText("");
			energy.SelectAll();
			SetChildGotFocus(true);
		}

		void energy_LostFocus(object sender, EventArgs e)
		{
			WriteEnergyText(energy.Text);
			SetChildGotFocus(false);
		}

        /// <summary>
        /// Outputs energy text and bar to the control
        /// </summary>
		public void OutputEnergy()
		{
            OutputEnergyText();
            int value = PresetControl.SettingsControl.CurrentSetting.Presets[PresetControl.PresetID].GetEnergyLevel(SystemID);
            energyBar.Value = value < 0 ? 0 : value > 300 ? 300 : value;
		}

        public void OutputEnergyText(string appendix = "%")
        {
            energy.Text = GetEnergyText() + appendix;
        }

        public string GetEnergyText()
        {
            return GetEnergyValue().ToString();
        }

        public int GetEnergyValue()
		{
			if (PresetControl == null || PresetControl.SettingsControl == null || PresetControl.SettingsControl.CurrentSetting == null)
				return 0;
			return PresetControl.SettingsControl.CurrentSetting.Presets[PresetControl.PresetID].GetEnergyLevel(SystemID);
		}

		public void SetEnergyValue(int ivalue)
		{
			if (PresetControl == null || PresetControl.SettingsControl == null || PresetControl.SettingsControl.CurrentSetting == null)
				return;
			PresetControl.SettingsControl.CurrentSetting.Presets[PresetControl.PresetID].SetEnergyLevel(SystemID, ivalue);
		}

        public void SetEnergyText(int ivalue)
        {
            energy.Text = ivalue.ToString();
            energy.SelectAll();
        }

		public void WriteEnergyText(string text)
		{
			string svalue = text.Replace("%", "");
			int ivalue;
			if (int.TryParse(svalue, out ivalue))
				SetEnergyValue(ivalue);
            OutputEnergy();
        }

		#endregion

		/// <summary>
		/// Force update of data displayed on the control
		/// </summary>
        public void UpdateDisplayedData()
		{
            if (PresetControl == null || PresetControl.SettingsControl.CurrentSetting == null)
			{
				energy.Text = "---";
				coolant.Text = "-";
				energyBar.Value = 0;
			}
			else
			{
				OutputEnergy();
				OutputCoolant();
			}
		}

		private void energyBar_ValueChanged()
		{
			if (energyBar.Value.ToString() != GetEnergyText())
			{
				SetEnergyValue(energyBar.Value);
				if (energy.Focused)
					energy_GotFocus(null, null);
				else
					energy.Focus();
			}
		}

        private void energy_ChangeBy(int value)
        {
            int curvalue;
            int.TryParse(energy.Text.Replace("%",""), out curvalue);
            int result = curvalue + value;
            if (curvalue == 100)
            {
                if (value == -10)
                    result = 70;
                else if (value == 10)
                    result = 130;
                else 
                    result = value < 0 && value > -24 ? 76 : value > 0 && value < 24 ? 124 : result;
            }
            SetEnergyText(result < 0 ? result + 300 : result > 300 ? result - 300 : result > 76 && result < 124 ? 100 : result);
        }

		private void energy_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				e.SuppressKeyPress = true;
                OutputEnergyText("");
			}
			if (e.KeyData == Keys.Enter)
			{
				e.SuppressKeyPress = true;
				energy_LostFocus(null, null);
                energy_GotFocus(null, null);
			}
			if (e.KeyData == Keys.Space)
			{
				e.SuppressKeyPress = true;
				ResetEnergy();
			}
			if (e.KeyData == (Keys.Space | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				ResetCoolant();
			}
			if (e.KeyData == (Keys.Space | Keys.Control))
			{
				e.SuppressKeyPress = true;
				ResetCoolant();
				ResetEnergy();
			}

            if ((e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)&& e.Modifiers == Keys.None)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(-1);
            }
            if ((e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) && e.Modifiers == Keys.None)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(1);
            }
            if ((e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) && e.Modifiers == Keys.Shift)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(-10);
            }
            if ((e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) && e.Modifiers == Keys.Shift)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(10);
            }
            if ((e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) && e.Modifiers == Keys.Control)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(-100);
            }
            if ((e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) && e.Modifiers == Keys.Control)
            {
                e.SuppressKeyPress = true;
                energy_ChangeBy(100);
            }

            if (!(
                   e.KeyData == Keys.D0
                || e.KeyData == Keys.D1
                || e.KeyData == Keys.D2
                || e.KeyData == Keys.D3
                || e.KeyData == Keys.D4
                || e.KeyData == Keys.D5
                || e.KeyData == Keys.D6
                || e.KeyData == Keys.D7
                || e.KeyData == Keys.D8
                || e.KeyData == Keys.D9
                || e.KeyData == Keys.NumPad0
                || e.KeyData == Keys.NumPad1
                || e.KeyData == Keys.NumPad2
                || e.KeyData == Keys.NumPad3
                || e.KeyData == Keys.NumPad4
                || e.KeyData == Keys.NumPad5
                || e.KeyData == Keys.NumPad6
                || e.KeyData == Keys.NumPad7
                || e.KeyData == Keys.NumPad8
                || e.KeyData == Keys.NumPad9
                ))
            {
                e.SuppressKeyPress = true;
            }
		}

		private void coolant_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				e.SuppressKeyPress = true;
                OutputCoolantText("");
			}
			if (e.KeyData == Keys.Enter)
			{
				e.SuppressKeyPress = true;
				coolant_LostFocus(null, null);
                coolant_GotFocus(null, null);
			}
			if (e.KeyData == Keys.Space)
			{
				e.SuppressKeyPress = true;
				ResetEnergy();
			}
			if (e.KeyData == (Keys.Space | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				ResetCoolant();
			}
			if (e.KeyData == (Keys.Space | Keys.Control))
			{
				e.SuppressKeyPress = true;
				ResetCoolant();
				ResetEnergy();
			}
            if (e.KeyData == Keys.OemMinus || e.KeyData == Keys.Subtract)
            {
                e.SuppressKeyPress = true;
                byte result;
                if (byte.TryParse(coolant.Text.Replace("*", ""), out result))
                    result = (byte)(result - 1);
                else
                    result = 8;
                SetCoolantText(result > 8 ? (byte)8 : result);
            }
            if (e.KeyData == Keys.Oemplus || e.KeyData == Keys.Add)
            {
                e.SuppressKeyPress = true;
                byte result;
                if (byte.TryParse(coolant.Text.Replace("*", ""), out result))
                    result = (byte)(result + 1);
                else
                    result = 0;
                SetCoolantText(result > 8 ? (byte)0 : result);
            }
            if (e.KeyData == Keys.D0 || e.KeyData == Keys.NumPad0)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(0);
            }
            if (e.KeyData == Keys.D1 || e.KeyData == Keys.NumPad1)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(1);
            }
            if (e.KeyData == Keys.D2 || e.KeyData == Keys.NumPad2)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(2);
            }
            if (e.KeyData == Keys.D3 || e.KeyData == Keys.NumPad3)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(3);
            }
            if (e.KeyData == Keys.D4 || e.KeyData == Keys.NumPad4)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(4);
            }
            if (e.KeyData == Keys.D5 || e.KeyData == Keys.NumPad5)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(5);
            }
            if (e.KeyData == Keys.D6 || e.KeyData == Keys.NumPad6)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(6);
            }
            if (e.KeyData == Keys.D7 || e.KeyData == Keys.NumPad7)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(7);
            }
            if (e.KeyData == Keys.D8 || e.KeyData == Keys.NumPad8)
            {
                e.SuppressKeyPress = true;
                SetCoolantText(8);
            }

            e.SuppressKeyPress = true;
		}

		private void energy_MouseDown(object sender, MouseEventArgs e)
		{
			energy.SelectAll();
		}

		private void coolant_MouseDown(object sender, MouseEventArgs e)
		{
			coolant.SelectAll();
		}

        #region Coolant bar operation

        private bool _leftMouseDownCoolant;

        private void coolantBar_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(coolantBar.BackColor);
			float coolantHeight = (float)coolantBar.Height / 8;
            int coolantCount = GetCoolantValue();
            double requiredCount = EngineeringPreset.GetCoolantNeed(GetEnergyValue());
			int i=0;
			Brush coolantBrush = new SolidBrush(Color.White);
            Brush coolantBrushRed = new SolidBrush(Color.Pink);
            Brush coolantBrushRose = new SolidBrush(Color.FromArgb(255, 224, 224));
            Pen coolantPen = new Pen(Color.Black, 1.0f);
            Pen coolantPenDarkGray = new Pen(Color.DarkGray, 1.0f);
            Pen coolantPenGray = new Pen(Color.LightGray, 1.0f);
            for (i = 1; i <= 8; i++)
			{
				Rectangle rect = new Rectangle(0, (int)Math.Round(coolantBar.Height - i * coolantHeight), coolantBar.Width-1, (int)Math.Round(coolantHeight - 3));
				if (i <= coolantCount)
				{
					e.Graphics.FillRectangle(coolantBrush, rect);
					e.Graphics.DrawRectangle(coolantPen, rect);
				}
                else if (i <= requiredCount)
                {
                    e.Graphics.FillRectangle(coolantBrushRed, rect);
                    e.Graphics.DrawRectangle(coolantPenDarkGray, rect);
                }
                else if (i - requiredCount < 1)
                {
                    e.Graphics.FillRectangle(coolantBrushRose, rect);
                    e.Graphics.DrawRectangle(coolantPenDarkGray, rect);
                }
				else
				{
					e.Graphics.DrawRectangle(coolantPenGray, rect);
				}
			}
		}

		private void coolantBar_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_leftMouseDownCoolant = true;

			if (_leftMouseDownCoolant)
				UpdateCoolantValueBasedOnMousePosition(1.0 - (double)e.Location.Y / this.Height);
		}

		private void coolantBar_MouseMove(object sender, MouseEventArgs e)
		{
			if (_leftMouseDownCoolant)
				UpdateCoolantValueBasedOnMousePosition(1.0 - (double)e.Location.Y / this.Height);
		}

		private void coolantBar_MouseUp(object sender, MouseEventArgs e)
		{
			_leftMouseDownCoolant = false;
		}

		private void UpdateCoolantValueBasedOnMousePosition(double position)
		{
			int result = (int)Math.Round(8.0 * position+0.5);
			result = result < 0 ? 0 : result > 8 ? 8 : result;
			SetCoolantValue((byte)result);
			if (coolant.Focused)
				coolant_GotFocus(null, null);
			else
				coolant.Focus();
		}

        #endregion

        private void ShipSystemControl_Resize(object sender, EventArgs e)
        {
            coolantBar.Invalidate();
        }

        private void ResetEnergy()
		{
			if (PresetControl == null)
				return;

			PresetControl.ResetEnergy();
		}

		private void ResetCoolant()
		{
			if (PresetControl == null)
				return;

			PresetControl.ResetCoolant();
		}

        /// <summary>
        /// Transmit the signal to the parent control, that this preset got or lost focus
        /// </summary>
		private void SetChildGotFocus(bool value)
		{
			if (PresetControl == null)
				return;
			PresetControl.ChildHasFocus = value;
		}
	}
}
