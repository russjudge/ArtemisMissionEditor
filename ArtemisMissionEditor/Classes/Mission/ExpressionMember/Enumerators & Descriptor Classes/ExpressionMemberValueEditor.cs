using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	public class ExpressionMemberValueEditor
	{
		/// <summary>
		/// Means it wont appear in the editor visually at all
		/// </summary>
		public static EMVE Nothing;
		/// <summary>
		/// Means it will appear as an uneditable label
		/// </summary>
		public static EMVE Label;
		public static EMVE DefaultInteger;
		public static EMVE DefaultBool;
		public static EMVE DefaultDouble;
		public static EMVE DefaultString;
		public static EMVE DefaultBody; 
		public static EMVE CreateType;
		public static EMVE DefaultCheckUnsorted;
		public static EMVE DefaultCheckSorted;
		public static EMVE XmlNameActionCheck;
		public static EMVE XmlNameConditionCheck;
		public static EMVE SetVariableCheck;
		public static EMVE AIType;
		public static EMVE PropertyObject;
		public static EMVE PropertyFleet;
		public static EMVE SkyboxIndex;
		public static EMVE Difficulty;
		public static EMVE ShipSystem;
		public static EMVE CountFrom;
		public static EMVE DamageValue;
		public static EMVE TeamIndex;
		public static EMVE TeamAmount;
		public static EMVE TeamAmountF;
        public static EMVE SpecialShipType;
        public static EMVE SpecialCapitainType;
        public static EMVE Side;
		public static EMVE Comparator;
		public static EMVE DistanceNebulaCheck;
		public static EMVE ConvertDirectCheck;
		public static EMVE TimerName;
		public static EMVE VariableName;
		public static EMVE NamedAllName;
		public static EMVE NamedStationName;
		public static EMVE ConsoleList;
		public static EMVE EliteAIType;
		public static EMVE EliteAbilityBits;
		public static EMVE PlayerNames;
		public static EMVE WarpState;
		public static EMVE PathEditor;
		public static EMVE HullID;
		public static EMVE RaceKeys;
		public static EMVE HullKeys;

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
		private Dictionary<int, string> _menuGroups;
		private List<string> _menuItems;
		private void NewMenuGroup(string title)
		{
			_menuGroups.Add(_valueToXml.Count, title);
		}

		public Dictionary<string, string> ValueToDisplayList { get { return _valueToDisplay; } }
		private Dictionary<string, string> _valueToDisplay;
		private Dictionary<string, string> _valueToXml;
		private void AddToDictionary(string xml, string display)
		{
			if (xml!=null)
                _valueToDisplay.Add(xml, display);
			_valueToXml.Add(display, xml);
			_menuItems.Add(display);
		}

		private Dictionary<string, string> _valueToMenu;
		private Dictionary<string, string> _valueFromMenu;
		private void AddToMenuDictionary(string xml, string menu)
		{
			_valueToMenu.Add(xml, menu);
			_valueFromMenu.Add(menu, xml);
		}

		public virtual string ValueToDisplay(string value, EMVT type, object min, object max)
		{
			switch (type)
			{
				case EMVT.VarBool:
					int tmpBool;
					if (!Helper.IntTryParse(value, out tmpBool))
						tmpBool = 0;
					if (min != null && max != null)
						value = tmpBool != 0 ? max.ToString() : min.ToString();
					else
						value = tmpBool != 0 ? "true" : "false";
					break;
				case EMVT.VarInteger:
					break;
				case EMVT.VarDouble:
					break;
				case EMVT.VarString:
					break;
				case EMVT.Body:
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

		public virtual string ValueToXml(string value, EMVT type, object min, object max)
		{
			switch (type)
			{
				case EMVT.VarBool:
					if ((min != null && value == min.ToString()) || (min == null && value == "false"))
						value = "0";
					if ((max != null && value == max.ToString()) || (max == null && value == "true"))
						value = "1";
					break;
				case EMVT.VarInteger:
					break;
				case EMVT.VarDouble:
					break;
				case EMVT.VarString:
					break;
				case EMVT.Body:
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
		public virtual void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
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
			_menuGroups = new Dictionary<int, string>();
			_menuItems = new List<string>();

			_prepareCMS = (ExpressionMemberContainer i, EMVE j, EditorActivationMode mode) => null;
			_cms = null;
			_lastUser = null;
		}

		public ContextMenuStrip PrepareCMS(ExpressionMemberContainer container, EditorActivationMode mode)
		{
			return _prepareCMS(container, this, mode);
		}

		private Func<ExpressionMemberContainer, EMVE, EditorActivationMode, ContextMenuStrip> _prepareCMS;

		private ContextMenuStrip _cms;
		private EMVD _lastUser;
		public void InvalidateCMS() { _cms = null; }

		private static Font CMSFont;
		protected void InitCMS()
		{
			_cms.Font = CMSFont;
			_cms.ShowImageMargin = false;
		}
		/// <summary> Required because otherwise there is no way to deselect the menu items!?! Micro$oft FAIL! </summary>
		protected void ShowHideCMS()
		{
			_cms.Show();
			_cms.Close();
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

			foreach (ToolStripItem innerItem in ((ToolStripMenuItem)item).DropDownItems)
				AssignCMSContainer_private_RecursivelyAssign(innerItem, container, value);

			item.Tag = container;
			if (Helper.AreEqual(container.ValueToXml(item.Text), value, container.Member.ValueDescription))
				item.Select();
		}

		private static void AssignCMSContainer(ContextMenuStrip cms, ExpressionMemberContainer container, string value, bool selectGUI=false)
		{
			foreach (ToolStripItem item in cms.Items)
				AssignCMSContainer_private_RecursivelyAssign(item, container, value);

			if (selectGUI)
				cms.Items[cms.Items.Count - 1].Select();
		}

		#region PrepareCMS functions

		private static ContextMenuStrip PrepareCMS_CreateType(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			string value = container.GetValue();
			
			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor._cms.Items.Clear();
				editor._cms.Tag = false;

				//Add all possible types, sorting by alphabet but putting named bunch first and Nameless Bunch second (NANASHI RENJUU!!! :)
				foreach (string item in editor._valueToXml.Keys.OrderBy((x) => (x[x.Length - 1] == 's' ? "b" : "a") + x))
				{
					ToolStripItem tsi = editor._cms.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}
				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, value);return editor._cms;
		}

		private static ContextMenuStrip PrepareCMS_DefaultList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor._cms == null || editor._lastUser!=container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
				editor._cms.Items.Clear();
				editor._cms.Tag = false;

				foreach (string item in editor._valueToXml.Keys)
				{
					ToolStripItem tsi = editor._cms.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container,value);return editor._cms;
		}

        private static ContextMenuStrip PrepareCMS_DefaultListWithFirstSeparated(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
            {
                editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor._cms.Items.Clear();
                editor._cms.Tag = false;

                bool doOnce = true;
                foreach (string item in editor._valueToXml.Keys)
                {
                    ToolStripItem tsi = editor._cms.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClick_Choose;

                    if (doOnce)
                    {
                        doOnce = false;
                        editor._cms.Items.Add(new ToolStripSeparator());
                    }
                }

                editor.ShowHideCMS();
            }

            AssignCMSContainer(editor._cms, container, value); return editor._cms;
        }

		private static ContextMenuStrip PrepareCMS_DefaultListPlusGUI(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor._cms == null || editor._lastUser!=container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
				editor._cms.Items.Clear();
				editor._cms.Tag = true;

				foreach (string item in editor._valueToXml.Keys)
				{
					ToolStripItem tsi = editor._cms.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor._cms.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor._cms.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, "", true);return editor._cms;
		}

        private static ContextMenuStrip PrepareCMS_DefaultListPlusGUIWithFirstSepearted(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
        {
            if (mode == EditorActivationMode.ForceGUI)
                return null;

            if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
            {
                editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor._cms.Items.Clear();
                editor._cms.Tag = true;

                bool doOnce = true;
                foreach (string item in editor._valueToXml.Keys)
                {
                    ToolStripItem tsi = editor._cms.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClick_Choose;

                    if (doOnce)
                    {
                        doOnce = false;
                        editor._cms.Items.Add(new ToolStripSeparator());
                    }
                }

                editor._cms.Items.Add(new ToolStripSeparator());
                ToolStripItem tsl = editor._cms.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClick_Show_GUI;

                editor.ShowHideCMS();
            }

            AssignCMSContainer(editor._cms, container, "", true); return editor._cms;
        }

		private static ContextMenuStrip PrepareCMS_NestedList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor._cms == null || editor._lastUser!=container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor._cms.Items.Clear();
				editor._cms.Tag = false;

				int i = 0;
				foreach (KeyValuePair<int, string> kvp in editor._menuGroups.OrderBy((KeyValuePair<int, string> x) => x.Key))
				{
					ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Value);
					editor._cms.Items.Add(tsm);

					while (i < kvp.Key)
					{
						string item = editor._menuItems[i++];

						ToolStripItem tsi = tsm.DropDownItems.Add(item);

						tsi.Tag = container;
						tsi.Click += ContextMenuClick_Choose;
					}
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, value);return editor._cms;
		}

		private static ContextMenuStrip PrepareCMS_DefaultCheckUnsorted(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_DefaultCheck(container, editor, false);
		}

		private static ContextMenuStrip PrepareCMS_DefaultCheckSorted(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_DefaultCheck(container, editor, true);
		}

		private static void				PrepareCMS_DefaultCheck_private_loopIteration(ExpressionMemberContainer container, EMVE editor, ContextMenuStrip cms, string value, string item, int index)
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

		private static ContextMenuStrip PrepareCMS_DefaultCheck(ExpressionMemberContainer container, EMVE editor, bool sorted)
		{
			string value = container.GetValue(); 
			
			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor._cms.Items.Clear();
				editor._cms.Tag = false;

				if (sorted)
					foreach (string item in ((ExpressionMemberCheck)container.Member).Choices.OrderBy((x) => x))
						PrepareCMS_DefaultCheck_private_loopIteration(container, editor, editor._cms, value, item, -1);
				else
					for (int i = 0; i < ((ExpressionMemberCheck)container.Member).Choices.Count(); i++)
						PrepareCMS_DefaultCheck_private_loopIteration(container, editor, editor._cms, value, ((ExpressionMemberCheck)container.Member).Choices[i], i);
				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, value);return editor._cms;
		}
		
		private static ContextMenuStrip PrepareCMS_DefaultBool(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			string value = container.GetValue(); 
			
			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				editor._cms.Items.Clear();
				editor._cms.Tag = false;

				//Add all possible types, sorting by alphabet but putting named bunch first and Nameless Bunch second (NANASHI RENJUU!!! :)
				foreach (string item in new string[2] { container.ValueToDisplay("1"), container.ValueToDisplay("0") })
				{
					ToolStripItem tsi = editor._cms.Items.Add(item);
					tsi.Tag = container;
					tsi.Click += ContextMenuClick_Choose;
				}

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, value);return editor._cms;
		}

		private static ContextMenuStrip PrepareCMS_SpecifiedListPlusGUI(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode, KeyValuePair<List<string>, List<string>> list)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				if (list.Key.Count == 0)
					return null;

				editor._cms.Items.Clear();
				editor._cms.Tag = true;

				if (list.Value.Count == 0)
				{
					foreach (string item in list.Key)
					{
						ToolStripItem tsi = editor._cms.Items.Add(item);
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
						editor._cms.Items.Add(tsm);

						for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == list.Value.Count - 1 ? list.Key.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
						{
							string item = list.Key[j];

							ToolStripItem tsi = tsm.DropDownItems.Add(item);

							tsi.Tag = container;
							tsi.Click += ContextMenuClick_Choose;
						}
					}
				}

				editor._cms.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor._cms.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, "", true);return editor._cms;
		}

		private static ContextMenuStrip PrepareCMS_TimerNameList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.TimerNamesList);
		}

		private static ContextMenuStrip PrepareCMS_VariableNameList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.VariableNamesList);
		}

		private static ContextMenuStrip PrepareCMS_NamedStationNameList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_SpecifiedListPlusGUI(container, editor, mode, Mission.Current.StationNamesList);
		}

		private static ContextMenuStrip PrepareCMS_SpecifiedNestedListPlusGUI(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode, Dictionary<string,List<string>> dict)
		{
			if (mode == EditorActivationMode.ForceGUI)
				return null;

			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;

				bool empty = true;
				foreach (KeyValuePair<string, List<string>> kvp in dict)
					if (kvp.Value.Count != 0) empty = false;
				if (empty)
					return null;

				editor._cms.Items.Clear();
				editor._cms.Tag = true;

				//For each list of named objects that is stored in the dictionary
				foreach (KeyValuePair<string, List<string>> kvp in dict)
				{
					if (kvp.Value.Count == 0)
						continue;
					
						ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Key);
						editor._cms.Items.Add(tsm);

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
								editor._cms.Items.Add(tsm);

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

				editor._cms.Items.Add(new ToolStripSeparator());
				ToolStripItem tsl = editor._cms.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += ContextMenuClick_Show_GUI;

				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, "", true);return editor._cms;
		}

		private static ContextMenuStrip PrepareCMS_NamedAllNameList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			return PrepareCMS_SpecifiedNestedListPlusGUI(container, editor, mode, Mission.Current.AllNamesLists);
		}

		private static ContextMenuStrip PrepareCMS_HullIDList(ExpressionMemberContainer container, EMVE editor, EditorActivationMode mode)
		{
			string value = container.GetValue();

			if (editor._cms == null || editor._lastUser != container.Member.ValueDescription)
			{
				editor._cms = new ContextMenuStrip(); editor.InitCMS(); editor._lastUser = container.Member.ValueDescription;
                editor._cms.Items.Clear();
				editor._cms.Tag = false;

                ToolStripItem tsn1 = editor._cms.Items.Add("NULL");
                tsn1.Tag = container;
                tsn1.Click += new EventHandler(ContextMenuClick_Choose);
                
                editor._cms.Items.Add(new ToolStripSeparator());
				
				foreach (string item in Settings.VesselData.vesselList.Keys)
				{
					ToolStripItem tsi = editor._cms.Items.Add(Settings.VesselData.VesselToString(item));
					tsi.Tag = container;
					tsi.Click += new EventHandler(ContextMenuClick_Choose);
				}

                if (Settings.VesselData.vesselList.Keys.Count > 0)
                {
                    editor._cms.Items.Add(new ToolStripSeparator());

                    ToolStripItem tsn2 = editor._cms.Items.Add("NULL");
                    tsn2.Tag = container;
                    tsn2.Click += new EventHandler(ContextMenuClick_Choose);
                    
                    editor._cms.Items.Add(new ToolStripSeparator());
                }

				ToolStripItem tsl = editor._cms.Items.Add("Input value...");
				tsl.Tag = container;
				tsl.Click += new EventHandler(ContextMenuClick_Show_GUI);

				
				editor.ShowHideCMS();
			}

			AssignCMSContainer(editor._cms, container, string.IsNullOrWhiteSpace(value) ? null : value); return editor._cms;
		}

		#endregion
	  
		static ExpressionMemberValueEditor()
		{
			CMSFont = new Font("Segoe UI", 16);

			Nothing = new EMVE(false, false);

			Label = new EMVE(true, false);

			DefaultInteger = new EMVE();

			DefaultBool = new EMVE();
			DefaultBool._prepareCMS = PrepareCMS_DefaultBool;

			DefaultDouble = new EMVE();

			DefaultString = new EMVE();

			DefaultBody = new EMVE();

			CreateType = new EMVE();
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
			CreateType._prepareCMS = PrepareCMS_CreateType;

			DefaultCheckUnsorted = new EMVE();
			DefaultCheckUnsorted._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

			DefaultCheckSorted = new EMVE();
			DefaultCheckSorted._prepareCMS = PrepareCMS_DefaultCheckSorted;

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


			XmlNameActionCheck._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

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

			XmlNameConditionCheck._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

			SetVariableCheck = new EMVE();
			SetVariableCheck.AddToDictionary("<to>", "to");
			SetVariableCheck.AddToMenuDictionary("<to>", "to an exact value");
			SetVariableCheck._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

			AIType = new EMVE();
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
			AIType._prepareCMS = PrepareCMS_DefaultList;

			PropertyObject = new EMVE();
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
			PropertyObject._prepareCMS = PrepareCMS_NestedList;

			PropertyFleet = new EMVE();
			PropertyFleet.AddToDictionary("fleetSpacing", "spacing");
			PropertyFleet.AddToDictionary("fleetMaxRadius", "max radius");
			PropertyFleet._prepareCMS = PrepareCMS_DefaultList;

			SkyboxIndex = new EMVE();

            for (int i = 0; i <= 29; i++)
            {
                SkyboxIndex.AddToDictionary(i.ToString(), i.ToString());
            }
			
			SkyboxIndex._prepareCMS = PrepareCMS_DefaultListPlusGUI;

			Difficulty = new EMVE();
            for (int i = 0; i <= 11; i++)
            {
                Difficulty.AddToDictionary(i.ToString(), i.ToString());
            }
			
			Difficulty._prepareCMS = PrepareCMS_DefaultList;

			ShipSystem = new EMVE();
			ShipSystem.AddToDictionary("systemBeam", "Primary Beam");
			ShipSystem.AddToDictionary("systemTorpedo", "Torpedo");
			ShipSystem.AddToDictionary("systemTactical", "Sensors");
			ShipSystem.AddToDictionary("systemTurning", "Maneuver");
			ShipSystem.AddToDictionary("systemImpulse", "Impulse");
			ShipSystem.AddToDictionary("systemWarp", "Warp");
			ShipSystem.AddToDictionary("systemFrontShield", "Front Shield");
			ShipSystem.AddToDictionary("systemBackShield", "Rear Shield");
			ShipSystem._prepareCMS = PrepareCMS_DefaultList;

			CountFrom = new EMVE();
			CountFrom.AddToDictionary("left", "left");
			CountFrom.AddToDictionary("top", "top");
			CountFrom.AddToDictionary("front", "front");
			CountFrom._prepareCMS = PrepareCMS_DefaultList;

			DamageValue = new ExpressionMemberValueEditor_Percent();
			DamageValue.AddToDictionary("0.0", "0%");
			DamageValue.AddToDictionary("0.25", "25%");
			DamageValue.AddToDictionary("0.5", "50%");
			DamageValue.AddToDictionary("0.75", "75%");
			DamageValue.AddToDictionary("1.0", "100%");
			DamageValue._prepareCMS = PrepareCMS_DefaultListPlusGUI;

			TeamIndex = new EMVE();
			TeamIndex.AddToDictionary("0", "0");
			TeamIndex.AddToDictionary("1", "1");
			TeamIndex.AddToDictionary("2", "2");
			TeamIndex._prepareCMS = PrepareCMS_DefaultList;

			TeamAmount = new EMVE();
			TeamAmount.AddToDictionary("0", "0");
			TeamAmount.AddToDictionary("1", "1");
			TeamAmount.AddToDictionary("2", "2");
			TeamAmount.AddToDictionary("3", "3");
			TeamAmount.AddToDictionary("4", "4");
			TeamAmount.AddToDictionary("5", "5");
			TeamAmount.AddToDictionary("6", "6");
			TeamAmount._prepareCMS = PrepareCMS_DefaultList;

			TeamAmountF = new EMVE();
			TeamAmountF.AddToDictionary("0", "0");
			TeamAmountF.AddToDictionary("1", "1");
			TeamAmountF.AddToDictionary("2", "2");
			TeamAmountF.AddToDictionary("3", "3");
			TeamAmountF.AddToDictionary("4", "4");
			TeamAmountF.AddToDictionary("5", "5");
			TeamAmountF.AddToDictionary("6", "6");
			TeamAmountF._prepareCMS = PrepareCMS_DefaultListPlusGUI;

            SpecialShipType = new EMVE();
            SpecialShipType.AddToDictionary(null, "Unspecified");
            SpecialShipType.AddToDictionary("-1", "Nothing");
            SpecialShipType.AddToDictionary("0",  "Dilapidated");
            SpecialShipType.AddToDictionary("1",  "Upgraded");
            SpecialShipType.AddToDictionary("2",  "Overpowered");
            SpecialShipType.AddToDictionary("3",  "Underpowered");
            SpecialShipType._prepareCMS = PrepareCMS_DefaultListWithFirstSeparated;

            SpecialCapitainType = new EMVE();
            SpecialCapitainType.AddToDictionary(null, "Unspecified");
            SpecialCapitainType.AddToDictionary("-1","Nothing");
            SpecialCapitainType.AddToDictionary("0", "Cowardly");
            SpecialCapitainType.AddToDictionary("1", "Brave");
            SpecialCapitainType.AddToDictionary("2", "Bombastic");
            SpecialCapitainType.AddToDictionary("3", "Seething");
            SpecialCapitainType.AddToDictionary("4", "Duplicitous");
            SpecialCapitainType.AddToDictionary("5", "Exceptional");
            SpecialCapitainType._prepareCMS = PrepareCMS_DefaultListWithFirstSeparated;

            Side = new EMVE();
            Side.AddToDictionary(null, "Default");
            Side.AddToDictionary("0", "0 (No side)");
            Side.AddToDictionary("1", "1 (Enemy)");
            Side.AddToDictionary("2", "2 (Player)");
            Side._prepareCMS = PrepareCMS_DefaultListPlusGUIWithFirstSepearted;

			Comparator = new EMVE();
			Comparator.AddToDictionary("GREATER", ">");
			Comparator.AddToDictionary("GREATER_EQUAL", ">=");
			Comparator.AddToDictionary("EQUALS", "=");
			Comparator.AddToDictionary("NOT", "!=");
			Comparator.AddToDictionary("LESS_EQUAL", "<=");
			Comparator.AddToDictionary("LESS", "<");
			Comparator._prepareCMS = PrepareCMS_DefaultList;

			DistanceNebulaCheck = new EMVE();
			DistanceNebulaCheck.AddToDictionary("anywhere2", "anywhere");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere", "anywhere on the map");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere2", "anywhere outside a nebula or up to ... if inside");
			DistanceNebulaCheck.AddToMenuDictionary("closer than", "closer than ... if outside a nebula or up to ... if inside");
			DistanceNebulaCheck._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

			ConvertDirectCheck = new EMVE();
			ConvertDirectCheck.AddToDictionary("Do nothing", "(Click here to convert)");
			ConvertDirectCheck.AddToDictionary("Convert to add_ai", "YOU SHOULD NEVER SEE THIS");
			ConvertDirectCheck.AddToMenuDictionary("Do nothing", "Do nothing");
			ConvertDirectCheck.AddToMenuDictionary("Convert to add_ai", "Convert to add_ai");
			ConvertDirectCheck._prepareCMS = PrepareCMS_DefaultCheckUnsorted;

			TimerName = new EMVE();
			TimerName._prepareCMS = PrepareCMS_TimerNameList;

			VariableName = new EMVE();
			VariableName._prepareCMS = PrepareCMS_VariableNameList;

			NamedAllName = new EMVE();
			NamedAllName._prepareCMS = PrepareCMS_NamedAllNameList;
			
			NamedStationName = new EMVE();
			NamedStationName._prepareCMS = PrepareCMS_NamedStationNameList;

			ConsoleList = new ExpressionMemberValueEditor_ConsoleList();

			EliteAIType = new EMVE();
			EliteAIType.AddToDictionary("0", "behave like a normal ship");
			EliteAIType._valueToDisplay.Add("0.0", "behave like a normal ship");
			EliteAIType.AddToDictionary("1", "follow nearest normal fleet");
			EliteAIType._valueToDisplay.Add("1.0", "follow nearest normal fleet");
			EliteAIType.AddToDictionary("2", "ignore everything except players");
			EliteAIType._valueToDisplay.Add("2.0", "ignore everything except players");
			EliteAIType._prepareCMS = PrepareCMS_DefaultList;

			EliteAbilityBits = new ExpressionMemberValueEditor_AbilityBits();

			PlayerNames = new EMVE();
			PlayerNames.AddToDictionary("Artemis", "Artemis");
			PlayerNames.AddToDictionary("Intrepid", "Intrepid");
			PlayerNames.AddToDictionary("Aegis", "Aegis");
			PlayerNames.AddToDictionary("Horatio", "Horatio");
			PlayerNames.AddToDictionary("Excalibur", "Excalibur");
			PlayerNames.AddToDictionary("Hera", "Hera");
			PlayerNames._prepareCMS = PrepareCMS_DefaultListPlusGUI;

			WarpState = new EMVE();
			WarpState.AddToDictionary("0", "0");
			WarpState.AddToDictionary("1", "1");
			WarpState.AddToDictionary("2", "2");
			WarpState.AddToDictionary("3", "3");
			WarpState.AddToDictionary("4", "4");
			WarpState._prepareCMS = PrepareCMS_DefaultList;

			PathEditor = new ExpressionMemberValueEditor_PathEditor();

			HullID = new ExpressionMemberValueEditor_HullID();
			HullID._prepareCMS = PrepareCMS_HullIDList;
	
			RaceKeys = new ExpressionMemberValueEditor_RaceKeys();

			HullKeys = new ExpressionMemberValueEditor_HullKeys();


		}
	}

	/// <summary>
	/// Editor for Xml Name check (aka main check / first check)
	/// </summary>
	public sealed class ExpressionMemberValueEditor_XmlName : ExpressionMemberValueEditor
	{
		public override string ValueToDisplay(string value, EMVT type, object min, object max)
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

		public override string ValueToXml(string value, EMVT type, object min, object max)
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
		public override string ValueToDisplay(string value, EMVT type, object min, object max)
		{
			string result = base.ValueToDisplay(value, type, min, max);
			if (result != value)
				return result;
			double tmp;
			Helper.DoubleTryParse(value, out tmp);
			value = Helper.DoubleToPercent(tmp);

			return value;
		}

		public override string ValueToXml(string value, EMVT type, object min, object max)
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
		public override string ValueToDisplay(string value, EMVT type, object min, object max)
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

		public override string ValueToXml(string value, EMVT type, object min, object max)
		{
			return value;
		}

		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
		public override void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
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
		public override string ValueToDisplay(string value, EMVT type, object min, object max)
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

		public override string ValueToXml(string value, EMVT type, object min, object max)
		{
			return value;
		}

		/// <summary>
		/// Shows a gui form to edit the container's expression member
		/// </summary>
		/// <param name="container"></param>
        public override void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
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
		public override void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
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
		public override string ValueToDisplay(string value, EMVT type, object min, object max)
		{
			if (string.IsNullOrWhiteSpace(value))
				return value;
			if (!Settings.VesselData.vesselList.ContainsKey(value))
				return "[" + value + "] Vessel does not exist";
			return Settings.VesselData.VesselToString(value);
		}

		public override string ValueToXml(string value, EMVT type, object min, object max)
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
        public override void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
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
        public override void ShowEditingGUI(ExpressionMemberContainer container, EMVD description, string def)
		{
			KeyValuePair<bool, string> result = DialogRHKeys.Show(container.Member.Name, container.GetValue(), DialogRHKeysMode.HullBroadTypesClassNames);
			if (result.Key)
				ValueChosen(container, result.Value);
		}
	}
}