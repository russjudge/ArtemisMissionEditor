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
	/// Represents a filled command to execute a search
	/// </summary>
	public class MissionSearchCommand
	{
        /// <summary> Text to look for </summary>
		public string	Input;

        /// <summary> Text to replace with </summary>
		public string	Replacement;
		
		/// <summary> Whether to look in attribute names </summary>
		public bool		XmlAttName;
		
		/// <summary> Whether to look in attribute values </summary>
		public bool		XmlAttValue;
		
		/// <summary> If not empty, look in exactly this attribute for value </summary>
		public string	AttName;
		
		/// <summary> Whether to look in node names </summary>
		public bool		NodeNames;
		
		/// <summary> Whether to look in commentaries </summary>
		public bool		Commentaries;
		
		/// <summary> Whether to look in statement text </summary>
		public bool		StatementText;
		
		/// <summary> Match exact case </summary>
		public bool		MatchCase;
		
		/// <summary> Match exact value </summary>
		public bool		MatchExact;

        /// <summary> Look only in current node </summary>
        public bool     OnlyInCurrentNode;

		public MissionSearchCommand(string input, string replacement, bool xmlAttName, bool xmlAttValue, string attName, bool nodeNames, bool commentaries, bool statementText, bool matchCase, bool matchExact, bool onlyInCurrentNode)
		{
			Input = input;
			Replacement = replacement;
			XmlAttName = xmlAttName;
			XmlAttValue = xmlAttValue;
			AttName = attName;
			NodeNames = nodeNames;
			Commentaries = commentaries;
			StatementText = statementText;
			MatchCase = matchCase;
			MatchExact = matchExact;
            OnlyInCurrentNode = onlyInCurrentNode;
		}
	}
}
