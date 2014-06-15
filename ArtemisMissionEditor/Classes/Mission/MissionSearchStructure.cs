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
	public struct MissionSearchStructure
	{
        /// <summary> Text to look for </summary>
		public string	input;

		public string	replacement;
		
		/// <summary> Wether to look in attribute names </summary>
		public bool		xmlAttName;
		
		/// <summary> Wether to look in attribute values </summary>
		public bool		xmlAttValue;
		
		/// <summary> If not empty, look in exactly this attribute for value </summary>
		public string	attName;
		
		/// <summary> Wether to look in node names </summary>
		public bool		nodeNames;
		
		/// <summary> Wether to look in commentaries </summary>
		public bool		commentaries;
		
		/// <summary> Wether to look in statement text </summary>
		public bool		statementText;
		
		/// <summary> Match exact case </summary>
		public bool		matchCase;
		
		/// <summary> Match exact value </summary>
		public bool		matchExact;

        /// <summary> Look only in current node </summary>
        public bool     onlyInCurrentNode;

		public MissionSearchStructure(string input, string replacement, bool xmlAttName, bool xmlAttValue, string attName, bool nodeNames, bool commentaries, bool statementText, bool matchCase, bool matchExact, bool onlyInCurrentNode)
		{
			this.input = input;
			this.replacement = replacement;
			this.xmlAttName = xmlAttName;
			this.xmlAttValue = xmlAttValue;
			this.attName = attName;
			this.nodeNames = nodeNames;
			this.commentaries = commentaries;
			this.statementText = statementText;
			this.matchCase = matchCase;
			this.matchExact = matchExact;
            this.onlyInCurrentNode = onlyInCurrentNode;
		}
	}
}