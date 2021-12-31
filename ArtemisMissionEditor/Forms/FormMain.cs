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
using System.Reflection;

namespace ArtemisMissionEditor.Forms
{
    /// <summary>
    ///  Main window of the application
    /// </summary>
    public partial class FormMain : FormSerializeableToRegistry, IMRUClient
    {
        private static string MainFormName = AboutBox.AssemblyTitle + " v" + AboutBox.AssemblyVersion;

        public void OpenMRUFile(string missionFileName)
        {
            Mission.Current.FromFile(missionFileName);
        }

        public void UpdateFormText()
        {
            string missionVersionDashed;
            if(String.IsNullOrEmpty(Mission.Current.VersionNumber) == true)
            {
                missionVersionDashed = " version ???";
            }
            else
            {
                missionVersionDashed = " version " + Mission.Current.VersionNumber;
            }

            string missionName;
            if(String.IsNullOrEmpty(Mission.Current.FilePath) == true)
            {
                missionName = "Unnamed";
            }
            else
            {
                missionName = Path.GetFileNameWithoutExtension(Mission.Current.FilePath) + missionVersionDashed;
            }

            Text = (Mission.Current.ChangesPending ? "* " : "") + missionName + " - " + MainFormName;
        }

        private Point MousePositionWhenOpeningDropDownMissionNode = new Point();
        private Point MousePositionWhenOpeningDropDownMissionStatement = new Point();


        public FormMain()
        {
            InitializeComponent();

            Log.NewLogEntry += _E_Log_NewLogEntry;
            _E_Log_NewLogEntry(this, null);

            Mission.Current = new Mission();
            Mission.Current.AssignFlowPanel(_FM_flp_Bottom_Right);
            Mission.Current.AssignNodeTreeView(_FM_tve_MissionNode);
            Mission.Current.AssignStatementTreeView(_FM_tve_MissionStatement);
            Mission.Current.AssignForm(this);
            Mission.Current.AssignLabel(_FM_lbl_Main);
            Mission.Current.AssignStatusToolStrip(_FM_ss_Main);
            Mission.Current.AssignContextMenuStripForLabels(_FM_cms_Label);
            
            Mission.Current.New(true);
        }

        private void _E_Log_NewLogEntry(object sender, NewLogEntryEventArgs e)
        {
            if (Log.LogLines.Count > 0)
                _FM_ss_Main_s_1.Text = Log.LogLines[Log.LogLines.Count-1].ToString();
            else
                _FM_ss_Main_s_1.Text = "Log is empty";
        }

        private void _E_FM_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Mission.Current.ChangesPending)
            {
                DialogResult result = MessageBox.Show("Do you want to save unsaved changes?", "Artemis Mission Editor",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (result == DialogResult.Yes)
                    if (!Mission.Current.Save())
                    {
                        e.Cancel = true;
                        return;
                    }
            }

            e.Cancel = false;

            SaveToRegistry();
        }

        private void _E_FM_Load(object sender, EventArgs e)
        {
            _mainSC = _FM_sc_Main;
            _rightSC = _FM_sc_Main_Right;
            ID = "FormMain";
            LoadFromRegistry();

            VesselData.VesselDataChanged += UpdateVesselDataText;
            Settings.SettingAutoSaveIntervalChanged += UpdateAutosaveTimer;

            
            UpdateVesselDataText(sender, null);
            UpdateAutosaveTimer(sender, null);

            MRUManager.Current.Initialize(this, _FM_ms_Main_File, recentFilesToolStripMenuItem, autosavedVersionsToolStripMenuItem);
        }

        private void UpdateVesselDataText(object sender, VesselDataChangedEventArgs e)
        {
            _FM_ss_Main_VesselData.Text = "[vesselData.xml]: " + VesselData.Current.HullRaceList.Count.ToString() + " races and " + VesselData.Current.VesselList.Count.ToString() + " vessels.";
        }

        private void UpdateAutosaveTimer(object sender, SettingAutoSaveIntervalChangedEventArgs e)
        {
            _FM_t_AutoUpdateTimer.Enabled = Settings.Current.AutoSaveInterval != 0;
            _FM_t_AutoUpdateTimer.Interval = 1 + Settings.Current.AutoSaveInterval * 60 * 1000;
        }

