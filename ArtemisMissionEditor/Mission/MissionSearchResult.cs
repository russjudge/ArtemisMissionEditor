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
	/// <summary>
	/// Represents a mission saved in a state for undo/redo
	/// </summary>
	public sealed class MissionSearchResult
	{
		public int SortOrder = 0;

        public int CurNode { get; private set; }
        public int CurStatement { get; private set; }

        /// <summary> Text representing the search result (to be displayed in the list box) </summary>
        public string Text { get; private set; }
        /// <summary> Text from the node that contains search result </summary>
        public string NodeText { get; private set; }
        /// <summary> Node from the mission nodes tree that is to be selected </summary>
        public TreeNode Node;
        /// <summary> Safe way to check for Node's tag </summary>
        public object NodeTag { get { return Node == null ? null : Node.Tag; } }
        /// <summary> Node from the statement tree that is to be selected </summary>
        public MissionStatement Statement;

		public DependencyEvent Event;

        public bool Valid;

        public override string ToString()
        {
            return Text;
        }

        /// <summary> Activate the current mission result, selecting the corresponding node and statement </summary>
        public void Activate(DependencyEvent item, bool highlightActions)
        {
			if (!Valid)
				return; 
			Mission.Current.SelectNode(Node);
			Mission.Current.SelectStatement(Statement, true);
			
			if (highlightActions)
				Mission.Current.ApplyHighlighting(Event, item, highlightActions);
			else
				Mission.Current.ApplyHighlighting(item, Event, highlightActions);
        }

		public void Activate()
		{
			if (!Valid)
				return;
			Mission.Current.SelectNode(Node);
			Mission.Current.SelectStatement(Statement, true);
		}

        public MissionSearchResult(int curNode, int curStatement, string text, TreeNode node, MissionStatement statement)
        {
            CurNode = curNode;
            CurStatement = curStatement;
            if (text == null)
            {
                Text = null;
                Valid = false;
            }
            else if (node == null)
            {
                Text =  text;
                NodeText = null;
                Valid = false;
            }
            else
            {
                //string node_text = node.Text.Length > 25 ? node.Text.Substring(0, 25 - 3) + "..." : string.Format("{0,-25}", node.Text);
                NodeText = node.Text;
                //Text = "(" + node_text + ") " + text;
                Text = text;
                Valid = true;
            }
            Node = node;
            Statement = statement;
			Event = null;
        }

        public MissionSearchResult(string prefix, string suffix, TreeNode node)
        {
            CurNode = -1;
            CurStatement = -1;
            //string node_text = node.Text.Length > 25 ? node.Text.Substring(0, 25 - 3) + "..." : string.Format("{0,-25}", node.Text);
			string node_text = node.Text.Length > 25 ? node.Text.Substring(0, 25 - 3) + "..." : string.Format("{0,-25}", node.Text);
			Text = prefix + node_text + suffix;
            Valid = true;
            Node = node;
            Statement = null;
			Event = null;
        }

		public MissionSearchResult(string prefix, string suffix, DependencyEvent _event, int sortOrder = 0)	: this(prefix, suffix, _event.Node)
		{
			Event = _event;
			SortOrder = sortOrder;
		}

		public MissionSearchResult()
		{
			CurNode = -1;
			CurStatement = -1;
			Text = null;
            NodeText = null;
			Valid = false;
			Node = null;
			Statement = null;
			Event = null;
		}

	}
}