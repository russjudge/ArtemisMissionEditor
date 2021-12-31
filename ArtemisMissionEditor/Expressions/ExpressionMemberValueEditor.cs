using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ArtemisMissionEditor.Forms;

namespace ArtemisMissionEditor.Expressions
{
    public enum ExpressionMemberValueEditorActivationMode
    {
        Normal,
        ForceDialog,
        NextMenuItem,
        PreviousMenuItem,

        Plus01,
        Plus,
        Plus10,
        Plus100,
        Plus1000,

        Minus01,
        Minus,
        Minus10,
        Minus100,
        Minus1000,
    }

    /// <summary>
    /// Defines everything about how the value is edited
    /// </summary>
    /// <remarks>
    /// This class defines how the value is edited. 
    /// For typical classes, default editor will be enough. In this case, simply a dialog will pop when required.
    /// However, if special ways of editing would be useful, an editor class must be defined.
    /// </remarks>
	public partial class ExpressionMemberValueEditor: IDisposable
	{
		/// <summary> This value is displayed in the GUI. </summary>
		public bool IsDisplayed { get { return _isDisplayed; } }
		private bool _isDisplayed;

		/// <summary> This value is interactive (can be focused and clicked and does something). </summary>
		public bool IsInteractive { get { return _isInteractive; } }
		private bool _isInteractive;

        /// <summary> This value editor's context menu strip must be recreated on every opening. </summary>
        public bool ForceRecreateMenu { get; private set; }

        /// <summary> For miscellaneous use. </summary>
        public object Tag;

        #region Value conversion (Display, Xml, Menu)

        /// <summary> Menu group dictionary, int denotes up to which index does the menu group start, string is the menu group name </summary>
		public Dictionary<int, string> MenuGroups;
		
        /// <summary>
        /// Add new menu group title (for very long menus that have to be separated into grops)
        /// </summary>
        public void NewMenuGroup(string title)
		{
			MenuGroups.Add(DisplayValueToXml.Count, title);
		}

        /// <summary> Dictionary for conversion of XML values to Display values </summary>
        public Dictionary<string, string> XmlValueToDisplay;

        /// <summary> Dictionary for conversion of Display values to XML values </summary>
        public Dictionary<string, string> DisplayValueToXml;
		
        /// <summary>
        /// Add XML to Display links to all relevant dictionaries
        /// </summary>
        /// <param name="xml">Value how it is stored in XML</param>
        /// <param name="display">Value how it is displayed</param>
        /// <param name="menu">String to display in menu (defaults to same as display)</param>
        public void AddToDictionary(string xml, string display, string menu = null)
        {
            if (xml != null)
                XmlValueToDisplay.Add(xml, display);
            if (!DisplayValueToXml.Keys.Contains(display))
                DisplayValueToXml.Add(display, xml);
            if (menu == null)
                menu = display;
            if (!MenuValueToXml.Keys.Contains(menu))
            {
                if (xml != null)
                    XmlValueToMenu.Add(xml, menu);
                MenuValueToXml.Add(menu, xml);
            }
        }

        /// <summary> Dictionary for conversion of XML values to Menu items </summary>
		public Dictionary<string, string> XmlValueToMenu;

        /// <summary> Dictionary for conversion of Menu items to XML values </summary>
        public Dictionary<string, string> MenuValueToXml;
		
        /// <summary>
        /// Convert XML value to display value
        /// </summary>
        /// <param name="value">Value from XML</param>
        /// <param name="type">Value type</param>
        /// <param name="min">Min limit</param>
        /// <param name="max">Max limit</param>
        public virtual string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
		{
			switch (type)
			{
                case ExpressionMemberValueType.VarBool:
					int tmpBool;
                    if (value == null)
                        break;
					if (!Helper.IntTryParse(value, out tmpBool))
						tmpBool = 0;
					if (min != null && max != null)
						value = tmpBool != 0 ? max.ToString() : min.ToString();
					else
						value = tmpBool != 0 ? "true" : "false";
					break;
				case ExpressionMemberValueType.VarInteger:
				case ExpressionMemberValueType.VarDouble:
				case ExpressionMemberValueType.VarEnumString:
				case ExpressionMemberValueType.VarString:
				case ExpressionMemberValueType.Body:
				default:
					break;
			}
			
			if (value == null)
				return value;
			if (XmlValueToDisplay.ContainsKey(value))
				return XmlValueToDisplay[value];
			else
				return value;
		}

        /// <summary>
        /// Convert display or input value to XML value
        /// </summary>
        /// <param name="value">Value from XML</param>
        /// <param name="type">Value type</param>
        /// <param name="min">Min limit</param>
        /// <param name="max">Max limit</param>
        public virtual string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			switch (type)
			{
				case ExpressionMemberValueType.VarBool:
                    if (value == "Default")
                        value = null;
					if ((min != null && value == min.ToString()) || (min == null && value == "false"))
						value = "0";
					if ((max != null && value == max.ToString()) || (max == null && value == "true"))
						value = "1";
					break;
				case ExpressionMemberValueType.VarInteger:
				case ExpressionMemberValueType.VarDouble:
				case ExpressionMemberValueType.VarEnumString:
				case ExpressionMemberValueType.VarString:
				case ExpressionMemberValueType.Body:
				default:
					break;
			}
			
