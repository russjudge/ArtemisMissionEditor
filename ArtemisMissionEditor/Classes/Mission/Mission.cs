using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Drawing;

namespace ArtemisMissionEditor
{
	public sealed class Mission
	{
		private static Mission _current;
		public static Mission Current { get { return _current; } set { _current = value; } }

        private bool ___STATIC_E_statementTV_SuppressSelectionEvents;//flag to supress selection events while pasting
        private bool ___STATIC_E_nodeTV_SupressSelectionEvents;//flag to supress selection events while pasting
		private bool ___STATIC_E_nodeTV_SupressExpandCollapseEvents;//flag to supress afterexpand and aftercollapse events while mass-operating
		private int ___STATIC_Update_Counter;//Semaphore for begin/end update

		#region Name lists and other tracked mission parameters

        //The list of names of objects present in such statements as creating set_variable create set_timer
		public KeyValuePair<List<string>, List<string>> VariableNamesList { get { return new KeyValuePair<List<string>,List<string>>(_variableNames,_variableHeaders); } }
        public KeyValuePair<List<string>, List<string>> TimerNamesList { get { return new KeyValuePair<List<string>,List<string>>(_timerNames,_timerHeaders); } }
		public KeyValuePair<List<string>, List<string>> StationNamesList { get { return new KeyValuePair<List<string>, List<string>>(_namedObjects["station"], new List<string>()); } }
		public Dictionary<string, List<string>> AllNamesLists { get { return _namedObjects; } }
        /// <summary> All nodes where the variable is checked</summary>
        public Dictionary<string, List<MissionNode>> VariableCheckLocations { get; private set; }
        /// <summary> All variables that are set</summary>
        public List<string> VariableSetNames { get; private set; }
        /// <summary> All variables that are checked</summary>
        public List<string> VariableCheckNames { get; private set; }
        /// <summary> All timers that are set</summary>
        public List<string> TimerSetNames { get; private set; }
        /// <summary> All timers that are checked</summary>
        public List<string> TimerCheckNames { get; private set; }
        /// <summary> All objects that are created</summary>
        public List<string> AllCreatedObjectNames { get; private set; }
        public bool ContainsMissionEndStatement { get; private set; }
        private List<string> _variableNames;
        private List<string> _variableHeaders; // Headers for separators
        private List<string> _timerNames;
        private List<string> _timerHeaders; // Headers for separators
        private Dictionary<string, List<string>> _namedObjects;
        
		#endregion

		#region Mission inner parameters

        private string _filePath;
		public string FilePath { get { return _filePath; } set { _filePath = value; FormMain.UpdateFormText(); } }

		private TreeNode _startNode;
		public TreeNode StartNode { get { return _startNode; } }
		/// <summary> Node whose create statements are used as background </summary>
		private TreeNode _bgNode;
		/// <summary> Node whose create statements are used as background </summary>
		public TreeNode BgNode { get { return _bgNode; } set { _bgNode = value; } }
		public string _commentsBefore;
		public string _commentsAfter;

		private int _eventCount;
		public int EventCount { get { return _eventCount; } }

        /// <summary> Flag that there are changes that need saving to file </summary>
        private bool _changesPending = false;
		/// <summary> Flag that there are changes that need saving to file </summary>
        public bool ChangesPending { get { return _changesPending; } set { if (_changesPending != value) { _changesPending = value; FormMain.UpdateFormText(); } } }

		private bool _loading;
		/// <summary> Loading from XML is in progress </summary>
		public bool Loading { get { return _loading; } }

		/// <summary> Dependancy graph of the mission </summary>
		public DependencyGraph Dependencies { get; set; }
		public void RecalculateDependencies()
		{
			Dependencies.Recalculate(this);
		}

		public Stack<MissionSavedState> _undoStack;
		public Stack<MissionSavedState> _redoStack;

		public string UndoDescription { get { if (_undoStack.Count < 2) return null; return _undoStack.Peek().Description; } }
		public string RedoDescription { get { if (_redoStack.Count == 0) return null; return _redoStack.Peek().Description; } }

        #endregion

		#region Assigned Controls

		/// <summary> TreeView control that displays a tree of nodes for this Mission class </summary>
        private TreeViewEx TreeViewNodes;
        /// <summary> TreeView control that displays a tree of statements for this Node </summary>
		private TreeViewEx TreeViewStatements;
		private FlowLayoutPanel FlowLayoutPanelMain;
		private FormMain FormMain;
		private Label LabelMain;
		private StatusStrip StatusStripMain;
		private ToolStripStatusLabel ToolStripObjectsTotal;
		private ContextMenuStrip ContextMenuStripForLabels;

		public void AssignLabelCMS(ContextMenuStrip value = null)
		{
			if (ContextMenuStripForLabels != null)
			{
				foreach (ToolStripItem item in ContextMenuStripForLabels.Items)
				{
					if (item is ToolStripMenuItem)
					{
						if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
						{
							foreach (ToolStripItem citem in ((ToolStripMenuItem)item).DropDownItems)
								citem.Click -= _E_l_CMS_Click;
						}
						else
						{
							item.Click -= _E_l_CMS_Click;
						}
					}
				}
			}
			ContextMenuStripForLabels = null;

			if (value == null)
				return;

			ContextMenuStripForLabels = value;
			foreach (ToolStripItem item in ContextMenuStripForLabels.Items)
			{
				if (item is ToolStripMenuItem)
				{
					if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
					{
						foreach (ToolStripItem citem in ((ToolStripMenuItem)item).DropDownItems)
							citem.Click += _E_l_CMS_Click;
					}
					else
					{
						item.Click += _E_l_CMS_Click;
					}
				}
			}
		}

		public void AssignNodeTreeView(TreeViewEx value = null)
		{
			if (TreeViewNodes != null)
			{
				TreeViewNodes.NodesClear();
				TreeViewNodes.IsFolder_Reset();
				TreeViewNodes.IsAllowedToHaveRelation_Reset();

				TreeViewNodes.NodeMoved -= _E_nodeTV_NodeMoved;
				TreeViewNodes.AfterLabelEdit -= _E_nodeTV_AfterLabelEdit;
				TreeViewNodes.BeforeSelect -= _E_nodeTV_BeforeSelect;
				TreeViewNodes.AfterSelect -= _E_nodeTV_AfterSelect;
				TreeViewNodes.KeyDown -= _E_nodeTV_KeyDown;
				TreeViewNodes.AfterExpand -= _E_nodeTV_AfterExpand;
				TreeViewNodes.AfterCollapse -= _E_nodeTV_AfterCollapse;
			}
			TreeViewNodes = null;

			if (value == null)
				return;

			TreeViewNodes = value;
			TreeViewNodes.IsFolder = (TreeNode ii) => ii.Tag != null && ii.Tag.GetType() == typeof(MissionNode_Folder);
			TreeViewNodes.IsAllowedToHaveRelation = NodeIsAllowedToHaveRelation;

			TreeViewNodes.NodeMoved += _E_nodeTV_NodeMoved;
			TreeViewNodes.AfterLabelEdit += _E_nodeTV_AfterLabelEdit;
			TreeViewNodes.BeforeSelect += _E_nodeTV_BeforeSelect;
			TreeViewNodes.AfterSelect += _E_nodeTV_AfterSelect;
			TreeViewNodes.KeyDown += _E_nodeTV_KeyDown;
			TreeViewNodes.AfterExpand += _E_nodeTV_AfterExpand;
			TreeViewNodes.AfterCollapse += _E_nodeTV_AfterCollapse;

			//_nodeTreeView.SelectedIndexChanged += _E_namedObjectsListBox_SelectedIndexChanged;
		}

		public void AssignStatementTreeView(TreeViewEx value = null)
		{
			if (TreeViewStatements != null)
			{
				TreeViewStatements.NodesClear();
				TreeViewStatements.IsFolder_Reset();
				TreeViewStatements.IsAllowedToHaveRelation_Reset();

				TreeViewStatements.NodeMoved -= _E_statementTV_NodeMoved;
				TreeViewStatements.BeforeSelect -= _E_statementTV_BeforeSelect;
				TreeViewStatements.AfterSelect -= _E_statementTV_AfterSelect;
				TreeViewStatements.KeyDown -= _E_statementTV_KeyDown;
                TreeViewStatements.MouseDoubleClick -= _E_statementTV_MouseDoubleClick;
			}
			TreeViewStatements = null;

			if (value == null)
				return;

			TreeViewStatements = value;
			TreeViewStatements.IsFolder = (TreeNode ii) => (ii.Tag is string);
			TreeViewStatements.IsAllowedToHaveRelation = StatementIsAllowedToHaveRelation;

			TreeViewStatements.NodeMoved += _E_statementTV_NodeMoved;
			TreeViewStatements.BeforeSelect += _E_statementTV_BeforeSelect;
			TreeViewStatements.AfterSelect += _E_statementTV_AfterSelect;
			TreeViewStatements.KeyDown += _E_statementTV_KeyDown;
            TreeViewStatements.MouseDoubleClick += _E_statementTV_MouseDoubleClick;
		}

		public void AssignFlowPanel(FlowLayoutPanel value = null)
		{
			if (FlowLayoutPanelMain != null)
			{
				FlowLayoutPanel_Clear();
				FlowLayoutPanelMain.Resize -= _E_flowLP_Resize;
			}
			FlowLayoutPanelMain = null;

			if (value == null)
				return;

			FlowLayoutPanelMain = value;
			FlowLayoutPanelMain.Resize += _E_flowLP_Resize;
		}

		public void AssignForm(FormMain value = null)
		{
			if (FormMain != null)
			{
				FormMain.KeyDown -= _E_form_KeyDown;
			}
			FormMain = null;

			if (value == null)
				return;

			FormMain = value;
			FormMain.KeyDown += _E_form_KeyDown;
		}

		public void AssignLabel(Label value = null)
		{
			if (LabelMain != null)
			{
				LabelMain.Text = "";
			}
			LabelMain = null;

			if (value == null)
				return;

			LabelMain = value;
		}

		public void AssignStatusToolStrip(StatusStrip value = null)
		{
			if (ToolStripObjectsTotal != null)
				ToolStripObjectsTotal.Text = "";

			StatusStripMain = null;
			ToolStripObjectsTotal = null;

			if (value == null)
				return;

			StatusStripMain = value;

			foreach (ToolStripItem item in StatusStripMain.Items)
			{
				if (item.GetType() == typeof(ToolStripStatusLabel) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "objectstotal")
					ToolStripObjectsTotal = (ToolStripStatusLabel)item;
			}
		}

        /// <summary>
        /// In this method we check wether a node can get placed near another node. 
        /// </summary>
        /// <param name="parent">Node being the receiver</param>
        /// <param name="child">Node that is trying to get attached</param>
        /// <example>
        /// You are not allowed to place Event above Start, so any relation which makes it so will be denied.
        /// </example>
		private bool NodeIsAllowedToHaveRelation(TreeNode parent, TreeNode child, NodeRelationship relation)
		{
			if (child.Tag == null)
				throw new ArgumentException("child", "Moving a TreeNode without a Tag, WTF?");
            if (parent.Tag == null)
                throw new ArgumentException("parent", "Moving a TreeNode without a Tag, WTF?");

            if (relation == NodeRelationship.ChildGoesInside && !TreeViewNodes.IsFolder(parent))
				return false;

			//Everything except comments cant go above Start (which can only occur in root)
			//Therefore, for each node that isnt a comment, and is trying to fit on one level with something that is in root...
            if (relation != NodeRelationship.ChildGoesInside && parent.Parent == null && !(child.Tag.GetType() == typeof(MissionNode_Comment) || child.Tag.GetType() == typeof(MissionNode_Start)))
			{
				//Check that we are not inserting directly above Start node...
                if (relation == NodeRelationship.ChildGoesAbove && parent.Tag.GetType() == typeof(MissionNode_Start))
					return false;
				//...and check that we are not inserting near a node that is above Start node
				if (TreeViewNodes.Nodes.IndexOf(_startNode) > TreeViewNodes.Nodes.IndexOf(parent))
					return false;
			}

			//Start, Comment and Unknown cannot go inside anything, start because it must stay on top and other two because they cant have Parent_ID attribute added to them
			if (child.Tag.GetType() == typeof(MissionNode_Start) || child.Tag.GetType() == typeof(MissionNode_Comment) || child.Tag.GetType() == typeof(MissionNode_Unknown))
			{
				//Refuse to go inside anything...
                if (relation == NodeRelationship.ChildGoesInside)
					return false;
				//...or next to a node that is inside something...
				if (parent.Parent != null)
					return false;
			}

			//Start additionally cannot go below anything that isnt a comment
			if (child.Tag.GetType() == typeof(MissionNode_Start))
			{
				//Never go under anything other than a comment or yourself
                if (relation == NodeRelationship.ChildGoesUnder && parent.Tag.GetType() != typeof(MissionNode_Comment) && parent != child)
					return false;

				//Go through nodes until we meet the node we are becoming parented to
				//If we find anything except comment on the way, this means we are going below
				for (int i = 0; i < TreeViewNodes.Nodes.Count; i++)
				{
					//When we found something other than a comment and the dragged node
					if (TreeViewNodes.Nodes[i].Tag.GetType() != typeof(MissionNode_Comment) && TreeViewNodes.Nodes[i] != child)
						//In case this is the node we are inserting above - allow it, otherwise deny it
                        if (relation == NodeRelationship.ChildGoesAbove && TreeViewNodes.Nodes[i] == parent)
							break;
						else
							return false;

					if (TreeViewNodes.Nodes[i] == parent)
						break;
				}
			}

			return true;
		}

		private bool StatementIsAllowedToHaveRelation(TreeNode parent, TreeNode child, NodeRelationship relation)
		{
			if (child.Tag == null || parent.Tag == null) // This might be a comment or a folder
				return false;

			//You cannot go inside something that isnt a folder
            if (relation == NodeRelationship.ChildGoesInside && !TreeViewStatements.IsFolder(parent))
				return false;

			//Non-statements cannot move at all
			if (!(child.Tag is MissionStatement))
				return false;

			//If going inside, Conditions can only go inside codintion folder, actions can go inside actions folder, comment can go anywhere
            if (relation == NodeRelationship.ChildGoesInside && (((string)parent.Tag == "conditions" && ((MissionStatement)child.Tag).Kind == MissionStatementKind.Action) || ((string)parent.Tag == "actions" && ((MissionStatement)child.Tag).Kind == MissionStatementKind.Condition) || ((string)parent.Tag != "actions" && (string)parent.Tag != "conditions")))
				return false;

			//Comments cannot go inside conditions!
            if (relation == NodeRelationship.ChildGoesInside && ((string)parent.Tag == "conditions" && ((MissionStatement)child.Tag).Kind == MissionStatementKind.Commentary))
				return false;

			//If going adjacent to something
			//Conditions can only go adjacent to conditions or comments inside conditions folder
			//Actions can only go adjacent to actions or comments inside actions folder
            if (relation == NodeRelationship.ChildGoesAbove || relation == NodeRelationship.ChildGoesUnder)
			{
				if (!(parent.Tag is MissionStatement))
					return false;

				if ((((MissionStatement)child.Tag).Kind == MissionStatementKind.Action) && ((string)parent.Parent.Tag != "actions"))
					return false;

				if ((((MissionStatement)child.Tag).Kind == MissionStatementKind.Condition) && ((string)parent.Parent.Tag != "conditions"))
					return false;
			}

			//Comments also cannot go under last condition
            if (relation == NodeRelationship.ChildGoesUnder && (((MissionStatement)child.Tag).Kind == MissionStatementKind.Commentary))
			{
				if (parent.Parent != null && parent.Parent.LastNode == parent && (string)parent.Parent.Tag == "conditions")
					return false;
			}

			return true;
		}

		private void FlowLayoutPanel_Clear()
		{
            foreach (Control c in FlowLayoutPanelMain.Controls)
                c.Dispose();
            FlowLayoutPanelMain.Controls.Clear();
		}

		private void FlowLayoutPanel_Suspend()
		{
			FlowLayoutPanelMain.SuspendLayout();
		}

		private void FlowLayoutPanel_Resume()
		{
			FlowLayoutPanelMain.ResumeLayout();
		}

		#endregion
        
        public Mission()
		{
			TreeViewNodes = null;
			TreeViewStatements = null;
			FlowLayoutPanelMain = null;
			FormMain = null;
			LabelMain = null;
            StatusStripMain = null;
            ToolStripObjectsTotal = null;
			ContextMenuStripForLabels = null;

			this.AssignFlowPanel();
			this.AssignNodeTreeView();
			this.AssignStatementTreeView();
			this.AssignForm();
			this.AssignLabel();
            this.AssignStatusToolStrip();
			this.AssignLabelCMS();

			Dependencies = new DependencyGraph();

            ContainsMissionEndStatement = false;
            VariableSetNames = new List<string>();
            VariableCheckNames = new List<string>();
            VariableCheckLocations = new Dictionary<string, List<MissionNode>>();
            TimerSetNames = new List<string>();
            TimerCheckNames = new List<string>();
            AllCreatedObjectNames = new List<string>();
            _variableNames = new List<string>();
            _variableHeaders = new List<string>();
			_timerNames = new List<string>();
            _timerHeaders = new List<string>();
			_namedObjects = new Dictionary<string,List<string>>();
            _namedObjects.Add("anomaly", new List<string>());
            _namedObjects.Add("blackHole", new List<string>());
            _namedObjects.Add("enemy", new List<string>());
            _namedObjects.Add("neutral", new List<string>());
            _namedObjects.Add("genericMesh", new List<string>());
            _namedObjects.Add("player", new List<string>());
            _namedObjects.Add("station", new List<string>());
            _namedObjects.Add("monster", new List<string>());
			_namedObjects.Add("whale", new List<string>());
            
			_undoStack = new Stack<MissionSavedState>();
			_redoStack = new Stack<MissionSavedState>();

			_eventCount = 0;
			
            ___STATIC_E_statementTV_SuppressSelectionEvents = false;
            ___STATIC_E_nodeTV_SupressSelectionEvents = false;
			___STATIC_E_nodeTV_SupressExpandCollapseEvents = false;
            ___STATIC_Update_Counter = 0;
		}
		
		#region Undo / Redo / Change registration

		public void Undo(MissionSavedState state = null)
		{
			if (_undoStack.Count < 2)
				return;

			state = state ?? _undoStack.Peek();
			while (_redoStack.Count==0 || _redoStack.Peek() != state)
				_redoStack.Push(_undoStack.Pop());
			FromState(_undoStack.Peek());

            UpdateObjectsText();
		}

		public void Redo(MissionSavedState state = null)
		{
			if (_redoStack.Count == 0)
				return;

			state = state ?? _redoStack.Peek();
			while (_undoStack.Count == 0 || _undoStack.Peek() != state)
				_undoStack.Push(_redoStack.Pop());
			FromState(_undoStack.Peek());
            
            UpdateObjectsText();
		}
        
		/// <summary> Registers a change, adding a new state into undo stack and cleaning redo stack in the process </summary>
		public void RegisterChange(string shortDescription, bool clean = false)
		{
			Dependencies.Invalidate();

			if (string.IsNullOrWhiteSpace(shortDescription))
				shortDescription = "NO DESCRIPTION!?";
			
			ChangesPending = !clean;
			
			if (clean) _undoStack.Clear();
			
			_undoStack.Push(ToState(shortDescription));
			_redoStack.Clear();

            UpdateObjectsText();

            UpdateObjectLists();

		}

