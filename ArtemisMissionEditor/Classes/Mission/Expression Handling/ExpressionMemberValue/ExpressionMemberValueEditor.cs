using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ArtemisMissionEditor
{
    /// <summary>
    /// Defines everything about how the value is edited
    /// </summary>
    /// <remarks>
    /// This class defines how the value is edited. 
    /// For typical classes, default editor will be enough. In this case, simply a dialog will pop when required.
    /// However, if special ways of editing would be useful, an editor class must be defined.
    /// </remarks>
	public class ExpressionMemberValueEditor
	{
		/// <summary> Means it wont appear in the editor visually at all </summary>
		public static ExpressionMemberValueEditor Nothing;
		/// <summary> Means it will appear as an uneditable label </summary>
		public static ExpressionMemberValueEditor Label;
        // Default editors for all types
		public static ExpressionMemberValueEditor DefaultInteger;
		public static ExpressionMemberValueEditor DefaultBool;
		public static ExpressionMemberValueEditor DefaultDouble;
		public static ExpressionMemberValueEditor DefaultString;
		public static ExpressionMemberValueEditor DefaultBody; 
		
        public static ExpressionMemberValueEditor CreateType;
		public static ExpressionMemberValueEditor DefaultCheckUnsorted;
		public static ExpressionMemberValueEditor DefaultCheckSorted;
		public static ExpressionMemberValueEditor XmlNameActionCheck;
		public static ExpressionMemberValueEditor XmlNameConditionCheck;
		public static ExpressionMemberValueEditor SetVariableCheck;
		public static ExpressionMemberValueEditor AIType;
		public static ExpressionMemberValueEditor PropertyObject;
		public static ExpressionMemberValueEditor PropertyFleet;
		public static ExpressionMemberValueEditor SkyboxIndex;
		public static ExpressionMemberValueEditor Difficulty;
		public static ExpressionMemberValueEditor ShipSystem;
		public static ExpressionMemberValueEditor CountFrom;
		public static ExpressionMemberValueEditor DamageValue;
		public static ExpressionMemberValueEditor TeamIndex;
		public static ExpressionMemberValueEditor TeamAmount;
		public static ExpressionMemberValueEditor TeamAmountF;
        public static ExpressionMemberValueEditor SpecialShipType;
        public static ExpressionMemberValueEditor SpecialCapitainType;
        public static ExpressionMemberValueEditor Side;
		public static ExpressionMemberValueEditor Comparator;
		public static ExpressionMemberValueEditor DistanceNebulaCheck;
		public static ExpressionMemberValueEditor ConvertDirectCheck;
		public static ExpressionMemberValueEditor TimerName;
		public static ExpressionMemberValueEditor VariableName;
		public static ExpressionMemberValueEditor NamedAllName;
		public static ExpressionMemberValueEditor NamedStationName;
		public static ExpressionMemberValueEditor ConsoleList;
		public static ExpressionMemberValueEditor EliteAIType;
		public static ExpressionMemberValueEditor EliteAbilityBits;
		public static ExpressionMemberValueEditor PlayerNames;
		public static ExpressionMemberValueEditor WarpState;
		public static ExpressionMemberValueEditor PathEditor;
		public static ExpressionMemberValueEditor HullID;
		public static ExpressionMemberValueEditor RaceKeys;
		public static ExpressionMemberValueEditor HullKeys;

		/// <summary>
		/// This value is displayed in GUI
		/// </summary>
		/// <returns></returns>
		public bool IsDisplayed { get { return _isDisplayed; } }
		private bool _isDisplayed;

		/// <summary>
		/// This value is interactive (can be focused and clicked and does something)
		/// </summary>
		/// <returns></returns>
		public bool IsInteractive { get { return _isInteractive; } }
		private bool _isInteractive;

		/// <summary>
		/// Menu group dictionary, int denotes up to which index does the menu group start, string is the menu group name
		/// </summary>
		private Dictionary<int, string> MenuGroups;
		private List<string> MenuItems;
		private void NewMenuGroup(string title)
		{
			MenuGroups.Add(_valueToXml.Count, title);
		}

		public Dictionary<string, string> ValueToDisplayList { get { return _valueToDisplay; } }
		private Dictionary<string, string> _valueToDisplay;
		private Dictionary<string, string> _valueToXml;
		private void AddToDictionary(string xml, string display)
		{
			if (xml!=null)
                _valueToDisplay.Add(xml, display);
			_valueToXml.Add(display, xml);
			MenuItems.Add(display);
		}

		private Dictionary<string, string> _valueToMenu;
		private Dictionary<string, string> _valueFromMenu;
		private void AddToMenuDictionary(string xml, string menu)
		{
			_valueToMenu.Add(xml, menu);
			_valueFromMenu.Add(menu, xml);
		}

        public virtual string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
		{
			switch (type)
			{
                case ExpressionMemberValueType.VarBool:
					int tmpBool;
					if (!Helper.IntTryParse(value, out tmpBool))
						tmpBool = 0;
					if (min != null && max != null)
						value = tmpBool != 0 ? max.ToString() : min.ToString();
					else
						value = tmpBool != 0 ? "true" : "false";
					break;
				case ExpressionMemberValueType.VarInteger:
					break;
				case ExpressionMemberValueType.VarDouble:
					break;
				case ExpressionMemberValueType.VarString:
					break;
				case ExpressionMemberValueType.Body:
					break;
				default:
					break;
			}
			
			if (value == null)
				return value;
			if (_valueToDisplay.ContainsKey(value))
				return _valueToDisplay[value];
			else
				return value;
		}

        public virtual string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			switch (type)
			{
				case ExpressionMemberValueType.VarBool:
					if ((min != null && value == min.ToString()) || (min == null && value == "false"))
						value = "0";
					if ((max != null && value == max.ToString()) || (max == null && value == "true"))
						value = "1";
					break;
				case ExpressionMemberValueType.VarInteger:
					break;
				case ExpressionMemberValueType.VarDouble:
					break;
				case ExpressionMemberValueType.VarString:
					break;
				case ExpressionMemberValueType.Body:
					break;
				default:
					break;
			}
			
			if (value == null) 
				return value;
			if (_valueFromMenu.ContainsKey(value))
				return _valueFromMenu[value];
			if (_valueToXml.ContainsKey(value))
				return _valueToXml[value];
			else
				return value;
		}

		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
		public virtual void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogSimple.Show(container.Member.Name, description, container.GetValue(), container.Member.Mandatory, def);
			if (result.Key)
				ValueChosen(container, result.Value);
		}

		protected ExpressionMemberValueEditor(bool isDisplayed = true, bool isInteractive = true)
		{
			_isDisplayed = isDisplayed;
			_isInteractive = isInteractive;

			_valueToDisplay = new Dictionary<string, string>();
			_valueToXml = new Dictionary<string, string>();
			_valueToMenu = new Dictionary<string, string>();
			_valueFromMenu = new Dictionary<string, string>();
			MenuGroups = new Dictionary<int, string>();
			MenuItems = new List<string>();

			PrepareContextMenuStripMethod = (ExpressionMemberContainer i, ExpressionMemberValueEditor j, EditorActivationMode mode) => null;
			cmsValueSelector = null;
			_lastUser = null;
		}

		public ContextMenuStrip PrepareContextMenuStrip(ExpressionMemberContainer container, EditorActivationMode mode)
		{
			ContextMenuStrip cms =  PrepareContextMenuStripMethod(container, this, mode);
            // I'd want to limit context menu vertically somehow, but it does not really scroll well so for now it's disabled
            //cms.MaximumSize = new Size(cms.MaximumSize.Width,  500);
            return cms;
		}

		private Func<ExpressionMemberContainer, ExpressionMemberValueEditor, EditorActivationMode, ContextMenuStrip> PrepareContextMenuStripMethod;

		private ContextMenuStrip cmsValueSelector;
		private ExpressionMemberValueDescription _lastUser;
		public void InvalidateCMS() { cmsValueSelector = null; }

		private static Font CMSFont;
		protected void InitCMS()
		{
			cmsValueSelector.Font = CMSFont;
			cmsValueSelector.ShowImageMargin = false;
		}
		/// <summary> Required because otherwise there is no way to deselect the menu items!?! Micro$oft FAIL! </summary>
		protected void ShowHideCMS()
		{
			cmsValueSelector.Show();
			cmsValueSelector.Close();
		}

		/// <summary>
		/// Function to call when a value is chosen by the editor
		/// </summary>
		/// <param name="container"></param>
		/// <param name="value"></param>
		protected static void ValueChosen(ExpressionMemberContainer container, string value)
		{
			container.SetValue(container.ValueToXml(value));
			Mission.Current.UpdateStatementTree();
			Mission.Current.RegisterChange("Expression member value changed");
		}

		private static void ContextMenuClick_Choose(object sender, EventArgs e)
		{
			ValueChosen(((ExpressionMemberContainer)((ToolStripItem)sender).Tag), ((ToolStripItem)sender).Text);
		}

		private static void ContextMenuClick_Show_GUI(object sender, EventArgs e)
		{
			((ExpressionMemberContainer)((ToolStripItem)sender).Tag).Member.ValueDescription.ShowEditingGUI(((ExpressionMemberContainer)((ToolStripItem)sender).Tag));
		}

		private static void AssignCMSContainer_private_RecursivelyAssign(ToolStripItem item, ExpressionMemberContainer container, string value)
		{
			if (!(item is ToolStripMenuItem))
				return;

            bool itemSelected = false;
            foreach (ToolStripItem innerItem in ((ToolStripMenuItem)item).DropDownItems)
            {
                AssignCMSContainer_private_RecursivelyAssign(innerItem, container, value);
                itemSelected = itemSelected || item.Selected;
            }

			item.Tag = container;
			if (Helper.AreEqual(container.ValueToXml(item.Text), value, container.Member.ValueDescription))
				item.Select();
		}

		private static void AssignCMSContainer(ContextMenuStrip cms, ExpressionMemberContainer container, string value, bool selectGUI=false)
		{
            bool itemSelected = false;
            foreach (ToolStripItem item in cms.Items)
            {
                AssignCMSContainer_private_RecursivelyAssign(item, container, value);
                itemSelected = itemSelected || item.Selected;
            }

			if (selectGUI && !itemSelected)
				cms.Items[cms.Items.Count - 1].Select();
		}

		#region PrepareCMS functions

		private static ContextMenuStrip PrepareContextMenuStrip_CreateType(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			string value = container.GetValue();
			
			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

				//Add all possible types, sorting by alphabet but putting named bunch first and Nameless Bunch second (NANASHI RENJUU!!! :)
				foreach (string item in editor._valueToXml.Keys.OrderBy((x) => (x[x.Length - 1] == 's' ? "b" : "a") + x))
				{
					ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}
				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, value);
            return editor.cmsValueSelector;
		}

		private static ContextMenuStrip PrepareContextMenuStrip_DefaultList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor.cmsValueSelector == null || editor._lastUser!=container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

				foreach (string item in editor._valueToXml.Keys)
				{
					ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container,value);
            return editor.cmsValueSelector;
		}

        private static ContextMenuStrip PrepareContextMenuStrip_DefaultListWithFirstSeparated(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
            {
                editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor.cmsValueSelector.Items.Clear();
                editor.cmsValueSelector.Tag = false;

                bool doOnce = true;
                foreach (string item in editor._valueToXml.Keys)
                {
                    ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClick_Choose;

                    if (doOnce)
                    {
                        doOnce = false;
                        editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
                    }
                }

                editor.ShowHideCMS();
            }

            AssignCMSContainer(editor.cmsValueSelector, container, value); 
            return editor.cmsValueSelector;
        }

		private static ContextMenuStrip PrepareContextMenuStrip_DefaultListPlusGUI(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor.cmsValueSelector == null || editor._lastUser!=container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = true;

				foreach (string item in editor._valueToXml.Keys)
				{
					ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor.cmsValueSelector.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

            AssignCMSContainer(editor.cmsValueSelector, container, container.GetValue(), true);
            return editor.cmsValueSelector;
		}

        private static ContextMenuStrip PrepareContextMenuStrip_DefaultListPlusGUIWithFirstSepearted(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
        {
            if (mode == EditorActivationMode.ForceGUI)
                return null;

            if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
            {
                editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor.cmsValueSelector.Items.Clear();
                editor.cmsValueSelector.Tag = true;

                bool doOnce = true;
                foreach (string item in editor._valueToXml.Keys)
                {
                    ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClick_Choose;

                    if (doOnce)
                    {
                        doOnce = false;
                        editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
                    }
                }

                editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
                ToolStripItem tsl = editor.cmsValueSelector.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClick_Show_GUI;

                editor.ShowHideCMS();
            }

            AssignCMSContainer(editor.cmsValueSelector, container, container.GetValue(), true); 
            return editor.cmsValueSelector;
        }

		private static ContextMenuStrip PrepareContextMenuStrip_NestedList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

				int i = 0;
				foreach (KeyValuePair<int, string> kvp in editor.MenuGroups.OrderBy((KeyValuePair<int, string> x) => x.Key))
				{
					ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Value);
					editor.cmsValueSelector.Items.Add(tsm);

					while (i < kvp.Key)
					{
						string item = editor.MenuItems[i++];

						ToolStripItem tsi = tsm.DropDownItems.Add(item);

						tsi.Tag = container;
						tsi.Click += ContextMenuClick_Choose;
					}
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, value);
            return editor.cmsValueSelector;
		}

		private static ContextMenuStrip PrepareContextMenuStrip_DefaultCheckUnsorted(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_DefaultCheck(container, editor, false);
		}

		private static ContextMenuStrip PrepareContextMenuStrip_DefaultCheckSorted(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_DefaultCheck(container, editor, true);
		}

		private static void				PrepareContextMenuStrip_DefaultCheck_private_loopIteration(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ContextMenuStrip cms, string value, string item, int index)
		{
			if (container.ValueToDisplay(item) == null)
				return;
			ToolStripItem tsi = cms.Items.Add((editor._valueToMenu.ContainsKey(item)) ? editor._valueToMenu[item] : container.ValueToDisplay(item));
			tsi.Tag = container;
			tsi.Click += new EventHandler(ContextMenuClick_Choose);
			if (Helper.AreEqual(container.ValueToXml(item), value, container.Member.ValueDescription))
				tsi.Select();
			if (((ExpressionMemberCheck)container.Member).SeparatorAt(index))
				cms.Items.Add(new ToolStripSeparator());
		}

		private static ContextMenuStrip PrepareContextMenuStrip_DefaultCheck(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, bool sorted)
		{
			string value = container.GetValue(); 
			
			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

				if (sorted)
					foreach (string item in ((ExpressionMemberCheck)container.Member).Choices.OrderBy((x) => x))
						PrepareContextMenuStrip_DefaultCheck_private_loopIteration(container, editor, editor.cmsValueSelector, value, item, -1);
				else
					for (int i = 0; i < ((ExpressionMemberCheck)container.Member).Choices.Count(); i++)
						PrepareContextMenuStrip_DefaultCheck_private_loopIteration(container, editor, editor.cmsValueSelector, value, ((ExpressionMemberCheck)container.Member).Choices[i], i);
				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, value);
            return editor.cmsValueSelector;
		}
		
		private static ContextMenuStrip PrepareContextMenuStrip_DefaultBool(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			string value = container.GetValue(); 
			
			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

				//Add all possible types, sorting by alphabet but putting named bunch first and Nameless Bunch second (NANASHI RENJUU!!! :)
				foreach (string item in new string[2] { container.ValueToDisplay("1"), container.ValueToDisplay("0") })
				{
					ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, value);
            return editor.cmsValueSelector;
		}

		private static ContextMenuStrip PrepareContextMenuStrip_SpecifiedListPlusGUI(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode, KeyValuePair<List<string>, List<string>> list)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				if (list.Key.Count == 0)
					return null;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = true;

				if (list.Value.Count == 0)
				{
					foreach (string item in list.Key)
					{
						ToolStripItem tsi = editor.cmsValueSelector.Items.Add(item);
						tsi.Tag = container;
						tsi.Click += ContextMenuClick_Choose;
					}
				}
				else
				{
					int i;

					for (i = 0; i < list.Value.Count; i++)
					{

						ToolStripMenuItem tsm = new ToolStripMenuItem(list.Value[i]);
						editor.cmsValueSelector.Items.Add(tsm);

						for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == list.Value.Count - 1 ? list.Key.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
						{
							string item = list.Key[j];

							ToolStripItem tsi = tsm.DropDownItems.Add(item);

							tsi.Tag = container;
							tsi.Click += ContextMenuClick_Choose;
						}
					}
				}

				editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor.cmsValueSelector.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

            AssignCMSContainer(editor.cmsValueSelector, container, container.GetValue(), true);
            return editor.cmsValueSelector;
		}

		private static ContextMenuStrip PrepareContextMenuStrip_TimerNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.TimerNamesList);
		}

		private static ContextMenuStrip PrepareContextMenuStrip_VariableNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.VariableNamesList);
		}

		private static ContextMenuStrip PrepareContextMenuStrip_NamedStationNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.StationNamesList);
		}

		private static ContextMenuStrip PrepareContextMenuStrip_SpecifiedNestedListPlusGUI(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode, Dictionary<string,List<string>> dict)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				bool empty = true;
				foreach (KeyValuePair<string, List<string>> kvp in dict)
					if (kvp.Value.Count != 0) empty = false;
				if (empty)
					return null;

				editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = true;

				//For each list of named objects that is stored in the dictionary
				foreach (KeyValuePair<string, List<string>> kvp in dict)
				{
					if (kvp.Value.Count == 0)
						continue;
					
						ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Key);
						editor.cmsValueSelector.Items.Add(tsm);

						//If they do not fit make a second nested menu inside the first one
						if (kvp.Value.Count > Settings.Current.NamesPerSubmenu)
						{
							int i;
							List<string> headers = new List<string>();
							for (i = 0; i < kvp.Value.Count / Settings.Current.NamesPerSubmenu; i++)
							{
								string first = kvp.Value[i * Settings.Current.NamesPerSubmenu];

								string last = kvp.Value[(i + 1) * Settings.Current.NamesPerSubmenu - 1];

								headers.Add(first + " - " + last);
							}
							if (i * Settings.Current.NamesPerSubmenu <= kvp.Value.Count - 1)
								headers.Add(kvp.Value[i * Settings.Current.NamesPerSubmenu] + " - " + kvp.Value[kvp.Value.Count - 1]);

							for (i = 0; i < headers.Count; i++)
							{

								ToolStripMenuItem tsm2 = (ToolStripMenuItem) tsm.DropDownItems.Add(headers[i]);
								editor.cmsValueSelector.Items.Add(tsm);

								for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == headers.Count - 1 ? kvp.Value.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
								{
									string item = kvp.Value[j];

									ToolStripItem tsi = tsm2.DropDownItems.Add(item);

									tsi.Tag = container;
									tsi.Click += ContextMenuClick_Choose;
								}
							}
						}
						else//If they fit in a single menu add everything into the first menu item
						{

							foreach (string item in kvp.Value)
							{
								ToolStripItem tsi = tsm.DropDownItems.Add(item);

								tsi.Tag = container;
								tsi.Click += ContextMenuClick_Choose;
							}
						}
				}

				editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor.cmsValueSelector.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, container.GetValue(), true);
            return editor.cmsValueSelector;
		}

		private static ContextMenuStrip PrepareContextMenuStrip_NamedAllNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			return PrepareContextMenuStrip_SpecifiedNestedListPlusGUI(container, editor, mode, Mission.Current.AllNamesLists);
		}

		private static ContextMenuStrip PrepareContextMenuStrip_HullIDList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor.cmsValueSelector == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor.cmsValueSelector = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor.cmsValueSelector.Items.Clear();
				editor.cmsValueSelector.Tag = false;

                ToolStripItem tsn1 = editor.cmsValueSelector.Items.Add("NULL");
                tsn1.Tag = container;
                tsn1.Click += new EventHandler(ContextMenuClick_Choose);
                editor.cmsValueSelector.Items.Add(new ToolStripSeparator());

                List<string> keys = Settings.VesselData.vesselList.Keys.ToList();

                if (keys.Count > Settings.Current.NamesPerSubmenu)
                {
                    int i;
                    List<string> headers = new List<string>();
                    for (i = 0; i < keys.Count / Settings.Current.NamesPerSubmenu; i++)
                    {
                        string first =  Settings.VesselData.VesselToString(keys[i * Settings.Current.NamesPerSubmenu]);

                        string last = Settings.VesselData.VesselToString(keys[(i + 1) * Settings.Current.NamesPerSubmenu - 1]);

                        headers.Add(first + " - " + last);
                    }
                    if (i * Settings.Current.NamesPerSubmenu <= keys.Count - 1)
                        headers.Add(Settings.VesselData.VesselToString(keys[i * Settings.Current.NamesPerSubmenu] )
                                  + " - " 
                                  + Settings.VesselData.VesselToString(keys[keys.Count - 1]));

                    for (i = 0; i < headers.Count; i++)
                    {
                        ToolStripMenuItem tsm2 = (ToolStripMenuItem)editor.cmsValueSelector.Items.Add(headers[i]);
                        editor.cmsValueSelector.Items.Add(tsm2);

                        for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == headers.Count - 1 ? keys.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
                        {
                            ToolStripItem tsi = tsm2.DropDownItems.Add(Settings.VesselData.VesselToString(keys[j]));

                            tsi.Tag = container;
                            tsi.Click += ContextMenuClick_Choose;
                        }
                    }
                }
                else
                {
                    foreach (string item in Settings.VesselData.vesselList.Keys)
                    {
                        ToolStripItem tsi = editor.cmsValueSelector.Items.Add(Settings.VesselData.VesselToString(item));
                        tsi.Tag = container;
                        tsi.Click += new EventHandler(ContextMenuClick_Choose);
                    }
                }

                if (Settings.VesselData.vesselList.Keys.Count > 0)
                {
                    editor.cmsValueSelector.Items.Add(new ToolStripSeparator());

                    // When we had a huge list of IDs, there was a purpose of having NULL entry both above and below.
                    // Now that we have separation into submenus, there is no need for that

                    //ToolStripItem tsn2 = editor.cmsValueSelector.Items.Add("NULL");
                    //tsn2.Tag = container;
                    //tsn2.Click += new EventHandler(ContextMenuClick_Choose);
                    
                    //editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
                }

				ToolStripItem tsl = editor.cmsValueSelector.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += new EventHandler(ContextMenuClick_Show_GUI);
				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor.cmsValueSelector, container, string.IsNullOrWhiteSpace(value) ? null : value); 
            return editor.cmsValueSelector;
		}

		#endregion
	  
		static ExpressionMemberValueEditor()
		{
			CMSFont = new Font("Segoe UI", 16);

			Nothing = new ExpressionMemberValueEditor(false, false);

			Label = new ExpressionMemberValueEditor(true, false);

			DefaultInteger = new ExpressionMemberValueEditor();

			DefaultBool = new ExpressionMemberValueEditor();
			DefaultBool.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultBool;

			DefaultDouble = new ExpressionMemberValueEditor();

			DefaultString = new ExpressionMemberValueEditor();

			DefaultBody = new ExpressionMemberValueEditor();

			CreateType = new ExpressionMemberValueEditor();
			CreateType.AddToDictionary("anomaly", "anomaly");
			CreateType.AddToDictionary("blackHole", "black hole");
			CreateType.AddToDictionary("monster", "monster");
			CreateType.AddToDictionary("player", "player");
			CreateType.AddToDictionary("neutral", "neutral");
			CreateType.AddToDictionary("station", "station");
			CreateType.AddToDictionary("enemy", "enemy");
			CreateType.AddToDictionary("genericMesh", "generic mesh");
			CreateType.AddToDictionary("whale", "whale");
			CreateType.AddToDictionary("asteroids", "asteroids");
			CreateType.AddToDictionary("nebulas", "nebulas");
			CreateType.AddToDictionary("mines", "mines");
			CreateType.PrepareContextMenuStripMethod = PrepareContextMenuStrip_CreateType;

			DefaultCheckUnsorted = new ExpressionMemberValueEditor();
			DefaultCheckUnsorted.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			DefaultCheckSorted = new ExpressionMemberValueEditor();
			DefaultCheckSorted.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckSorted;

			XmlNameActionCheck = new ExpressionMemberValueEditor_XmlName();
            XmlNameActionCheck.AddToDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles: ");
            XmlNameActionCheck.AddToDictionary("end_getting_keypresses_from", "End getting keypresses from consoles: ");
            XmlNameActionCheck.AddToDictionary("set_special", "Set");
            XmlNameActionCheck.AddToDictionary("set_side_value", "Set side");
            XmlNameActionCheck.AddToDictionary("set_ship_text", "Set text strings");
            XmlNameActionCheck.AddToDictionary("<destroy>", "Destroy");
			XmlNameActionCheck.AddToDictionary("clear_ai", "Clear AI command stack");
			XmlNameActionCheck.AddToDictionary("add_ai", "Add an AI command");
			XmlNameActionCheck.AddToDictionary("set_object_property", "Set property");
			XmlNameActionCheck.AddToDictionary("addto_object_property", "Add");
			XmlNameActionCheck.AddToDictionary("copy_object_property", "Copy property");
			XmlNameActionCheck.AddToDictionary("set_relative_position", "Set position");
			XmlNameActionCheck.AddToDictionary("incoming_comms_text", "Show incoming text message");
			XmlNameActionCheck.AddToDictionary("incoming_message", "Show incoming audio message");
			XmlNameActionCheck.AddToDictionary("warning_popup_message", "Show warning popup message");
			XmlNameActionCheck.AddToDictionary("big_message", "Show big message on main screen");
			XmlNameActionCheck.AddToDictionary("set_player_grid_damage", "Set player ship's damage");
			XmlNameActionCheck.AddToDictionary("play_sound_now", "Play sound on main screen");
			XmlNameActionCheck.AddToDictionary("set_damcon_members", "Set DamCon members");
			XmlNameActionCheck.AddToDictionary("log", "Log new entry");
			

			//We need to display both as same, so we have to go around using AddToDictionary, otherwise _valueToXml will have same key exception!
			XmlNameActionCheck._valueToDisplay.Add("set_fleet_property", "Set property");
			XmlNameActionCheck._valueToDisplay.Add("set_to_gm_position", "Set position");
            XmlNameActionCheck.AddToMenuDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles");
            XmlNameActionCheck.AddToMenuDictionary("end_getting_keypresses_from", "End getting keypresses from consoles");
            XmlNameActionCheck.AddToMenuDictionary("set_special", "Set ship's special values");
            XmlNameActionCheck.AddToMenuDictionary("set_side_value", "Set object's side");
            XmlNameActionCheck.AddToMenuDictionary("set_ship_text", "Set object's text");
			XmlNameActionCheck.AddToMenuDictionary("set_object_property", "Set property of object");
			XmlNameActionCheck.AddToMenuDictionary("copy_object_property", "Copy property of object");
			XmlNameActionCheck.AddToMenuDictionary("addto_object_property", "Add to property of object");
			XmlNameActionCheck.AddToMenuDictionary("set_fleet_property", "Set property of fleet");
			XmlNameActionCheck.AddToMenuDictionary("set_relative_position", "Set position relative to object");
			XmlNameActionCheck.AddToMenuDictionary("add_ai", "Add AI command");
			XmlNameActionCheck.AddToMenuDictionary("clear_ai", "Clear AI commands");
			XmlNameActionCheck.AddToMenuDictionary("set_to_gm_position", "Set position relative to GM position");
			XmlNameActionCheck.AddToMenuDictionary("create", "Create object(s)");
			XmlNameActionCheck.AddToMenuDictionary("<destroy>", "Destroy object(s)");
			XmlNameActionCheck.AddToMenuDictionary("direct", "Direct object to object / position");
			
			//XmlNameCheck.AddToMenuDictionary("set_timer",			"Set / start timer"); //Looks BAD


			XmlNameActionCheck.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			XmlNameConditionCheck = new ExpressionMemberValueEditor_XmlName();
			XmlNameConditionCheck.AddToDictionary("if_variable", "Variable");
			XmlNameConditionCheck.AddToDictionary("if_timer_finished", "Timer");
			XmlNameConditionCheck.AddToDictionary("if_damcon_members", "Amount of DamCon members");
			XmlNameConditionCheck.AddToDictionary("if_fleet_count", "Ship count");
			XmlNameConditionCheck.AddToDictionary("if_docked", "Player ship is docked");
			XmlNameConditionCheck.AddToDictionary("if_player_is_targeting", "Player ship is targeting");
			XmlNameConditionCheck.AddToDictionary("<existance>", "Object");
			XmlNameConditionCheck.AddToDictionary("if_object_property", "Property");
			XmlNameConditionCheck._valueToDisplay.Add("<location>", "Object");
			XmlNameConditionCheck.AddToDictionary("if_distance", "Distance");
			XmlNameConditionCheck.AddToDictionary("if_difficulty", "Difficulty level");
			XmlNameConditionCheck.AddToDictionary("if_gm_key", "GM pressed");
			XmlNameConditionCheck.AddToDictionary("if_client_key", "Client pressed");
			//variable
			XmlNameConditionCheck.AddToMenuDictionary("if_timer_finished", "Timer has finished");
			XmlNameConditionCheck.AddToMenuDictionary("if_damcon_members", "Amount of DamCon members");
			XmlNameConditionCheck.AddToMenuDictionary("if_fleet_count", "Ship count (in fleet)");
			//docked
			//targeting
			XmlNameConditionCheck.AddToMenuDictionary("<existance>", "Object exists / doesn't");
			XmlNameConditionCheck.AddToMenuDictionary("if_object_property", "Object property");
			XmlNameConditionCheck.AddToMenuDictionary("<location>", "Object is located");
			XmlNameConditionCheck.AddToMenuDictionary("if_distance", "Distance between objects");
			XmlNameConditionCheck.AddToMenuDictionary("if_gm_key", "GM pressed a key");
			XmlNameConditionCheck.AddToMenuDictionary("if_client_key", "Client pressed a key");

			XmlNameConditionCheck.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			SetVariableCheck = new ExpressionMemberValueEditor();
			SetVariableCheck.AddToDictionary("<to>", "to");
			SetVariableCheck.AddToMenuDictionary("<to>", "to an exact value");
			SetVariableCheck.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			AIType = new ExpressionMemberValueEditor();
			AIType.AddToDictionary("TRY_TO_BECOME_LEADER", "TRY TO BECOME LEADER");
			AIType.AddToDictionary("CHASE_PLAYER", "CHASE PLAYER ");
			AIType.AddToDictionary("CHASE_AI_SHIP", "CHASE AI SHIP ");
			AIType.AddToDictionary("CHASE_STATION", "CHASE STATION ");
			AIType.AddToDictionary("CHASE_WHALE", "CHASE WHALE ");
			AIType.AddToDictionary("AVOID_WHALE", "AVOID WHALE ");
			AIType.AddToDictionary("AVOID_BLACK_HOLE", "AVOID BLACK HOLE ");
			AIType.AddToDictionary("CHASE_ANGER", "CHASE ANGER");
			AIType.AddToDictionary("CHASE_FLEET", "CHASE FLEET ");
			AIType.AddToDictionary("FOLLOW_LEADER", "FOLLOW LEADER");
			AIType.AddToDictionary("FOLLOW_COMMS_ORDERS", "FOLLOW COMMS ORDERS");
			//AIType.AddToDictionary("FOLLOW_NEUTRAL_PATH", "FOLLOW NEUTRAL PATH");
			AIType.AddToDictionary("LEADER_LEADS", "LEADER LEADS");
			AIType.AddToDictionary("ELITE_AI", "ELITE AI");
			AIType.AddToDictionary("DIR_THROTTLE", "DIR THROTTLE ");
			AIType.AddToDictionary("POINT_THROTTLE", "POINT THROTTLE ");
			AIType.AddToDictionary("TARGET_THROTTLE", "TARGET THROTTLE ");
			AIType.AddToDictionary("ATTACK", "ATTACK ");
			AIType.AddToDictionary("DEFEND", "DEFEND ");
			AIType.AddToDictionary("PROCEED_TO_EXIT", "PROCEED TO EXIT");
			AIType.AddToDictionary("FIGHTER_BINGO", "FIGHTER BINGO");
			AIType.AddToDictionary("LAUNCH_FIGHTERS", "LAUNCH FIGHTERS ");
			AIType.AddToDictionary("GUARD_STATION", "GUARD STATION ");
			AIType.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			PropertyObject = new ExpressionMemberValueEditor();
			PropertyObject.AddToDictionary("positionX", "positionX");
			PropertyObject.AddToDictionary("positionY", "positionY");
			PropertyObject.AddToDictionary("positionZ", "positionZ");
			PropertyObject.AddToDictionary("deltaX", "deltaX");
			PropertyObject.AddToDictionary("deltaY", "deltaY");
			PropertyObject.AddToDictionary("deltaZ", "deltaZ");
			PropertyObject.AddToDictionary("angle", "angle");
			PropertyObject.AddToDictionary("pitch", "pitch");
			PropertyObject.AddToDictionary("roll", "roll");
            PropertyObject.AddToDictionary("sideValue", "sideValue");
            
			PropertyObject.NewMenuGroup("All objects");
			PropertyObject.AddToDictionary("blocksShotFlag", "blocksShotFlag");
			PropertyObject.AddToDictionary("pushRadius", "pushRadius");
			PropertyObject.AddToDictionary("pitchDelta", "pitchDelta");
			PropertyObject.AddToDictionary("rollDelta", "rollDelta");
			PropertyObject.AddToDictionary("angleDelta", "angleDelta");
			PropertyObject.AddToDictionary("artScale", "artScale");
			PropertyObject.NewMenuGroup("Generic meshes");
			PropertyObject.AddToDictionary("shieldState", "shieldState");
			PropertyObject.AddToDictionary("canBuild", "canBuild");
			PropertyObject.AddToDictionary("missileStoresHoming", "missileStoresHoming");
			PropertyObject.AddToDictionary("missileStoresNuke", "missileStoresNuke");
			PropertyObject.AddToDictionary("missileStoresMine", "missileStoresMine");
			PropertyObject.AddToDictionary("missileStoresECM", "missileStoresECM");
			PropertyObject.NewMenuGroup("Stations");
			PropertyObject.AddToDictionary("throttle", "throttle");
			PropertyObject.AddToDictionary("steering", "steering");
			PropertyObject.AddToDictionary("topSpeed", "topSpeed");
			PropertyObject.AddToDictionary("turnRate", "turnRate");
			PropertyObject.AddToDictionary("shieldStateFront", "shieldStateFront");
			PropertyObject.AddToDictionary("shieldMaxStateFront", "shieldMaxStateFront");
			PropertyObject.AddToDictionary("shieldStateBack", "shieldStateBack");
			PropertyObject.AddToDictionary("shieldMaxStateBack", "shieldMaxStateBack");
			PropertyObject.AddToDictionary("shieldsOn", "shieldsOn");
			PropertyObject.AddToDictionary("triggersMines", "triggersMines");
			PropertyObject.AddToDictionary("systemDamageBeam", "systemDamageBeam");
			PropertyObject.AddToDictionary("systemDamageTorpedo", "systemDamageTorpedo");
			PropertyObject.AddToDictionary("systemDamageTactical", "systemDamageTactical");
			PropertyObject.AddToDictionary("systemDamageTurning", "systemDamageTurning");
			PropertyObject.AddToDictionary("systemDamageImpulse", "systemDamageImpulse");
			PropertyObject.AddToDictionary("systemDamageWarp", "systemDamageWarp");
			PropertyObject.AddToDictionary("systemDamageFrontShield", "systemDamageFrontShield");
			PropertyObject.AddToDictionary("systemDamageBackShield", "systemDamageBackShield");
			PropertyObject.AddToDictionary("shieldBandStrength0", "shieldBandStrength0");
			PropertyObject.AddToDictionary("shieldBandStrength1", "shieldBandStrength1");
			PropertyObject.AddToDictionary("shieldBandStrength2", "shieldBandStrength2");
			PropertyObject.AddToDictionary("shieldBandStrength3", "shieldBandStrength3");
			PropertyObject.AddToDictionary("shieldBandStrength4", "shieldBandStrength4");
			PropertyObject.NewMenuGroup("Shielded ships");
			PropertyObject.AddToDictionary("targetPointX", "targetPointX");
			PropertyObject.AddToDictionary("targetPointY", "targetPointY");
			PropertyObject.AddToDictionary("targetPointZ", "targetPointZ");
			PropertyObject.AddToDictionary("hasSurrendered", "hasSurrendered");
			PropertyObject.AddToDictionary("eliteAIType", "eliteAIType");
			PropertyObject.AddToDictionary("eliteAbilityBits", "eliteAbilityBits");
			PropertyObject.AddToDictionary("eliteAbilityState", "eliteAbilityState");
			PropertyObject.AddToDictionary("surrenderChance", "surrenderChance");
			PropertyObject.NewMenuGroup("Enemies");
			PropertyObject.AddToDictionary("exitPointX", "exitPointX");
			PropertyObject.AddToDictionary("exitPointY", "exitPointY");
			PropertyObject.AddToDictionary("exitPointZ", "exitPointZ");
			PropertyObject.NewMenuGroup("Neutrals");
			PropertyObject.AddToDictionary("countHoming", "countHoming");
			PropertyObject.AddToDictionary("countNuke", "countNuke");
			PropertyObject.AddToDictionary("countMine", "countMine");
			PropertyObject.AddToDictionary("countECM", "countECM");
			PropertyObject.AddToDictionary("energy", "energy");
			PropertyObject.AddToDictionary("warpState", "warpState");
			PropertyObject.AddToDictionary("currentRealSpeed", "currentRealSpeed");
			PropertyObject.NewMenuGroup("Players");
			PropertyObject.PrepareContextMenuStripMethod = PrepareContextMenuStrip_NestedList;

			PropertyFleet = new ExpressionMemberValueEditor();
			PropertyFleet.AddToDictionary("fleetSpacing", "spacing");
			PropertyFleet.AddToDictionary("fleetMaxRadius", "max radius");
			PropertyFleet.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			SkyboxIndex = new ExpressionMemberValueEditor();

            for (int i = 0; i <= 29; i++)
            {
                SkyboxIndex.AddToDictionary(i.ToString(), i.ToString());
            }
			
			SkyboxIndex.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListPlusGUI;

			Difficulty = new ExpressionMemberValueEditor();
            for (int i = 0; i <= 11; i++)
            {
                Difficulty.AddToDictionary(i.ToString(), i.ToString());
            }
			
			Difficulty.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			ShipSystem = new ExpressionMemberValueEditor();
			ShipSystem.AddToDictionary("systemBeam", "Primary Beam");
			ShipSystem.AddToDictionary("systemTorpedo", "Torpedo");
			ShipSystem.AddToDictionary("systemTactical", "Sensors");
			ShipSystem.AddToDictionary("systemTurning", "Maneuver");
			ShipSystem.AddToDictionary("systemImpulse", "Impulse");
			ShipSystem.AddToDictionary("systemWarp", "Warp");
			ShipSystem.AddToDictionary("systemFrontShield", "Front Shield");
			ShipSystem.AddToDictionary("systemBackShield", "Rear Shield");
			ShipSystem.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			CountFrom = new ExpressionMemberValueEditor();
			CountFrom.AddToDictionary("left", "left");
			CountFrom.AddToDictionary("top", "top");
			CountFrom.AddToDictionary("front", "front");
			CountFrom.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			DamageValue = new ExpressionMemberValueEditor_Percent();
			DamageValue.AddToDictionary("0.0", "0%");
			DamageValue.AddToDictionary("0.25", "25%");
			DamageValue.AddToDictionary("0.5", "50%");
			DamageValue.AddToDictionary("0.75", "75%");
			DamageValue.AddToDictionary("1.0", "100%");
			DamageValue.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListPlusGUI;

			TeamIndex = new ExpressionMemberValueEditor();
			TeamIndex.AddToDictionary("0", "0");
			TeamIndex.AddToDictionary("1", "1");
			TeamIndex.AddToDictionary("2", "2");
			TeamIndex.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			TeamAmount = new ExpressionMemberValueEditor();
			TeamAmount.AddToDictionary("0", "0");
			TeamAmount.AddToDictionary("1", "1");
			TeamAmount.AddToDictionary("2", "2");
			TeamAmount.AddToDictionary("3", "3");
			TeamAmount.AddToDictionary("4", "4");
			TeamAmount.AddToDictionary("5", "5");
			TeamAmount.AddToDictionary("6", "6");
			TeamAmount.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			TeamAmountF = new ExpressionMemberValueEditor();
			TeamAmountF.AddToDictionary("0", "0");
			TeamAmountF.AddToDictionary("1", "1");
			TeamAmountF.AddToDictionary("2", "2");
			TeamAmountF.AddToDictionary("3", "3");
			TeamAmountF.AddToDictionary("4", "4");
			TeamAmountF.AddToDictionary("5", "5");
			TeamAmountF.AddToDictionary("6", "6");
			TeamAmountF.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListPlusGUI;

            SpecialShipType = new ExpressionMemberValueEditor();
            SpecialShipType.AddToDictionary(null, "Unspecified");
            SpecialShipType.AddToDictionary("-1", "Nothing");
            SpecialShipType.AddToDictionary("0",  "Dilapidated");
            SpecialShipType.AddToDictionary("1",  "Upgraded");
            SpecialShipType.AddToDictionary("2",  "Overpowered");
            SpecialShipType.AddToDictionary("3",  "Underpowered");
            SpecialShipType.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            SpecialCapitainType = new ExpressionMemberValueEditor();
            SpecialCapitainType.AddToDictionary(null, "Unspecified");
            SpecialCapitainType.AddToDictionary("-1","Nothing");
            SpecialCapitainType.AddToDictionary("0", "Cowardly");
            SpecialCapitainType.AddToDictionary("1", "Brave");
            SpecialCapitainType.AddToDictionary("2", "Bombastic");
            SpecialCapitainType.AddToDictionary("3", "Seething");
            SpecialCapitainType.AddToDictionary("4", "Duplicitous");
            SpecialCapitainType.AddToDictionary("5", "Exceptional");
            SpecialCapitainType.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            Side = new ExpressionMemberValueEditor();
            Side.AddToDictionary(null, "Default");
            Side.AddToDictionary("0", "0 (No side)");
            Side.AddToDictionary("1", "1 (Enemy)");
            Side.AddToDictionary("2", "2 (Player)");
            Side.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListPlusGUIWithFirstSepearted;

			Comparator = new ExpressionMemberValueEditor();
			Comparator.AddToDictionary("GREATER", ">");
			Comparator.AddToDictionary("GREATER_EQUAL", ">=");
			Comparator.AddToDictionary("EQUALS", "=");
			Comparator.AddToDictionary("NOT", "!=");
			Comparator.AddToDictionary("LESS_EQUAL", "<=");
			Comparator.AddToDictionary("LESS", "<");
			Comparator.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			DistanceNebulaCheck = new ExpressionMemberValueEditor();
			DistanceNebulaCheck.AddToDictionary("anywhere2", "anywhere");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere", "anywhere on the map");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere2", "anywhere outside a nebula or up to ... if inside");
			DistanceNebulaCheck.AddToMenuDictionary("closer than", "closer than ... if outside a nebula or up to ... if inside");
			DistanceNebulaCheck.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			ConvertDirectCheck = new ExpressionMemberValueEditor();
			ConvertDirectCheck.AddToDictionary("Do nothing", "(Click here to convert)");
			ConvertDirectCheck.AddToDictionary("Convert to add_ai", "YOU SHOULD NEVER SEE THIS");
			ConvertDirectCheck.AddToMenuDictionary("Do nothing", "Do nothing");
			ConvertDirectCheck.AddToMenuDictionary("Convert to add_ai", "Convert to add_ai");
			ConvertDirectCheck.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultCheckUnsorted;

			TimerName = new ExpressionMemberValueEditor();
			TimerName.PrepareContextMenuStripMethod = PrepareContextMenuStrip_TimerNameList;

			VariableName = new ExpressionMemberValueEditor();
			VariableName.PrepareContextMenuStripMethod = PrepareContextMenuStrip_VariableNameList;

			NamedAllName = new ExpressionMemberValueEditor();
			NamedAllName.PrepareContextMenuStripMethod = PrepareContextMenuStrip_NamedAllNameList;
			
			NamedStationName = new ExpressionMemberValueEditor();
			NamedStationName.PrepareContextMenuStripMethod = PrepareContextMenuStrip_NamedStationNameList;

			ConsoleList = new ExpressionMemberValueEditor_ConsoleList();

			EliteAIType = new ExpressionMemberValueEditor();
			EliteAIType.AddToDictionary("0", "behave like a normal ship");
			EliteAIType._valueToDisplay.Add("0.0", "behave like a normal ship");
			EliteAIType.AddToDictionary("1", "follow nearest normal fleet");
			EliteAIType._valueToDisplay.Add("1.0", "follow nearest normal fleet");
			EliteAIType.AddToDictionary("2", "ignore everything except players");
			EliteAIType._valueToDisplay.Add("2.0", "ignore everything except players");
			EliteAIType.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			EliteAbilityBits = new ExpressionMemberValueEditor_AbilityBits();

			PlayerNames = new ExpressionMemberValueEditor();
			PlayerNames.AddToDictionary("Artemis", "Artemis");
			PlayerNames.AddToDictionary("Intrepid", "Intrepid");
			PlayerNames.AddToDictionary("Aegis", "Aegis");
			PlayerNames.AddToDictionary("Horatio", "Horatio");
			PlayerNames.AddToDictionary("Excalibur", "Excalibur");
			PlayerNames.AddToDictionary("Hera", "Hera");
			PlayerNames.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultListPlusGUI;

			WarpState = new ExpressionMemberValueEditor();
			WarpState.AddToDictionary("0", "0");
			WarpState.AddToDictionary("1", "1");
			WarpState.AddToDictionary("2", "2");
			WarpState.AddToDictionary("3", "3");
			WarpState.AddToDictionary("4", "4");
			WarpState.PrepareContextMenuStripMethod = PrepareContextMenuStrip_DefaultList;

			PathEditor = new ExpressionMemberValueEditor_PathEditor();

			HullID = new ExpressionMemberValueEditor_HullID();
			HullID.PrepareContextMenuStripMethod = PrepareContextMenuStrip_HullIDList;
	
			RaceKeys = new ExpressionMemberValueEditor_RaceKeys();

			HullKeys = new ExpressionMemberValueEditor_HullKeys();


		}
	}

	/// <summary>
	/// Editor for Xml Name check (aka main check / first check)
	/// </summary>
	public sealed class ExpressionMemberValueEditor_XmlName : ExpressionMemberValueEditor
	{
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

	/// <summary>
	/// 
	/// </summary>
	public sealed class ExpressionMemberValueEditor_Percent : ExpressionMemberValueEditor
	{
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

        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			string result = base.ValueToXml(value, type, min, max);
			if (result != value)
				return result;
			return value;
		}

	}

	/// <summary>
	/// Class for console list: a string of mhwesco
	/// </summary>
	public sealed class ExpressionMemberValueEditor_ConsoleList : ExpressionMemberValueEditor
	{
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

			if (result.Length > 0)
				return result.Substring(0, result.Length - 1);
			else
				return "None";
		}

        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			return value;
		}

		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
		public override void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogConsoleList.Show(container.Member.Name, container.GetValue());
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}

	/// <summary>
	/// Class for console list: a string of mhwesco
	/// </summary>
	public sealed class ExpressionMemberValueEditor_AbilityBits : ExpressionMemberValueEditor
	{
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
		{
			string result = "";

			if (value == null || !Helper.IntTryParse(value))
				return "[NO]";

			int bits = Helper.StringToInt(value);

			if ((bits & 1) == 1)        result += "[INV-MS] ";
			if ((bits & 2) == 2)        result += "[INV-LRS/TAC] ";
			if ((bits & 4) == 4)        result += "[CLOAK] ";
			if ((bits & 8) == 8)        result += "[HET] ";
			if ((bits & 16) == 16)      result += "[WARP] ";
			if ((bits & 32) == 32)      result += "[TELEPORT] ";
            if ((bits & 64) == 64)      result += "[TRACTOR] ";
            if ((bits & 128) == 128)    result += "[DRONE] ";
            if ((bits & 256) == 256)    result += "[ANTI-MINE] ";
            if ((bits & 512) == 512)    result += "[ANTI-TORP] ";
            if ((bits & 1024) == 1024)  result += "[ANTI-SHLD] ";

			if (result.Length > 0)
				return result.Substring(0, result.Length - 1);
			else
				return "[NO]";
		}

        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			return value;
		}

		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
        public override void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogAbilityBits.Show(container.Member.Name, container.GetValue());
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}

	public sealed class ExpressionMemberValueEditor_PathEditor : ExpressionMemberValueEditor
	{
		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
		public override void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogSimple.Show(container.Member.Name, description, container.GetValue(), container.Member.Mandatory, def, true);
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ExpressionMemberValueEditor_HullID : ExpressionMemberValueEditor
	{
        public override string ValueToDisplay(string value, ExpressionMemberValueType type, object min, object max)
		{
			if (string.IsNullOrWhiteSpace(value))
				return value;
			if (!Settings.VesselData.vesselList.ContainsKey(value))
				return "[" + value + "] Vessel does not exist";
			return Settings.VesselData.VesselToString(value);
		}

        public override string ValueToXml(string value, ExpressionMemberValueType type, object min, object max)
		{
			string result = value;
			int tmp;
				
			if (string.IsNullOrWhiteSpace(result)) 
				return null;
				
			if (Helper.IntTryParse(result, out tmp)) 
				return result;
			result = result.Replace("[", "").Replace(" ","");
			if (result.IndexOf("]") == -1)
				return null;
			result = result.Substring(0, result.IndexOf("]"));
			if (Helper.IntTryParse(result, out tmp))
				return result;
				
			return null;
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ExpressionMemberValueEditor_RaceKeys : ExpressionMemberValueEditor
	{
		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
        public override void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogRHKeys.Show(container.Member.Name, container.GetValue(), DialogRHKeysMode.RaceNamesKeys);
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ExpressionMemberValueEditor_HullKeys : ExpressionMemberValueEditor
	{
		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
        public override void ShowEditingGUI(ExpressionMemberContainer container, ExpressionMemberValueDescription description, string def)
		{
			KeyValuePair<bool, string> result = DialogRHKeys.Show(container.Member.Name, container.GetValue(), DialogRHKeysMode.HullBroadTypesClassNames);
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}
}