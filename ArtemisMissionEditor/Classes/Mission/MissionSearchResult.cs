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

		public int CurNode;
        public int CurStatement;

        /// <summary> Text representing the search result (to be displayed in the list box) </summary>
        public string Text;
        /// <summary> Node from the mission nodes tree that is to be selected </summary>
        public TreeNode Node;
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
			if (Statement == null)
				Mission.Current.SelectStatement(Node.Tag);
			else 
				Mission.Current.SelectStatement(Statement);
			
			if (highlightActions)
				Mission.Current.SetSelection(Event, item, highlightActions);
			else
				Mission.Current.SetSelection(item, Event, highlightActions);
        }

		public void Activate()
		{
			if (!Valid)
				return;
			Mission.Current.SelectNode(Node);
			if (Statement == null)
				Mission.Current.SelectStatement(Node.Tag);
			else
				Mission.Current.SelectStatement(Statement);
		}

        public MissionSearchResult(int curNode, int curStatement, string text, TreeNode node, MissionStatement statement)
        {
            CurNode = curNode;
            CurStatement = curStatement;
            if (text==null)
            {
                Text = null;
                Valid = false;
            }
            else
            {
				string node_text = node.Text.Length > 25 ? node.Text.Substring(0, 25 - 3) + "..." : string.Format("{0,-25}", node.Text);
                Text = "[" + CurNode.ToString("000") + "][" + (CurStatement==0 ? "--" : CurStatement.ToString("###00")) + "] : " + "("+node_text +") "+text;
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
			Valid = false;
			Node = null;
			Statement = null;
			Event = null;
		}

	}
}