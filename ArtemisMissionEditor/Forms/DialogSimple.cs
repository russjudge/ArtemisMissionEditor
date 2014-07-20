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

namespace ArtemisMissionEditor
{
	public partial class DialogSimple : Form
	{
		private object _min;
		private object _max;
		private string _def;
		private ExpressionMemberValueType _type;
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
		}

		public static KeyValuePair<bool, string> Show(string name, ExpressionMemberValueType type, string initialValue, bool mandatory, string def, object min, object max, bool pathEditor = false)
		{
			
            using (DialogSimple form = new DialogSimple())
            {

                string caption = "";
                caption += type == ExpressionMemberValueType.VarDouble ? "Float " : "";
                caption += type == ExpressionMemberValueType.VarString ? "String " : "";
                caption += type == ExpressionMemberValueType.Body ? "Node body " : "";
                caption += type == ExpressionMemberValueType.VarInteger ? "Integer " : "";
                caption += type == ExpressionMemberValueType.VarBool ? "Boolean " : "";
                caption += name;
                if (min != null || max != null)
                {
                    if (type == ExpressionMemberValueType.VarBool)
                    {
                        //Dunno what to do for bools yet
                        //caption += ": ";
                        //caption += min.ToString();
                        //caption += "/";
                        //caption += max.ToString();
                    }
                    else if (type == ExpressionMemberValueType.VarString)
                    {

                    }
                    else
                    {
                        caption += ": ";
                        caption += min == null ? "-INF" : min.ToString();
                        caption += " ... ";
                        caption += max == null ? "+INF" : max.ToString();
                    }
                }

                if (pathEditor)
                {
                    form.OpenFileExtensions = (string)min;
                    form.OpenFileHeader = (string)max;
                    form._min = null;
                    form._max = null;
                    form.openFileButton.Visible = true;
                }
                else
                {
                    form._min = min;
                    form._max = max;
                    form.openFileButton.Visible = false;
                }

                form.Text = caption;
                form.input.Text = initialValue;
                form._type = type;
                form._def = def;
                form.NullMode = initialValue == null;
                form.nullButton.Text = def == null ? "Null" : "Default";
                form.Width = type == ExpressionMemberValueType.VarString ? 390 : 212;
                form.Height = 103;

                if (type == ExpressionMemberValueType.Body)
                {
                    form.Height = 300;
                    form.Width = 900;
                    form.input.Multiline = true;
                }


                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                string result = form.NullMode ? def : form.input.Text;
                //R. Judge: Changes Jan 16, 2013, to band-aid up to 1.7
                if (result != null && type == ExpressionMemberValueType.VarDouble)
                {
                    double resd = 0;

                    if (double.TryParse(result, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out resd))
                    {
                        result = Helper.DoubleToString(resd);
                    }
                }
                if (result != null && type == ExpressionMemberValueType.VarInteger)
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
				if (_min != null)
					input.Text = _min.ToString();
			}

			if (e.KeyCode == Keys.End && Control.ModifierKeys == Keys.Alt)
			{
				if (_max != null)
					input.Text = _max.ToString();
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
			//Check
			switch (_type)
			{
				case ExpressionMemberValueType.VarDouble:
					double d; 
					Valid = Helper.DoubleTryParse(input.Text,out d);
					Valid = Valid && (_min == null || d >= (double)_min);
					Valid = Valid && (_max == null || d <= (double)_max);
					break;
				case ExpressionMemberValueType.VarInteger:
					int i; 
					Valid = Helper.IntTryParse(input.Text,out i);
					Valid = Valid && (_min == null || i >= (int)_min);
					Valid = Valid && (_max == null || i <= (int)_max);
					break;
				case ExpressionMemberValueType.VarBool:
					break;
				default:
					break;
			}
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
	}
}