        private void _E_FM_ms_Main_File_OpenMission_Click(object sender, EventArgs e)
        {
            Mission.Current.Open();
        }

        private void _E_FM_ms_Main_File_SaveMission_Click(object sender, EventArgs e)
        {
            Mission.Current.Save();
        }

        private void _E_FM_ms_Main_File_NewMission_Click(object sender, EventArgs e)
        {
            Mission.Current.New();
        }

        private void _E_FM_ms_Main_File_SaveAsMission_Click(object sender, EventArgs e)
        {
            Mission.Current.SaveAs();
        }

        private void _E_FM_ms_Main_File_DropDownOpening(object sender, EventArgs e)
        {
            _FM_ms_Main_File_SaveMission.Enabled = Mission.Current.ChangesPending;
        }

        private void _E_FM_ms_Main_File_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _E_FM_ms_Main_Edit_Copy_Click(object sender, EventArgs e)
        {
            if (_FM_tve_MissionNode.Focused)
                Mission.Current.NodeCopy();
            if (_FM_tve_MissionStatement.Focused)
                Mission.Current.StatementCopy();
        }

        private void _E_FM_ms_Main_Edit_Cut_Click(object sender, EventArgs e)
        {
            if (_FM_tve_MissionNode.Focused && Mission.Current.NodeCopy())
                Mission.Current.NodeDelete();
            if (_FM_tve_MissionStatement.Focused && Mission.Current.StatementCopy())
                Mission.Current.StatementDelete();
        }

        private void _E_FM_ms_Main_Edit_Paste_Click(object sender, EventArgs e)
        {
            if (_FM_tve_MissionNode.Focused)
                Mission.Current.NodePaste();
            if (_FM_tve_MissionStatement.Focused)
                Mission.Current.StatementPaste();
        }

        private void _E_FM_ms_Main_Edit_Delete_Click(object sender, EventArgs e)
        {
            if (_FM_tve_MissionNode.Focused)
                Mission.Current.NodeDelete();
            if (_FM_tve_MissionStatement.Focused)
                Mission.Current.StatementDelete();
        }

        private void _E_FM_ms_Main_Edit_Undo_Click(object sender, EventArgs e)
        {
            Mission.Current.Undo();
        }

        private void _E_FM_ms_Main_Edit_Redo_Click(object sender, EventArgs e)
        {
            Mission.Current.Redo();
        }

