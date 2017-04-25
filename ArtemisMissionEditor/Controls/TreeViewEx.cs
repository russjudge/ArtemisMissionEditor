using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace ArtemisMissionEditor
{
    public enum NodeRelationship
    {
        Null = 0,
        ChildGoesAbove = 1,
        ChildGoesInside = 2,
        ChildGoesUnder = 3
    }

    /// <summary>
    /// Extended TreeView class with specific features (drag&drop support etc.)
    /// </summary>
    public sealed class TreeViewEx : TreeView
    {
        public bool DraggingInProgress { get { return _dragNode != null; } }
        private TreeNode _dragNode = null;
		private TreeNode _dropNode = null;
        private NodeRelationship _dropNodeLocation = NodeRelationship.Null; //1 = over, 2 = mid, 3 = under

        /// <summary> Color of the selection rectangle around the node, in owner-draw mode, when this control is focused </summary>
        public Color SelectedNodeColorFocused { get { return _selectedNodeFocusedColor; } set { _selectedNodeFocusedTreeBrush = new SolidBrush(value); _selectedNodeFocusedColor = value; } }
        private Color _selectedNodeFocusedColor;
        private Brush _selectedNodeFocusedTreeBrush;
        
        /// <summary> Color of the selection rectangle around the node, in owner-draw mode, when this control hasnt got focus </summary>
		public Color SelectedNodeColorUnfocused { get { return _selectedNodeUnfocusedColor; } set { _selectedNodeUnfocusedTreeBrush = new SolidBrush(value); _selectedNodeUnfocusedColor = value; } }
        private Color _selectedNodeUnfocusedColor;
        private Brush _selectedNodeUnfocusedTreeBrush;
        
		/// <summary> Color of the selection rectangle around the node, in owner-draw mode, when the node is highlighted </summary>
		public Color HighlightedNodeColor { get { return _highlightedNodeColor; } set { _highlightedTreeBrush = new SolidBrush(value); _highlightedNodeColor = value; } }
        private Color _highlightedNodeColor;
        private Brush _highlightedTreeBrush;
        
        public List<object> HighlightedTagList;

		/// <summary> Color of the selection rectangle around the node, in owner-draw mode, when the node is highlighted </summary>
		public Color SelectedListNodeFocusedColor { get { return _selectedListNodeFocusedColor; } set { _selectedListFocusedTreeBrush = new SolidBrush(value); _selectedListNodeFocusedColor = value; } }
        private Color _selectedListNodeFocusedColor;
        private Brush _selectedListFocusedTreeBrush;
        
        /// <summary> Color of the selection rectangle around the node, in owner-draw mode, when the node is highlighted </summary>
		public Color SelectedListNodeUnfocusedColor { get { return _selectedListNodeUnfocusedColor; } set { _selectedListUnfocusedTreeBrush = new SolidBrush(value); _selectedListNodeUnfocusedColor = value; } }
        private Color _selectedListNodeUnfocusedColor;
        private Brush _selectedListUnfocusedTreeBrush;
        
        public List<TreeNode> SelectedNodes;

		/// <summary> Happens when a node was moved in the tree </summary>
        public delegate void NodeMovedEventHandler(TreeNode node, bool suspendUpdate);
        public event NodeMovedEventHandler NodeMoved;

        /// <summary>
        /// Occurs when a node was moved in the TreeViewWx
        /// </summary>
        public void OnNodeMoved(TreeNode node, bool suspendUpdate = false)
        {
            if (node!=null && NodeMoved != null)
                NodeMoved(node, suspendUpdate);
        }

		/// <summary> Checks is the node is a folder (can accept child nodes) </summary>
        public delegate bool IsFolderFunction(TreeNode node);
        /// <summary> Checks if node is a folder (can accept child nodes) </summary>
        public IsFolderFunction IsFolder;
		public void IsFolder_Reset() { IsFolder = IsFolderDefault;}
        private static bool IsFolderDefault(TreeNode node) { return true; }

        /// <summary>
        /// Checks if the parent node is allowed to accept the child node with the specified form of relationship
        /// </summary>
        /// <param name="parent">The node that is dropped on, the one that is "accepting" the relationship</param>
        /// <param name="child">The node that is being dragged, the one that "proposes" the relationship</param>
        /// <param name="relation">1 = Insert child above, 2 = Add child into, 3 = Insert child under</param>
        /// <returns></returns>
        public delegate bool IsAllowedToHaveRelationFunction(TreeNode parent, TreeNode child, NodeRelationship relation);
        /// <summary>
        /// Checks if the parent node is allowed to accept the child node with the specified form of relationship; 
        /// Params: Parent, Child, Relationship;
        /// Relationships: 1 = Insert child above, 2 = Add child into, 3 = Insert child under
        /// </summary>
        public IsAllowedToHaveRelationFunction IsAllowedToHaveRelation;
		public void IsAllowedToHaveRelation_Reset() {IsAllowedToHaveRelation = IsAllowedToHaveRelationDefault;}
        private static bool IsAllowedToHaveRelationDefault(TreeNode parent, TreeNode child, NodeRelationship relation) { return true; }

        private void PaintDragDestinationGraphics(NodeRelationship _prevDNL = NodeRelationship.Null, TreeNode _prevDN = null)
        {
            if ((_prevDN == null || _prevDNL == NodeRelationship.Null) && (_dropNode == null || _dropNodeLocation == NodeRelationship.Null))
				return;

			Color DrawColor;
			Color SelectColor;
			TreeNode node;
            NodeRelationship location;
			bool removal;
            if (_prevDN != null & _prevDNL != NodeRelationship.Null)
			{
				DrawColor = SystemColors.Window;
				SelectColor = SystemColors.Window;
				node = _prevDN;
				location = _prevDNL;
				removal = true;
			}
			else
			{
				DrawColor = SystemColors.WindowText;
				SelectColor = SystemColors.MenuHighlight;
				node = _dropNode;
				location = _dropNodeLocation;
				removal=false;
			}

            Graphics g = CreateGraphics();

            int dropNodeImageWidth = ImageList.Images[node.ImageIndex == -1 ? 0 : node.ImageIndex].Size.Width + 2;
            // Once again, we are not dragging to node over, draw the placeholder using the ParentDragDrop bounds
            int LeftPos, RightPos, VertPos = 0;
            LeftPos = node.Bounds.Left - dropNodeImageWidth;
            RightPos = Width;

            switch (location)
            {
                case NodeRelationship.ChildGoesAbove:
                    VertPos = node.Bounds.Top;
                    break;
                case NodeRelationship.ChildGoesInside:
                    VertPos = node.Bounds.Bottom - node.Bounds.Height / 2;
                    LeftPos = node.Bounds.Right + 6;
					if (removal)
					{
						using (Brush brush = new System.Drawing.SolidBrush(SelectColor))
                            g.FillRectangle(brush, new Rectangle(node.Bounds.Location, node.Bounds.Size));
						if (node.TreeView != null)
							OnDrawNode(new DrawTreeNodeEventArgs(g, node, Helper.MakeDrawNodeRectangle(node.Bounds), TreeNodeStates.Default));
					}
					else
					{
						using (Brush brush = new System.Drawing.SolidBrush(Color.FromArgb(64, SelectColor)))
                            g.FillRectangle(brush, new Rectangle(node.Bounds.Location, node.Bounds.Size));
					}	
                    using (Pen pen = new System.Drawing.Pen(DrawColor, 1))
                        g.DrawRectangle(pen, new Rectangle(node.Bounds.Location, node.Bounds.Size));
                    Point[] triangle = new Point[4] { new Point(LeftPos, VertPos-1), new Point(LeftPos, VertPos), new Point(LeftPos + 11, VertPos + 4), new Point(LeftPos + 11, VertPos - 5) };
                    using (Brush brush = new System.Drawing.SolidBrush(DrawColor))
                        g.FillPolygon(brush, triangle);
                    break;
                case NodeRelationship.ChildGoesUnder:
                    VertPos = node.Bounds.Bottom;
                    break;
            }
            using (Pen pen = new System.Drawing.Pen(DrawColor, 2))
                g.DrawLine(pen, new Point(LeftPos, VertPos), new Point(RightPos, VertPos));
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _dragNode = (TreeNode)e.Item;
            SelectedNode = _dragNode;
            DoDragDrop(_dragNode, DragDropEffects.Move);
            _dragNode = null;
            base.OnItemDrag(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
			if (!DraggingInProgress)
			{
				drgevent.Effect = DragDropEffects.None;
				return;
			}

            drgevent.Effect = DragDropEffects.Move;

            TreeNode prevDN = _dropNode;
            NodeRelationship prevDNL = _dropNodeLocation;

            // Get actual drop node
            _dropNode = GetNodeAt(PointToClient(new Point(drgevent.X, drgevent.Y)));
            if (_dropNode != null)
            {

                int OffsetY = PointToClient(Cursor.Position).Y - _dropNode.Bounds.Top;

                if (IsFolder(_dropNode))
                {
                    if (OffsetY < (_dropNode.Bounds.Height / 3))
                        _dropNodeLocation = NodeRelationship.ChildGoesAbove;
                    else if (OffsetY > (_dropNode.Bounds.Height * 2 / 3))
                        _dropNodeLocation = NodeRelationship.ChildGoesUnder;
                    else
                        _dropNodeLocation = NodeRelationship.ChildGoesInside;
                }
                else
                {
                    if (OffsetY < (_dropNode.Bounds.Height / 2))
                        _dropNodeLocation = NodeRelationship.ChildGoesAbove;
                    else
                        _dropNodeLocation = NodeRelationship.ChildGoesUnder;
                }

                //Check if we are allowed to put this into that
                if (!IsAllowedToHaveRelation(_dropNode, _dragNode, _dropNodeLocation))
                {
                    drgevent.Effect = DragDropEffects.None;
                    _dropNodeLocation = NodeRelationship.Null;
                }

                if (_dropNode == _dragNode && _dropNodeLocation == NodeRelationship.ChildGoesInside)
				{
					drgevent.Effect = DragDropEffects.None;
					_dropNodeLocation = NodeRelationship.Null;
				}

                // Avoid that drop node is child of drag node 
                TreeNode tmpNode = _dropNode;
                while (tmpNode.Parent != null)
                {
					if (tmpNode.Parent == this._dragNode)
					{
						drgevent.Effect = DragDropEffects.None;
						_dropNodeLocation = NodeRelationship.Null;
					}
                    tmpNode = tmpNode.Parent;
                }
                
            }

			//Mechanism to scroll when reaching the end
			if (_dropNode != null)
			{
				//Always ensure that the node we are dropping over/in/under is visible
				_dropNode.EnsureVisible();
				//Highlight previous node if it exists and...
				if (_dropNode.PrevNode != null)
				{
					//If previous node has no nodes or isn't expanded then this node is a single node and should be visible
					if (_dropNode.PrevNode.Nodes.Count == 0 || !_dropNode.PrevNode.IsExpanded)
						_dropNode.PrevNode.EnsureVisible();
					//If previous node has nodes and is expanded then its last visible child should be made visible
					else
					{
						//Find last visible child recursively
						TreeNode lastChild = _dropNode.PrevNode.Nodes[_dropNode.PrevNode.Nodes.Count - 1];
						while (lastChild.Nodes.Count > 0 && lastChild.IsExpanded)
							lastChild = lastChild.Nodes[lastChild.Nodes.Count - 1];
						lastChild.EnsureVisible();
					}
				}
				//Highlight parent node if previous node doesnt exist (meaning node above this node is its parent)
				else
				{
					if (_dropNode.Parent != null)
						_dropNode.Parent.EnsureVisible();
				}
				//Highlight next node if it exists and...
				if (_dropNode.NextNode != null)
				{
					//If this node has no nodes or isn't expanded then it is single, and next node should be visible
					if (_dropNode.Nodes.Count == 0 || !_dropNode.IsExpanded)
						_dropNode.NextNode.EnsureVisible();
					//If this node has nodes and is expanded then we should select its first child
					else
						_dropNode.Nodes[0].EnsureVisible();
				}
				//Highlight next node after parent node if there is no next node
				else
				{
					if (_dropNode.Nodes.Count == 0 || !_dropNode.IsExpanded)
						if (_dropNode.Parent != null && _dropNode.Parent.NextNode != null)
							_dropNode.Parent.NextNode.EnsureVisible();
				}
			}

            if (prevDNL != _dropNodeLocation || prevDN != _dropNode)
            {
				PaintDragDestinationGraphics(prevDNL, prevDN);
                PaintDragDestinationGraphics();
            }

            base.OnDragOver(drgevent);
        }
        
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (_dragNode != null && _dropNode!=null&& _dropNodeLocation != NodeRelationship.Null)
                MoveNode(_dragNode, _dropNode, _dropNodeLocation);

            base.OnDragDrop(drgevent);
        }

		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            //Try to skip drawing invisible nodes
            if (e.Bounds.Height == 0) // If node is inside a collapsed node
                return;
            if (e.Bounds.Y + e.Bounds.Height < 0) // If node is above control visible area
                return;
            if (e.Bounds.Y > Height) // If node is below control visible area
                return;


            //TreeNodeStates treeState = e.State; // no longer required as we have custom selection method
            Font treeFont = e.Node.NodeFont ?? e.Node.TreeView.Font;
            
            // Colors.
            Color foreColor = e.Node.ForeColor;
            
            // Set default font color.
            if (foreColor == Color.Empty)
                foreColor = e.Node.TreeView.ForeColor;

            // Draw bounding box
			if (e.Node == e.Node.TreeView.SelectedNode)
			{
				if (this.Focused)
				{
					foreColor = SystemColors.HighlightText;
					e.Graphics.FillRectangle(_selectedNodeFocusedTreeBrush, e.Bounds);
					ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, foreColor, SystemColors.Highlight);
				}
				else
				{
					e.Graphics.FillRectangle(_selectedNodeUnfocusedTreeBrush, e.Bounds);
					ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, foreColor, SystemColors.Highlight);
				}
			}
			else
			{
				if (SelectedNodes.Contains(e.Node))
				{
					if (this.Focused)
					{
						foreColor = SystemColors.HighlightText;
						e.Graphics.FillRectangle(_selectedListFocusedTreeBrush, e.Bounds);
						ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, foreColor, SystemColors.Highlight);
					}
					else
					{
						e.Graphics.FillRectangle(_selectedListUnfocusedTreeBrush, e.Bounds);
						ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, foreColor, SystemColors.Highlight);
					}
				}
				else if (HighlightedTagList.Contains(e.Node.Tag))
					e.Graphics.FillRectangle(_highlightedTreeBrush, e.Bounds); //highlighted nodemmm
				else
					e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
			}
            
            TextRenderer.DrawText(e.Graphics, e.Node.Text, treeFont, e.Bounds, foreColor, TextFormatFlags.GlyphOverhangPadding);

            base.OnDrawNode(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode underMouse = GetNodeAt(new Point(e.X, e.Y));
                if (underMouse != null)
                    SelectedNode = underMouse;
            }
            base.OnMouseDown(e);
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            LabelEdit = false;
            base.OnAfterLabelEdit(e);
        }

        /// <summary> Begin editing label of the current selected TreeNode </summary>
        public void BeginEdit()
        {
            LabelEdit = true;
            SelectedNode.BeginEdit();
        }

        public void MoveNode(TreeNode what, TreeNode where, NodeRelationship relation, NodeRelationship subRelation = NodeRelationship.Null, bool suspendUpdate = false)
        {
            if (!IsAllowedToHaveRelation(where,what,relation))
                return;

            if (subRelation == NodeRelationship.Null)
                subRelation = Settings.Current.DragIntoFolderToLastPosition ? NodeRelationship.ChildGoesUnder : NodeRelationship.ChildGoesAbove;
            
            TreeNode result = null;
            
            if (!suspendUpdate)
                BeginUpdate();

            if (where == null)
            {
				what.Remove();
				Nodes.Add(what);
            }
            else
            {
				if (what != where)
					switch (relation)
					{
                        case NodeRelationship.ChildGoesAbove:
							what.Remove();
							if (where.Parent != null)
								where.Parent.Nodes.Insert(where.Index, what);
							else
								Nodes.Insert(where.Index, what);
							result = what;
							break;
                        case NodeRelationship.ChildGoesInside:
							what.Remove();
                            if (subRelation == NodeRelationship.ChildGoesUnder)
								where.Nodes.Add(what);
                            if (subRelation == NodeRelationship.ChildGoesAbove)
								where.Nodes.Insert(0, what);
							result = what;
							break;
                        case NodeRelationship.ChildGoesUnder:
							what.Remove();
							if (where.Parent != null)
								where.Parent.Nodes.Insert(where.Index + 1, what);
							else
								Nodes.Insert(where.Index + 1, what);
							result = what;
							break;
					}
            }
            
            if (result!=null)
                SelectedNode = result;

            if (!suspendUpdate)
                EndUpdate();

            OnNodeMoved(result, suspendUpdate);
        }

		public TreeNode FindNode_RecursivelyFind(TreeNode node, Func<TreeNode, bool> searcher)
		{
			TreeNode result = null;

			foreach (TreeNode cnode in node.Nodes)
				if ((result = FindNode_RecursivelyFind(cnode, searcher)) != null)
					return result;

			if (searcher(node))
				return node;
			else
				return result;
		}

        /// <summary> Finds first node that conforms to the passed condition </summary>
        public TreeNode FindNode(Func<TreeNode, bool> searcher)
        {
			TreeNode result = null;
			foreach (TreeNode node in Nodes)
				if ((result = FindNode_RecursivelyFind(node, searcher)) != null)
					return result;
			return result;
        }

		/// <summary> Check if one node is located inside another node (at any nested level) </summary>
		public bool NodeIsInsideNode(TreeNode what, TreeNode where)
		{
			if (what == where)
				return true;
			foreach (TreeNode node in where.Nodes)
				if (FindNode_RecursivelyFind(node, (TreeNode x) => x == what) != null)
					return true;
			return false;
		}

		public void NodesClear()
		{
			Nodes.Clear();
			SelectedNodes.Clear();
		}

		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
            if (!(Control.ModifierKeys == (Keys.Shift)) || Helper.PasteInProgress)
            {
                SelectedNodes.Clear();
                Invalidate();
            }

            if (!SelectedNodes.Contains(SelectedNode))
                SelectedNodes.Add(SelectedNode);

            base.OnAfterSelect(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			foreach (TreeNode node in SelectedNodes)
				OnDrawNode(new DrawTreeNodeEventArgs(this.CreateGraphics(), node, Helper.MakeDrawNodeRectangle(node.Bounds), TreeNodeStates.Default));
					
			base.OnLostFocus(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			foreach (TreeNode node in SelectedNodes)
				OnDrawNode(new DrawTreeNodeEventArgs(this.CreateGraphics(), node, Helper.MakeDrawNodeRectangle(node.Bounds), TreeNodeStates.Default));
			
			base.OnGotFocus(e);
		}

		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (!(Control.ModifierKeys == (Keys.Shift)) && e.Node == SelectedNode)
				{
					SelectedNodes.Clear();
					if (!SelectedNodes.Contains(SelectedNode))
						SelectedNodes.Add(SelectedNode);
					Invalidate();
				}
			}

			base.OnNodeMouseClick(e);
		}

        /// <summary> Makes this TreeView have only Vertical Scrollbar </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x8000; // TVS_NOHSCROLL
                return cp;
            }
        }

        public TreeViewEx() : base()
        {
            IsFolder_Reset();
			IsAllowedToHaveRelation_Reset();

            SelectedNodeColorFocused = Color.FromArgb(50, 150, 250);
			SelectedListNodeFocusedColor = Color.FromArgb(85, 185, 255);
			SelectedNodeColorUnfocused = Color.FromArgb(225, 240, 255);
			SelectedListNodeUnfocusedColor = Color.FromArgb(245, 255, 255);
			HighlightedNodeColor = Color.FromArgb(255, 230, 220);

			HighlightedTagList = new List<object>();
			SelectedNodes = new List<TreeNode>();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
            int TVS_EX_DOUBLEBUFFER = 0x0004;
            if (Settings.Current.TreeViewFlickeringFix)
                SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            
            base.OnHandleCreated(e);
        }
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
