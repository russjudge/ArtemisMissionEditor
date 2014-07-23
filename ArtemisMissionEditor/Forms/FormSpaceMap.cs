using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Drawing.Drawing2D;

namespace ArtemisMissionEditor
{
	public partial class FormSpaceMap : FormSerializeableToRegistry
    {
		private bool WasLoaded;

        private FormSpaceMap()
        {
            //BASE INIT            
            InitializeComponent();

            //Init objects
            pSpaceMap.AssignForm(this);
            pSpaceMap.AssignStatusToolStrip(_FSM_ss_Main);
            pSpaceMap.AssignPropertyGrid(this._FSM_pg_Properties);
            pSpaceMap.AssignNamelessObjectsListBox(_FSM_lb_NamelessObjects);
            pSpaceMap.AssignNamedObjectsListBox(this._FSM_lb_NamedObjects);

			WasLoaded = false;
        }

        private void _E_FSM_ms_Main_Settings_UseYNameless_CheckedChanged(object sender, EventArgs e)
        {
			if (!WasLoaded)
				return;
			Settings.Current.UseYForNameless = _FSM_ms_Main_Settings_UseYNameless.Checked;
			Settings.Save();
            pSpaceMap.Redraw();
        }

        private void _E_FSM_ms_Main_Settings_UseYNamed_CheckedChanged(object sender, EventArgs e)
        {
			if (!WasLoaded)
				return;
            Settings.Current.UseYForNamed = _FSM_ms_Main_Settings_UseYNamed.Checked;
			Settings.Save();
            pSpaceMap.Redraw();
        }

        private void _E_FSM_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (DialogResult==DialogResult.Cancel)
				pSpaceMap.ExecuteCommand(KeyCommands.FinishEditing);

			if (DialogResult == DialogResult.Cancel ||DialogResult == DialogResult.None)
			{
				e.Cancel = true;
				DialogResult = DialogResult.None;
				return;
			}

			SaveToRegistry();
        }

        private void _E_FSM_ms_Main_SpaceMap_Close_Click(object sender, EventArgs e)
        {
			pSpaceMap.ExecuteCommand(KeyCommands.FinishEditing);
        }

        public static SpaceMap AddViaSpaceMap(string bgXml)
        {
            using (FormSpaceMap form = CreateSpaceMapForm("Add new objects", new List<int>(), new List<int>(), "<input></input>", bgXml))
            {
                if (form.ShowDialog() != DialogResult.Yes)
                    return null;
                else
                    return form.pSpaceMap.SpaceMap;
            }
        }

        public static SpaceMap EditOnSpaceMap(List<int> namedList, List<int> namelessList, string editXml, string bgXml)
        {
            using (FormSpaceMap form = CreateSpaceMapForm("Edit or add objects", namedList, namelessList, editXml, bgXml))
            {
                if (form.ShowDialog() != DialogResult.Yes)
                    return null;
                else
                    return form.pSpaceMap.SpaceMap;
            }
        }

        private static FormSpaceMap CreateSpaceMapForm(string caption, List<int> namedList, List<int> namelessList, string editXml, string bgXml)
        {
            FormSpaceMap form = new FormSpaceMap();
           
            form.Text = caption;
            form.pSpaceMap.SpaceMap.FromXml(editXml);
			form.pSpaceMap.SpaceMap.FromXml(bgXml);
            form.pSpaceMap.SpaceMap.namedIdList = namedList;
            form.pSpaceMap.SpaceMap.namelessIdList = namelessList;
            form.pSpaceMap.SpaceMap.MarkAsImported();

            return form;
        }

        private void _E_FSM_ms_Main_SpaceMap_Accept_Click(object sender, EventArgs e)
        {
            pSpaceMap.ExecuteCommand(KeyCommands.FinishEditingForceYes);
        }

        private void _E_FSM_ms_Main_SpaceMap_Discard_Click(object sender, EventArgs e)
        {
            pSpaceMap.ExecuteCommand(KeyCommands.FinishEditingForceNo);
        }

		private void _E_FSM_Load(object sender, EventArgs e)
		{
			_mainSC = _FSM_sc_Main;
			_rightSC = _FSM_sc_Main_Right;
			ID = "FormSpaceMap";
			LoadFromRegistry();

			_FSM_ms_Main_Settings_UseYNamed.Checked = Settings.Current.UseYForNamed;
			_FSM_ms_Main_Settings_UseYNameless.Checked = Settings.Current.UseYForNameless;
			_FSM_ms_Main_Settings_MarkWhitespaceNames.Checked = Settings.Current.MarkWhitespaceNamesOnSpaceMap;
			pSpaceMap.RegisterChange("Form Space Map Loaded",false,true);
			pSpaceMap.UpdateObjectLists();
			pSpaceMap.UpdateObjectsText();
			pSpaceMap.Reset();
			pSpaceMap.SetFocus();
			WasLoaded = true;
		}

		private void _E_FSM_ms_Main_Settings_MarkWhitespaceNames_CheckStateChanged(object sender, EventArgs e)
		{
			if (!WasLoaded)
				return;
			Settings.Current.MarkWhitespaceNamesOnSpaceMap = _FSM_ms_Main_Settings_MarkWhitespaceNames.Checked;
			Settings.Save();
			pSpaceMap.Redraw();
		}

        private void _E_FSM_ms_Main_Edit_Focus_Click(object sender, EventArgs e)
        {
            pSpaceMap.ExecuteCommand(KeyCommands.FocusMap);
        }