        private void _E_FM_ms_Main_Edit_DropDownOpening(object sender, EventArgs e)
        {
            _FM_ms_Main_Edit_Undo.Enabled = Mission.Current.UndoDescription != null;
            _FM_ms_Main_Edit_Undo.Text = !_FM_ms_Main_Edit_Undo.Enabled ? "Undo" : "Undo \"" + Mission.Current.UndoDescription + "\"";
            _FM_ms_Main_Edit_Redo.Enabled = Mission.Current.RedoDescription != null;
            _FM_ms_Main_Edit_Redo.Text = !_FM_ms_Main_Edit_Redo.Enabled ? "Redo" : "Redo \"" + Mission.Current.RedoDescription + "\"";

            _FM_ms_Main_Edit_Copy.Enabled = (_FM_tve_MissionNode.Focused && _FM_tve_MissionNode.SelectedNode != null) || (_FM_tve_MissionStatement.Focused && _FM_tve_MissionStatement.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement);
            _FM_ms_Main_Edit_Cut.Enabled = (_FM_tve_MissionNode.Focused && _FM_tve_MissionNode.SelectedNode != null) || (_FM_tve_MissionStatement.Focused && _FM_tve_MissionStatement.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement);
            _FM_ms_Main_Edit_Paste.Enabled = (_FM_tve_MissionNode.Focused) || (_FM_tve_MissionStatement.Focused);
            _FM_ms_Main_Edit_Delete.Enabled = (_FM_tve_MissionNode.Focused && _FM_tve_MissionNode.SelectedNode != null) || (_FM_tve_MissionStatement.Focused && _FM_tve_MissionStatement.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement);

            int i = 0;
            int maxi = Mission.Current.UndoStack.Count - 1;
            _FM_ms_Main_Edit_UndoList.DropDownItems.Clear();
            
            foreach (MissionSavedState item in Mission.Current.UndoStack)
            {
                i++;
                if (i < maxi && i > 9 && maxi != 10)
                    continue;
                if (i == maxi && maxi > 10)
                    _FM_ms_Main_Edit_UndoList.DropDownItems.Add("...");
                ToolStripItem tsi = _FM_ms_Main_Edit_UndoList.DropDownItems.Add("["+i.ToString("00")+"] "+ item.Description);
                tsi.Tag = item;
                tsi.Click += _E_FM_ms_Main_Edit_UndoListItem_Click;
            }
            if (_FM_ms_Main_Edit_UndoList.DropDownItems.Count > 0)
                _FM_ms_Main_Edit_UndoList.DropDownItems.RemoveAt(_FM_ms_Main_Edit_UndoList.DropDownItems.Count - 1);
            if (_FM_ms_Main_Edit_UndoList.DropDownItems.Count == 0)
            {
                _FM_ms_Main_Edit_UndoList.Text = "Redo list (empty)";
                _FM_ms_Main_Edit_UndoList.Enabled = false;
            }
            else
            {
                _FM_ms_Main_Edit_UndoList.Text = "Undo list (" + maxi + " " + (maxi > 1 ? "entries" : "entry") + ")";
                _FM_ms_Main_Edit_UndoList.Enabled = true;
                _FM_ms_Main_Edit_UndoList.DropDownItems.Add(new ToolStripSeparator());
                ToolStripItem tsul = _FM_ms_Main_Edit_UndoList.DropDownItems.Add("Initial state");
                tsul.Enabled = false;
            }

            i = 0;
            maxi = Mission.Current.RedoStack.Count;
            _FM_ms_Main_Edit_RedoList.DropDownItems.Clear();
            foreach (MissionSavedState item in Mission.Current.RedoStack)
            {
                i++;
                if (i < maxi && i > 9 && maxi != 10)
                    continue;
                if (i == maxi && maxi > 10)
                    _FM_ms_Main_Edit_RedoList.DropDownItems.Add("...");
                ToolStripItem tsi = _FM_ms_Main_Edit_RedoList.DropDownItems.Add("["+i.ToString("00")+"] "+item.Description);
                tsi.Tag = item;
                tsi.Click += _E_FM_ms_Main_Edit_RedoListItem_Click;
            }
            if (_FM_ms_Main_Edit_RedoList.DropDownItems.Count == 0)
            {
                _FM_ms_Main_Edit_RedoList.Text = "Redo list (empty)";
                _FM_ms_Main_Edit_RedoList.Enabled = false;
            }
            else
            {
                _FM_ms_Main_Edit_RedoList.Text = "Redo list (" + maxi + " " + (maxi > 1 ? "entries" : "entry") + ")";
                _FM_ms_Main_Edit_RedoList.Enabled = true;
                _FM_ms_Main_Edit_RedoList.DropDownItems.Add(new ToolStripSeparator());
                ToolStripItem tsrl = _FM_ms_Main_Edit_RedoList.DropDownItems.Add("Final state");
                tsrl.Enabled = false;
            }
        }

        void _E_FM_ms_Main_Edit_UndoListItem_Click(object sender, EventArgs e)
        {
            Mission.Current.Undo((MissionSavedState)((ToolStripItem)sender).Tag);
        }

        void _E_FM_ms_Main_Edit_RedoListItem_Click(object sender, EventArgs e)
        {
            Mission.Current.Redo((MissionSavedState)((ToolStripItem)sender).Tag);
        }

        private void _E_FM_ms_Main_Tools_Comm_into_Names_Click(object sender, EventArgs e)
        {
            Mission.Current.ConvertCommentariesIntoNames();
        }

        private void _E_FM_ms_Main_Tools_Comm_into_Names_Ex_Click(object sender, EventArgs e)
        {
            Mission.Current.ConvertCommentariesIntoNames(true);
        }

        private void _E_FM_ms_Main_Tools_Settings_Click(object sender, EventArgs e)
        {
            Program.FormSettingsInstance.Show();
            Program.FormSettingsInstance.BringToFront();
        }

