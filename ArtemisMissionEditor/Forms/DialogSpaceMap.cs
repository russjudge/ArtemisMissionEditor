using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public partial class DialogSpaceMap : FormSerializeableToRegistry
    {
        public DialogSpaceMap()
        {
            //BASE INIT            
            InitializeComponent();

            //Init objects
            pSpaceMap.AssignForm(this);
            //pSpaceMap.AssignStatusToolStrip(_FSM_ss_Main);
            pSpaceMap.AssignPropertyGrid(this._DSM_pg_Properties);
            //pSpaceMap.AssignNamelessObjectsListBox(_FSM_lb_NamelessObjects);
            //pSpaceMap.AssignNamedObjectsListBox(this._FSM_lb_NamedObjects);
        }

        private static DialogSpaceMap CreateSpaceMapDialog(string caption, string statementXml, string editXml, string bgXml)
        {
            DialogSpaceMap form = new DialogSpaceMap();

            form.Text = caption;
            form.pSpaceMap.SpaceMap.FromXml(statementXml);
            form.pSpaceMap.SpaceMap.FromXml(editXml);
            form.pSpaceMap.SpaceMap.FromXml(bgXml);
            form.pSpaceMap.SpaceMap.MarkAsImported();
            form.pSpaceMap.Mode = PanelSpaceMapMode.SingleSpecial;
            form._DSM_pg_Properties.SelectedObject = form.pSpaceMap.SpaceMap.SelectionSpecial;
            
            return form;
        }

        public static SpaceMap EditStatementOnSpaceMap(string statementXml, string editXml, string bgXml)
        {
            DialogSpaceMap form = CreateSpaceMapDialog("Edit statement", statementXml, editXml, bgXml);

            if (form.ShowDialog() != DialogResult.Yes)
                return null;
            else
                return form.pSpaceMap.SpaceMap;
        }

        private void DialogSpaceMap_Load(object sender, EventArgs e)
        {
            _mainSC = _DSM_sc_Main;
			_rightSC = null;
            ID = "DialogSpaceMap";
            LoadFromRegistry();

			pSpaceMap.RegisterChange("Dialog Space Map Loaded", false, true);
            pSpaceMap.UpdateObjectLists();
            pSpaceMap.UpdateObjectsText();
            pSpaceMap.Reset();
            pSpaceMap.SetFocus();
        }

        private void DialogSpaceMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                pSpaceMap.ExecuteCommand(KeyCommands.FinishEditing);

            if (DialogResult == DialogResult.Cancel || DialogResult == DialogResult.None)
            {
                e.Cancel = true;
                DialogResult = DialogResult.None;
                return;
            }

            SaveToRegistry();
        }

        private void _DSM_b_OK_Click(object sender, EventArgs e)
        {
            pSpaceMap.ExecuteCommand(KeyCommands.FinishEditingForceYes);
        }

        private void _DSM_b_Cancel_Click(object sender, EventArgs e)
        {
            pSpaceMap.ExecuteCommand(KeyCommands.FinishEditingForceNo);
        }

    }
}