			if (value == null) 
				return value;
			if (MenuValueToXml.ContainsKey(value))
				return MenuValueToXml[value];
			if (DisplayValueToXml.ContainsKey(value))
				return DisplayValueToXml[value];
			else
				return value;
		}

        #endregion

        /// <summary>
        /// Shows a dialog to edit the container's expression member
        /// </summary>
        public virtual void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
		{
			KeyValuePair<bool, string> result = DialogSimple.Show(container.Member.Name, description, container.GetValue(), container.Member.Mandatory, defaultValue);
			if (result.Key)
				ValueChosen(container, result.Value);
		}

		/// <summary>
        /// Method that calls PrepareContextMenuStripMethod and returns the result.
		/// </summary>
        public ContextMenuStrip PrepareContextMenuStrip(ExpressionMemberContainer container, ExpressionMemberValueEditorActivationMode mode)
		{
			ContextMenuStrip cms =  PrepareContextMenuStripMethod(container, this, mode);
            //TODO: I'd want to limit context menu vertically somehow, but it does not really scroll well so for now it's disabled
            //cms.MaximumSize = new Size(cms.MaximumSize.Width,  500);
            return cms;
		}

        /// <summary> Method that generates context menu for the editor, it can be assigned to provide custom functionality </summary>
		public Func<ExpressionMemberContainer, ExpressionMemberValueEditor, ExpressionMemberValueEditorActivationMode, ContextMenuStrip> PrepareContextMenuStripMethod;

        /// <summary> Current Context Menu Strip of the class </summary>
		public ContextMenuStrip ValueSelectorContextMenuStrip;

        /// <summary> Last user of the Context Menu Strip (usually there is no need to recreate if last user was the same)</summary>
        public ExpressionMemberValueDescription LastUser;
		
        /// <summary> 
        /// Forces recreation of the context menu when it is next opened. 
        /// Used for menus that depend on content created elsewhere, like list of variable names
        /// </summary>
        public void InvalidateContextMenuStrip()
        {
            if (ValueSelectorContextMenuStrip != null)
                ValueSelectorContextMenuStrip.Dispose();
            ValueSelectorContextMenuStrip = null;
        }

        /// <summary> Font used for contest menu strips</summary>
        private static Font CMSFont = new Font("Segoe UI", 16);

        /// <summary> Initialise all default values of Context Menu Strip</summary>
        public void InitContextMenuStrip()
		{
			ValueSelectorContextMenuStrip.Font = CMSFont;
			ValueSelectorContextMenuStrip.ShowImageMargin = false;
		}

		/// <summary> 
        /// Show then hide the Context Menu Strip. 
        /// Required because otherwise there is no way to deselect menu items!!! Micro$oft FAIL! 
        /// </summary>
		public void ShowHideContextMenuStrip()
		{
			ValueSelectorContextMenuStrip.Show();
			ValueSelectorContextMenuStrip.Close();
		}

        protected virtual void Dispose(Boolean disposing)
        {
            if (CMSFont != null)
                CMSFont.Dispose();
            if (ValueSelectorContextMenuStrip != null)
                ValueSelectorContextMenuStrip.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ExpressionMemberValueEditor(bool isDisplayed = true, bool isInteractive = true, bool forceRecreateMenu = false)
        {
            ForceRecreateMenu = forceRecreateMenu;
            _isDisplayed = isDisplayed;
            _isInteractive = isInteractive;

            XmlValueToDisplay = new Dictionary<string, string>();
            DisplayValueToXml = new Dictionary<string, string>();
            XmlValueToMenu = new Dictionary<string, string>();
            MenuValueToXml = new Dictionary<string, string>();
            MenuGroups = new Dictionary<int, string>();

            PrepareContextMenuStripMethod = (ExpressionMemberContainer i, ExpressionMemberValueEditor j, ExpressionMemberValueEditorActivationMode mode) => null;
            ValueSelectorContextMenuStrip = null;
            LastUser = null;
        }
    }

    /// <summary>
    /// Editor for Elite ability list
    /// </summary>
    public sealed class ExpressionMemberValueEditor_AbilityBits : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = "";

            if (value == null || !Helper.IntTryParse(value))
                return "(None)";

            int bits = Helper.StringToInt(value);

            if ((bits & 1) == 1) result += "Stealth ";
            if ((bits & 2) == 2) result += "LowVis ";
            if ((bits & 4) == 4) result += "Cloak ";
            if ((bits & 8) == 8) result += "HET ";
            if ((bits & 16) == 16) result += "Warp ";
            if ((bits & 32) == 32) result += "Teleport ";
            if ((bits & 64) == 64) result += "Tractor ";
            if ((bits & 128) == 128) result += "Drones ";
            if ((bits & 256) == 256) result += "AntiMine ";
            if ((bits & 512) == 512) result += "AntiTorp ";
            if ((bits & 1024) == 1024) result += "ShldDrain ";
            if ((bits & 2048) == 2048) result += "ShldVamp ";
            if ((bits & 4096) == 4096) result += "TeleBack ";
            if ((bits & 8192) == 8192) result += "ShldReset ";