        private void _E_FM_ms_Main_SpaceMap_Help_DebugXML_Click(object sender, EventArgs e)
        {
            string result = Mission.Current.GetDifferenceVersusSource();
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.Show("There is no difference between input mission file and current inner mission state", "Success!");
            }
            else
            {
                while (result.IndexOf("\r\n") == 0)
                    result = result.Substring(2);
                Program.FormNotepadInstance.ShowText(result, "Difference with the source file");
            }
        }

        private void _E_FM_cms_MissionNodeTree_New_Folder_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeAddFolder(_FM_tve_MissionNode.GetNodeAt(_FM_tve_MissionNode.PointToClient(MousePositionWhenOpeningDropDownMissionNode)));
        }

        private void _E_FM_cms_MissionNodeTree_New_Comment_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeAddCommentary(_FM_tve_MissionNode.GetNodeAt(_FM_tve_MissionNode.PointToClient(MousePositionWhenOpeningDropDownMissionNode)));
        }

        private void _E_FM_cms_MissionNodeTree_New_Event_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeAddEvent( _FM_tve_MissionNode.GetNodeAt(_FM_tve_MissionNode.PointToClient(MousePositionWhenOpeningDropDownMissionNode)));
        }

        private void _E_FM_cms_MissionNodeTree_Opening(object sender, CancelEventArgs e)
        {
            MousePositionWhenOpeningDropDownMissionNode = Cursor.Position;

            _FM_cms_MissionNodeTree_Copy.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_Cut.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_Delete.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_MoveDown.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_MoveIn.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_MoveOut.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_MoveUp.Enabled = _FM_tve_MissionNode.SelectedNode != null;
            _FM_cms_MissionNodeTree_Rename.Enabled = _FM_tve_MissionNode.SelectedNode != null;

            if (_FM_tve_MissionNode.SelectedNode != null)
            {
                int count = 0;
                foreach (TreeNode node in _FM_tve_MissionNode.SelectedNodes)
                    count += node.Tag is MissionNode_Event ? 1 : 0;

                _FM_cms_MissionNodeTree_Disable.Enabled = count > 0;
                _FM_cms_MissionNodeTree_Enable.Enabled = count > 0;
            }

            //_FM_cms_MissionNodeTree_ConvertTo.Enabled = _FM_tve_Mission.SelectedNode != null;
            if (_FM_tve_MissionNode.SelectedNode!=null)
            {
                _FM_cms_MissionNodeTree_MoveUp.Enabled = _FM_tve_MissionNode.SelectedNode.PrevNode != null;
                _FM_cms_MissionNodeTree_MoveDown.Enabled = _FM_tve_MissionNode.SelectedNode.NextNode != null;
                _FM_cms_MissionNodeTree_MoveOut.Enabled = _FM_tve_MissionNode.SelectedNode.Parent != null;
                _FM_cms_MissionNodeTree_MoveIn.Enabled = (_FM_tve_MissionNode.SelectedNode.PrevNode != null && _FM_tve_MissionNode.IsFolder(_FM_tve_MissionNode.SelectedNode.PrevNode))
                    || (_FM_tve_MissionNode.SelectedNode.NextNode != null && _FM_tve_MissionNode.IsFolder(_FM_tve_MissionNode.SelectedNode.NextNode));
            }

            _FM_cms_MissionNodeTree_SetAsBackground.Enabled =  _FM_tve_MissionNode.SelectedNode != null && (_FM_tve_MissionNode.SelectedNode.Tag is MissionNode_Event || _FM_tve_MissionNode.SelectedNode.Tag is MissionNode_Start) && Mission.Current.BackgroundNode != _FM_tve_MissionNode.SelectedNode;
            _FM_cms_MissionNodeTree_XML.Enabled = Mission.Current.CanGetNodeXmlText();
        }

        private void _E_FM_cms_MissionNodeTree_Rename_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeRename();
        }

        private void _E_FM_cms_MissionNodeTree_Delete_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeDelete();
        }

        private void _E_FM_cms_MissionNodeTree_MoveUp_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeMoveUp();
        }

        private void _E_FM_cms_MissionNodeTree_MoveDown_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeMoveDown();
        }

        private void _E_FM_cms_MissionNodeTree_MoveIn_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeMoveIn();
        }

        private void _E_FM_cms_MissionNodeTree_MoveOut_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeMoveOut();
        }

        private void _E_FM_cms_MissionNodeTree_ConvertTo_Comment_Click(object sender, EventArgs e)
        {
            Mission.Current.ConvertSelectedNodesToComments();
        }

        private void _E_FM_cms_MissionNodeTree_ConvertTo_Folder_Click(object sender, EventArgs e)
        {
            Mission.Current.ConvertSelectedNodesToFolders();
        }

        private void _E_FM_cms_MissionNodeTree_ConvertTo_Event_Click(object sender, EventArgs e)
        {
            Mission.Current.ConvertSelectedNodesToEvents();
        }

        private void _E_FM_cms_MissionNodeTree_ShowXML_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeShowXml();
        }

        private void _E_FM_cms_MissionNodeTree_CopyXML_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeCopyXml();
        }

        private void _E_FM_cms_MissionNodeTree_Copy_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeCopy();
        }

        private void _E_FM_cms_MissionNodeTree_Cut_Click(object sender, EventArgs e)
        {
            if (Mission.Current.NodeCopy())
                Mission.Current.NodeDelete();
        }

        private void _E_FM_cms_MissionNodeTree_Paste_Click(object sender, EventArgs e)
        {
            if (!Mission.Current.NodePaste() && Mission.Current.StatementPaste() && Settings.Current.FocusOnStatementPaste)
                _FM_tve_MissionStatement.Focus();
        }

        private void _E_FM_cms_MissionStatementTree_Delete_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementDelete();
        }

        private void _E_FM_cms_MissionStatementTree_MoveUp_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementMoveUp();
        }

        private void _E_FM_cms_MissionStatementTree_MoveDown_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementMoveDown();
        }

        private void _E_FM_cms_MissionStatementTree_NewCondition_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementAddCondition(_FM_tve_MissionStatement.GetNodeAt(_FM_tve_MissionStatement.PointToClient(MousePositionWhenOpeningDropDownMissionStatement)));
        }

        private void _E_FM_cms_MissionStatementTree_NewAction_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementAddAction(_FM_tve_MissionStatement.GetNodeAt(_FM_tve_MissionStatement.PointToClient(MousePositionWhenOpeningDropDownMissionStatement)));
        }

        private void _E_FM_cms_MissionStatementTree_NewComment_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementAddCommentary(_FM_tve_MissionStatement.GetNodeAt(_FM_tve_MissionStatement.PointToClient(MousePositionWhenOpeningDropDownMissionStatement)));
        }
        
        private void _E_FM_cms_MissionStatementTree_Opening(object sender, CancelEventArgs e)
        {
            MousePositionWhenOpeningDropDownMissionStatement = Cursor.Position;

            _FM_cms_MissionStatementTree_Copy.Enabled = _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null && (_FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement || _FM_tve_MissionStatement.SelectedNodes.Count > 0);
            _FM_cms_MissionStatementTree_Cut.Enabled = _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null && (_FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement || _FM_tve_MissionStatement.SelectedNodes.Count > 0);
            _FM_cms_MissionStatementTree_Delete.Enabled = _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null && (_FM_tve_MissionStatement.SelectedNode.Tag is MissionStatement || _FM_tve_MissionStatement.SelectedNodes.Count > 0);
            _FM_cms_MissionStatementTree_MoveDown.Enabled = _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null;
            _FM_cms_MissionStatementTree_MoveUp.Enabled = _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null;
            _FM_cms_MissionStatementTree_NewCondition.Enabled = _FM_tve_MissionNode.SelectedNode != null && !(_FM_tve_MissionNode.SelectedNode.Tag is MissionNode_Start);// && _FM_tve_MissionStatement.SelectedNode != null;
            _FM_cms_MissionStatementTree_NewComment.Enabled = _FM_tve_MissionNode.SelectedNode != null ;//&& _FM_tve_MissionStatement.SelectedNode != null;
            _FM_cms_MissionStatementTree_NewAction.Enabled = _FM_tve_MissionNode.SelectedNode != null ;//&& _FM_tve_MissionStatement.SelectedNode != null;

            if ( _FM_tve_MissionNode.SelectedNode != null && _FM_tve_MissionStatement.SelectedNode != null)
            {
                _FM_cms_MissionStatementTree_MoveUp.Enabled = _FM_tve_MissionStatement.SelectedNode.PrevNode != null;
                _FM_cms_MissionStatementTree_MoveDown.Enabled = _FM_tve_MissionStatement.SelectedNode.NextNode != null;
            }

            _FM_cms_MissionStatementTree_AddViaSpaceMap.Enabled = Mission.Current.CanInvokeSpaceMapAddOrEdit();
            _FM_cms_MissionStatementTree_EditOnSpaceMap.Enabled = Mission.Current.CanInvokeSpaceMapEdit();
            _FM_cms_MissionStatementTree_StatementEditOnSpaceMap.Enabled = Mission.Current.CanInvokeSpaceMapStatement();

            _FM_cms_MissionStatementTree_XML.Enabled = Mission.Current.CanGetStatementXmlText();
            _FM_cms_MissionStatementTree_ShowSourceXML.Enabled = Mission.Current.StatementHasSourceXml();
        }

        private void _E_FM_cms_MissionStatementTree_ShowXML_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementShowXml();
        }

        private void _E_FM_cms_MissionStatementTree_CopyXML_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementCopyXml();
        }

        private void _E_FM_cms_MissionStatementTree_AddViaSpaceMap_Click(object sender, EventArgs e)
        {
            Mission.Current.AddCreateStatementsViaSpaceMap(_FM_tve_MissionStatement.GetNodeAt(_FM_tve_MissionStatement.PointToClient(MousePositionWhenOpeningDropDownMissionStatement)));
        }

        private void _E_FM_cms_MissionStatementTree_EditOnSpaceMap_Click(object sender, EventArgs e)
        {
            Mission.Current.EditCreateStatementsOnSpaceMap(_FM_tve_MissionStatement.GetNodeAt(_FM_tve_MissionStatement.PointToClient(MousePositionWhenOpeningDropDownMissionStatement)));
        }

        private void _E_FM_cms_MissionStatementTree_Copy_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementCopy();
        }

        private void _E_FM_cms_MissionStatementTree_Cut_Click(object sender, EventArgs e)
        {
            if (Mission.Current.StatementCopy())
                Mission.Current.StatementDelete();
        }

        private void _E_FM_cms_MissionStatementTree_Paste_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementPaste();
        }

        private void _E_FM_cms_MissionStatementTree_Help_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementHelp();
        }

        private void _E_FM_cms_MissionStatementTree_ShowSourceXML_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementShowXml(true);
        }

        private void _E_FM_cms_MissionStatementTree_StatementEditOnSpaceMap_Click(object sender, EventArgs e)
        {
            Mission.Current.EditStatementOnSpaceMap();
        }

        private void _E_FM_ss_Main_s_1_Click(object sender, EventArgs e)
        {
            Program.FormLogInstance.Show();
            Program.FormLogInstance.BringToFront();
        }

        private void _E_FM_ss_Main_VesselData_Click(object sender, EventArgs e)
        {
            _FM_cms_VesselData.Show(Cursor.Position);   
        }

        private void _E_FM_cms_VesselData_Load_Click(object sender, EventArgs e)
        {
            DialogResult res;
            string filename = null;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
               
               
                ofd.CheckFileExists = true;
                ofd.AddExtension = true;
                ofd.Multiselect = false;
                ofd.Filter = "XML Files|*.xml|All Files|*.*";
                ofd.Title = "Select vesselData.xml file";

                res = ofd.ShowDialog();
                filename = ofd.FileName;
            }
            if (res != DialogResult.OK)
                return;

            VesselData.Current.Load(filename);
        }

        private void _E_FM_cms_Label_Opening(object sender, CancelEventArgs e)
        {
            _FM_cms_Label_EditOnSpaceMap.Enabled = Mission.Current.CanInvokeSpaceMapStatement();
        }

        private void _E_FM_ms_Main_Edit_Find_Click(object sender, EventArgs e)
        {
            Mission.Current.ShowFindForm();
        }

        private void _E_FM_ms_Main_Edit_Replace_Click(object sender, EventArgs e)
        {
            Mission.Current.ShowReplaceForm();
        }

        private void _E_FM_ms_Main_Edit_FindNext_Click(object sender, EventArgs e)
        {
            Program.FormFindReplaceInstance.FindNext();
        }

        private void _E_FM_ms_Main_Edit_FindPrevious_Click(object sender, EventArgs e)
        {
            Program.FormFindReplaceInstance.FindPrevious();
        }

        private void _E_FM_ms_Main_Tools_Dependencies_ShowForCurrent_Click(object sender, EventArgs e)
        {
            Mission.Current.ShowEventDependencyForm(true);
        }

        private void _E_FM_ms_Main_Tools_Dependencies_OpenForm_Click(object sender, EventArgs e)
        {
            Mission.Current.ShowEventDependencyForm();
        }

        private void _E_FM_ms_Main_Tools_HighlightErrors_Click(object sender, EventArgs e)
        {
            Mission.Current.HighlightErrors();
        }

        private void _E_FM_cms_MissionNodeTree_ExpandAll_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeExpandAll();
        }

        private void _E_FM_cms_MissionNodeTree_CollapseAll_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeCollapseAll();
        }

        private void helpFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FormHelpInstance.ShowPage(FormHelpPage.Miscellaneous);
        }

        private void _E_FM_ms_Main_Edit_Properties_Click(object sender, EventArgs e)
        {
            Mission.Current.ShowMissionPropertiesForm();
        }

        private void _E_FM_ms_Main_SpaceMap_Help_About_Click(object sender, EventArgs e)
        {
            using (AboutBox ab = new AboutBox())
            {
                ab.ShowDialog();
            }
        }

        private void _E_FM_t_AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!Mission.Current.ChangesPending)
            {
                // Nothing to do.
                return;
            }
            string programFolder = Environment.ExpandEnvironmentVariables(Settings.ProgramDataFolder);

            List<string> files = Directory.GetFiles(programFolder, "autosave*.xml").ToList();

            while (files.Count > Settings.Current.AutoSaveFilesCount - 1)
            {
                int lastIndex = -1;
                DateTime lastTime = DateTime.MaxValue;
                for (int i = 0; i < files.Count; i++)
                {
                    DateTime curTime;
                    if ((curTime = File.GetLastWriteTime(files[i])) < lastTime)
                    {
                        lastTime = curTime;
                        lastIndex = i;
                    }
                }

                File.Delete(files[lastIndex]);
                files.RemoveAt(lastIndex);
            }

            int targetID = 1;
            while (File.Exists(programFolder + "autosave" + targetID.ToString("000") + ".xml"))
                targetID++;

            Mission.Current.Save(programFolder + "autosave" + targetID.ToString("000") + ".xml", true);

            Log.Add("Autosaved successfully to: " + programFolder + "autosave" + targetID.ToString("000") + ".xml");
        }

        private void _E_FM_cms_MissionNodeTree_SetAsBackground_Click(object sender, EventArgs e)
        {
            if (_FM_tve_MissionNode.SelectedNode.Tag is MissionNode_Start || _FM_tve_MissionNode.SelectedNode.Tag is MissionNode_Event) 
                Mission.Current.BackgroundNode = _FM_tve_MissionNode.SelectedNode;
        }

        private void _E_FM_cms_MissionNodeTree_DisableEnable_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeSetEnabled(false);
        }

        private void _E_FM_cms_MissionNodeTree_Enable_Click(object sender, EventArgs e)
        {
            Mission.Current.NodeSetEnabled(true);
        }

        private void _E_FM_cms_MissionStatementTree_Disable_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementSetEnabled(false);
        }

        private void _E_FM_cms_MissionStatementTree_Enable_Click(object sender, EventArgs e)
        {
            Mission.Current.StatementSetEnabled(true);
        }

        private void showHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FormHelpInstance.ShowPage(FormHelpPage.Analysis);
        }

        private void findPotentialProblemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FormSearchResultsInstance.FindProblems();
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            Label l = (Label)sender;
            //e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, l.Height-2, l.Width, l.Height-2);
            e.Graphics.DrawLine(SystemPens.WindowFrame, 0, 0, l.Width, 0);
            e.Graphics.DrawLine(SystemPens.WindowFrame, 0, 0, 0, l.Height - 1);
            e.Graphics.DrawLine(SystemPens.WindowFrame, l.Width - 1, 0, l.Width - 1, l.Height - 1);
        }
    }
}
