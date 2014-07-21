using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor
{
	/// <summary>
	/// How the value is stored in Xml
	/// </summary>
	public enum ExpressionMemberValueBehaviorInXml
	{
		/// <summary>
		/// The value is not being stored in Xml at all
		/// </summary>
		Ignored,
		/// <summary>
		/// The value is stored as is - even if null or missing
		/// </summary>
		AsIs,
		/// <summary>
		/// The value is stored as is - but only if its filled (nonMin / nonempty / nonfalse)
		/// </summary>
		AsIsWhenFilled
	}

}