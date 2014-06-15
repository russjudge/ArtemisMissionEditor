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
	public sealed class MissionSavedState
	{
		/// <summary>
		/// Xml representing the mission
		/// </summary>
		public string Xml;

		public string CommentsBefore;
		public string CommentsAfter;
		public string FilePath;
		
		/// <summary>
		/// Short description of the saved state - aka what was changed in it comparing to the previous one
		/// </summary>
		public string Description;

		/// <summary> Selected node (from mission nodes)</summary>
		public int SelectedNode;
		/// <summary> Selected condition inside node </summary>
		public int SelectedCondition;
		/// <summary> Selected action inside node </summary>
		public int SelectedAction;
		/// <summary> Selected label inside selected statement </summary>
		public int SelectedLabel;

        public bool ChangesPending;
	}
}