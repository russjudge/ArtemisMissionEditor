﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Resources;

namespace ArtemisMissionEditor
{
	public partial class DialogSimple : Form
	{
		private string DefaultValue;
		private ExpressionMemberValueDescription Description;
        private List<int> ErrorPositions;
        private List<ValidateResultWarning> WarningPositions;
        private Brush BrushError;
        private Brush BrushWarning;
        public string OpenFileExtensions { get; set; }
		public string OpenFileHeader { get; set; }

		/// <summary> Wether the input value is considered to be null (distinguishes between null and empty string)</summary>
		private bool NullMode { get { return _nullMode; } set { _nullMode = value; UpdateNullStatus(); } }
        /// <summary> NEVER ACCESS THIS! </summary>
        private bool _nullMode;
		
		/// <summary> Is the input in the textbox valid or not</summary>
		private bool Valid { get { return _valid; } set { _valid = value; UpdateValidStatus(); } }
        private bool _valid;
		
		/// <summary> Raised to temporarily prevent input TextBox's "OnTextChanged" events</summary>
        private bool PreventTextBoxEvents;

		private DialogSimple()
		{
			InitializeComponent();
			Valid = true;
			PreventTextBoxEvents = false;
			OpenFileExtensions = "";
			OpenFileHeader = "";
            Screen myScreen = Screen.FromControl(this);
            this.MaximumSize = new Size(myScreen.WorkingArea.Width, myScreen.WorkingArea.Height);

            BrushError = new SolidBrush(Color.DarkRed);
            BrushWarning = new SolidBrush(Color.DarkBlue);
		}

		public static KeyValuePair<bool, string> Show(string name, ExpressionMemberValueDescription description, string initialValue, bool mandatory, string def, bool pathEditor = false)
		{
            using (DialogSimple form = new DialogSimple())
            {
                string caption = "";
                caption += description.Type == ExpressionMemberValueType.VarDouble ? "[float] " : "";
                caption += description.Type == ExpressionMemberValueType.VarString ? "[string] " : "";
                caption += description.Type == ExpressionMemberValueType.Body ? "Element contents" : "";
                caption += description.Type == ExpressionMemberValueType.VarInteger ? "[integer] " : "";
                caption += description.Type == ExpressionMemberValueType.VarBool ? "[boolean] " : "";
                caption += name;
                if (description.Min != null || description.Max != null)
                {
                    if (description.Type == ExpressionMemberValueType.VarBool)
                    {
                        //Dunno what to do for bools yet
                        //caption += ": ";
                        //caption += min.ToString();
                        //caption += "/";
                        //caption += max.ToString();
                    }
                    else if (description.Type == ExpressionMemberValueType.VarString)
                    {

                    }
                    else
                    {
                        caption += " [";
                        caption += description.Min == null ? "-INF" : description.Min.ToString();
                        caption += " ... ";
                        caption += description.Max == null ? "+INF" : description.Max.ToString();
                        caption += "]";
                    }
                }

                if (pathEditor)
                {
                    form.OpenFileExtensions = (string)description.Min;
                    form.OpenFileHeader = (string)description.Max;
                    form.openFileButton.Visible = true;
                }
                else
                {
                    form.openFileButton.Visible = false;
                }

                form.Text = caption;
                form.Description = description;
                form.input.Text = initialValue;
                form.DefaultValue = def;
                form.NullMode = initialValue == null;
                form.nullButton.Text = def == null ? "Null" : "Default";
                //form.Width = type == ExpressionMemberValueType.VarString ? 390 : 212;
                //TODO: Type in exact values
                //form.Width = 390;
                //form.Height = 103;

                if (description.Type == ExpressionMemberValueType.Body)
                {
                    form.Height = 300;
                    form.Width = 900;
                    form.input.Multiline = true;
                }
                form.warningLabel.Width = form.button1.Left - form.warningLabel.Left - 2;

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                string result = form.NullMode ? def : form.input.Text;
                //R. Judge: Changes Jan 16, 2013, to band-aid up to 1.7
                if (result != null && description.Type == ExpressionMemberValueType.VarDouble)
                {
                    double resd = 0;

                    if (double.TryParse(result, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out resd))
                    {
                        result = Helper.DoubleToString(resd);
                    }
                }
                if (result != null && description.Type == ExpressionMemberValueType.VarInteger)
                {
                    int resi = 0;
                    if (int.TryParse(result, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out resi))
                    {
                        result = resi.ToString();
                    }
                }
                //R. Judge: End changes Jan 16, 2013.
                return new KeyValuePair<bool, string>(true, result);
            }
		}

