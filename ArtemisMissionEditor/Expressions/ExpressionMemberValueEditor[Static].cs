using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    public partial class ExpressionMemberValueEditor : IDisposable
    {
        /// <summary>
        /// To be called when a value is chosen by the editor
        /// </summary>
        protected static void ValueChosen(ExpressionMemberContainer container, string value)
        {
            container.SetValue(container.Member.ValueToXml(value));
            Mission.Current.UpdateStatementTree();
            Mission.Current.RegisterChange("Expression member value changed");
        }

        /// <summary>
        /// Assigns value when chosen from context menu. 
        /// To be assigned as event handler for context menu item being clicked.
        /// </summary>
        public static void ContextMenuClickChoose(object sender, EventArgs e)
        {
            ValueChosen(((ExpressionMemberContainer)((ToolStripItem)sender).Tag), ((ToolStripItem)sender).Text);
        }

        /// <summary>
        /// Shows dialog window when chosen from context menu. 
        /// To be assigned as event handler for context menu item being clicked.
        /// </summary>
        public static void ContextMenuClickShowDialog(object sender, EventArgs e)
        {
            ((ExpressionMemberContainer)((ToolStripItem)sender).Tag).Member.ValueDescription.ShowEditingDialog(((ExpressionMemberContainer)((ToolStripItem)sender).Tag));
        }

        /// <summary>
        /// Assign ExpressionMemberContainer to each of Context Menu Strip's items. 
        /// Item with value equal to current also gets selected.
        /// </summary>
        /// <param name="cms">Context Menu Strip to assign to.</param>
        /// <param name="container">Container to be assigned.</param>
        /// <param name="currentValue">Item with this value will be selected.</param>
        /// <param name="selectDialogItem">If true, "Edit in Dialog" option will be selected by default.</param>
        public static void AssignContainerToContextMenuStrip(ContextMenuStrip cms, ExpressionMemberContainer container, string currentValue, bool selectDialogItem = false)
        {
            bool itemSelected = false;
            foreach (ToolStripItem item in cms.Items)
            {
                AssignContainerToContextMenuStrip_RecursivelyAssign(item, container, currentValue);
                itemSelected = itemSelected || item.Selected;
            }

            if (selectDialogItem && !itemSelected)
                cms.Items[cms.Items.Count - 1].Select();
        }

        /// <summary>
        /// Assign ExpressionMemberContainer to each of Context Menu Strip's items that are child to specified item. 
        /// Item with value equal to current also gets selected.
        /// Called from AssignContainerToContextMenuStrip and recursively calls itself.
        /// </summary>
        /// <param name="item">Current item.</param>
        /// <param name="container">Container to be asigned.</param>
        /// <param name="currentValue">Item with this value will be selected.</param>
        private static void AssignContainerToContextMenuStrip_RecursivelyAssign(ToolStripItem item, ExpressionMemberContainer container, string currentValue)
        {
            if (!(item is ToolStripMenuItem))
                return;

            bool itemSelected = false;
            foreach (ToolStripItem innerItem in ((ToolStripMenuItem)item).DropDownItems)
            {
                AssignContainerToContextMenuStrip_RecursivelyAssign(innerItem, container, currentValue);
                itemSelected = itemSelected || item.Selected;
            }

            item.Tag = container;
            if (container.Member.ValueDescription.AreEqual(container.Member.ValueToXml(item.Text), currentValue))
                item.Select();
        }

        #region PrepareContextMenuStrip Functions

        /// <summary>
        /// Default context menu strip, with no Dialog option and no separator for the first value.
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_DefaultListWithOptions(container, editor, mode, false, false);
        }

        /// <summary>
        /// Default context menu strip, with separator for the first value and no Dialog option.
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultListWithFirstSeparated(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_DefaultListWithOptions(container, editor, mode, false, true);
        }

        /// <summary>
        /// Default context menu strip, with Dialog option and no separator for the first value.
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultListPlusDialog(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_DefaultListWithOptions(container, editor, mode, true, false);
        }

        /// <summary>
        /// Default context menu strip, with Dialog option and separator for the first value.
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultListPlusDialogWithFirstSepearted(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_DefaultListWithOptions(container, editor, mode, true, true);
        }

        /// <summary>
        /// Default context menu strip, providing a choice of Dialog and first value separator options
        /// </summary>
        private static ContextMenuStrip PrepareContextMenuStrip_DefaultListWithOptions(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode, bool plusDialogItem, bool firstSeparated)
        {
            if (mode == ExpressionMemberValueEditorActivationMode.ForceDialog)
                return null;

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip();
                editor.InitContextMenuStrip();
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = plusDialogItem;

                bool doOnce = firstSeparated;
                foreach (string item in editor.DisplayValueToXml.Keys)
                {
                    ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClickChoose;

                    if (doOnce)
                    {
                        doOnce = false;
                        editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                    }
                }

                if (plusDialogItem)
                {
                    if (firstSeparated != true || editor.DisplayValueToXml.Keys.Count != 1)
                        editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                    ToolStripItem tsl = editor.ValueSelectorContextMenuStrip.Items.Add("Input value...");
                    tsl.Tag = container;
                    tsl.Click += ContextMenuClickShowDialog;
                }
                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, container.GetValue(), true);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Context menu strip with nested items (based on menu groups).
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_NestedList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip(); 
                editor.InitContextMenuStrip(); 
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = false;

                int i = 0;
                foreach (KeyValuePair<int, string> kvp in editor.MenuGroups.OrderBy((KeyValuePair<int, string> x) => x.Key))
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Value);
                    editor.ValueSelectorContextMenuStrip.Items.Add(tsm);

                    while (i < kvp.Key)
                    {
                        string item = editor.MenuItems[i++];

                        ToolStripItem tsi = tsm.DropDownItems.Add(item);

                        tsi.Tag = container;
                        tsi.Click += ContextMenuClickChoose;
                    }
                }

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, value);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Default context menu strip for checks
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultCheck(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip(); 
                editor.InitContextMenuStrip(); 
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = false;

                for (int i = 0; i < ((ExpressionMemberCheck)container.Member).Choices.Count(); i++)
                {
                    string item = ((ExpressionMemberCheck)container.Member).Choices[i];
                    if (container.Member.ValueToDisplay(item) == null)
                        continue;
                    ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add((editor.XmlValueToMenu.ContainsKey(item)) ? editor.XmlValueToMenu[item] : container.Member.ValueToDisplay(item));
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClickChoose;
                    if (container.Member.ValueDescription.AreEqual(container.Member.ValueToXml(item), value))
                        tsi.Select();
                    if (((ExpressionMemberCheck)container.Member).SeparatorAt(i))
                        editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                }

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, value);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Default context menu strip for booleans (yes/no)
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_DefaultBool(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip(); 
                editor.InitContextMenuStrip(); 
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = false;

                foreach (string item in new string[2] { container.Member.ValueToDisplay("1"), container.Member.ValueToDisplay("0") })
                {
                    ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClickChoose;
                }

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, value);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Context menu strip for timer names
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_TimerNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_SpecifiedListPlusDialog(container, editor, mode, Mission.Current.TimerNamesList);
        }

        /// <summary>
        /// Context menu strip for variable names
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_VariableNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_SpecifiedListPlusDialog(container, editor, mode, Mission.Current.VariableNamesList);
        }

        /// <summary>
        /// Context menu strip for station names
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_NamedStationNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_SpecifiedListPlusDialog(container, editor, mode, Mission.Current.StationNamesList);
        }

        /// <summary>
        /// Context menu strip for specified list (used by timers, variables and stations)
        /// </summary>
        private static ContextMenuStrip PrepareContextMenuStrip_SpecifiedListPlusDialog(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode, KeyValuePair<List<string>, List<string>> list)
        {
            if (mode == ExpressionMemberValueEditorActivationMode.ForceDialog)
                return null;
            if (list.Key.Count == 0)
                return null;

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip();
                editor.InitContextMenuStrip();
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = true;

                if (list.Value.Count == 0)
                {
                    foreach (string item in list.Key)
                    {
                        ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add(item);
                        tsi.Tag = container;
                        tsi.Click += ContextMenuClickChoose;
                    }
                }
                else
                {
                    int i;

                    for (i = 0; i < list.Value.Count; i++)
                    {

                        ToolStripMenuItem tsm = new ToolStripMenuItem(list.Value[i]);
                        editor.ValueSelectorContextMenuStrip.Items.Add(tsm);

                        for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == list.Value.Count - 1 ? list.Key.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
                        {
                            string item = list.Key[j];

                            ToolStripItem tsi = tsm.DropDownItems.Add(item);

                            tsi.Tag = container;
                            tsi.Click += ContextMenuClickChoose;
                        }
                    }
                }

                editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                ToolStripItem tsl = editor.ValueSelectorContextMenuStrip.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClickShowDialog;

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, container.GetValue(), true);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Context Menu Strip for names of named objects
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_NamedAllNameList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            return PrepareContextMenuStrip_SpecifiedNestedListPlusDialog(container, editor, mode, Mission.Current.AllNamesLists);
        }

        /// <summary>
        /// Context menu strip for nested specified list (used by named objects)
        /// </summary>
        private static ContextMenuStrip PrepareContextMenuStrip_SpecifiedNestedListPlusDialog(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode, Dictionary<string, List<string>> dict)
        {
            if (mode == ExpressionMemberValueEditorActivationMode.ForceDialog)
                return null;
            bool empty = true;
            foreach (KeyValuePair<string, List<string>> kvp in dict)
                if (kvp.Value.Count != 0) empty = false;
            if (empty)
                return null;

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip();
                editor.InitContextMenuStrip();
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = true;

                //For each list of named objects that is stored in the dictionary
                foreach (KeyValuePair<string, List<string>> kvp in dict)
                {
                    if (kvp.Value.Count == 0)
                        continue;

                    ToolStripMenuItem tsm = new ToolStripMenuItem(kvp.Key);
                    editor.ValueSelectorContextMenuStrip.Items.Add(tsm);

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
                            ToolStripMenuItem tsm2 = (ToolStripMenuItem)tsm.DropDownItems.Add(headers[i]);
                            editor.ValueSelectorContextMenuStrip.Items.Add(tsm);

                            for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == headers.Count - 1 ? kvp.Value.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
                            {
                                string item = kvp.Value[j];
                                ToolStripItem tsi = tsm2.DropDownItems.Add(item);
                                tsi.Tag = container;
                                tsi.Click += ContextMenuClickChoose;
                            }
                        }
                    }
                    else//If they fit in a single menu add everything into the first menu item
                    {
                        foreach (string item in kvp.Value)
                        {
                            ToolStripItem tsi = tsm.DropDownItems.Add(item);
                            tsi.Tag = container;
                            tsi.Click += ContextMenuClickChoose;
                        }
                    }
                }

                editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                ToolStripItem tsl = editor.ValueSelectorContextMenuStrip.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClickShowDialog;

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, container.GetValue(), true);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Context menu strip for HullID
        /// </summary>
        internal static ContextMenuStrip PrepareContextMenuStrip_HullIDList(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            string value = container.GetValue();

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu)
            {
                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip(); 
                editor.InitContextMenuStrip(); 
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = false;

                ToolStripItem tsn1 = editor.ValueSelectorContextMenuStrip.Items.Add("NULL");
                tsn1.Tag = container;
                tsn1.Click += ContextMenuClickChoose;
                editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());

                List<string> keys = VesselData.Current.VesselList.Keys.ToList();

                if (keys.Count > Settings.Current.NamesPerSubmenu)
                {
                    int i;
                    List<string> headers = new List<string>();
                    for (i = 0; i < keys.Count / Settings.Current.NamesPerSubmenu; i++)
                    {
                        string first = VesselData.Current.VesselToString(keys[i * Settings.Current.NamesPerSubmenu]);
                        string last = VesselData.Current.VesselToString(keys[(i + 1) * Settings.Current.NamesPerSubmenu - 1]);
                        headers.Add(first + " - " + last);
                    }
                    if (i * Settings.Current.NamesPerSubmenu <= keys.Count - 1)
                        headers.Add(VesselData.Current.VesselToString(keys[i * Settings.Current.NamesPerSubmenu])
                                  + " - "
                                  + VesselData.Current.VesselToString(keys[keys.Count - 1]));

                    for (i = 0; i < headers.Count; i++)
                    {
                        ToolStripMenuItem tsm2 = (ToolStripMenuItem)editor.ValueSelectorContextMenuStrip.Items.Add(headers[i]);
                        editor.ValueSelectorContextMenuStrip.Items.Add(tsm2);

                        for (int j = i * Settings.Current.NamesPerSubmenu; j < (i == headers.Count - 1 ? keys.Count : (i + 1) * Settings.Current.NamesPerSubmenu); j++)
                        {
                            ToolStripItem tsi = tsm2.DropDownItems.Add(VesselData.Current.VesselToString(keys[j]));
                            tsi.Tag = container;
                            tsi.Click += ContextMenuClickChoose;
                        }
                    }
                }
                else
                {
                    foreach (string item in VesselData.Current.VesselList.Keys)
                    {
                        ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add(VesselData.Current.VesselToString(item));
                        tsi.Tag = container;
                        tsi.Click += ContextMenuClickChoose;
                    }
                }

                if (VesselData.Current.VesselList.Keys.Count > 0)
                {
                    editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());

                    // When we had a huge list of IDs, there was a purpose of having NULL entry both above and below.
                    // Now that we have separation into submenus, there is no need for that

                    //ToolStripItem tsn2 = editor.cmsValueSelector.Items.Add("NULL");
                    //tsn2.Tag = container;
                    //tsn2.Click += ContextMenuClick_Choose;

                    //editor.cmsValueSelector.Items.Add(new ToolStripSeparator());
                }

                ToolStripItem tsl = editor.ValueSelectorContextMenuStrip.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClickShowDialog;

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, string.IsNullOrWhiteSpace(value) ? null : value);
            return editor.ValueSelectorContextMenuStrip;
        }

        /// <summary>
        /// Context menu strip for player names
        /// </summary>
        /// <param name="container"></param>
        /// <param name="editor"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        internal static ContextMenuStrip PrepareContextMenuStrip_PlayerListPlusDialog(ExpressionMemberContainer container, ExpressionMemberValueEditor editor, ExpressionMemberValueEditorActivationMode mode)
        {
            if (mode == ExpressionMemberValueEditorActivationMode.ForceDialog)
                return null;

            // Recreate the menu if player ship names changed since last time
            //TODO: This should (probably) be moved to an event as well!
            bool forceRecreate = true;
            if (editor.Tag != null && editor.Tag as string[] != null)
            {
                string[] prevNames = (string[])editor.Tag;
                string[] curNames = Mission.Current.PlayerShipNames;
                if (prevNames.Length == curNames.Length)
                {
                    bool identical = true;
                    for (int i = 0; identical && i < prevNames.Length; i++)
                        if (prevNames[i] != curNames[i])
                            identical = false;
                    if (identical)
                        forceRecreate = false;
                }
            }
            editor.Tag = Mission.Current.PlayerShipNames;

            if (editor.ValueSelectorContextMenuStrip == null || editor.LastUser != container.Member.ValueDescription || editor.ForceRecreateMenu || forceRecreate)
            {
                editor.XmlValueToDisplay.Clear();
                editor.DisplayValueToXml.Clear();
                editor.MenuItems.Clear();
                foreach (string playerShipName in Mission.Current.PlayerShipNames)
                    editor.AddToDictionary(playerShipName, playerShipName);

                editor.ValueSelectorContextMenuStrip = new ContextMenuStrip(); 
                editor.InitContextMenuStrip(); 
                editor.LastUser = container.Member.ValueDescription;
                editor.ValueSelectorContextMenuStrip.Items.Clear();
                editor.ValueSelectorContextMenuStrip.Tag = true;

                foreach (string item in editor.DisplayValueToXml.Keys)
                {
                    ToolStripItem tsi = editor.ValueSelectorContextMenuStrip.Items.Add(item);
                    tsi.Tag = container;
                    tsi.Click += ContextMenuClickChoose;
                }

                editor.ValueSelectorContextMenuStrip.Items.Add(new ToolStripSeparator());
                ToolStripItem tsl = editor.ValueSelectorContextMenuStrip.Items.Add("Input value...");
                tsl.Tag = container;
                tsl.Click += ContextMenuClickShowDialog;

                editor.ShowHideContextMenuStrip();
            }

            AssignContainerToContextMenuStrip(editor.ValueSelectorContextMenuStrip, container, container.GetValue(), true);
            return editor.ValueSelectorContextMenuStrip;
        }

        #endregion

    }

}
