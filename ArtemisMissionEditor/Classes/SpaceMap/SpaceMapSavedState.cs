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
	public sealed class SpaceMapSavedState
	{
		/// <summary>
		/// Xml representing the map
		/// </summary>
		public string Xml;
		public string BgXml;
		public string StatementXml;

		public List<int> NamedIdList;
		public List<int> NamelessIdList;

		public List<int> NamedImported;
		public List<int> NamelessImported;

		public List<int> NamedSelected;
		public int? NamelessSelected;

		/// <summary>
		/// Short description of the saved state - aka what was changed in it comparing to the previous one
		/// </summary>
		public string Description;

		public bool ChangesPending;
	}
}