            if (result.Length > 0)
                return "(" + result.Substring(0, result.Length - 1) + ")";
            else
                return "(None)";
            
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            return value;
        }

        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        /// <param name="container"></param>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogAbilityBits.Show(container.Member.Name, container.GetValue());
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for Comms Types
    /// </summary>
    public sealed class ExpressionMemberValueEditor_CommTypes : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            return value ?? "None";
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            return value;
        }

        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        /// <param name="container"></param>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogCommTypes.Show(container.Member.Name, container.GetValue());
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for list of Consoles
    /// </summary>
    public sealed class ExpressionMemberValueEditor_ConsoleList : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = "";

            if (value == null)
                return "None";

            value = value.ToLower();

            if (value.Contains('m'))
                result += "Msc ";
            if (value.Contains('h'))
                result += "Hlm ";
            if (value.Contains('w'))
                result += "Wep ";
            if (value.Contains('e'))
                result += "Eng ";
            if (value.Contains('s'))
                result += "Sci ";
            if (value.Contains('c'))
                result += "Com ";
            if (value.Contains('o'))
                result += "Obs ";
            if (value.Contains('f'))
                result += "Ftr ";

            if (result.Length > 0)
                return result.Substring(0, result.Length - 1);
            else
                return "None";
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            return value;
        }

        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        /// <param name="container"></param>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogConsoleList.Show(container.Member.Name, container.GetValue());
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for Hull IDs
    /// </summary>
    public sealed class ExpressionMemberValueEditor_HullID : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;
            if (!VesselData.Current.VesselList.ContainsKey(value))
                return "[" + value + "] Vessel does not exist";
            return VesselData.Current.VesselToString(value);
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = value;
            int tmp;

            if (string.IsNullOrWhiteSpace(result))
                return null;

            if (Helper.IntTryParse(result, out tmp))
                return result;
            result = result.Replace("[", "").Replace(" ", "");
            if (result.IndexOf("]") == -1)
                return null;
            result = result.Substring(0, result.IndexOf("]"));
            if (Helper.IntTryParse(result, out tmp))
                return result;

            return null;
        }

    }

    /// <summary>
    /// Editor for Hull Keys
    /// </summary>
    public sealed class ExpressionMemberValueEditor_HullKeys : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        /// <param name="container"></param>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogRHKeys.Show(container.Member.Name, container.GetAttribute("raceKeys"), container.GetAttribute("hullKeys"), DialogRHKeysMode.HullBroadTypesClassNames);
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for File Paths
    /// </summary>
    public sealed class ExpressionMemberValueEditor_PathEditor : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        /// <param name="container"></param>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogSimple.Show(container.Member.Name, description, container.GetValue(), container.Member.Mandatory, defaultValue, true);
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for percentages
    /// </summary>
    public sealed class ExpressionMemberValueEditor_Percent : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = base.ValueToDisplay(value, type, min, max);
            if (result != value)
                return result;
            double tmp;
            Helper.DoubleTryParse(value, out tmp);
            value = Helper.DoubleToPercent(tmp);

            return value;
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = base.ValueToXml(value, type, min, max);
            if (result != value)
                return result;
            return value;
        }

    }

    /// <summary>
    /// Editor for Race Keys
    /// </summary>
    public sealed class ExpressionMemberValueEditor_RaceKeys : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Shows a GUI form to edit the container's expression member
        /// </summary>
        public override void ShowEditingDialog(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string defaultValue)
        {
            KeyValuePair<bool, string> result = DialogRHKeys.Show(container.Member.Name, container.GetAttribute("raceKeys"), container.GetAttribute("hullKeys"), DialogRHKeysMode.RaceNamesKeys);
            if (result.Key)
                ValueChosen(container, result.Value);
        }
    }

    /// <summary>
    /// Editor for XML Name check (aka main check / first check) 
    /// (where we choose the kind of statement this is, like "If variable" or "Set property")
    /// </summary>
    public sealed class ExpressionMemberValueEditor_XmlName : ExpressionMemberValueEditor
    {
        /// <summary>
        /// Convert inner value into displayed value
        /// </summary>
        /// <param name="value">Inner value</param>
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = base.ValueToDisplay(value, type, min, max);
            if (result != value)
                return result;
            //Prevent empty entry from appearing in the menu or in text representation of the node
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.Replace('_', ' ');
            value = value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);

            return value;
        }

        /// <summary>
        /// Convert displayed or input value into value stored in XML
        /// </summary>
        /// <param name="value">Displayed or input value</param>
        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
        {
            string result = base.ValueToXml(value, type, min, max);
            if (result != value)
                return result;
            if (string.IsNullOrWhiteSpace(value))
                return null;
            value = value.ToLower();
            value = value.Replace(' ', '_');
            return value;
        }
    }
}
