using System;
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
		private string _def;
		private ExpressionMemberValueDescription _description;
        private List<int> _errorPosList;
        private List<int> _warningPosList;
        private Brush _errorBrush;
        private Brush _warningBrush;
        public string OpenFileExtensions { get; set; }
		public string OpenFileHeader { get; set; }

		/// <summary>
		/// NEVER ACCESS THIS!
		/// </summary>
		private bool _xxx_nullMode;
		/// <summary>
		/// Null mode
		/// </summary>
		private bool NullMode { get { return _xxx_nullMode; } set { _xxx_nullMode = value; UpdateNullStatus(); } }

		private bool _valid;
		/// <summary>
		/// Is the input in the textbox valid
		/// </summary>
		private bool Valid { get { return _valid; } set { _valid = value; UpdateValidStatus(); } }

		private bool _preventTextBoxEvents;

		private DialogSimple()
		{
			InitializeComponent();
			Valid = true;
			_preventTextBoxEvents = false;
			OpenFileExtensions = "";
			OpenFileHeader = "";
            Screen myScreen = Screen.FromControl(this);
            this.MaximumSize = new Size(myScreen.WorkingArea.Width, myScreen.WorkingArea.Height);

            _errorBrush = new SolidBrush(Color.DarkRed);
            _warningBrush = new SolidBrush(Color.DarkBlue);
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
                form._description = description;
                form.input.Text = initialValue;
                form._def = def;
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
				if (_description.Min != null)
                    input.Text = _description.Min.ToString();
			}

			if (e.KeyCode == Keys.End && Control.ModifierKeys == Keys.Alt)
			{
                if (_description.Max != null)
                    input.Text = _description.Max.ToString();
			}

			if (e.KeyCode == Keys.Delete && Control.ModifierKeys == Keys.Alt)
			{
				SetNullMode();
			}
		}

		private void SetNullMode()
		{
			_preventTextBoxEvents = true;
			input.Text = _def;
			NullMode = _def == null;
            ValidateInput();
			_preventTextBoxEvents = false;
		}

		private void input_TextChanged(object sender, EventArgs e)
		{
			if (_preventTextBoxEvents) 
				return;
			_preventTextBoxEvents = true;

			NullMode = false;
			ValidateInput();
			UpdateNullStatus();
			
			_preventTextBoxEvents = false;
		}

		/// <summary>
		/// Validates input in the text window
		/// </summary>
		private void ValidateInput()
		{
            if (NullMode)
            {
                warningLabel.Text = "";
                _errorPosList = null;
                _warningPosList = null;
                panelHighlighter.Invalidate();
                return;
            }
            ValidateResult vr =  Helper.Validate(input.Text, _description);
            Valid = vr.Valid;
            warningLabel.Text = vr.WarningText;
            _errorPosList = vr.ErrorList;
            _warningPosList = vr.WarningList;
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
			nullButton.Enabled = !NullMode && input.Text != _def;
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
            if (!String.IsNullOrWhiteSpace(warningLabel.Text))
                MessageBox.Show(warningLabel.Text, "All warning and error messages");
        }

        private void panelHighlighter_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                string measureString = input.Text.Length > 1 ? input.Text : "XX";
                if (_errorPosList != null)
                {
                    foreach (int pos in _errorPosList)
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
                        g.FillPolygon(_errorBrush, underscore);
                        //g.DrawImage(_upArrow, x - 5, y);
                    }
                }
                if (_warningPosList != null)
                {
                    foreach (int pos in _warningPosList)
                    {
                        if (_errorPosList != null && _errorPosList.Contains(pos))
                            continue;
                        while (measureString.Length < pos + 1) measureString += "X";
                        Font font = input.Font;
                        Graphics g = e.Graphics;

                        int left = pos == 0 ?
                            2 * TextRenderer.MeasureText(measureString.Substring(0, 1), font).Width - TextRenderer.MeasureText(measureString.Substring(0, 2), font).Width :
                            TextRenderer.MeasureText(measureString.Substring(0, pos), font).Width;
                        int right = TextRenderer.MeasureText(measureString.Substring(0, pos + 1), font).Width;
                        int x = (left + right) / 2 - 4;
                        int y = 0;

                        Point[] underscore = new Point[] { 
                            new Point(x - 4, y),
                            new Point(x + 3, y),
                            new Point(x + 3, y + 2), 
                            new Point(x - 4, y + 2), 
                            };
                        g.FillPolygon(_warningBrush, underscore);
                    }
                }
            }
            catch
            {
            }
        }
	}
}