        #endregion

        #region Mission input/output (file, state, ...) and New mission creation

        public void New(bool force = false)
        {
			if (!force && ChangesPending)
			{
				DialogResult result = MessageBox.Show("Do you want to save unsaved changes?", "Artemis Mission Editor",
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result == DialogResult.Cancel)
					return;
				if (result == DialogResult.Yes)
					if (!Save())
						return;
			}

            Clear();

            TreeNode newTNode = new TreeNode();
            MissionNode_Start newMNode = new MissionNode_Start();
			
			string xml = "<root>" + Settings.Current.NewMissionStartBlock + "</root>";
			XmlDocument xDoc = new XmlDocument();
			try
			{
				xDoc.LoadXml(xml);

				foreach (XmlNode node in xDoc.ChildNodes[0].ChildNodes)
				{
					newMNode.Actions.Add(MissionStatement.NewFromXML(node, newMNode));
				}
			}
			catch (Exception e)
			{
				Log.Add("Problems while trying to parse new mission start node text:");
				Log.Add(e.Message);
			}

            newTNode.Text = newMNode.Name;
            newTNode.ImageIndex = newMNode.ImageIndex;
            newTNode.SelectedImageIndex = newMNode.ImageIndex;
            newTNode.Tag = newMNode;

            TreeViewNodes.Nodes.Add(newTNode);
            _startNode = newTNode;
            FlowLayoutPanel_Clear();

			TreeViewNodes.SelectedNode = newTNode;
			
			_bgNode = newTNode;

            RegisterChange("New mission created", true);
        }

        private void Clear()
        {
            FilePath = "";
            _startNode = null;
			_bgNode = null;
            _commentsAfter = "";
            _commentsBefore = "";

            BeginUpdate();

            TreeViewNodes.NodesClear();
            TreeViewStatements.NodesClear();

            EndUpdate();

            _eventCount = 0;

			if (Program.FormSearchResultsInstance != null)
				Program.FormSearchResultsInstance.ClearList();

			if (Program.FormMissionPropertiesInstance != null)
				if (Program.FormMissionPropertiesInstance.Visible)
					Program.FormMissionPropertiesInstance.Close();
        }

        public void FromFile(string fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string text = streamReader.ReadToEnd();
                text = Helper.FixMissionXml(text);

                if (FromXml(text))
                {
                    FilePath = fileName;
                    TreeViewNodes.SelectedNode = _startNode;
                    RegisterChange("Mission read from file", true);
                }
                else
                {
                    New(true);
                }
            }
        }

        public void Open(bool force = false)
		{
            if (!force && ChangesPending)
            {
				DialogResult result = MessageBox.Show("Do you want to save unsaved changes?", "Artemis Mission Editor",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result==DialogResult.Cancel)
                    return;
                if (result == DialogResult.Yes)
                    if (!Save())
                        return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                DialogResult res;
                string fileName;
                ofd.CheckFileExists = true;
                ofd.AddExtension = true;
                ofd.Multiselect = false;
                ofd.Filter = "XML Files|*.xml|All Files|*.*";
                ofd.Title = "Open mission";

                res = ofd.ShowDialog();
                fileName = ofd.FileName;
                if (res != DialogResult.OK)
                    return;

                FromFile(fileName);
            }
		}

		public bool FromXml(string text, bool supressLoadingSignal = false)
		{
			XmlDocument xDoc = new XmlDocument();

			try
			{
				xDoc.LoadXml(text);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);

				return false;
			}

			Clear();
			_loading = !supressLoadingSignal;
			BeginUpdate();
			
			List<TreeNode> NodesToExpand = new List<TreeNode>();
			Guid? bgGuid = null;

			XmlNode root = null;
			int i;

			for (i = 0; i < xDoc.ChildNodes.Count; i++)
			{
				XmlNode item = xDoc.ChildNodes[i];
				if (item.GetType() == typeof(XmlComment))
					if (root == null)
						_commentsBefore += item.Value + "\r\n";
					else
						_commentsAfter += item.Value + "\r\n";
				if (item.Name == "mission_data")
					root = item;
			}
			if (_commentsBefore.Length >= 2 && _commentsBefore.Substring(_commentsBefore.Length - 2, 2) == "\r\n")
				_commentsBefore.Substring(0, _commentsBefore.Length - 2);
			if (_commentsAfter.Length>=2 && _commentsAfter.Substring(_commentsAfter.Length - 2, 2) == "\r\n")
				_commentsAfter.Substring(0, _commentsAfter.Length - 2);

            if (root == null)
            {
                Log.Add("No mission_data node found in specified Xml file. Mission was not loaded");
                _loading = false;
                EndUpdate();
                return false;
            }

			foreach (XmlAttribute att in root.Attributes)
			{
				Guid tmp;
				if (att.Name == "background_id_arme")
					if (Guid.TryParse(att.Value, out tmp))
						bgGuid = tmp;
			}

			foreach (XmlNode item in root.ChildNodes)
			{
				TreeNode newNode = new TreeNode();
				MissionNode newMissionNode = MissionNode.NewFromXML(item);

				newNode.Text = newMissionNode.Name;
				newNode.Tag = newMissionNode;
				newNode.ImageIndex = newMissionNode.ImageIndex;
				newNode.SelectedImageIndex = newMissionNode.ImageIndex;
				if (newMissionNode is MissionNode_Event)
					_eventCount++;

				TreeNode parentNode = newMissionNode.ParentID == null ? null : TreeViewNodes.FindNode((TreeNode ii) => ((MissionNode)ii.Tag).ID == newMissionNode.ParentID);

				if (parentNode != null)
					parentNode.Nodes.Add(newNode);
				else
					TreeViewNodes.Nodes.Add(newNode);

				if (newMissionNode.ExtraAttributes.Contains("expanded_arme"))
					NodesToExpand.Add(newNode);

				//If background id matches then remember this node
				if (newMissionNode.ID == bgGuid)
					_bgNode = newNode;
			}

			_startNode = TreeViewNodes.FindNode((TreeNode ii) => ii.Tag != null && ii.Tag.GetType() == typeof(MissionNode_Start));

			if (_startNode == null)
			{
				Log.Add("No start node found in the mission! Adding blank start node to the beginning of the mission");

				TreeNode newTNode = new TreeNode();
				MissionNode_Start newMNode = new MissionNode_Start();

				newTNode.Text = newMNode.Name;
				newTNode.ImageIndex = newMNode.ImageIndex;
				newTNode.SelectedImageIndex = newMNode.ImageIndex;
				newTNode.Tag = newMNode;

				TreeViewNodes.Nodes.Insert(0, newTNode);
				_startNode = newTNode;
			}

			if (_bgNode == null)
				_bgNode = _startNode;

			EndUpdate();
			_loading = false;
			
			//Expand nodes that are supposed to be expanded
			foreach (TreeNode node in NodesToExpand)
				node.Expand();