		private void _E_FSM_ms_Main_Edit_DropDownOpening(object sender, EventArgs e)
		{
			_FSM_ms_Main_Edit_Undo.Enabled = pSpaceMap.SpaceMap.UndoDescription != null;
			_FSM_ms_Main_Edit_Undo.Text = !_FSM_ms_Main_Edit_Undo.Enabled ? "Undo" : "Undo \"" + pSpaceMap.SpaceMap.UndoDescription + "\"";
			_FSM_ms_Main_Edit_Redo.Enabled = pSpaceMap.SpaceMap.RedoDescription != null;
			_FSM_ms_Main_Edit_Redo.Text = !_FSM_ms_Main_Edit_Redo.Enabled ? "Redo" : "Redo \"" + pSpaceMap.SpaceMap.RedoDescription + "\"";

			int i = 0;
            int maxi = pSpaceMap.SpaceMap._undoStack.Count-1;
            _FSM_ms_Main_Edit_UndoList.DropDownItems.Clear();
            
			foreach (SpaceMapSavedState item in pSpaceMap.SpaceMap._undoStack)
			{
				i++;
                if (i<maxi && i > 9 && maxi != 10)
                    continue;
                if (i == maxi && maxi > 10)
                    _FSM_ms_Main_Edit_UndoList.DropDownItems.Add("...");
				ToolStripItem tsi = _FSM_ms_Main_Edit_UndoList.DropDownItems.Add("[" + i.ToString("00") + "] " + item.Description);
				tsi.Tag = item;
				tsi.Click += _E_FSM_ms_Main_Edit_UndoListItem_Click;
			}
			if (_FSM_ms_Main_Edit_UndoList.DropDownItems.Count > 0)
				_FSM_ms_Main_Edit_UndoList.DropDownItems.RemoveAt(_FSM_ms_Main_Edit_UndoList.DropDownItems.Count - 1);
			if (_FSM_ms_Main_Edit_UndoList.DropDownItems.Count == 0)
			{
				_FSM_ms_Main_Edit_UndoList.Text = "Undo list (empty)";
				_FSM_ms_Main_Edit_UndoList.Enabled = false;
			}
			else
			{
				_FSM_ms_Main_Edit_UndoList.Text = "Undo list (" +maxi + " " + (maxi > 1 ? "entries" : "entry") + ")";
				_FSM_ms_Main_Edit_UndoList.Enabled = true;
				_FSM_ms_Main_Edit_UndoList.DropDownItems.Add(new ToolStripSeparator());
				ToolStripItem tsul = _FSM_ms_Main_Edit_UndoList.DropDownItems.Add("Initial state");
				tsul.Enabled = false;
			}

			i = 0;
            maxi = pSpaceMap.SpaceMap._redoStack.Count;
			_FSM_ms_Main_Edit_RedoList.DropDownItems.Clear();
			foreach (SpaceMapSavedState item in pSpaceMap.SpaceMap._redoStack)
			{
				i++;
                if (i < maxi && i > 9 && maxi != 10)
                    continue;
                if (i == maxi && maxi > 10)
                    _FSM_ms_Main_Edit_RedoList.DropDownItems.Add("...");
				ToolStripItem tsi = _FSM_ms_Main_Edit_RedoList.DropDownItems.Add("[" + i.ToString("00") + "] " + item.Description);
				tsi.Tag = item;
				tsi.Click += _E_FSM_ms_Main_Edit_RedoListItem_Click;
			}
			if (_FSM_ms_Main_Edit_RedoList.DropDownItems.Count == 0)
			{
				_FSM_ms_Main_Edit_RedoList.Text = "Redo list (empty)";
				_FSM_ms_Main_Edit_RedoList.Enabled = false;
			}
			else
			{
				_FSM_ms_Main_Edit_RedoList.Text = "Redo list (" + maxi + " " + (maxi > 1 ? "entries" : "entry") + ")";
				_FSM_ms_Main_Edit_RedoList.Enabled = true;
				_FSM_ms_Main_Edit_RedoList.DropDownItems.Add(new ToolStripSeparator());
				ToolStripItem tsrl = _FSM_ms_Main_Edit_RedoList.DropDownItems.Add("Final state");
				tsrl.Enabled = false;
			}
		}

		void _E_FSM_ms_Main_Edit_UndoListItem_Click(object sender, EventArgs e)
		{
			pSpaceMap.Undo((SpaceMapSavedState)((ToolStripItem)sender).Tag);
		}

		void _E_FSM_ms_Main_Edit_RedoListItem_Click(object sender, EventArgs e)
		{
			pSpaceMap.Redo((SpaceMapSavedState)((ToolStripItem)sender).Tag);
		}


		private void _E_FSM_ms_Main_Edit_Undo_Click(object sender, EventArgs e)
		{
			pSpaceMap.Undo();
		}

		private void _E_FSM_ms_Main_Edit_Redo_Click(object sender, EventArgs e)
		{
			pSpaceMap.Redo();
		}

		private void _FSM_ms_Main_Edit_Cut_Click(object sender, EventArgs e)
		{
			pSpaceMap.Cut();
			pSpaceMap.SetFocus();
		}

		private void _FSM_ms_Main_Edit_Copy_Click(object sender, EventArgs e)
		{
			pSpaceMap.Copy();
			pSpaceMap.SetFocus();
		}

		private void _FSM_ms_Main_Edit_Paste_Click(object sender, EventArgs e)
		{
			pSpaceMap.Paste();
			pSpaceMap.SetFocus();
		}

		private void _FSM_ms_Main_Edit_Delete_Click(object sender, EventArgs e)
		{
			pSpaceMap.ExecuteCommand(KeyCommands.DeleteSelected);
			pSpaceMap.SetFocus();
		}
    }
}