		private void input_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && Control.ModifierKeys == Keys.None && !input.Multiline)
			{
				Close_OK();
			}

			if (e.KeyCode == Keys.Enter && Control.ModifierKeys == Keys.Shift)
			{
				Close_OK();
			}

			if (e.KeyCode == Keys.Home && Control.ModifierKeys == Keys.Alt)
			{
				if (Description.Min != null)
                    input.Text = Description.Min.ToString();
			}

			if (e.KeyCode == Keys.End && Control.ModifierKeys == Keys.Alt)
			{
                if (Description.Max != null)
                    input.Text = Description.Max.ToString();
			}

			if (e.KeyCode == Keys.Delete && Control.ModifierKeys == Keys.Alt)
			{
				SetNullMode();
			}
		}

		private void SetNullMode()
		{
			PreventTextBoxEvents = true;
			
            input.Text = DefaultValue;
			NullMode = DefaultValue == null;
            ValidateInput();
			
            PreventTextBoxEvents = false;
		}

		private void input_TextChanged(object sender, EventArgs e)
		{
			if (PreventTextBoxEvents) 
				return;
			PreventTextBoxEvents = true;

            if (NullMode)
                timerValidate_Tick(null, null);
            NullMode = false;
            
            timerValidate.Stop();
            timerValidate.Start();

            

			PreventTextBoxEvents = false;
		}

		/// <summary>
		/// Validates input in the text window
		/// </summary>
		private void ValidateInput()
		{
            if (NullMode)
            {
                warningLabel.Text = "";
                ErrorPositions = null;
                WarningPositions = null;
                panelHighlighter.Invalidate();
                return;
            }
            ValidateResult vr =  Helper.Validate(input.Text, Description);
            Valid = vr.Valid;
            string[] labels = vr.WarningText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            warningLabel.Text = labels.Length>0?labels[0]:"" ;
            warningLabel.Tag = vr.WarningText;
            ErrorPositions = vr.ErrorList;
            WarningPositions = vr.WarningList;
            panelHighlighter.Invalidate();
		}

		/// <summary>
		/// Updates visualisation of input status
		/// </summary>
		private void UpdateValidStatus()
		{
			//Visualise
			if (!Valid && input.BackColor != Color.LightSalmon)
				input.BackColor = Color.LightSalmon;
			if (Valid && input.BackColor == Color.LightSalmon)
				input.BackColor = Color.White;
            //R. Judge: Changes Jan 16, 2013, to band-aid up to 1.7
			//okButton.Enabled = Valid;
            //R. Judge: End changes Jan 16, 2013.
		}

		/// <summary>
		/// Update the visualisation of the null status
		/// </summary>
		private void UpdateNullStatus()
		{
			Valid = Valid || NullMode;
			nullButton.Enabled = !NullMode && input.Text != DefaultValue;
			if (NullMode && input.BackColor != Color.LightGray)
				input.BackColor = Color.LightGray;
			if (!NullMode && input.BackColor == Color.LightGray)
				input.BackColor = Color.White;
		}

		private void ckButton_Click(object sender, EventArgs e)
		{
			Close_OK();
		}

		private void Close_OK()
		{
            //R. Judge: Changes Jan 16, 2013, to band-aid up to 1.7
            //if (Valid)
            //{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			//}
            //R. Judge: End changes Jan 16, 2013.
		}

		private void nullButton_Click(object sender, EventArgs e)
		{
			SetNullMode();
            timerValidate.Stop();
            input.Focus();
		}

		private void DialogSimple_Shown(object sender, EventArgs e)
		{
			ValidateInput();
			UpdateValidStatus();
			UpdateNullStatus();
		}

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The box will display in gray if the value is null, or in red if the value is invalid.\r\nYou can use Alt + Del to set the value to null (meaning it will not be present in xml), or to default value, if null value isnt allowed for this attribute", "How to use");
            input.Focus();
        }

		private void openFileButton_Click(object sender, EventArgs e)
		{
            DialogResult res;
            string filename;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = false;
                ofd.AddExtension = true;
                ofd.Multiselect = false;
                ofd.Filter = OpenFileExtensions;
                ofd.Title = OpenFileHeader;

                res = ofd.ShowDialog();
                filename = ofd.FileName;
            }
			if (res != DialogResult.OK)
				return;

			int pos = -1;
			
			//If file is inside mission folder - take only file name
            if (!string.IsNullOrEmpty(Mission.Current.FilePath) && Path.GetDirectoryName(filename) == Path.GetDirectoryName(Mission.Current.FilePath))
				input.Text = Path.GetFileName(filename);
			//If file is inside dat folder - take only part from dat and on
			else if ((pos = filename.LastIndexOf("dat\\")) != -1)
				input.Text = filename.Substring(pos, filename.Length - pos);
			//Else - take absolute path
			else
				input.Text = filename;
		}

        private void warningLabel_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(warningLabel.Tag as string))
                MessageBox.Show((string)warningLabel.Tag, "All warning and error messages");
        }

        private void panelHighlighter_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                string measureString = input.Text.Length > 1 ? input.Text : "XX";
                if (ErrorPositions != null)
                {
                    foreach (int pos in ErrorPositions)
                    {
                        while (measureString.Length < pos + 1) measureString += "X";
                        Font font = input.Font;
                        Graphics g = e.Graphics;

                        int left = pos == 0 ? 
                            2 * TextRenderer.MeasureText(measureString.Substring(0, 1), font).Width - TextRenderer.MeasureText(measureString.Substring(0, 2), font).Width :
                            TextRenderer.MeasureText(measureString.Substring(0, pos), font).Width;
                        int right = TextRenderer.MeasureText(measureString.Substring(0, pos + 1), font).Width;
                        int x = (left + right) / 2 - 4;
                        int y = 0;

                        //Point[] triangle = new Point[] { 
                        //    new Point(x - 1, y),
                        //    new Point(x + 1, y),
                        //    new Point(x + 3, y + 7), 
                        //    new Point(x + 1, y + 7), 
                        //    new Point(x + 1, y + 10), 
                        //    new Point(x - 1 , y + 10), 
                        //    new Point(x - 1, y + 7), 
                        //    new Point(x - 4, y + 7) };
                        Point[] underscore = new Point[] { 
                            new Point(x - 4, y),
                            new Point(x + 3, y),
                            new Point(x + 3, y + 3), 
                            new Point(x - 4, y + 3), 
                            };
                        g.FillPolygon(BrushError, underscore);
                        //g.DrawImage(_upArrow, x - 5, y);
                    }
                }
                if (WarningPositions != null)
                {
                    foreach (ValidateResultWarning warning in WarningPositions)
                    {
                        if (ErrorPositions != null && ErrorPositions.Contains(warning.Position))
                            continue;
                        while (measureString.Length < warning.Position + warning.Length) measureString += "X";
                        Font font = input.Font;
                        Graphics g = e.Graphics;

                        int left = warning.Position == 0 ?
                            2 * TextRenderer.MeasureText(measureString.Substring(0, 1), font).Width - TextRenderer.MeasureText(measureString.Substring(0, 2), font).Width :
                            TextRenderer.MeasureText(measureString.Substring(0, warning.Position), font).Width;
                        int right = TextRenderer.MeasureText(measureString.Substring(0, warning.Position + 1), font).Width;
                        int x1 = (left + right) / 2 - 4;
                        left = warning.Position + warning.Length == 0 ?
                            2 * TextRenderer.MeasureText(measureString.Substring(0, 1), font).Width - TextRenderer.MeasureText(measureString.Substring(0, 2), font).Width :
                            TextRenderer.MeasureText(measureString.Substring(0, warning.Position + warning.Length), font).Width;
                        right = TextRenderer.MeasureText(measureString.Substring(0, warning.Position + warning.Length), font).Width;
                        int x2 = (left + right) / 2 - 4;
                        int y = 0;

                        Point[] underscore = new Point[] { 
                            new Point(x1 - 4, y),
                            new Point(x2, y),
                            new Point(x2, y + 2), 
                            new Point(x1 - 4, y + 2), 
                            };
                        g.FillPolygon(BrushWarning, underscore);
                    }
                }
            }
            catch
            {
            }
        }

        private void timerValidate_Tick(object sender, EventArgs e)
        {
            ValidateInput();
            UpdateNullStatus();

            timerValidate.Stop();
        }

        private void warningLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
	}
}