			return true;
		}

		public void ToXml_private_RecursivelyOut(XmlNode root, XmlDocument xDoc, TreeNode tNode, bool full)
		{
			//Add current node's XmlNode to the document
			root.AppendChild(((MissionNode)tNode.Tag).ToXml(xDoc, full));
            //Continue recursion
			for (int i = 0; i < tNode.Nodes.Count; i++)
                ToXml_private_RecursivelyOut(root, xDoc, tNode.Nodes[i], full);
		}

		public string ToXml(bool full=false)
		{
			XmlDocument xDoc;
			XmlElement root;
			XmlAttribute xAtt;

			xDoc = new XmlDocument();

			foreach (string item in _commentsBefore.Split(new string[1]{"\r\n"}, StringSplitOptions.None))
				if (!string.IsNullOrWhiteSpace(item))
					xDoc.AppendChild(xDoc.CreateComment(item));

			root = xDoc.CreateElement("mission_data");
			if (true)
			{
				xAtt = xDoc.CreateAttribute("version");
				xAtt.Value = "1.7";
				root.Attributes.Append(xAtt);
				xDoc.AppendChild(root);
			}
			if (_bgNode!=null)
			{
				xAtt = xDoc.CreateAttribute("background_id_arme");
				xAtt.Value = ((MissionNode)_bgNode.Tag).ID.ToString();
				root.Attributes.Append(xAtt);
				xDoc.AppendChild(root);
			}

			//Out the xml's!
            for (int i = 0; i < TreeViewNodes.Nodes.Count; i++)
                ToXml_private_RecursivelyOut(root, xDoc, TreeViewNodes.Nodes[i], full);

			foreach (string item in _commentsAfter.Split(new string[1] { "\r\n" }, StringSplitOptions.None))
				if (!string.IsNullOrWhiteSpace(item))
					xDoc.AppendChild(xDoc.CreateComment(item));

			//I make this look GOOD!
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();

			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.NewLineChars = "\r\n";
			settings.NewLineHandling = NewLineHandling.Replace;
			settings.OmitXmlDeclaration = true;
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				xDoc.Save(writer);
			}

			return sb.ToString();
		}

		private void FromState_private_RecursiveSelectByStep(TreeNode node, int step)
		{
			foreach (TreeNode child in node.Nodes)
				FromState_private_RecursiveSelectByStep(child, step);

			FromState_private_Step++;
			if (step == FromState_private_Step)
				TreeViewNodes.SelectedNode = node;

		}

		private void FromState_private_RecursiveSelectByTag(TreeNode node, MissionStatement statement)
		{
			foreach (TreeNode child in node.Nodes)
				FromState_private_RecursiveSelectByTag(child, statement);

			if (!(node.Tag is MissionStatement))
				return;

			if (((MissionStatement)node.Tag) == statement)
				TreeViewStatements.SelectedNode = node;
		}

		private int FromState_private_Step;		
		
		private void FromState(MissionSavedState state)
		{
			Dependencies.Invalidate();
			
			BeginUpdate();

			FromXml(state.Xml, true);

			ChangesPending = state.ChangesPending;
			FilePath = state.FilePath;
			_commentsBefore = state.CommentsBefore;
			_commentsAfter = state.CommentsAfter;

            //Find selected node
			if (state.SelectedNode >= 0)
			{
				FromState_private_Step = -1;
				foreach (TreeNode node in TreeViewNodes.Nodes)
					FromState_private_RecursiveSelectByStep(node, state.SelectedNode);
			}

			if (state.SelectedAction >= 0)
				foreach (TreeNode node in TreeViewStatements.Nodes)
					FromState_private_RecursiveSelectByTag(node, ((MissionNode)TreeViewNodes.SelectedNode.Tag).Actions[state.SelectedAction]);

			if (state.SelectedCondition >= 0)
				foreach (TreeNode node in TreeViewStatements.Nodes)
					FromState_private_RecursiveSelectByTag(node, ((MissionNode)TreeViewNodes.SelectedNode.Tag).Conditions[state.SelectedCondition]);

			if (state.SelectedLabel >= 0)
				FlowLayoutPanelMain.Controls[state.SelectedLabel].Focus();

            RecalculateNodeCount();

            EndUpdate();

		}

		private bool ToState_private_RecursivelyGetSelected(TreeNode node)
		{
			foreach (TreeNode child in node.Nodes)
				if (ToState_private_RecursivelyGetSelected(child)) return true;

			ToState_private_Step++;
			if (TreeViewNodes.SelectedNode == node)
				return true;
			return false;
		}

		private int ToState_private_Step;		

		private MissionSavedState ToState(string shortDescription)
		{
			MissionSavedState result = new MissionSavedState();
			result.Xml = ToXml(true);
			result.Description = shortDescription;
            result.ChangesPending = ChangesPending;
			result.FilePath = FilePath;
			result.CommentsBefore = _commentsBefore;
			result.CommentsAfter = _commentsAfter;
            result.SelectedNode = -1;
			if (TreeViewNodes.SelectedNode!=null)
			{
				ToState_private_Step = -1;
				foreach (TreeNode node in TreeViewNodes.Nodes)
					if (ToState_private_RecursivelyGetSelected(node)) break;
				result.SelectedNode = ToState_private_Step;
			}

			result.SelectedAction = -1;
			result.SelectedCondition = -1;
			result.SelectedLabel = -1;
			if (TreeViewNodes.SelectedNode != null && TreeViewStatements.SelectedNode != null && TreeViewStatements.SelectedNode.Tag is MissionStatement)
			{
				MissionNode curNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;
				MissionStatement curStatement = (MissionStatement)TreeViewStatements.SelectedNode.Tag;
				if (curNode.Actions.Contains(curStatement))
					result.SelectedAction = curNode.Actions.IndexOf(curStatement);
				if (curNode.Conditions.Contains(curStatement))
					result.SelectedCondition = curNode.Conditions.IndexOf(curStatement);
				foreach (Control c in FlowLayoutPanelMain.Controls)
					if (c.Focused)
						result.SelectedLabel = FlowLayoutPanelMain.Controls.IndexOf(c);
			}

			return result;
		}

		public bool SaveAs()
		{
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                DialogResult res;
                string fileName;
                sfd.AddExtension = true;
                sfd.Filter = "XML Files|*.xml|All Files|*.*";
                sfd.Title = "Save Mission As";

                res = sfd.ShowDialog();
                fileName = sfd.FileName;
                if (res != DialogResult.OK)
                    return false;

                return Save(fileName);
            }
		}

		public bool Save()
		{
            if (String.IsNullOrEmpty(FilePath))
                return SaveAs();
            else
                if (ChangesPending)
                    return Save(FilePath);
                else
                    return false;
		}

		public bool Save(string fileName, bool autosave = false)
		{
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fileName, false))
                {
                    streamWriter.Write(ToXml());

                    if (!autosave)
                    {
                        FilePath = fileName;
                        RegisterChange("Mission saved", true);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Add("Could not save mission: "+e.Message);
                return false;
            }
            return true;
		}

        #endregion

        /// <summary> Begin update of trees: calls BeginUpdate() of both TreeViews </summary>
		private void BeginUpdate(bool suppressSelectionEvents = false)
		{
            if (___STATIC_Update_Counter++ == 0)
            {
				Helper.PasteInProgress = true;
				___STATIC_E_nodeTV_SupressExpandCollapseEvents = true;
				TreeViewNodes.BeginUpdate();
                TreeViewStatements.BeginUpdate();
                ___STATIC_E_statementTV_SuppressSelectionEvents = suppressSelectionEvents;
                ___STATIC_E_nodeTV_SupressSelectionEvents = suppressSelectionEvents;
            }
		}

		/// <summary> End update of trees: calls EndUpdate() of both TreeViews </summary>
        private void EndUpdate(bool suppressSelectionEvents = false)
		{
            if (--___STATIC_Update_Counter == 0)
            {
				Helper.PasteInProgress = false;
				___STATIC_E_nodeTV_SupressExpandCollapseEvents = false;
                TreeViewNodes.EndUpdate();
                TreeViewStatements.EndUpdate();
                if (suppressSelectionEvents)
                {
                    ___STATIC_E_nodeTV_SupressSelectionEvents = false;
                    OutputMissionNodeContentsToTree();
                    ___STATIC_E_statementTV_SuppressSelectionEvents = false;
					UpdateExpression();
                }
            }
		}

        #region Statement operation (delete, move, add...)

		public void StatementEnableDisable(bool enabled)
		{
			int count = 0;
			foreach (TreeNode node in TreeViewStatements.SelectedNodes)
			{
				if (!(node.Tag is MissionStatement))
					continue;

				MissionStatement mNE = (MissionStatement)node.Tag;

				if (enabled && mNE.Kind == MissionStatementKind.Commentary)
				{
					try
					{
						XmlDocument xD = new XmlDocument();
						xD.LoadXml(mNE.Body);
						mNE.FromXml(xD.ChildNodes[0]);
						mNE.Update();
						count++;
					}
					catch
					{
					}
				}

				if (!enabled && mNE.Kind != MissionStatementKind.Commentary)
				{
					try
					{
						XmlDocument xD = new XmlDocument();
						xD.LoadXml("<!--" + mNE.ToXml(xD, true).OuterXml + "--><root/>");
						mNE.FromXml(xD.ChildNodes[0]);
						mNE.Update();
						count++;
					}
					catch
					{
					}
				}

				UpdateStatementTree();

				node.ImageIndex = mNE.ImageIndex;
				node.SelectedImageIndex = mNE.ImageIndex;
			}

			if (count > 0)
				RegisterChange((enabled ? "Enabled" : "Disabled") + " " + count + " statement(s)");
		}

		public void StatementDelete()
		{
			if (TreeViewStatements.SelectedNode == null)
				return;

			BeginUpdate();

			foreach(TreeNode node in TreeViewStatements.SelectedNodes.ToList())
				if (node.Tag is MissionStatement)
					TreeViewStatements.Nodes.Remove(node);

			EndUpdate();
			
			ImportMissionNodeContentsFromStatementTree();

            RegisterChange("Deleted statement(s)");
		}

		public void StatementMoveUp()
		{
			if (TreeViewStatements.SelectedNode == null)
				return;

			if (TreeViewStatements.SelectedNode.PrevNode != null)
			{
				TreeViewStatements.MoveNode(TreeViewStatements.SelectedNode, TreeViewStatements.SelectedNode.PrevNode, NodeRelationship.ChildGoesAbove);
				TreeViewStatements.Focus();
			}
		}

		public void StatementMoveDown()
		{
			if (TreeViewStatements.SelectedNode == null)
				return;

			if (TreeViewStatements.SelectedNode.NextNode != null)
			{
				TreeViewStatements.MoveNode(TreeViewStatements.SelectedNode, TreeViewStatements.SelectedNode.NextNode, NodeRelationship.ChildGoesUnder);
				TreeViewStatements.Focus();
			}
		}

		public void StatementAddCommentary(bool underCursor = false, TreeNode nodeUnderCursor = null)
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			TreeViewStatements.Focus();

			TreeNode newTNode = new TreeNode();
			MissionStatement newMStatement = new MissionStatement((MissionNode)TreeViewNodes.SelectedNode.Tag);
			newMStatement.Type = MissionStatementType.Commentary;
			newMStatement.Name = "Commentary";
			newMStatement.Body = "";
			newMStatement.Update();

			newTNode.Text = newMStatement.Text;
			newTNode.ImageIndex = newMStatement.ImageIndex;
			newTNode.SelectedImageIndex = newMStatement.ImageIndex;
			newTNode.Tag = newMStatement;

			bool needUpdate = Statement_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewStatements.SelectedNode);
			TreeViewStatements.SelectedNode = newTNode;

			ImportMissionNodeContentsFromStatementTree();

			if (needUpdate)
				RegisterChange("New commentary statement");
		}

		public void StatementAddCondition(bool underCursor = false, TreeNode nodeUnderCursor = null)
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			TreeViewStatements.Focus();

			TreeNode newTNode = new TreeNode();
			MissionStatement newMStatement = new MissionStatement((MissionNode)TreeViewNodes.SelectedNode.Tag);
			newMStatement.Type = MissionStatementType.Statement;
			newMStatement.Name = "if_variable";
			newMStatement.Body = "";
			newMStatement.Update();

			newTNode.Text = newMStatement.Text;
			newTNode.ImageIndex = newMStatement.ImageIndex;
			newTNode.SelectedImageIndex = newMStatement.ImageIndex;
			newTNode.Tag = newMStatement;

			bool needUpdate = Statement_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewStatements.SelectedNode);
			TreeViewStatements.SelectedNode = newTNode;

			ImportMissionNodeContentsFromStatementTree();

			if (needUpdate)
                RegisterChange("New condition statement");
		}

		public void StatementAddAction(bool underCursor = false, TreeNode nodeUnderCursor = null)
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			TreeViewStatements.Focus();

			TreeNode newTNode = new TreeNode();
			MissionStatement newMStatement = new MissionStatement((MissionNode)TreeViewNodes.SelectedNode.Tag);
			newMStatement.Type = MissionStatementType.Statement;
			newMStatement.Name = "set_variable";
			newMStatement.Body = "";
			newMStatement.Update();

			newTNode.Text = newMStatement.Text;
			newTNode.ImageIndex = newMStatement.ImageIndex;
			newTNode.SelectedImageIndex = newMStatement.ImageIndex;
			newTNode.Tag = newMStatement;

			bool needUpdate = Statement_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewStatements.SelectedNode);
			TreeViewStatements.SelectedNode = newTNode;

			ImportMissionNodeContentsFromStatementTree();

			if (needUpdate)
                RegisterChange("New action statement");
		}

		/// <summary>
		/// Adds one statement inside other statement, if possible, below it or then above it, if not possible
		/// </summary>
		/// <param name="toAdd">Statement to be added</param>
		/// <param name="selected">Statement that receives the added one</param>
		public bool Statement_AddUnderNode(TreeNode toAdd, TreeNode selected)
		{
			TreeNode ultimateParent = null;
			foreach (TreeNode node in TreeViewStatements.Nodes)
			{
                if (!(node.Tag is string))
                    continue;
				if ((string)node.Tag == "conditions" && ((MissionStatement)toAdd.Tag).Kind == MissionStatementKind.Condition)
					ultimateParent = node;
				if ((string)node.Tag == "actions" && ((MissionStatement)toAdd.Tag).Kind == MissionStatementKind.Action)
					ultimateParent = node;
				if (((MissionStatement)toAdd.Tag).Kind == MissionStatementKind.Commentary)
					ultimateParent = node;
			}

			if (ultimateParent == null)
				return false;

			if (selected != null && TreeViewStatements.IsAllowedToHaveRelation(selected, toAdd, NodeRelationship.ChildGoesInside))
                TreeViewStatements.MoveNode(toAdd, selected, NodeRelationship.ChildGoesInside, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesUnder : NodeRelationship.ChildGoesAbove, true);
            else if (selected != null && TreeViewStatements.IsAllowedToHaveRelation(selected, toAdd, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesAbove : NodeRelationship.ChildGoesUnder))
                TreeViewStatements.MoveNode(toAdd, selected, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesAbove : NodeRelationship.ChildGoesUnder, NodeRelationship.Null, true);
            else if (selected != null && TreeViewStatements.IsAllowedToHaveRelation(selected, toAdd, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesUnder : NodeRelationship.ChildGoesAbove))
                TreeViewStatements.MoveNode(toAdd, selected, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesUnder : NodeRelationship.ChildGoesAbove, NodeRelationship.Null, true);
			else
			{
				if (!Settings.Current.InsertNewOverElement)
					ultimateParent.Nodes.Insert(0,toAdd);
				else
					ultimateParent.Nodes.Add(toAdd);
			}
			
			return true;
		}

		public bool StatementHasSourceXml()
		{
			if (TreeViewStatements.SelectedNode == null)
				return false;

			if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
				return false; 
			
			return !string.IsNullOrEmpty(((MissionStatement)TreeViewStatements.SelectedNode.Tag).SourceXML);
		}

		private string StatementGetXml()
		{
			if (TreeViewStatements.SelectedNode == null)
				return null;

			if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
				return null;

			//XmlDocument xDoc = new XmlDocument();
			//xDoc.AppendChild(((MissionStatement)_statementTV.SelectedNode.Tag).ToXml(xDoc));
			
			//StringBuilder sb = new StringBuilder();
			//XmlWriterSettings settings = new XmlWriterSettings();

			//settings.Indent = true;
			//settings.IndentChars = "  ";
			//settings.NewLineChars = "\r\n";
			//settings.NewLineHandling = NewLineHandling.Replace;
			//settings.OmitXmlDeclaration = true;
			//using (XmlWriter writer = XmlWriter.Create(sb, settings))
			//{
			//    xDoc.Save(writer);
			//}

			//return sb.ToString();

			return ((MissionStatement)TreeViewStatements.SelectedNode.Tag).ToXml(new XmlDocument()).OuterXml;
		}
		
		public void StatementShowXml(bool source = false)
		{
			if (TreeViewStatements.SelectedNode == null)
				return;

			if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
				return;

			if (source)
			{
				string result = ((MissionStatement)TreeViewStatements.SelectedNode.Tag).SourceXML;
				if (result != null)
					MessageBox.Show(result, "Xml source of the statement");
			}
			else
			{
				string result = StatementGetXml();
				if (result != null)
					MessageBox.Show(result, "Xml code of the statement");
			}
		}

		public void StatementCopyXml()
		{
			string result = StatementGetXml();

			if (result != null) 
				Clipboard.SetText(result);
		}

		public bool StatementCopy()
		{
			if (TreeViewStatements.SelectedNode == null)
				return false;

			string Xml = "";

			foreach (TreeNode node in TreeViewStatements.SelectedNodes)
				if (node.Tag is MissionStatement)
					Xml += ((MissionStatement)node.Tag).ToXml(new XmlDocument(), true).OuterXml;

			if (!string.IsNullOrWhiteSpace(Xml))
				Clipboard.SetText(Xml);

			return !string.IsNullOrWhiteSpace(Xml);
		}

        
        public bool StatementPaste()
		{
			if (TreeViewNodes.SelectedNode==null)
				return false;
			if (!(TreeViewNodes.SelectedNode.Tag is MissionNode_Event || TreeViewNodes.SelectedNode.Tag is MissionNode_Start))
				return false;

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml("<root>" + Helper.RemoveNodes(Helper.FixMissionXml(Clipboard.GetText())) + "</root>");
            }
            catch
            {
                return false;
            }

            XmlNode root = xDoc.ChildNodes[0];

			List<MissionStatement> newStatements = new List<MissionStatement>();
            TreeNode lastNode = null;

            BeginUpdate(true);

            foreach (XmlNode childNode in root)
            {
                MissionStatement newMStatement = MissionStatement.NewFromXML(childNode, (MissionNode)TreeViewNodes.SelectedNode.Tag);
                newMStatement.Update();
                newStatements.Add(newMStatement);
            }

            int needUpdate = 0;

            for (int i = 0; i < newStatements.Count; i++)
            {
				int j = Settings.Current.InsertNewOverElement ? i : newStatements.Count - 1 - i;

                TreeNode newTNode = new TreeNode();
                MissionStatement newMStatement = newStatements[j];

                newTNode.Text = newMStatement.Text;
                newTNode.ImageIndex = newMStatement.ImageIndex;
                newTNode.SelectedImageIndex = newMStatement.ImageIndex;
                newTNode.Tag = newMStatement;

                lastNode = TreeViewStatements.SelectedNode;
                needUpdate += Statement_AddUnderNode(newTNode, TreeViewStatements.SelectedNode) ? 1 : 0;
                TreeViewStatements.SelectedNode = lastNode;
                lastNode = newTNode;
				TreeViewStatements.ExpandAll();
				
                ImportMissionNodeContentsFromStatementTree();
            }

            if (lastNode!=null)
                TreeViewStatements.SelectedNode = lastNode;

            if (needUpdate > 1)
                RegisterChange("Pasted statements");
            if (needUpdate == 1)
                RegisterChange("Pasted statement");

            EndUpdate(true);

			return true;
		}

        #endregion

        /// <summary>
		/// Used when drag and drop or copypaste or move or delete or w/e operation was preformed on the Statements TreeView 
		/// and will reload MissionNode's list of statements from the control
		/// </summary>
		private void ImportMissionNodeContentsFromStatementTree()
		{
			if (TreeViewNodes.SelectedNode == null)
				throw new ArgumentNullException("Moving statements while selected node is null!?");
			if (!(TreeViewNodes.SelectedNode.Tag is MissionNode))
				throw new ArgumentOutOfRangeException("Moving statements within a non-MissionNode node!");
			
			MissionNode mn = (MissionNode)TreeViewNodes.SelectedNode.Tag;

			mn.Actions.Clear();
			mn.Conditions.Clear();

			foreach (TreeNode node in TreeViewStatements.Nodes)
			{
                if (!(node.Tag is string))
                    continue;

				//Read all actions from the TreeView
				if ((string)node.Tag == "actions")
					foreach (TreeNode child in node.Nodes)
						mn.Actions.Add((MissionStatement)child.Tag);

				//Read all conditions from the TreeView
				if ((string)node.Tag == "conditions")
					foreach (TreeNode child in node.Nodes)
						mn.Conditions.Add((MissionStatement)child.Tag);
			}
		}

        #region Node operation (delete, move, add...)

        public void NodeDelete()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;
			
			BeginUpdate();

			foreach (TreeNode node in TreeViewNodes.SelectedNodes.ToList())
				if (!(node.Tag is MissionNode_Start))
				{
					TreeViewNodes.Nodes.Remove(node);
					if (_bgNode == node)
						_bgNode = _startNode;
				}
				else if (Settings.Current.ClearStartNodeOnDelete)
				{
					((MissionNode_Start)node.Tag).Actions.Clear();
					if (node == StartNode)
						OutputMissionNodeContentsToTree();
				}

			EndUpdate();
			
            RecalculateNodeCount();

			RegisterChange("Deleted node(s)");
		}

		public void NodeMoveUp()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			if (TreeViewNodes.SelectedNode.PrevNode != null)
                TreeViewNodes.MoveNode(TreeViewNodes.SelectedNode, TreeViewNodes.SelectedNode.PrevNode, NodeRelationship.ChildGoesAbove);
		}

		public void NodeMoveDown()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			if (TreeViewNodes.SelectedNode.NextNode != null)
                TreeViewNodes.MoveNode(TreeViewNodes.SelectedNode, TreeViewNodes.SelectedNode.NextNode, NodeRelationship.ChildGoesUnder);
		}

		public void NodeMoveIn()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

            if (TreeViewNodes.SelectedNode.PrevNode != null && TreeViewNodes.IsFolder(TreeViewNodes.SelectedNode.PrevNode) && TreeViewNodes.IsAllowedToHaveRelation(TreeViewNodes.SelectedNode.PrevNode, TreeViewNodes.SelectedNode, NodeRelationship.ChildGoesInside))
                TreeViewNodes.MoveNode(TreeViewNodes.SelectedNode, TreeViewNodes.SelectedNode.PrevNode, NodeRelationship.ChildGoesInside);
			else if (TreeViewNodes.SelectedNode.NextNode != null)
                TreeViewNodes.MoveNode(TreeViewNodes.SelectedNode, TreeViewNodes.SelectedNode.NextNode, NodeRelationship.ChildGoesInside);
		}

		public void NodeMoveOut()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			if (TreeViewNodes.SelectedNode.Parent != null)
                TreeViewNodes.MoveNode(TreeViewNodes.SelectedNode, TreeViewNodes.SelectedNode.Parent, NodeRelationship.ChildGoesUnder);
		}

		private string NodeGetXml()
		{
			if (TreeViewNodes.SelectedNode == null)
				return null;

			if (!(TreeViewNodes.SelectedNode.Tag is MissionNode))
				return null;

			XmlDocument xDoc = new XmlDocument();
			xDoc.AppendChild(((MissionNode)TreeViewNodes.SelectedNode.Tag).ToXml(xDoc));

			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();

			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.NewLineChars = "\r\n";
			settings.NewLineHandling = NewLineHandling.Replace;
			settings.OmitXmlDeclaration = true;
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				xDoc.Save(writer);
			}

			return sb.ToString(); 
		}
		
		public void NodeShowXml()
		{
			if (TreeViewNodes.SelectedNode == null)
				return;

			if (!(TreeViewNodes.SelectedNode.Tag is MissionNode))
				return;

			string result = NodeGetXml();

			if (result != null)
				MessageBox.Show(result, "Xml code of the Node");
		}

		public void NodeCopyXml()
		{
			string result = NodeGetXml();

			if (result != null)
				Clipboard.SetText(result);
		}

		public bool NodeCopy()
		{
			if (TreeViewNodes.SelectedNode == null)
				return false;

			string Xml = "";

			foreach (TreeNode node in TreeViewNodes.SelectedNodes)
				Xml += ((MissionNode)node.Tag).ToXml(new XmlDocument(), true).OuterXml;
			
			if (!string.IsNullOrWhiteSpace(Xml))
				Clipboard.SetText(Xml);

			return !string.IsNullOrWhiteSpace(Xml);
		}
		
		public bool NodePaste_private_RecursivelySearch(TreeNode node, Guid? ID)
		{
			foreach (TreeNode child in node.Nodes)
				if (NodePaste_private_RecursivelySearch(child, ID)) return true;

			if (((MissionNode)node.Tag).ID==ID)
				return true;
			
			return false;
		}

		public bool NodePaste_private_Exists(Guid?  ID)
		{
			foreach (TreeNode node in TreeViewNodes.Nodes)
				if (NodePaste_private_RecursivelySearch(node, ID)) return true;
			return false;
		}

		public bool NodePaste()
		{
			XmlDocument xDoc = new XmlDocument();
			try
			{
                xDoc.LoadXml("<root>" + Helper.FixMissionXml(Clipboard.GetText()) + "</root>");
			}
			catch (Exception e)
			{
				Log.Add("Error parsing XML: "+e.Message);
				return false;
			}

            XmlNode root = xDoc.ChildNodes[0];

			foreach (XmlNode node in root.ChildNodes)
				if (node.Name == "mission_data")
					root = node;

            List<MissionNode> newNodes = new List<MissionNode>();
            TreeNode lastNode = null;
			bool needStartUpdate = false;
			bool folderMode = false;

            BeginUpdate(true);
			
			//PASTING FOLDER
			if (root.ChildNodes.Count == 1 && root.ChildNodes[0].Name == "folder_arme")
			{
				Guid? guid = null;
				TreeNode node = null;

				//Continue if there is a node with such guid in the mission
				if ((guid = MissionNode.NewFromXML(root.ChildNodes[0]).ID) != null && (node = TreeViewNodes.FindNode((TreeNode x) => ((MissionNode)x.Tag).ID == guid)) != null)
				{
					folderMode = true;

					Node_AddUnderNode(Helper.TrueClone(node), TreeViewNodes.SelectedNode);



					RegisterChange("Pasted folder");
				}
			}
			//PASTING STATEMENTS
			if (!folderMode)
			{
				foreach (XmlNode childNode in root)
				{
					if (childNode.Name == "start")
					{
						DialogResult res = MessageBox.Show("Shall the start node contents be appended to the current start node's contents (Yes) or replace them (No)?", "Pasting start node", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

						if (res == DialogResult.Cancel)
							continue;
						if (res == DialogResult.No)
						{
							needStartUpdate = (((MissionNode)_startNode.Tag).Actions.Count > 0);
							((MissionNode)_startNode.Tag).Actions.Clear();
						}

						MissionNode newMNode = MissionNode.NewFromXML(childNode);

						foreach (MissionStatement item in newMNode.Actions)
							((MissionNode)_startNode.Tag).Actions.Add(item);

						lastNode = _startNode;

						needStartUpdate = needStartUpdate || newMNode.Actions.Count > 0;

						if (needStartUpdate)
							RegisterChange("Pasted start node");
					}
					if (childNode.Name == "event" || childNode.Name == "disabled_event" || childNode is XmlComment || childNode.Name == "folder_arme")
					{
						MissionNode newMNode = MissionNode.NewFromXML(childNode);
						newNodes.Add(newMNode);
						newMNode.ParentID = null;
						bool id_exists = NodePaste_private_Exists(newMNode.ID);
						if (id_exists)
							newMNode.ID = Guid.NewGuid();
						if (childNode.Name == "event" || childNode.Name == "disabled_event")
							_eventCount++;
					}
				}

				for (int i = 0; i < newNodes.Count; i++)
				{
					int j = !Settings.Current.InsertNewOverElement ? newNodes.Count - 1 - i : i;

					TreeNode newTNode = new TreeNode();
					MissionNode newMNode = newNodes[j];

					newTNode.Text = newMNode.Name;
					newTNode.ImageIndex = newMNode.ImageIndex;
					newTNode.SelectedImageIndex = newMNode.ImageIndex;
					newTNode.Tag = newMNode;

					lastNode = TreeViewNodes.SelectedNode;
					Node_AddUnderNode(newTNode, TreeViewNodes.SelectedNode);
					TreeViewNodes.SelectedNode = lastNode;
					lastNode = newTNode;
				}

				if (lastNode != null)
					TreeViewNodes.SelectedNode = lastNode;

				if (newNodes.Count > 1)
					RegisterChange("Pasted event nodes");
				if (newNodes.Count == 1)
					RegisterChange("Pasted event node");
			}

			EndUpdate(true);

			if (!needStartUpdate && !folderMode && newNodes.Count == 0)
				return false;
			else
				return true;
		}

		public void NodeExpandAll()
		{
			BeginUpdate();
			TreeViewNodes.ExpandAll();
			EndUpdate();
			if (TreeViewNodes.FindNode((TreeNode x) => x.Tag is MissionNode_Folder && x.Nodes.Count > 0) != null)
				RegisterChange("Expanded all folders");
		}

		public void NodeCollapseAll()
		{
			BeginUpdate();
			TreeViewNodes.CollapseAll();
			EndUpdate();
			if (TreeViewNodes.FindNode((TreeNode x) => x.Tag is MissionNode_Folder && x.Nodes.Count > 0) != null)
				RegisterChange("Collapsed all folders");
		}

        public void OutputMissionNodeContentsToTree()
		{
			BeginUpdate();

			MissionStatement mStatement = TreeViewStatements.SelectedNode != null && TreeViewStatements.SelectedNode.Tag is MissionStatement ? (MissionStatement)TreeViewStatements.SelectedNode.Tag : null;

			TreeViewStatements.NodesClear();
			FlowLayoutPanel_Clear();

			MissionNode mNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;

			UpdateNodeTag();

			TreeNode conditions;
			TreeNode actions;
			TreeNode nNode;
			switch (mNode.GetType().ToString())
			{
				case "ArtemisMissionEditor.MissionNode_Folder":
					nNode = TreeViewStatements.Nodes.Add("folder", mNode.ToXml(new XmlDocument()).OuterXml.Replace("&", "&&"), 3, 3);
					nNode.Tag = mNode;
					LabelMain.Text = "Folder";
					break;
				case "ArtemisMissionEditor.MissionNode_Start":
					actions = TreeViewStatements.Nodes.Add("Actions", "Actions", 1, 1);
					actions.Tag = "actions";
					foreach (MissionStatement item in mNode.Actions)
					{
						nNode = actions.Nodes.Add(item.Text, item.Text.Replace("&", "&&"), item.ImageIndex, item.ImageIndex);
						nNode.Tag = item;
					}
					break;
				case "ArtemisMissionEditor.MissionNode_Event":
					conditions = TreeViewStatements.Nodes.Add("Conditions", "Conditions", 0, 0);
					conditions.Tag = "conditions";
					foreach (MissionStatement item in mNode.Conditions)
					{
                        nNode = conditions.Nodes.Add(item.Text, item.Text.Replace("&","&&"), item.ImageIndex, item.ImageIndex);
						nNode.Tag = item;
					}
					actions = TreeViewStatements.Nodes.Add("Actions", "Actions", 1, 1);
					actions.Tag = "actions";
					foreach (MissionStatement item in mNode.Actions)
					{
						nNode = actions.Nodes.Add(item.Text, item.Text.Replace("&", "&&"), item.ImageIndex, item.ImageIndex);
						nNode.Tag = item;
					}
					break;
				case "ArtemisMissionEditor.MissionNode_Comment":
					nNode = TreeViewStatements.Nodes.Add("comment", mNode.ToXml(new XmlDocument()).OuterXml.Replace("&", "&&"), 2, 2);
					nNode.Tag = mNode;
					LabelMain.Text = "Commentary";
					break;
			}

			TreeViewStatements.ExpandAll();

            if (TreeViewStatements.Nodes.Count>0)
                TreeViewStatements.Nodes[0].EnsureVisible();

            SelectStatement(mStatement);

			EndUpdate();
		}

		public void NodeRename()
		{
			TreeViewNodes.BeginEdit();
		}

		public void NodeEnableDisable(bool enabled)
		{
			int count = 0;
			foreach (TreeNode node in TreeViewNodes.SelectedNodes)
			{
				if (!(node.Tag is MissionNode_Event))
					continue;

				MissionNode_Event mNE = (MissionNode_Event)node.Tag;
				count += mNE.Enabled != enabled ? 1 : 0;
				mNE.Enabled = enabled;
				node.ImageIndex = mNE.ImageIndex;
				node.SelectedImageIndex = mNE.ImageIndex;
			}

			if (count > 0)
				RegisterChange((enabled ? "Enabled" : "Disabled") + " " + count + " event(s)");
		}

        /// <summary>
        /// Adds one node inside other node, if possible, or below it, if not possible
        /// </summary>
        /// <param name="toAdd">Node to be added</param>
        /// <param name="selected">Node that receives the added one</param>
        public void Node_AddUnderNode(TreeNode toAdd, TreeNode selected)
        {
			___STATIC_E_nodeTV_SupressExpandCollapseEvents = true;
            if (selected != null && TreeViewNodes.IsAllowedToHaveRelation(selected, toAdd, NodeRelationship.ChildGoesInside))
                TreeViewNodes.MoveNode(toAdd, selected, NodeRelationship.ChildGoesInside, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesUnder : NodeRelationship.ChildGoesAbove, true);
            else if (selected != null && TreeViewNodes.IsAllowedToHaveRelation(selected, toAdd, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesAbove : NodeRelationship.ChildGoesUnder))
                TreeViewNodes.MoveNode(toAdd, selected, Settings.Current.InsertNewOverElement ? NodeRelationship.ChildGoesAbove : NodeRelationship.ChildGoesUnder, NodeRelationship.Null, true);
            else
                TreeViewNodes.Nodes.Add(toAdd);
			___STATIC_E_nodeTV_SupressExpandCollapseEvents = false;
        }

        public void NodeAddEvent(bool underCursor = false, TreeNode nodeUnderCursor = null)
        {
            _eventCount++;

            TreeNode newTNode = new TreeNode();
            MissionNode_Event newMNode = new MissionNode_Event();

            newTNode.Text = newMNode.Name;
            newTNode.ImageIndex = newMNode.ImageIndex;
            newTNode.SelectedImageIndex = newMNode.ImageIndex;
            newTNode.Tag = newMNode;

            Node_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewNodes.SelectedNode);
            TreeViewNodes.SelectedNode = newTNode;

            RegisterChange("New event node");
        }

        public void NodeAddCommentary(bool underCursor = false, TreeNode nodeUnderCursor = null)
        {
            TreeNode newTNode = new TreeNode();
            MissionNode_Comment newMNode = new MissionNode_Comment();

			newMNode.Name = " - - - - - - - - - - ";
			
			newTNode.Text = newMNode.Name;
            newTNode.ImageIndex = newMNode.ImageIndex;
            newTNode.SelectedImageIndex = newMNode.ImageIndex;
            newTNode.Tag = newMNode;

            Node_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewNodes.SelectedNode);
            TreeViewNodes.SelectedNode = newTNode;

            RegisterChange("New commentary node");
        }

        public void NodeAddFolder(bool underCursor = false, TreeNode nodeUnderCursor = null)
        {
            TreeNode newTNode = new TreeNode();
            MissionNode_Folder newMNode = new MissionNode_Folder();

            newTNode.Text = newMNode.Name;
            newTNode.ImageIndex = newMNode.ImageIndex;
            newTNode.SelectedImageIndex = newMNode.ImageIndex;
            newTNode.Tag = newMNode;

            Node_AddUnderNode(newTNode, underCursor ? nodeUnderCursor : TreeViewNodes.SelectedNode);
            TreeViewNodes.SelectedNode = newTNode;

            RegisterChange("New folder node");
        }
		
        #endregion

		public void Convert_CommentariesIntoNames(bool excludeMultiline = false)
		{
			BeginUpdate();
			for (int i = TreeViewNodes.Nodes.Count - 1; i > 0; i--)
			{
				if (TreeViewNodes.Nodes[i].Tag is MissionNode_Event || TreeViewNodes.Nodes[i].Tag is MissionNode_Folder || TreeViewNodes.Nodes[i].Tag is MissionNode_Start)
				{
					MissionNode mNode = (MissionNode)TreeViewNodes.Nodes[i].Tag;
					if (mNode.DefaultName == 0 && TreeViewNodes.Nodes[i - 1].Tag is MissionNode_Comment && (!excludeMultiline || i == 1 || !(TreeViewNodes.Nodes[i - 2].Tag is MissionNode_Comment)))
					{
						mNode.Name = ((MissionNode_Comment)TreeViewNodes.Nodes[i-1].Tag).Name;
						TreeViewNodes.Nodes[i].Text = mNode.Name;
						TreeViewNodes.Nodes.RemoveAt(i - 1);
						i--;
					}
				}
			}
			EndUpdate();

			RegisterChange("Converted commentaries into names");
		}

        #region Update and Recalculate

        private void RecalculateNodeCount_private_Recursive(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
                RecalculateNodeCount_private_Recursive(child);

            if (node.Tag is MissionNode_Event)
                _eventCount = ((MissionNode_Event)node.Tag).DefaultName > _eventCount ? ((MissionNode_Event)node.Tag).DefaultName : _eventCount;
        }

        private void RecalculateNodeCount()
        {
            _eventCount = 0;
            foreach (TreeNode node in TreeViewNodes.Nodes)
                RecalculateNodeCount_private_Recursive(node);
        }
        
        private void UpdateObjectsText_private_RecursivelyCount(TreeNode node, ref int folders, ref int events, ref int comments, ref int unknowns, ref int actions, ref int conditions)
        {
            foreach (TreeNode child in node.Nodes)
                UpdateObjectsText_private_RecursivelyCount(child, ref folders, ref events, ref comments, ref unknowns,ref actions, ref conditions);

            if (node.Tag is MissionNode_Folder)
                folders++;
            if (node.Tag is MissionNode_Event)
                events++; 
            if (node.Tag is MissionNode_Comment)
                comments++;
            if (node.Tag is MissionNode_Unknown)
                unknowns++;

            if (node.Tag is MissionNode_Event || node.Tag is MissionNode_Start)
            {
                foreach (MissionStatement statement in ((MissionNode)node.Tag).Actions)
                {
                    actions += statement.Kind == MissionStatementKind.Action ? 1 : 0;
                    conditions += statement.Kind == MissionStatementKind.Condition ? 1 : 0;
                    comments += statement.Kind == MissionStatementKind.Commentary ? 1 : 0;
                }
                foreach (MissionStatement statement in ((MissionNode)node.Tag).Conditions)
                {
                    actions += statement.Kind == MissionStatementKind.Action ? 1 : 0;
                    conditions += statement.Kind == MissionStatementKind.Condition ? 1 : 0;
                    comments += statement.Kind == MissionStatementKind.Commentary ? 1 : 0;
                }
            }

                 
        }

        /// <summary> Update TOTAL: text in the status bar </summary>
        private void UpdateObjectsText()
        {
            if (ToolStripObjectsTotal == null)
                return;

            int folders = 0, events = 0, comments = 0, unknowns = 0, actions = 0, conditions = 0;
            foreach (TreeNode node in TreeViewNodes.Nodes)
                UpdateObjectsText_private_RecursivelyCount(node, ref folders, ref events, ref comments, ref unknowns, ref actions, ref conditions);

            ToolStripObjectsTotal.Text = "Total: "+events.ToString()+"E, "+folders.ToString()+"F, "+comments.ToString()+"C";
            if (unknowns > 0)
                ToolStripObjectsTotal.Text += ", "+unknowns.ToString()+"U";
            ToolStripObjectsTotal.Text += " [" + conditions.ToString()+" CND, "+actions.ToString()+" ACT]";


        }

        private void UpdateObjectLists_private_RecursivelyScan(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
                UpdateObjectLists_private_RecursivelyScan(child);

            if (!(node.Tag is MissionNode_Start || node.Tag is MissionNode_Event))
                return;

            //Fill lists based on Conditions
            foreach (MissionStatement statement in ((MissionNode)node.Tag).Conditions)
            {
                if (statement.Kind != MissionStatementKind.Condition)
                    continue;

				if (statement.Name == "if_variable")
				{
					string var_name = statement.GetAttribute("name");
                    if (var_name != null)
                    {
                        if (!_variableNames.Contains(var_name))
                            _variableNames.Add(var_name);
                        if (!VariableCheckNames.Contains(var_name))
                            VariableCheckNames.Add(var_name);
                        if (!VariableCheckLocations.Keys.Contains(var_name))
                            VariableCheckLocations.Add(var_name, new List<MissionNode>());
                        VariableCheckLocations[var_name].Add(((MissionNode)node.Tag));
                    }
				}

				if (statement.Name == "if_timer_finished")
				{
					string var_timer = statement.GetAttribute("name");
                    if (var_timer != null)
                    {
                        if (!_timerNames.Contains(var_timer))
                            _timerNames.Add(var_timer);
                        if (!TimerCheckNames.Contains(var_timer))
                            TimerCheckNames.Add(var_timer);
                    }
				}
            } 
            
            //Fill lists based on Actions
            foreach (MissionStatement statement in ((MissionNode)node.Tag).Actions)
            {
                if (statement.Kind != MissionStatementKind.Action)
                    continue;

                if (statement.Name=="create")
                {
                    string type = statement.GetAttribute("type");
					string named_name;
                    if (type != null && (named_name = statement.GetAttribute("name")) != null)
                    {
                        if (_namedObjects.ContainsKey(type) && !_namedObjects[type].Contains(named_name))
                            _namedObjects[type].Add(named_name);
                        if (!AllCreatedObjectNames.Contains(named_name))
                            AllCreatedObjectNames.Add(named_name);
                    }
                }

				if (statement.Name == "set_variable")
                {
                    string var_name = statement.GetAttribute("name");
                    if (var_name != null)
                    {
                        if (!_variableNames.Contains(var_name))
                            _variableNames.Add(var_name);
                        if (!VariableSetNames.Contains(var_name))
                            VariableSetNames.Add(var_name);
                    }
                }

				if (statement.Name == "set_timer") 
                {
                    string var_timer = statement.GetAttribute("name");
                    if (var_timer != null)
                    {
                        if (!_timerNames.Contains(var_timer))
                            _timerNames.Add(var_timer);
                        if (!TimerSetNames.Contains(var_timer))
                            TimerSetNames.Add(var_timer);
                    }
                }

                if (statement.Name == "end_mission")
                {
                    ContainsMissionEndStatement = true;
                }
            }
        }

        private void UpdateObjectLists()
        {
            foreach (KeyValuePair<string, List<string>> kvp in _namedObjects)
                kvp.Value.Clear();
            ContainsMissionEndStatement = false;
            _variableNames.Clear();
            VariableSetNames.Clear();
            VariableCheckNames.Clear();
            VariableCheckLocations.Clear();
            TimerSetNames.Clear();
            TimerCheckNames.Clear();
            AllCreatedObjectNames.Clear();
            _variableHeaders.Clear();
            _timerNames.Clear();
            _timerHeaders.Clear();

            foreach (TreeNode node in TreeViewNodes.Nodes)
                UpdateObjectLists_private_RecursivelyScan(node);

            _timerNames.Sort();
            _variableNames.Sort();

            if (_variableNames.Count > Settings.Current.NamesPerSubmenu)
            {
                int i;
                for (i = 0; i < _variableNames.Count / Settings.Current.NamesPerSubmenu; i++)
                {
                    string first = _variableNames[i * Settings.Current.NamesPerSubmenu];
                    //if (i == 0 || first[0] != _variables[i * Settings.Current.NamesPerSubmenu - 1][0]) first = first.Substring(0,1).ToUpper();
                    string last = _variableNames[(i + 1) * Settings.Current.NamesPerSubmenu - 1];
                    //if ((i + 1) * Settings.Current.NamesPerSubmenu == _variables.Count - 1 || last[0] != _variables[(i + 1) * Settings.Current.NamesPerSubmenu - 2][0]) last = last.Substring(0, 1).ToUpper();
                    _variableHeaders.Add(first + " - " + last);
                }
                //_variableHeaders.Add(_variables[i * Settings.Current.NamesPerSubmenu] + " - " + _variables[_variables.Count - 1][0]);
				if (_variableNames.Count - 1 >= i * Settings.Current.NamesPerSubmenu)
					_variableHeaders.Add(_variableNames[i * Settings.Current.NamesPerSubmenu] + " - " + _variableNames[_variableNames.Count - 1]);
            }

			if (_timerNames.Count > Settings.Current.NamesPerSubmenu)
			{
				int i;
				for (i = 0; i < _timerNames.Count / Settings.Current.NamesPerSubmenu; i++)
				{
					string first = _timerNames[i * Settings.Current.NamesPerSubmenu];
					//if (i == 0 || first[0] != _timers[i * Settings.Current.NamesPerSubmenu - 1][0]) first = first.Substring(0,1).ToUpper();
					string last = _timerNames[(i + 1) * Settings.Current.NamesPerSubmenu - 1];
					//if ((i + 1) * Settings.Current.NamesPerSubmenu == _timers.Count - 1 || last[0] != _timers[(i + 1) * Settings.Current.NamesPerSubmenu - 2][0]) last = last.Substring(0, 1).ToUpper();
					_timerHeaders.Add(first + " - " + last);
				}
				//_timerHeaders.Add(_timers[i * Settings.Current.NamesPerSubmenu] + " - " + _timers[_timers.Count - 1][0]);
				if (_timerNames.Count - 1 >= i * Settings.Current.NamesPerSubmenu)
					_timerHeaders.Add(_timerNames[i * Settings.Current.NamesPerSubmenu] + " - " + _timerNames[_timerNames.Count - 1]);
			}

			ExpressionMemberValueEditor.TimerName.InvalidateCMS();
			ExpressionMemberValueEditor.VariableName.InvalidateCMS();
			ExpressionMemberValueEditor.NamedAllName.InvalidateCMS();
			ExpressionMemberValueEditor.NamedStationName.InvalidateCMS();
        }

        /// <summary> Update the expression shown in the flow layout panel </summary>
		public void UpdateExpression()
		{
            Control activeControl = null;

			foreach (Control c in FlowLayoutPanelMain.Controls)
				if (c.Focused)
					activeControl = c;
			
			ExpressionMemberValueDescription lastFocusedValueDescription = (activeControl != null && activeControl is NormalSelectableLabel && activeControl.Tag is ExpressionMemberContainer) ? ((ExpressionMemberContainer)activeControl.Tag).Member.ValueDescription : null;
			string lastFocusedValueName = (activeControl != null && activeControl is NormalSelectableLabel && activeControl.Tag is ExpressionMemberContainer) ? ((ExpressionMemberContainer)activeControl.Tag).Member.Name : null;
			List<NormalLabel> listMatchesValueDescription = new List<NormalLabel>();
			List<NormalLabel> listMatchesValueName = new List<NormalLabel>();

			FlowLayoutPanel_Clear();
			
			TreeNode node = TreeViewStatements.SelectedNode;
			if (node == null || !(node.Tag is MissionStatement)) 
				return;
			MissionStatement statement = (MissionStatement)node.Tag;

            int countActive = 0;

			FlowLayoutPanel_Suspend();
			
			//TODO: Update the expression in the flowchart [Forgot what this means] <-- WTF does this mean?
			foreach (ExpressionMemberContainer item in statement.Expression)
			{
				if (!item.Member.ValueDescription.IsDisplayed)
					continue;

				NormalLabel label;
				if (item.Member.ValueDescription.IsInteractive)
				{
                    label = new NormalSelectableLabel(FormMain);
                    label.MouseClick += _E_l_MouseClick;
					label.PreviewKeyDown += _E_l_PreviewKeyDown;
                    label.Number = ++countActive;
				}
				else
				{
					label = new NormalLabel(FormMain);
				}
				
				label.Tag = item;
				
				//l.BorderStyle = BorderStyle.FixedSingle;
				UpdateLabel(label);

				if (item.Member.RequiresLinebreak)
					FlowLayoutPanelMain.SetFlowBreak(FlowLayoutPanelMain.Controls[FlowLayoutPanelMain.Controls.Count - 1], true);

				FlowLayoutPanelMain.Controls.Add(label);

			}

			//Find all items that match value name or value description
			foreach (Control c in FlowLayoutPanelMain.Controls)
			{
				if (((ExpressionMemberContainer)((NormalLabel)c).Tag).Member.Name == lastFocusedValueName)
					listMatchesValueName.Add((NormalLabel)c);
				if (((ExpressionMemberContainer)((NormalLabel)c).Tag).Member.ValueDescription == lastFocusedValueDescription)
					listMatchesValueDescription.Add((NormalLabel)c);
			}

			switch (listMatchesValueDescription.Count)
			{
				case 0:
					//If none matches description - pick one that matches name
					if (listMatchesValueName.Count > 0) listMatchesValueName[0].Focus();
					break;
				case 1:
					//If only one matches the description - pick it
					listMatchesValueDescription[0].Focus();
					break;
				default:
					//If more than one matches the description, but none match the name - pick first from description
					if (listMatchesValueName.Count == 0)
						listMatchesValueDescription[0].Focus();
					//If at least one matches in name ...
					else
					{
						//We need to narrow down...
						for (int i = listMatchesValueName.Count - 1; i >= 0; i--)
						{
							//Remove all that match in name that do not match in description
							if (!listMatchesValueDescription.Contains(listMatchesValueName[i]))
								listMatchesValueName.RemoveAt(i);
						}
						//If none of those matching in name match in description -  pick first from description
						if (listMatchesValueName.Count == 0)
							listMatchesValueDescription[0].Focus();
						//If only one matches in name and in description - pick it
						if (listMatchesValueName.Count == 1)
							listMatchesValueName[0].Focus();
						//If more than one matches in name and in description...
						if (listMatchesValueName.Count > 1)
						{
							//We need to narrow down again
							for (int i = listMatchesValueDescription.Count - 1; i >= 0; i--)
							{
								//Remove all that match in description but do not match in name
								if (!listMatchesValueName.Contains(listMatchesValueDescription[i]))
									listMatchesValueDescription.RemoveAt(i);
							}
							//If at least one matches in description and in name = pick first from description
							if (listMatchesValueDescription.Count >= 1)
								listMatchesValueDescription[0].Focus();
							else // else pick first from those matching by name
								listMatchesValueName[0].Focus();
						}
					}
					break;
			}

			
			FlowLayoutPanel_Resume();
		}
		
		private void SelectExpressionLabel(int index)
		{
			foreach (Control c in FlowLayoutPanelMain.Controls)
			{
				if (c is NormalSelectableLabel && ((NormalSelectableLabel)c).Number == index)
				{
                    if (((NormalSelectableLabel)c).SelectedByKeyboard && c.Focused)
                        _E_l_Activated((NormalLabel)c);
                    c.Focus();
                    ((NormalSelectableLabel)c).SelectedByKeyboard = true;
					return;
				}
			}
		}

        private void UpdateStatementTree_private_Recursive(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
                UpdateStatementTree_private_Recursive(child);

			if (!(node.Tag is MissionStatement))
				return;

            MissionStatement ms = (MissionStatement)node.Tag;
            if (ms.Invalidated)
            {
                if (ms.InvalidatedLabel)
                {
					node.Text = ms.Text.Replace("&", "&&");
                    node.SelectedImageIndex = ms.ImageIndex;
                    node.ImageIndex = ms.ImageIndex;
                }

                if (node == TreeViewStatements.SelectedNode)
                {
                    if (ms.InvalidatedExpression)
                    {
						node.Text = ms.Text.Replace("&", "&&");
                        UpdateExpression();
                        ms.ConfirmUpdate();
                    }

                    if (ms.InvalidatedLabel)
                        foreach (Control c in FlowLayoutPanelMain.Controls)
                            UpdateLabel((NormalLabel)c);

                }

                ms.ConfirmUpdate();
            }
        }
        
        /// <summary> Update all the statements in the statement tree and updates expression if nessecary </summary>
		public void UpdateStatementTree()
		{
			BeginUpdate();
			foreach (TreeNode node in TreeViewStatements.Nodes)
				UpdateStatementTree_private_Recursive(node);
			EndUpdate();
		}

		/// <summary> Refreshes the label display - resetting text, width and height </summary>
		private void UpdateLabel(NormalLabel l)
		{
			Graphics g = l.CreateGraphics();
			l.Text = ((ExpressionMemberContainer)l.Tag).GetValueDisplay();
			l.SpecialMode = l is NormalSelectableLabel && ((ExpressionMemberContainer)l.Tag).IsInvalid;
				
			if (string.IsNullOrWhiteSpace(l.Text)&&l is NormalSelectableLabel) l.Text = "[]";
			l.Width = (int)Math.Round(g.MeasureString(l.Text, l.Font, 100000, StringFormat.GenericTypographic).Width + (l.Text.Length > 0 && l.Text.Substring(l.Text.Length - 1) == " " ? g.MeasureString(" ", l.Font).Width : 0) + 0.5);
			//If width is too big (label wont fit into control) 
			if (l.Width > FlowLayoutPanelMain.Width - 23 && !((ExpressionMemberContainer)l.Tag).Member.RequiresLinebreak)
			{
				//and the text length its required to have in order to fit is bigger than zero then refit the string
				if (l.Text.Length * (FlowLayoutPanelMain.Width - 23) / l.Width - 4 > 0)
					l.Text = l.Text.Substring(0, l.Text.Length * (FlowLayoutPanelMain.Width - 23) / l.Width - 4) + "... ";
				//else shrink the string to one character
				else
					l.Text = l.Text.Substring(0, 1) + "... ";
				l.Width = FlowLayoutPanelMain.Width - 23;
				if (l.Width < 0) l.Width = 0;
			}
			l.Height = (int)Math.Round(g.MeasureString(l.Text, l.Font).Height + 0.5);
			l.Invalidate();
		}

		/// <summary> Output selected node's tag to the tabpage's top text </summary>
		private void UpdateNodeTag()
		{
			LabelMain.Text = ((MissionNode)TreeViewNodes.SelectedNode.Tag).Name;
		}

        #endregion

        public bool CanGetStatementXmlText()
		{
			if (TreeViewStatements.SelectedNode == null)
				return false;

			if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
				return false;

			return true;
		}
		
		public bool CanGetNodeXmlText()
		{
			if (TreeViewNodes.SelectedNode == null)
				return false;

			if (!(TreeViewNodes.SelectedNode.Tag is MissionNode))
				return false;

			return true;
		}

        public void ShowFindForm()
        {
			Program.FormFindReplaceInstance._FFR_tc_Main.SelectedTab = Program.FormFindReplaceInstance._FFR_tc_Main.TabPages[0];
			Program.FormFindReplaceInstance.Show();
			if (Program.FormSearchResultsInstance.Visible)
				Program.FormSearchResultsInstance.BringToFront();
			Program.FormFindReplaceInstance.BringToFront();
		}

        public void ShowReplaceForm()
        {
			Program.FormFindReplaceInstance._FFR_tc_Main.SelectedTab = Program.FormFindReplaceInstance._FFR_tc_Main.TabPages[1];
			Program.FormFindReplaceInstance.Show();
			if (Program.FormSearchResultsInstance.Visible)
				Program.FormSearchResultsInstance.BringToFront();
			Program.FormFindReplaceInstance.BringToFront();
        }

		public void ShowEventDependencyForm(bool recalculate = false)
		{
			Program.FormDependencyInstance.OpenEventDependency(TreeViewNodes.SelectedNode, recalculate);
		}

		public void ShowMissionPropertiesForm()
		{
			if (Program.FormMissionPropertiesInstance.Visible)
				Program.FormMissionPropertiesInstance.BringToFront();
			else
			{
				Program.FormMissionPropertiesInstance.ReadDataFromMission();
				Program.FormMissionPropertiesInstance.Show();
			}
		}

		/// <summary> Check for match, taking search terms in account </summary>
		public bool Find_CheckStringForMatch(string text1, MissionSearchCommand msc)
		{
			string text2 = msc.Input;
			if (!msc.MatchCase)
			{
				text1 = text1.ToLower();
				text2 = text2.ToLower();
			}

			if (msc.MatchExact)
				return text1 == text2;
			else
				return text1.Contains(text2);
		}

        /// <summary> Checks if current selection (node and/or statement) matches the search criteria </summary>
        public bool Find_DoesCurrentSelectionMatch(MissionSearchCommand msc)
        {
            if (Find_CheckNodeForMatch(TreeViewNodes.SelectedNode, (MissionNode)TreeViewNodes.SelectedNode.Tag, -1, msc).Valid)
                return true;

			if (TreeViewStatements.SelectedNode == null || !(TreeViewStatements.SelectedNode.Tag is MissionStatement))
				return false;

            return Find_CheckStatementForMatch(TreeViewNodes.SelectedNode, (MissionStatement)TreeViewStatements.SelectedNode.Tag, -1, -1, msc).Valid;
        }

        /// <summary> Check if the mission node matches the search criteria, return search result if it does </summary>
        public MissionSearchResult Find_CheckNodeForMatch(TreeNode node, MissionNode mNode, int curNode, MissionSearchCommand msc)
        {
            bool statisfies = false;

            //We are interested in event/start/folder nodes...
            if (msc.NodeNames && (node.Tag is MissionNode_Event || node.Tag is MissionNode_Folder || node.Tag is MissionNode_Start))
                statisfies = statisfies | Find_CheckStringForMatch(mNode.Name, msc);

            //... or commentaries
            if (msc.Commentaries && node.Tag is MissionNode_Comment)
                statisfies = statisfies | Find_CheckStringForMatch(mNode.Name, msc);
                    
            if (statisfies)
                return new MissionSearchResult(curNode, 0, mNode.Name, node, null);

            return new MissionSearchResult(curNode, 0, null, node, null);
        }

        /// <summary> Check if mission statement matches the search criteria, return search result if it does </summary>
        public MissionSearchResult Find_CheckStatementForMatch(TreeNode node, MissionStatement statement, int curNode, int curStatement, MissionSearchCommand msc)
        {
            //We are interested in comments only if we are especially looking for them
            if (statement.Kind == MissionStatementKind.Commentary)
            {
                if (msc.Commentaries)
                    if (Find_CheckStringForMatch(statement.Body, msc))
                        return new MissionSearchResult(curNode, curStatement, statement.Body, node, statement);

                return new MissionSearchResult(curNode, curStatement, null, node, statement);
            }
            
            bool statisfies = false;

            //Look for xml attribute names
            if (msc.XmlAttName)
                foreach (KeyValuePair<string, string> kvp in statement.GetAttributes())
                    statisfies = statisfies | Find_CheckStringForMatch(kvp.Key, msc);

            //Look for xml attribute values
            if (msc.XmlAttValue)
                foreach (KeyValuePair<string, string> kvp in statement.GetAttributes())
                    statisfies = statisfies | ((string.IsNullOrEmpty(msc.AttName) || kvp.Key == msc.AttName) && Find_CheckStringForMatch(kvp.Value, msc));

            //Look for statement text
            if (msc.StatementText)
                statisfies = statisfies | Find_CheckStringForMatch(statement.Text, msc);

            if (statisfies)
                return new MissionSearchResult(curNode, curStatement, statement.Text, node, statement);

            return new MissionSearchResult(curNode, curStatement, null, node, statement);
        }

        /// <summary> Add the search result to the list if its not null </summary>
        public bool Find_TryAdd(List<MissionSearchResult> list, MissionSearchResult item, bool first, ref int limitNode, ref int limitStatement)
        {
            //Stopper for when coming for a second time
            bool last = limitNode == item.CurNode && limitStatement == item.CurStatement;
            
            //Remember from what to begin if in first mode
            if (first && !last && TreeViewNodes.SelectedNode == item.Node && (GetSelectedStatementPos() == item.CurStatement || (TreeViewStatements.SelectedNode != null && TreeViewStatements.SelectedNode.Tag == item.Statement)))
            {
                limitNode = item.CurNode;
                limitStatement = item.CurStatement;
                return false;
            }

            if (item.Valid && (!first || limitNode!=-1))
            {
                list.Add(item);
                return first || last;
            }
            return last;
        }
        
		public bool FindAll_private_RecursivelyFind(TreeNode node, ref int curNode, List<MissionSearchResult> list, MissionSearchCommand msc, bool forward, bool first, ref int limitNode, ref int limitStatement)
		{
            MissionNode mNode = (MissionNode)node.Tag;

            curNode += forward ? 1 : -1;
            int curStatement = forward ? 0 : mNode.Conditions.Count + mNode.Actions.Count + 1;

            for (int i = 0; i < node.Nodes.Count; i++)
                if (FindAll_private_RecursivelyFind(node.Nodes[forward ? i : node.Nodes.Count - 1 - i], ref curNode, list, msc, forward, first, ref limitNode, ref limitStatement)) return true;

            //Skip node in we are only looking in the current node and this isnt current node
            if (msc.OnlyInCurrentNode && !TreeViewNodes.NodeIsInsideNode(node, TreeViewNodes.SelectedNode))
                return false;

			if (forward)
			{
				//Check if node matches our search criteria
				if (Find_TryAdd(list, Find_CheckNodeForMatch(node, mNode, curNode, msc), first, ref limitNode, ref limitStatement)) return true;

				//Then we start looking through statements
				for (int i = 0; i < mNode.Conditions.Count; i++)
					if (Find_TryAdd(list, Find_CheckStatementForMatch(node, mNode.Conditions[forward ? i : mNode.Conditions.Count - 1 - i], curNode, forward ? ++curStatement : --curStatement, msc), first, ref limitNode, ref limitStatement)) return true;
				for (int i = 0; i < mNode.Actions.Count; i++)
					if (Find_TryAdd(list, Find_CheckStatementForMatch(node, mNode.Actions[forward ? i : mNode.Actions.Count - 1 - i], curNode, forward ? ++curStatement : --curStatement, msc), first, ref limitNode, ref limitStatement)) return true;
			}
			else
			{
				//Then we start looking through statements
				for (int i = 0; i < mNode.Actions.Count; i++)
					if (Find_TryAdd(list, Find_CheckStatementForMatch(node, mNode.Actions[forward ? i : mNode.Actions.Count - 1 - i], curNode, forward ? ++curStatement : --curStatement, msc), first, ref limitNode, ref limitStatement)) return true;
				for (int i = 0; i < mNode.Conditions.Count; i++)
					if (Find_TryAdd(list, Find_CheckStatementForMatch(node, mNode.Conditions[forward ? i : mNode.Conditions.Count - 1 - i], curNode, forward ? ++curStatement : --curStatement, msc), first, ref limitNode, ref limitStatement)) return true;

				//Check if node matches our search criteria
				if (Find_TryAdd(list, Find_CheckNodeForMatch(node, mNode, curNode, msc), first, ref limitNode, ref limitStatement)) return true;
			}
			
            return false;
		}

		/// <summary>
		/// Find all items matching the criteria set in search structure and return the list of matching items
        /// Can be used to look for the first after the current too
		/// </summary>
		public List<MissionSearchResult> FindAll(MissionSearchCommand msc, bool forward = true, bool first = false)
        {
            List<MissionSearchResult> result = new List<MissionSearchResult>();

            int curNode, limitNode = -1, limitStatement = -1;

            for (int j = 0; j < 1 + (first ? 1 : 0); j++)//do twice if looking for first
            {
                curNode = forward ? 0 : GetNodeCount() + 1;
                for (int i = 0; i < TreeViewNodes.Nodes.Count; i++)
                    if (FindAll_private_RecursivelyFind(TreeViewNodes.Nodes[forward ? i : TreeViewNodes.Nodes.Count - 1 - i], ref curNode, result, msc, forward, first, ref limitNode, ref limitStatement)) break;
            }

            return result;
        }

        //TODO: Finish writing this
        public List<MissionSearchResult> FindProblems()
        {
            List<MissionSearchResult> result = new List<MissionSearchResult>();

            int curNode = 0;

            foreach (TreeNode node in TreeViewNodes.Nodes)
                FindProblems_RecursivelyCheckNodes(node, ref curNode, result);

            if (!ContainsMissionEndStatement)
                result.Add(new MissionSearchResult(0, 0, "\"End Mission\" action is not present anywhere in the script. Unless you are making a script that only ends with player ship being destroyed, you should consider adding this action.", null, null));

            return result;
        }

        private void FindProblems_RecursivelyCheckNodes(TreeNode node, ref int curNode, List<MissionSearchResult> result)
        {
            //Preset list of statements that take names (under different, khm, names) of named objects
            string[] statementsThatTakeName = new string[]{
                "destroy", 
                "destroy_near", 
                "add_ai",
                "clear_ai",
                "direct",
                "set_object_property",
                "addto_object_property",
                "set_to_gm_position",
                "set_ship_text",
                "set_special",
                "set_side_value",
                "if_inside_box",
                "if_outside_box",
                "if_inside_sphere",
                "if_outside_sphere",
                "if_docked",
                "if_player_is_targeting",
                "if_exists",
                "if_not_exists",
                "if_object_property",
                };
            string[] statementsThatTakeTargetName = new string[]{
                "direct",
                };
            string[] statementsThatTakeName12 = new string[]{
                "copy_object_property",
                "set_relative_position",
                "if_distance",
                };

            curNode++;

            if (node.Tag as MissionNode_Start != null || node.Tag as MissionNode_Event != null)
            {
                MissionNode mNode = (MissionNode)node.Tag;
                bool isStartNode = node.Tag as MissionNode_Start != null;

                // Find all objects created in this node
                List<string> namesCreatedInThisNode = new List<string>();
                bool hasEndMission = false;
                bool hasNonEndMission = false;
                foreach (MissionStatement statement in mNode.Actions)
                {
                    if (statement.Kind != MissionStatementKind.Action)
                        continue;
                    string attName;
                    if (statement.Name == "create" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")))
                        namesCreatedInThisNode.Add(attName);
                    if (statement.Name == "end_mission")
                        hasEndMission = true;
                    else
                        hasNonEndMission = true;
                }

                if (hasEndMission && hasNonEndMission)
                    result.Add(new MissionSearchResult(curNode, -1, "Other actions in the same node with the \"End Mission\" action make little sense, as the mission will end immediately and their effects will most likely go unnoticed by the players.", node, null));

                if (!isStartNode)
                {
                    // Event has no conditions
                    if (mNode.Conditions.Count == 0)
                    {
                        result.Add(new MissionSearchResult(curNode, 0, "An event contains no conditions. It will keep executing on every tick, potentially introducing performance issues or even crashes.", node, null));
                    }
                    if (mNode.Conditions.Count > 0)
                    {
                        bool onlyTimers = true;
                        bool onlyNotExists = true;
                        bool noIfVariable = true;
                        List<string> variablesCheckedHere = new List<string>();
                        List<string> timersCheckedHere = new List<string>();
                        foreach (MissionStatement statement in mNode.Conditions)
                        {
                            if (statement.Kind == MissionStatementKind.Condition && statement.Name != "if_timer_finished")
                                onlyTimers = false;
                            if (statement.Kind == MissionStatementKind.Condition && statement.Name != "if_not_exists")
                                onlyNotExists = false;
                            if (statement.Kind == MissionStatementKind.Condition && statement.Name == "if_variable")
                            {
                                noIfVariable = false;
                                variablesCheckedHere.Add(statement.GetAttribute("name"));
                            }
                            if (statement.Kind == MissionStatementKind.Condition && statement.Name == "if_timer_finished")
                                timersCheckedHere.Add(statement.GetAttribute("name"));
                        }
                        
                        // Event has only "timer_finished" conditions
                        if (onlyTimers && !hasEndMission)
                            result.Add(new MissionSearchResult(curNode, 0, "Event contains only \"Timer finished\" condition(s). Once the timer finishes, it will keep executing on every tick, potentially introducing performance issues or even crashes.", node, null));
                        
                        // Event has only "object_not_exists" conditions
                        if (onlyNotExists && !hasEndMission)
                            result.Add(new MissionSearchResult(curNode, 0, "Event contains only \"Object does not exist\" condition(s). While the specified object does not exist, it will keep executing on every tick, potentially introducing performance issues or even crashes.", node, null));
                        
                        // Event has no "if_variable" conditions
                        if (noIfVariable && !hasEndMission)
                            result.Add(new MissionSearchResult(curNode, 0, "Event contains no \"If variable\" condition. While this is not a mistake, it is recommended to have at least one such condition, or ensure that something inside the event always invalidates one of the conditions.", node, null));
                        
                        // Event has no set_variable mirroring if_variable
                        if (timersCheckedHere.Count > 0 || variablesCheckedHere.Count > 0)
                        {
                            List<string> variablesSetHere = new List<string>();
                            List<string> timersSetHere = new List<string>();
                            for (int i = 0; i < mNode.Actions.Count; i++)
                            {
                                if (mNode.Actions[i].Kind == MissionStatementKind.Action && mNode.Actions[i].Name == "set_variable")
                                    variablesSetHere.Add(mNode.Actions[i].GetAttribute("name"));
                                if (mNode.Actions[i].Kind == MissionStatementKind.Action && mNode.Actions[i].Name == "set_timer")
                                    timersSetHere.Add(mNode.Actions[i].GetAttribute("name"));
                            }
                            bool noCorrespondingSet = true;
                            foreach (string checkedVar in variablesCheckedHere)
                            {
                                if (String.IsNullOrEmpty(checkedVar))
                                    continue;
                                foreach (string setVar in variablesSetHere)
                                {
                                    if (String.IsNullOrEmpty(setVar))
                                        continue;
                                    if (setVar == checkedVar)
                                        noCorrespondingSet = false;
                                }
                            }
                            foreach (string checkedTimer in timersCheckedHere)
                            {
                                if (String.IsNullOrEmpty(checkedTimer))
                                    continue;
                                foreach (string setTimer in timersSetHere)
                                {
                                    if (String.IsNullOrEmpty(setTimer))
                                        continue;
                                    if (setTimer == checkedTimer)
                                        noCorrespondingSet = false;
                                }
                            }
                            if (noCorrespondingSet && !hasEndMission)
                            {
                                string innerText = "";
                                if (variablesCheckedHere.Count > 0)
                                    innerText += "no \"Set variable\" condition matching an \"If variable\" condition";
                                if (timersCheckedHere.Count > 0)
                                    innerText += (String.IsNullOrEmpty(innerText) ? "" : ", and ") + "no \"Set timer\" condition matching an \"If timer finished\" condition";
                                result.Add(new MissionSearchResult(curNode, 0, "Event contains " + innerText + ". While this is not a mistake, generally you should change at least one variable or start at least one timer present in the conditions in order to prevent the event from immediately executing again. If leaving as is, ensure that something inside the event always invalidates one of the conditions.", node, null));
                            }
                        }

                        for (int i = 0; i < mNode.Conditions.Count; i++)
                        {
                            MissionStatement statement = mNode.Conditions[i];
                            if (statement.Kind != MissionStatementKind.Condition)
                                continue;

                            // Reference to a variable that is never set
                            string attName;
                            if (statement.Name == "if_variable" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && !VariableSetNames.Contains(attName))
                                result.Add(new MissionSearchResult(curNode, i + 1, "Variable named \"" + attName + "\" is checked for, but never set.", node, statement));

                            // Reference to a timer that is never set
                            if (statement.Name == "if_timer_finished" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && !TimerSetNames.Contains(attName))
                                result.Add(new MissionSearchResult(curNode, i + 1, "Timer named \"" + attName + "\" is checked for, but never set.", node, statement));

                            // Distance equality check (hardly ever happens)
                            if (statement.Name == "if_distance" && (statement.GetAttribute("comparator") == "EQUALS" || statement.GetAttribute("comparator") == "NOT"))
                                result.Add(new MissionSearchResult(curNode, i + 1, "Distance is checked for equality. In practice, distance will almost never be equal to an exact value. You should use \"lesser\" and \"greater\" comparisons instead.", node, statement));

                            // Reference to a name never created
                            List<string> namesToCheck = new List<string>();
                            if (statementsThatTakeName.Contains(statement.Name))
                                namesToCheck.Add(statement.GetAttribute("name"));
                            if (statementsThatTakeTargetName.Contains(statement.Name))
                                namesToCheck.Add(statement.GetAttribute("targetName"));
                            if (statementsThatTakeName12.Contains(statement.Name))
                            {
                                namesToCheck.Add(statement.GetAttribute("name1"));
                                namesToCheck.Add(statement.GetAttribute("name2"));
                            }
                            foreach (string name in namesToCheck)
                            {
                                if (String.IsNullOrWhiteSpace(name))
                                    continue;
                                if (!AllCreatedObjectNames.Contains(name))
                                    result.Add(new MissionSearchResult(curNode, i + 1, "Object named \"" + name + "\" is referenced in a statement, but never created.", node, statement));
                            }
                        }
                    }
                }

                for (int i = 0; i < mNode.Actions.Count; i++)
                {
                    MissionStatement statement = mNode.Actions[i];
                    if (statement.Kind != MissionStatementKind.Action)
                        continue;

                    // Reference to a variable that is never checked
                    string attName;
                    if (statement.Name == "set_variable" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && !VariableCheckNames.Contains(attName))
                        result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "Variable named \"" + attName + "\" is set, but never checked.", node, statement));
                    
                    // Reference to a timer that is never checked
                    if (statement.Name == "set_timer" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && !TimerCheckNames.Contains(attName))
                        result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "Timer named \"" + attName + "\" is set, but never checked.", node, statement));
                    
                    // Add/set_property for object which name figures in the list of objects created in the same statement
                    // TODO: Confirm or denyt this problem exists and fix this statement (also maybe copy_object_property?
                    //if ((statement.Name == "addto_object_property" || statement.Name == "set_object_property"|| statement.Name == "add_ai"|| statement.Name == "clear_ai") && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && listCreatedInThisNode.Contains(attName))
                    //    result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "Changing properties of an object \"" + attName + "\" in the event where it was just created will not work correctly.", node, statement));
                    
                    // Setting side of an object that was just created
                    if (statement.Name == "set_side_value" && !String.IsNullOrEmpty(attName = statement.GetAttribute("name")) && namesCreatedInThisNode.Contains(attName))
                        result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "You can set the side value of an object \"" + attName + "\" in its create statement without utilising a second statement.", node, statement));
                    
                    // Check path to art
                    if (statement.Name == "create" && statement.GetAttribute("type") == "genericMesh")
                    {
                        string[] pathsToCheck = new string[] { statement.GetAttribute("meshFileName"), statement.GetAttribute("textureFileName") };
                        foreach (string path in pathsToCheck)
                        {
                            if (String.IsNullOrWhiteSpace(path))
                                continue;
                            if (path.Contains(":\\"))
                                result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "It looks like you have specified an absolute path. You should avoid using absolute paths, as they will almost never be the same on the other people's computers.", node, statement));
                            else if (!path.Contains("dat"))
                                result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "It looks like you have specified a path relative to the mission folder. Paths to art assets should be relative to the Artemis folder.", node, statement));
                        }
                    }
                    
                    // Check path to sound
                    string soundPath;
                    if ((statement.Name == "incoming_message" && !String.IsNullOrWhiteSpace(soundPath = statement.GetAttribute("fileName")))
                     || (statement.Name == "play_sound_now" && !String.IsNullOrWhiteSpace(soundPath = statement.GetAttribute("filemame"))))
                    {
                        string path = soundPath;
                        if (String.IsNullOrWhiteSpace(path))
                            continue;
                        if (path.Contains(":\\"))
                            result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "It looks like you have specified an absolute path. You should avoid using absolute paths, as they will almost never be the same on the other people's computers.", node, statement));
                        else if (path.Contains("dat"))
                            result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "It looks like you have specified a path relative to the Artemis folder. Paths to sound assets should be relative to the mission folder.", node, statement));
                    }
                    
                    // Refernce to a name never created
                    List<string> namesToCheck = new List<string>();
                    if (statementsThatTakeName.Contains(statement.Name))
                        namesToCheck.Add(statement.GetAttribute("name"));
                    if (statementsThatTakeTargetName.Contains(statement.Name))
                        namesToCheck.Add(statement.GetAttribute("targetName"));
                    if (statementsThatTakeName12.Contains(statement.Name))
                    {
                        namesToCheck.Add(statement.GetAttribute("name1"));
                        namesToCheck.Add(statement.GetAttribute("name2"));
                    }
                    foreach (string name in namesToCheck)
                    {
                        if (String.IsNullOrWhiteSpace(name))
                            continue;
                        if (!AllCreatedObjectNames.Contains(name))
                            result.Add(new MissionSearchResult(curNode, mNode.Conditions.Count + i + 1, "Object named \"" + name + "\" is referenced in a statement, but never created.", node, statement));
                    }
                }
            }
            
            foreach (TreeNode child in node.Nodes)
                FindProblems_RecursivelyCheckNodes(child, ref curNode, result);
        }


		public bool HighlightErrors_private_CheckStatement(MissionStatement statement)
		{
			bool result = !statement.IsGreen();
			if (result)
				TreeViewStatements.HighlightedTagList.Add(statement);
			return result;
		}

		public int HighlightErrors_private_RecursivelyFind(TreeNode node)
		{
			foreach (TreeNode cnode in node.Nodes)
				HighlightErrors_private_RecursivelyFind(cnode);
			
			bool error = false;
			foreach(MissionStatement statement in ((MissionNode)node.Tag).Conditions)
				error = error | HighlightErrors_private_CheckStatement(statement);

			foreach(MissionStatement statement in ((MissionNode)node.Tag).Actions)
				error = error | HighlightErrors_private_CheckStatement(statement);
		
			if (error)
				TreeViewNodes.HighlightedTagList.Add(node.Tag);

			return error ? 1 : 0;
		}

		public void HighlightErrors()
		{
			TreeViewNodes.HighlightedTagList.Clear();
			TreeViewStatements.HighlightedTagList.Clear();

			int count = 0;

			foreach (TreeNode node in TreeViewNodes.Nodes)
				count += HighlightErrors_private_RecursivelyFind(node);

			Log.Add("Total "+count+" nodes with errors found.");

			TreeViewNodes.Invalidate();
			TreeViewStatements.Invalidate();
		}

        public string Replace_ReplaceInString(string text1, MissionSearchCommand msc)
        {
            if (msc.MatchExact)
                text1 = msc.Replacement;
            else
                if (msc.MatchCase)
					text1 = text1.Replace(msc.Input, msc.Replacement);
                else
					text1 = Helper.StringReplaceEx(text1, msc.Input, msc.Replacement);
            return text1;
        }

		public int Replace_InNode(TreeNode node, int curNode, List<MissionSearchResult> list, MissionSearchCommand msc)
        {
            int replacements = 0;

            //Replace in node name
			if ((msc.NodeNames || msc.Commentaries) && node != null)
			{
				MissionNode mNode = (MissionNode)node.Tag;

				//We are interested in event/start/folder nodes...
				if ((msc.NodeNames && (node.Tag is MissionNode_Event || node.Tag is MissionNode_Folder || node.Tag is MissionNode_Start))
					|| msc.Commentaries && node.Tag is MissionNode_Comment)
					if (Find_CheckStringForMatch(mNode.Name, msc))
					{
						mNode.Name = Replace_ReplaceInString(mNode.Name, msc);
						replacements++;
					}

				node.Text = mNode.Name;
				if (replacements>0)
					list.Add(new MissionSearchResult(curNode, 0, mNode.Name, node, null));
			}

			return replacements;
		}

		public int Replace_InStatement(TreeNode node, MissionStatement statement, int curNode, int curStatement, List<MissionSearchResult> list, MissionSearchCommand msc)
		{
			int replacements = 0;

            //Replace in statement
			if (statement!=null)
            {
				//We are interested in comments only if we are especially looking for them
                if (statement.Kind == MissionStatementKind.Commentary)
                {
                    if (msc.Commentaries)
                        if (Find_CheckStringForMatch(statement.Body, msc))
                        {
                            statement.Body = Replace_ReplaceInString(statement.Body, msc);
                            replacements++;
                        }
                }
                else
                    if (msc.XmlAttValue) //Look for xml attribute values
                        foreach (KeyValuePair<string, string> kvp in statement.GetAttributes())
                            if ((string.IsNullOrEmpty(msc.AttName) || kvp.Key == msc.AttName) && Find_CheckStringForMatch(kvp.Value, msc))
                            {
                                statement.SetAttribute(kvp.Key, Replace_ReplaceInString(kvp.Value, msc));
                                replacements++;
                            }
				statement.Update();
				if (replacements>0)
					list.Add(new MissionSearchResult(curNode, curStatement, statement.Text, node, statement));
            }

			return replacements;
        }

		/// <summary> Replace in currently selected node and statement </summary>
		public int ReplaceCurrent(MissionSearchCommand msc)
		{
			int replacements = 0;

			replacements += Replace_InNode(TreeViewNodes.SelectedNode, 0, new List<MissionSearchResult>(), msc);
			replacements += TreeViewStatements.SelectedNode == null || !(TreeViewStatements.SelectedNode.Tag is MissionStatement) ? 0 : Replace_InStatement(TreeViewNodes.SelectedNode, (MissionStatement)TreeViewStatements.SelectedNode.Tag, 0, 0, new List<MissionSearchResult>(), msc);

			OutputMissionNodeContentsToTree();

			RegisterChange("Replaced '" + msc.Input + "' with '" + msc.Replacement + "' " + replacements.ToString() + " time(s).");

			return replacements;
		}

		public int ReplaceAll_private_RecursiveReplace(TreeNode node, ref int curNode, List<MissionSearchResult> list, MissionSearchCommand msc)
		{
			int replacements = 0;

			curNode++;
			int curStatement = 0;

			for (int i = 0; i < node.Nodes.Count; i++)
				replacements += ReplaceAll_private_RecursiveReplace(node.Nodes[i], ref curNode, list, msc);

			//Skip node in we are only looking in the current node and this isnt current node
			if (msc.OnlyInCurrentNode && !TreeViewNodes.NodeIsInsideNode(node, TreeViewNodes.SelectedNode))
                return replacements;

			MissionNode mNode = (MissionNode)node.Tag;
			
			//Check if node matches our search criteria (before statements if going forward)
			replacements += Replace_InNode(node, curNode, list, msc);

			//Then we start looking through statements
			for (int i = 0; i < mNode.Conditions.Count; i++)
				replacements += Replace_InStatement(node, mNode.Conditions[i], curNode, ++curStatement, list, msc);
			for (int i = 0; i < mNode.Actions.Count; i++)
				replacements += Replace_InStatement(node, mNode.Actions[i], curNode, ++curStatement, list, msc); 

			return replacements;
		}

        public int ReplaceAll(List<MissionSearchResult> list, MissionSearchCommand msc)
        {
            BeginUpdate();

			int replacements = 0;
			int curNode = 0;

			for (int i = 0; i < TreeViewNodes.Nodes.Count; i++)
				replacements += ReplaceAll_private_RecursiveReplace(TreeViewNodes.Nodes[i], ref curNode, list, msc);

			OutputMissionNodeContentsToTree();
			
            RegisterChange("Replaced '" + msc.Input + "' with '" + msc.Replacement + "' " + replacements.ToString() + " time(s).");

			EndUpdate();

            return replacements;
        }

        public void SelectNode(TreeNode node)
        {
            TreeViewNodes.SelectedNode = node;
        }

		public TreeNode GetStatementNode_private_RecursivelySelect(TreeNode node, object tag)
		{
			TreeNode result = null;
			foreach (TreeNode child in node.Nodes)
				if ((result = GetStatementNode_private_RecursivelySelect(child, tag)) != null) return result;

			if (node.Tag == tag)
				return node;
			
			return null;
		}

		public TreeNode GetStatementNode(object tag)
		{
			if (tag == null)
				return null;

			TreeNode result = null;
			
			foreach (TreeNode node in TreeViewStatements.Nodes)
				if ((result = GetStatementNode_private_RecursivelySelect(node, tag)) != null) return result;

			return null;
		}

        public void SelectStatement(object tag, bool forbidMultiSelect = false)
        {
			TreeNode node = GetStatementNode(tag);

			if (node!=null)
			{
                if (forbidMultiSelect)
                    TreeViewStatements.SelectedNodes.Clear();
				TreeViewStatements.SelectedNode = node;
				TreeViewStatements.SelectedNode.EnsureVisible();
			}
			else
            {
                TreeViewStatements.SelectedNode = null;
                _E_statementTV_AfterSelect(null, null);
            }
        }

		public NormalLabel FindFirstPlusMinusAbleLabel()
		{
			NormalLabel l = null;
			foreach(Control c in FlowLayoutPanelMain.Controls)
				if (c is NormalLabel)
				{
					l = (NormalLabel)c;
					ExpressionMemberContainer emc = ((ExpressionMemberContainer)c.Tag);
					if (emc.Member as ExpressionMemberCheck == null && (emc.Member.ValueDescription.Type == ExpressionMemberValueType.VarBool || emc.Member.ValueDescription.Type == ExpressionMemberValueType.VarDouble || emc.Member.ValueDescription.Type == ExpressionMemberValueType.VarInteger))
						break;
					l = null;
				}
			return l;
		}

		public void SetSelection(List<MissionSearchResult> list = null)
		{
			TreeViewNodes.HighlightedTagList.Clear();
			TreeViewStatements.HighlightedTagList.Clear();
			if (list != null)
			{
				foreach (MissionSearchResult item in list)
				{
					if (!TreeViewNodes.HighlightedTagList.Contains(item.NodeTag) && item.NodeTag != null)
						TreeViewNodes.HighlightedTagList.Add(item.NodeTag);
					if (!TreeViewStatements.HighlightedTagList.Contains(item.Statement) && item.NodeTag != null)
						TreeViewStatements.HighlightedTagList.Add(item.NodeTag);
					if (!TreeViewStatements.HighlightedTagList.Contains(item.Statement) && item.Statement != null)
						TreeViewStatements.HighlightedTagList.Add(item.Statement);
				}
			}

			TreeViewNodes.Invalidate();
			TreeViewStatements.Invalidate();
		}

		public void SetSelection(DependencyEvent precursorEvent, DependencyEvent selectedEvent, bool highlightActions)
		{
			TreeViewNodes.HighlightedTagList.Clear();
			TreeViewStatements.HighlightedTagList.Clear();

			foreach (DependencyCondition condition in selectedEvent.Conditions)
			{
				DependencyPrecursor dp = condition.GetPrecursor(precursorEvent);
				if (dp != null)
				{
					if (highlightActions)
					{
						TreeViewStatements.HighlightedTagList.Add(dp.Statement);
						if (TreeViewStatements.SelectedNode == null)
							GetStatementNode(dp.Statement).EnsureVisible();
					}
					else
					{
						TreeViewStatements.HighlightedTagList.Add(condition.Statement);
						if (TreeViewStatements.SelectedNode == null)
							GetStatementNode(condition.Statement).EnsureVisible();
					}
				}
			}

			TreeViewNodes.Invalidate();
			TreeViewStatements.Invalidate();
		}

		public void ConvertToComment()
		{
			bool convertEvents = false;
			bool convertFolders = false;
			int convertedCount = 0;
			
			foreach (TreeNode node in TreeViewNodes.SelectedNodes)
			{
				if (node.Parent != null)
					continue; 
				
				convertEvents = convertEvents || (node.Tag is MissionNode_Event && (((MissionNode_Event)node.Tag).Actions.Count > 0 || ((MissionNode_Event)node.Tag).Conditions.Count > 0));
				convertFolders = convertFolders || (node.Nodes.Count > 0);
			}

			convertEvents = convertEvents && MessageBox.Show("Selection contains non-empty events.\r\nDo you want them to be converted as well?\r\n(This will clear their contents)", "Artemis Mission Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
			convertFolders = convertFolders && MessageBox.Show("Selection contains non-empty folders.\r\nDo you want them to be converted as well?\r\n(This will clear their contents)", "Artemis Mission Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

			foreach (TreeNode node in TreeViewNodes.SelectedNodes.ToList())
			{
				//Node was already removed since a folder with it was converted to comment earlier in this loop
				if (TreeViewNodes.FindNode((TreeNode x) => x == node) == null)
					continue;
				//Cant touch start node
				if (node.Tag is MissionNode_Start)
					continue;
				//Skip nodes of already correct type
				if (node.Tag is MissionNode_Comment)
					continue;
				//Cannot convert node inside a folder to comment
				if (node.Parent != null)
					continue;
				//Skip nodes user decided not to convert
				if (!convertEvents && (node.Tag is MissionNode_Event && (((MissionNode_Event)node.Tag).Actions.Count > 0 || ((MissionNode_Event)node.Tag).Conditions.Count > 0)))
					continue;
				if (!convertFolders && node.Nodes.Count > 0)
					continue;
				
				
				MissionNode_Comment mnc = new MissionNode_Comment();
				mnc.Name = node.Text;
				node.Nodes.Clear();
				node.Tag = mnc;
				node.ImageIndex = mnc.ImageIndex;
				node.SelectedImageIndex = mnc.ImageIndex;
				
				if (node == TreeViewNodes.SelectedNode)
					OutputMissionNodeContentsToTree();

				convertedCount++;
			}

			if (convertedCount > 0)
				RegisterChange("Converted node(s) type to comment");
		}

		public void ConvertToEvent()
		{
			bool convertFolders = false;
			int convertedCount = 0;

			foreach (TreeNode node in TreeViewNodes.SelectedNodes)
			{
				if (node.Tag is MissionNode_Comment && TreeViewNodes.Nodes.IndexOf(_startNode) > TreeViewNodes.Nodes.IndexOf(node))
					continue; 
				
				convertFolders = convertFolders || (node.Nodes.Count > 0);
			}

			convertFolders = convertFolders && MessageBox.Show("Selection contains non-empty folders.\r\nDo you want them to be converted as well?\r\n(This will clear their contents)", "Artemis Mission Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

			foreach (TreeNode node in TreeViewNodes.SelectedNodes.ToList())
			{
				//Node was already removed since a folder with it was converted to comment earlier in this loop
				if (TreeViewNodes.FindNode((TreeNode x) => x == node) == null)
					continue;
				//Cant touch start node
				if (node.Tag is MissionNode_Start)
					continue;
				//Skip nodes of already correct type
				if (node.Tag is MissionNode_Event)
					continue;
				//Cannot convert comment over start node to something else
				if (node.Tag is MissionNode_Comment && TreeViewNodes.Nodes.IndexOf(_startNode) > TreeViewNodes.Nodes.IndexOf(node))
					continue;
				//Skip nodes user decided not to convert
				if (!convertFolders && node.Nodes.Count > 0)
					continue;

				MissionNode mn = (MissionNode)node.Tag;
				MissionNode_Event mne = new MissionNode_Event();
				mne.Name = node.Text;
				if (mn.ID != null)
				{
					mne.ID = mn.ID;
					mne.ParentID = mn.ParentID;
				}
				else
				{
					mne.ID = Guid.NewGuid();
					mne.ParentID = null;
				}
				node.Nodes.Clear();
				node.Tag = mne;
				node.ImageIndex = mne.ImageIndex;
				node.SelectedImageIndex = mne.ImageIndex;

				if (node == TreeViewNodes.SelectedNode)
					OutputMissionNodeContentsToTree();

				convertedCount++;
			}

			if (convertedCount > 0)
				RegisterChange("Converted node(s) type to event");
		}

		public void ConvertToFolder()
		{
			bool convertEvents = false;
			int convertedCount = 0;

			foreach (TreeNode node in TreeViewNodes.SelectedNodes)
			{
				if (node.Tag is MissionNode_Comment && TreeViewNodes.Nodes.IndexOf(_startNode) > TreeViewNodes.Nodes.IndexOf(node))
					continue; 

				convertEvents = convertEvents || (node.Tag is MissionNode_Event && (((MissionNode_Event)node.Tag).Actions.Count > 0 || ((MissionNode_Event)node.Tag).Conditions.Count > 0));
			}

			convertEvents = convertEvents && MessageBox.Show("Selection contains non-empty events.\r\nDo you want them to be converted as well?\r\n(This will clear their contents)", "Artemis Mission Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

			foreach (TreeNode node in TreeViewNodes.SelectedNodes.ToList())
			{
				//Cant touch start node
				if (node.Tag is MissionNode_Start)
					continue;
				//Skip nodes of already correct type
				if (node.Tag is MissionNode_Folder)
					continue;
				//Cannot convert comment over start node to something else
				if (node.Tag is MissionNode_Comment && TreeViewNodes.Nodes.IndexOf(_startNode) > TreeViewNodes.Nodes.IndexOf(node))
					continue;
				//Skip nodes user decided not to convert
				if (!convertEvents && (node.Tag is MissionNode_Event && (((MissionNode_Event)node.Tag).Actions.Count > 0 || ((MissionNode_Event)node.Tag).Conditions.Count > 0)))
					continue;

				MissionNode mn = (MissionNode)node.Tag;
				MissionNode_Folder mnf = new MissionNode_Folder();
				mnf.Name = node.Text;
				if (mn.ID != null)
				{
					mnf.ID = mn.ID;
					mnf.ParentID = mn.ParentID;
				}
				else
				{
					mnf.ID = Guid.NewGuid();
					mnf.ParentID = null;
				} 
				node.Tag = mnf;
				node.ImageIndex = mnf.ImageIndex;
				node.SelectedImageIndex = mnf.ImageIndex;

				if (node == TreeViewNodes.SelectedNode)
					OutputMissionNodeContentsToTree();

				convertedCount++;
			}

			if (convertedCount > 0)
				RegisterChange("Converted node(s) type to folder");
		}

        #region Space map interaction

        /// <summary>
        /// Wether or not user can "Add via space map" or "Edit on space map".
        /// Returns -1 if not, or amount of create statements in current node if yes
        /// </summary>
        /// <returns>-1 if not, or amount of create statements in current node if yes</returns>
        public int CanInvokeSpaceMapCreate(TreeNode node = null)
        {
			if (node == null)
				node = TreeViewNodes.SelectedNode;
            if (node == null)
                return -1;
			if (!(node.Tag is MissionNode_Event) && !(node.Tag is MissionNode_Start))
                return -1;
            int i=0;
			foreach (MissionStatement statement in ((MissionNode)node.Tag).Actions)
                i += (statement.IsSomeKindOfCreateStatement()) ? 1 : 0;
            return i;
        }

        public void AddCreateStatementsViaSpaceMap(bool underCursor = false, TreeNode nodeUnderCursor = null)
        {
            if (CanInvokeSpaceMapCreate() == -1)
                return;

            int i = 0;
            int topmost = -1;

            MissionNode curNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;

			XmlDocument xDoc;
			XmlNode root;
			string bgXml = "<bgInput></bgInput>";
			
			if (Settings.Current.ShowStartStatementsInBackground && _bgNode != TreeViewNodes.SelectedNode)
			{
				MissionNode bgNode = (MissionNode)_bgNode.Tag;

				xDoc = new XmlDocument();
				root = xDoc.CreateElement("bgInput");
				xDoc.AppendChild(root);

				for (i = 0; i < bgNode.Actions.Count; i++)
				{
					MissionStatement statement = bgNode.Actions[i];
					if (statement.IsCreateNamedStatement())
						root.AppendChild(statement.ToXml(xDoc, true));
					if (statement.IsCreateNamelessStatement())
						root.AppendChild(statement.ToXml(xDoc, true));
				}
				bgXml = xDoc.OuterXml;
			}

            TreeNode curTreeNode = underCursor ? nodeUnderCursor : TreeViewStatements.SelectedNode;

			if (curTreeNode != null)
			{
				if (curTreeNode.Tag is string)
				{
					//What to do if actions node is selected
					if (curTreeNode.Tag.ToString() == "actions")
						topmost = -1;

					//What to do if conditions node is selected
					if (curTreeNode.Tag.ToString() == "conditions")
						topmost = -1;
				}
				if (curTreeNode.Tag is MissionStatement)
				{
					for (i = 0; i < curNode.Actions.Count && topmost == -1; i++)
						if (curNode.Actions[i] == (MissionStatement)curTreeNode.Tag)
							topmost = i + 1;
				}
			}
            
            SpaceMap result = FormSpaceMap.AddViaSpaceMap(bgXml);

            ParseSpaceMapCreateResults(new List<MissionStatement>(), topmost, result);
        }

        public void EditCreateStatementsOnSpaceMap(bool underCursor = false, TreeNode nodeUnderCursor = null)
        {
			if (CanInvokeSpaceMapCreate() == 0)
			{
				AddCreateStatementsViaSpaceMap(underCursor, nodeUnderCursor);
				return;
			}
			if (CanInvokeSpaceMapCreate() ==-1)
                return;

            int i = 0;
            int topmost = -1;

            MissionNode curNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;

            List<int> namedObjects = new List<int>();
            List<int> namelessObjects = new List<int>();
            List<MissionStatement> toDelete = new List<MissionStatement>();

			XmlDocument xDoc;
			XmlNode root;
            string editXml = "<createInput></createInput>";
            string bgXml = "<bgInput></bgInput>";
			
            xDoc = new XmlDocument();
            root = xDoc.CreateElement("createInput");
            xDoc.AppendChild(root);

            for (i = 0; i < curNode.Actions.Count;i++)
            {
                MissionStatement statement = curNode.Actions[i];
                if (statement.IsCreateNamedStatement())
                {
                    namedObjects.Add(i);
                    toDelete.Add(statement);
                    root.AppendChild(statement.ToXml(xDoc, true));
                }
                if (statement.IsCreateNamelessStatement())
                {
                    namelessObjects.Add(i);
                    toDelete.Add(statement);
                    root.AppendChild(statement.ToXml(xDoc, true));
                }
            }
			editXml = xDoc.OuterXml;

			if (Settings.Current.ShowStartStatementsInBackground && _bgNode != TreeViewNodes.SelectedNode)
			{
				MissionNode bgNode = (MissionNode)_bgNode.Tag;

				xDoc = new XmlDocument();
				root = xDoc.CreateElement("bgInput");
				xDoc.AppendChild(root);

				for (i = 0; i < bgNode.Actions.Count; i++)
				{
					MissionStatement statement = bgNode.Actions[i];
					if (statement.IsCreateNamedStatement())
						root.AppendChild(statement.ToXml(xDoc, true));
					if (statement.IsCreateNamelessStatement())
						root.AppendChild(statement.ToXml(xDoc, true));
				}
				 bgXml = xDoc.OuterXml;
			}

            topmost = (namedObjects.Count > 0 && (namedObjects[0] < topmost || topmost == -1)) ? namedObjects[0] : topmost;
            topmost = (namelessObjects.Count > 0 && (namelessObjects[0] < topmost || topmost == -1)) ? namelessObjects[0] : topmost;

            SpaceMap result = FormSpaceMap.EditOnSpaceMap(namedObjects, namelessObjects, editXml, bgXml);

            ParseSpaceMapCreateResults(toDelete, topmost, result);
        }

        private void ParseSpaceMapCreateResults(List<MissionStatement> toDelete, int topmost, SpaceMap result)
        {
            if (result == null)
                return;

            int i;
            MissionNode curNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;
            List<string> missingProperties = new List<string>();
            XmlDocument xDoc = new XmlDocument();

            //Target point for all objects that are not after an Imported object
            int target_point = topmost == -1 ? curNode.Actions.Count : topmost;
            
            #region Import Named objects from Space map

            i = result.namedObjects.Count;

            // Original intention here seems to have been:
            //   Go through list of originally imported statements and try to match them to the new statements
            //   For each originally imported statement, put one new statement in its place
            //   When out of statements, add all remaining statements to where it's set in the settings
            while (result.namedIdList.Count > 0)
            {
                ParseSpaceMapCreateResults_InsertNamed(ref i, curNode, result.namedIdList[result.namedIdList.Count - 1], result, xDoc, missingProperties);
                if (result.namedObjects[i].Imported)
                    result.namedIdList.RemoveAt(result.namedIdList.Count - 1);
            }
            while (i > 0)
            {
                ParseSpaceMapCreateResults_InsertNamed(ref i, curNode, target_point, result, xDoc, missingProperties);
            }

            #endregion

            #region Import nameless objects from Space map

            i = result.namelessObjects.Count;

            while (result.namelessIdList.Count > 0)
            {
                ParseSpaceMapCreateResults_InsertNameless(ref i, curNode, result.namelessIdList[result.namelessIdList.Count - 1], result, xDoc, missingProperties);
                if (result.namelessObjects[i].Imported)
                    result.namelessIdList.RemoveAt(result.namelessIdList.Count - 1);
            }
            while (i > 0)
            {
                ParseSpaceMapCreateResults_InsertNameless(ref i, curNode, target_point, result, xDoc, missingProperties);
            }
            
            #endregion

            foreach (MissionStatement statement in toDelete)
                curNode.Actions.Remove(statement);

            OutputMissionNodeContentsToTree();
            RegisterChange("Changes to create statements via space map");
        }

        private void ParseSpaceMapCreateResults_InsertNamed(ref int i, MissionNode curNode, int position, SpaceMap result, XmlDocument xDoc, List<string> missingProperties)
        {
            i--;
            missingProperties.Clear(); 
            curNode.Actions.Insert(position,
                MissionStatement.NewFromXML(result.namedObjects[i].ToXml(xDoc, missingProperties), curNode));

            if (missingProperties.Count > 0 && Settings.Current.AddFailureComments)
            {
                curNode.Actions.Insert(position,
                MissionStatement.NewFromXML(xDoc.CreateComment("will fail because it lacks " + missingProperties.Aggregate((x, y) => x + ", " + y) + " "), curNode));
            }
        }

        private void ParseSpaceMapCreateResults_InsertNameless(ref int i, MissionNode curNode, int position, SpaceMap result, XmlDocument xDoc, List<string> missingProperties)
        {
            i--;
            missingProperties.Clear();
            curNode.Actions.Insert(position,
                MissionStatement.NewFromXML(result.namelessObjects[i].ToXml(xDoc, missingProperties), curNode));
            if (missingProperties.Count > 0 && Settings.Current.AddFailureComments)
            {
                curNode.Actions.Insert(position,
                MissionStatement.NewFromXML(xDoc.CreateComment("will fail because it lacks " + missingProperties.Aggregate((x, y) => x + ", " + y) + " "), curNode));
            }
        }

        /// <summary> Wether or not user can "Edit statement on space map" </summary>
        public bool CanInvokeSpaceMapStatement()
        {
            if (TreeViewNodes.SelectedNode == null || TreeViewStatements.SelectedNode == null)
                return false;
            if (!(TreeViewNodes.SelectedNode.Tag is MissionNode_Event) && !(TreeViewNodes.SelectedNode.Tag is MissionNode_Start))
                return false;
            if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
                return false;

            return (((MissionStatement)TreeViewStatements.SelectedNode.Tag).IsSpaceMapEditableStatement());
        }

        public void EditStatementOnSpaceMap()
        {
            if (!CanInvokeSpaceMapStatement())
                return;

            int i = 0;

            MissionNode curNode = (MissionNode)TreeViewNodes.SelectedNode.Tag;
            
            XmlDocument xDoc;
            XmlNode root;
            string statementXml = "<statementInput></statementInput>";
            string editXml = "<createInput></createInput>";
            string bgXml = "<bgInput></bgInput>";

            xDoc = new XmlDocument();
            root = xDoc.CreateElement("createInput");
            xDoc.AppendChild(root);

            for (i = 0; i < curNode.Actions.Count; i++)
            {
                MissionStatement statement = curNode.Actions[i];
                if (statement.IsCreateNamedStatement())
                    root.AppendChild(statement.ToXml(xDoc, true));
                if (statement.IsCreateNamelessStatement())
                    root.AppendChild(statement.ToXml(xDoc, true));
            }
            editXml = xDoc.OuterXml;

            xDoc = new XmlDocument();
            root = xDoc.CreateElement("statementInput");
            xDoc.AppendChild(root);

            MissionStatement curStatement = (MissionStatement)TreeViewStatements.SelectedNode.Tag;
            root.AppendChild(curStatement.ToXml(xDoc));
            statementXml = xDoc.OuterXml;
			
            if (Settings.Current.ShowStartStatementsInBackground)
            {
				MissionNode bgNode = (MissionNode)_bgNode.Tag;

                xDoc = new XmlDocument();
                root = xDoc.CreateElement("bgInput");
                xDoc.AppendChild(root);

                for (i = 0; i < bgNode.Actions.Count; i++)
                {
                    MissionStatement statement = bgNode.Actions[i];
                    if (statement.IsCreateNamedStatement())
                        root.AppendChild(statement.ToXml(xDoc, true));
                    if (statement.IsCreateNamelessStatement())
                        root.AppendChild(statement.ToXml(xDoc, true));
                }
                bgXml = xDoc.OuterXml;
            }

            SpaceMap result = DialogSpaceMap.EditStatementOnSpaceMap(statementXml, editXml, bgXml);

            ParseSpaceMapStatementResults(curStatement, result);
        }

        private void ParseSpaceMapStatementResults(MissionStatement curStatement, SpaceMap result)
        {
            if (result == null)
                return;

            curStatement.FromXml(result.SelectionSpecial.ToXml(new XmlDocument()));

            curStatement.Update();

            OutputMissionNodeContentsToTree();
            RegisterChange("Changes to statement via space map");
        }

        #endregion

        #region EVENTS

        private void _E_l_Activated(NormalLabel label, bool byMouse = false, EditorActivationMode mode = EditorActivationMode.Normal)
        {
            Point where;
            if (byMouse)
                where = Cursor.Position;
            else
            {
                where = new Point();
                where.X += label.Width / 2;
                where.Y += label.Height / 2;
                where = label.PointToScreen(where);
            }
             ((ExpressionMemberContainer)label.Tag).OnClick(label, where, mode);
        }
       
        private void _E_l_MouseClick(object sender, MouseEventArgs e)
        {
			if (e.Button == MouseButtons.Left)
				_E_l_Activated((NormalLabel)sender, true);
			if (e.Button == MouseButtons.Right)
			{
				ContextMenuStripForLabels.Tag = sender;
				ContextMenuStripForLabels.Show(((NormalLabel)sender).PointToScreen(e.Location));
			}
        }

		private void _E_l_CMS_Click(object sender, EventArgs e)
		{
			string tag = (string)((ToolStripItem)sender).Tag;
			switch (tag)
			{
				case "edit":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag);
					break;
				case "edit_dialog":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.ForceGUI);
					break;
				case "edit_next":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.NextMenuItem);
					break;
				case "edit_previous":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.PreviousMenuItem);
					break;
				case "edit_-1000":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Minus1000);
					break;
				case "edit_-100":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Minus100);
					break;
				case "edit_-10":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Minus10);
					break;
				case "edit_-1":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Minus);
					break;
				case "edit_-01":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Minus01);
					break;
				case "edit_+01":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Plus01);
					break;
				case "edit_+1":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Plus);
					break;
				case "edit_+10":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Plus10);
					break;
				case "edit_+100":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Plus100);
					break;
				case "edit_+1000":
					_E_l_Activated((NormalLabel)ContextMenuStripForLabels.Tag, false, EditorActivationMode.Plus1000);
					break;
				case "edit_space":
					EditStatementOnSpaceMap();
					break;
			}
		}

		private void _E_l_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Back to Statement tree
			if (e.KeyData == (Keys.Enter | Keys.Shift))
			{
				e.IsInputKey = true;
				TreeViewStatements.Focus();
			}
            //Label activation
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.F2)
            {
                e.IsInputKey = true;
                _E_l_Activated((NormalLabel)sender);
            }
            if (e.KeyData == (Keys.Enter | Keys.Control))
            {
                e.IsInputKey = true;
                _E_l_Activated((NormalLabel)sender, false, EditorActivationMode.ForceGUI);
            }
			if (e.KeyData == (Keys.F2 | Keys.Control))
			{
				e.IsInputKey = true;
				EditStatementOnSpaceMap();
			}
            if (e.KeyData == Keys.Space)
            {
                e.IsInputKey = true;
                _E_l_Activated((NormalLabel)sender, false, EditorActivationMode.NextMenuItem);
            }
			if (e.KeyData == (Keys.Space | Keys.Shift))
			{
				e.IsInputKey = true;
				_E_l_Activated((NormalLabel)sender, false, EditorActivationMode.PreviousMenuItem);
			}
			if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
			{
				e.IsInputKey = true;
				
				EditorActivationMode mode = EditorActivationMode.Minus;
				mode = Control.ModifierKeys == (Keys.Alt)					? EditorActivationMode.Minus01		: mode;
				mode = Control.ModifierKeys == (Keys.Shift)					? EditorActivationMode.Minus10		: mode;
				mode = Control.ModifierKeys == (Keys.Control)				? EditorActivationMode.Minus100		: mode;
				mode = Control.ModifierKeys == (Keys.Shift | Keys.Control)	? EditorActivationMode.Minus1000	: mode;

				_E_l_Activated((NormalLabel)sender, false, mode);
			}
			if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add)
			{
				e.IsInputKey = true;
				
				EditorActivationMode mode = EditorActivationMode.Plus;
				mode = Control.ModifierKeys == (Keys.Alt)					? EditorActivationMode.Plus01		: mode;
				mode = Control.ModifierKeys == (Keys.Shift)					? EditorActivationMode.Plus10		: mode;
				mode = Control.ModifierKeys == (Keys.Control)				? EditorActivationMode.Plus100		: mode;
				mode = Control.ModifierKeys == (Keys.Shift | Keys.Control)	? EditorActivationMode.Plus1000		: mode;

				_E_l_Activated((NormalLabel)sender, false, mode);
			}

			if (!e.IsInputKey)
			{
				_E_statementTV_KeyDown(sender, new KeyEventArgs(e.KeyData));
			}
        }
                
        private void _E_statementTV_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			//TODO: Maybe prevent selection of other member if there are errors?
		}

		public void _E_statementTV_AfterSelect(object sender, TreeViewEventArgs e)
		{
            if (!___STATIC_E_statementTV_SuppressSelectionEvents)
                UpdateExpression();
		}

        private void _E_statementTV_NodeMoved(TreeNode node, bool suspendUpdate)
		{
			ImportMissionNodeContentsFromStatementTree();

            if (!suspendUpdate)
			    RegisterChange("Statement moved");
		}
		
		private void _E_nodeTV_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
            //TODO: Maybe prevent selection of other member if there are errors?
		}

		private void _E_nodeTV_AfterSelect(object sender, TreeViewEventArgs e)
		{
            if (!___STATIC_E_nodeTV_SupressSelectionEvents)
                OutputMissionNodeContentsToTree();
		}

		private void _E_nodeTV_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label == null)
				return;
			MissionNode mNode = (MissionNode)e.Node.Tag;
			string prevName = mNode.Name;
			try
			{
				mNode.Name = e.Label;
				// Miscrosoft.Preformance is so helpful! Yeah sure, if I could just call OuterXml without assigning it, I'd never do this!
                string justSoThatOuterXmlGetLogicIsCalled = mNode.ToXml(new XmlDocument()).OuterXml;
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.Message,"Bad node name!");
				mNode.Name = prevName;
				e.CancelEdit = true;
				return;
			}
			UpdateNodeTag();
			if (mNode is MissionNode_Folder || mNode is MissionNode_Comment)
				OutputMissionNodeContentsToTree();

			RegisterChange("Node label edited");
		}

        private void _E_nodeTV_NodeMoved(TreeNode node, bool suspendUpdate)
		{
			if (node == null)
				return;

			//Assign parentship
			if (node.Parent != null)
				((MissionNode)node.Tag).ParentID = ((MissionNode)node.Parent.Tag).ID;
			else
				((MissionNode)node.Tag).ParentID = null;

			//Adjust start node
			if (_startNode.Tag == node.Tag)
				_startNode = node;

            if (!suspendUpdate)
			    RegisterChange("Node moved");
		}

		private void _E_nodeTV_AfterExpand(object sender, TreeViewEventArgs e)
		{
			MissionNode mNode = ((MissionNode)e.Node.Tag);
			if (!mNode.ExtraAttributes.Contains("expanded_arme"))
			{
				mNode.ExtraAttributes.Add("expanded_arme");
				if (!___STATIC_E_nodeTV_SupressExpandCollapseEvents && !TreeViewNodes.DraggingInProgress)
					RegisterChange("Expanded folder");
			}
		}

		private void _E_nodeTV_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			MissionNode mNode = ((MissionNode)e.Node.Tag);
			if (mNode.ExtraAttributes.Contains("expanded_arme"))
			{
				mNode.ExtraAttributes.Remove("expanded_arme");
				if (!___STATIC_E_nodeTV_SupressExpandCollapseEvents && !TreeViewNodes.DraggingInProgress)
					RegisterChange("Collapsed folder");
			}
		}

		private void _E_form_KeyDown(object sender, KeyEventArgs e)
		{
			//File commands
            if (e.KeyData == (Keys.N | Keys.Control))
            {
                e.SuppressKeyPress = true;
                FormMain.BeginInvoke(new Action(() =>
				New()
				));
            } 
            if (e.KeyData == (Keys.O | Keys.Control))
			{
				e.SuppressKeyPress = true;
				FormMain.BeginInvoke(new Action(() =>
				Open()
				));
			}
            if (e.KeyData == (Keys.S | Keys.Control))
			{
				e.SuppressKeyPress = true;
				FormMain.BeginInvoke(new Action(() =>
				Save()
				));
			}
            if (e.KeyData == (Keys.S | Keys.Control | Keys.Alt))
            {
                e.SuppressKeyPress = true;
				FormMain.BeginInvoke(new Action(() =>
				SaveAs()
				));
            }

			//Edit commands
			if (e.KeyData == (Keys.Z | Keys.Control))
			{
				e.SuppressKeyPress = true;
				Undo();
            }
            if (e.KeyData == (Keys.Y | Keys.Control))
            {
                e.SuppressKeyPress = true;
                Redo();
            }
			if (e.KeyData == (Keys.F | Keys.Control) || e.KeyData == (Keys.F | Keys.Control | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				ShowFindForm();
			}
			if (e.KeyData == (Keys.H | Keys.Control) || e.KeyData == (Keys.H | Keys.Control | Keys.Shift))
            {
                e.SuppressKeyPress = true;
                ShowReplaceForm();
            }
            if (e.KeyData == Keys.F3 )
            {
                e.SuppressKeyPress = true;
                Program.FormFindReplaceInstance.FindNext();
            }
            if (e.KeyData == (Keys.F3 | Keys.Shift))
            {
                e.SuppressKeyPress = true;
                Program.FormFindReplaceInstance.FindPrevious();
            }
            if (e.KeyData == Keys.F4)
            {
                e.SuppressKeyPress = true;
				ShowEventDependencyForm(true);
            }
            if (e.KeyData == (Keys.F4 | Keys.Shift))
            {
                e.SuppressKeyPress = true;
				ShowEventDependencyForm();
            }
			if (e.KeyData == (Keys.P | Keys.Control))
			{
				e.SuppressKeyPress = true;
				ShowMissionPropertiesForm();
			}
            //Label selection keys
			if (e.KeyCode == Keys.D1)
			{
				SelectExpressionLabel(1);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D2)
			{
				SelectExpressionLabel(2);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D3)
			{
				SelectExpressionLabel(3);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D4)
			{
				SelectExpressionLabel(4);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D5)
			{
				SelectExpressionLabel(5);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D6)
			{
				SelectExpressionLabel(6);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D7)
			{
				SelectExpressionLabel(7);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D8)
			{
				SelectExpressionLabel(8);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D9)
			{
				SelectExpressionLabel(9);
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.D0)
			{
				SelectExpressionLabel(10);
				e.SuppressKeyPress = true;
			}

			//Expand/collapse
			if (e.KeyData == (Keys.E | Keys.Control))
			{
				e.SuppressKeyPress = true;
				NodeExpandAll();
			}
			if (e.KeyData == (Keys.R | Keys.Control))
			{
				e.SuppressKeyPress = true;
				NodeCollapseAll();
			}
		}

		private void _E_nodeTV_KeyDown(object sender, KeyEventArgs e)
		{
			//Forward to Statement tree
			if (e.KeyCode == Keys.Enter && Control.ModifierKeys == Keys.None)
			{
				TreeViewStatements.Focus();
				e.SuppressKeyPress = true;
			}

			//Edit menu keys
			if (e.KeyData == (Keys.C | Keys.Control) || e.KeyData == (Keys.Insert | Keys.Control))
			{
				if (NodeCopy())
					e.SuppressKeyPress = true;
			}
			if (e.KeyData == (Keys.X | Keys.Control) || e.KeyData == (Keys.Delete | Keys.Shift))
			{
				if (NodeCopy())
				{
					e.SuppressKeyPress = true;
					NodeDelete();
				}
			}
			if (e.KeyData == (Keys.V | Keys.Control) || e.KeyData == (Keys.Insert | Keys.Shift))
			{
				e.SuppressKeyPress = true;

				if (!NodePaste() && StatementPaste() && Settings.Current.FocusOnStatementPaste)
				{
					TreeViewStatements.Focus();
				}
			}
			//CMS keys
			if (e.KeyCode == Keys.Up && Control.ModifierKeys == Keys.Control)
			{
				NodeMoveUp();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Down && Control.ModifierKeys == Keys.Control)
			{
				NodeMoveDown();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Right && Control.ModifierKeys == Keys.Control)
			{
				NodeMoveIn();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Left && Control.ModifierKeys == Keys.Control)
			{
				NodeMoveOut();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Delete && Control.ModifierKeys == Keys.None)
			{
				NodeDelete();
				e.SuppressKeyPress = true;
			}
			if (e.KeyData == (Keys.Control | Keys.D))
			{
				e.SuppressKeyPress = true;
				NodeEnableDisable(false);
			}
			if (e.KeyData == (Keys.Control |Keys.Shift | Keys.D))
			{
				e.SuppressKeyPress = true;
				NodeEnableDisable(true);
			}
			if (e.KeyCode == Keys.F2 && Control.ModifierKeys == Keys.None)
			{
				NodeRename();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == Keys.None)
			{
				NodeAddEvent();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == Keys.Alt)
			{
				NodeAddFolder();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == (Keys.Control | Keys.Shift))
			{
				NodeAddCommentary();
				e.SuppressKeyPress = true;
			}

			//Catch events that will DING
            if (e.KeyData == (Keys.Control | Keys.F) || e.KeyData == (Keys.Control | Keys.H))
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void _E_statementTV_KeyDown(object sender, KeyEventArgs e)
		{
			//Forward to Flow layout panel
			if (e.KeyCode == Keys.Enter && Control.ModifierKeys == Keys.None)
			{
				e.SuppressKeyPress = true;
				SelectExpressionLabel(1);
			}

			//Back to Node tree
			if (e.KeyCode == Keys.Enter && Control.ModifierKeys == Keys.Shift)
			{
				e.SuppressKeyPress = true;
				TreeViewNodes.Focus();
			}

            //Edit menu keys
			if (e.KeyData == (Keys.C | Keys.Control) || e.KeyData == (Keys.Insert | Keys.Control))
			{
				if (StatementCopy())
					e.SuppressKeyPress = true;
			}
			if (e.KeyData == (Keys.X | Keys.Control) || e.KeyData == (Keys.Delete | Keys.Shift))
			{
				if (StatementCopy())
				{
					e.SuppressKeyPress = true;
					StatementDelete();
				}
			}
			if (e.KeyData == (Keys.V | Keys.Control) || e.KeyData == (Keys.Insert | Keys.Shift))
			{
				e.SuppressKeyPress = true;
				StatementPaste();
				TreeViewStatements.Focus();
			}
            //EDIT keys
            if (e.KeyData == Keys.F2)
            {
				e.SuppressKeyPress = true;
				EditCreateStatementsOnSpaceMap();
            }
            if (e.KeyData == (Keys.F2 | Keys.Shift))
            {
				e.SuppressKeyPress = true;
				AddCreateStatementsViaSpaceMap();
            }
            if (e.KeyData == (Keys.F2 | Keys.Control))
            {
				e.SuppressKeyPress = true;
				EditStatementOnSpaceMap();
            }
			if (e.KeyData == (Keys.Control | Keys.D))
			{
				e.SuppressKeyPress = true;
				StatementEnableDisable(false);
			}
			if (e.KeyData == (Keys.Control | Keys.Shift | Keys.D))
			{
				e.SuppressKeyPress = true;
				StatementEnableDisable(true);
			}
			
			//CMS keys
			if (e.KeyCode == Keys.Up && Control.ModifierKeys == Keys.Control)
			{
				StatementMoveUp();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Down && Control.ModifierKeys == Keys.Control)
			{
				StatementMoveDown();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Delete && Control.ModifierKeys == Keys.None)
			{
				StatementDelete();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == Keys.None)
			{
				StatementAddAction();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == Keys.Alt)
			{
				StatementAddCondition();
				e.SuppressKeyPress = true;
			}
			if (e.KeyCode == Keys.Insert && Control.ModifierKeys == (Keys.Control | Keys.Shift))
			{
				StatementAddCommentary();
				e.SuppressKeyPress = true;
			}

            //Catch events that will DING
			if (e.KeyData == (Keys.Control | Keys.F) || e.KeyData == (Keys.Shift | Keys.Control | Keys.F) || e.KeyData == (Keys.Control | Keys.H) || e.KeyData == (Keys.Shift | Keys.Control | Keys.H))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

			//Catch +/-
			if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract || e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add)
			{
				NormalLabel l = FindFirstPlusMinusAbleLabel();
				if (l != null)
				{
					if (Settings.Current.SelectLabelWhenUsingPlusMinus)
						l.Focus();
					e.SuppressKeyPress = true;
					_E_l_PreviewKeyDown(l, new PreviewKeyDownEventArgs(e.KeyData));
				}
			}
		}

        private void _E_flowLP_Resize(object sender, EventArgs e)
		{
			foreach (Control c in FlowLayoutPanelMain.Controls)
			{
				if (c is NormalLabel)
					UpdateLabel((NormalLabel)c);
			}

			FlowLayoutPanelMain.PerformLayout();

			FlowLayoutPanelMain.Invalidate(true);
		}

        void _E_statementTV_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (CanInvokeSpaceMapStatement())
            {
                EditStatementOnSpaceMap();
                return;
            }

            if (TreeViewNodes.SelectedNode == null || TreeViewStatements.SelectedNode == null)
                return;
            if (!(TreeViewNodes.SelectedNode.Tag is MissionNode_Event) && !(TreeViewNodes.SelectedNode.Tag is MissionNode_Start))
                return;
            if (!(TreeViewStatements.SelectedNode.Tag is MissionStatement))
                return;
            if (((MissionStatement)TreeViewStatements.SelectedNode.Tag).IsSomeKindOfCreateStatement())
            {
                EditCreateStatementsOnSpaceMap();
                return;
            }

        }

        #endregion

        #region Debug XML output

        private void GetDebugXmlOutput_private_GetDiff(string caption, MissionStatement statement, ref string output, ref bool first)
		{
			if (statement.Kind == MissionStatementKind.Commentary)
				return;
			if (string.IsNullOrWhiteSpace(statement.SourceXML))
				return;

			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(statement.SourceXML);
			XmlNode xOld = xDoc.ChildNodes[0];
			XmlNode xNew = statement.ToXml(xDoc);
			string diffs = "";

			if (xOld.Name!=xNew.Name)
				diffs+="\r\n"+"Name: <"+xOld.Name+"> => <"+xNew.Name+">";
			foreach (XmlAttribute atto in xOld.Attributes)
			{
				XmlAttribute attn = xNew.Attributes[atto.Name];
				if (attn == null)
					diffs += "\r\n" + "Attribute: <" + atto.Name + "> removed!";
				else if (attn.Value!=atto.Value)
                    diffs += "\r\n" + "Attribute: <" + atto.Name + "> value changed <" + atto.Value + "> => <" + attn.Value + ">";
			}
			foreach (XmlAttribute attn in xNew.Attributes)
			{
				XmlAttribute atto = xOld.Attributes[attn.Name];
				if (atto == null)
                    diffs += "\r\n" + "Attribute: added <" + attn.Name + ">";
			}
			if (!String.IsNullOrEmpty(diffs))
			{
				if (first)
				{
					first = false;
					output += caption;
				}
				output += "\r\n" + "\r\n" + xOld.OuterXml + " => " + xNew.OuterXml;
				output += diffs;
			}
		}

		private void GetDebugXmlOutput_private_RecursivelyOutput(TreeNode node, ref string output)
		{
			foreach (TreeNode child in node.Nodes)
				GetDebugXmlOutput_private_RecursivelyOutput(child, ref output);

			if (!(node.Tag is MissionNode_Start) && !(node.Tag is MissionNode_Event))
				return;

			MissionNode curMNode = (MissionNode)node.Tag;

            bool first = true;
            string caption = "\r\n" + "\r\n" + "\r\n" + curMNode.Name + ":";
            foreach (MissionStatement statement in curMNode.Conditions)
                GetDebugXmlOutput_private_GetDiff(caption, statement, ref output, ref first);

            foreach (MissionStatement statement in curMNode.Actions)
                GetDebugXmlOutput_private_GetDiff(caption, statement, ref output, ref first);
		}

		public string GetDebugXmlOutput()
		{
			string output = "";
			foreach (TreeNode node in TreeViewNodes.Nodes)
				GetDebugXmlOutput_private_RecursivelyOutput(node, ref output);
			return output;
        }

        public int GetNodeCount_private_RecursivelyCount(TreeNode node)
        {
            int result = 0;
            foreach (TreeNode cnode in node.Nodes)
                result += GetNodeCount_private_RecursivelyCount(cnode);
            return result + 1;
        }
        
        public int GetNodeCount()
        {
            int result = 0;
            foreach (TreeNode node in TreeViewNodes.Nodes)
                result += GetNodeCount_private_RecursivelyCount(node);
            return result;
        }

		public void GetNodes_private_RecursivelyCount(TreeNode node, List<TreeNode> list)
		{
			list.Add(node);
			
			foreach (TreeNode cnode in node.Nodes)
				GetNodes_private_RecursivelyCount(cnode, list);
		}

		public List<TreeNode> GetNodes()
		{
			List<TreeNode> list = new List<TreeNode>();

			foreach (TreeNode node in TreeViewNodes.Nodes)
				GetNodes_private_RecursivelyCount(node, list);

			return list;
		}

		public TreeNode GetSelectedNode()
		{
			return TreeViewNodes.SelectedNode;
		}

		public int GetSelectedStatementPos()
        {
            if (TreeViewStatements.SelectedNode == null)
                return 0;
            if (TreeViewStatements.SelectedNode.Tag is string && (string)TreeViewStatements.SelectedNode.Tag == "conditions")
                return 0;
            if (TreeViewStatements.SelectedNode.Tag is string && (string)TreeViewStatements.SelectedNode.Tag == "actions")
                return ((MissionNode)TreeViewNodes.SelectedNode.Tag).Conditions.Count;
            return -1;
        }

        #endregion
    }
}


