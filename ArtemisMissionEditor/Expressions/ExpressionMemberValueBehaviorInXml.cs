using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor.Expressions
{
	/// <summary>
	/// Describes if / how the value is stored in XML
	/// </summary>
	public enum ExpressionMemberValueBehaviorInXml
	{
		/// <summary> The value is not being stored in XML at all</summary>
		NotStored,
		/// <summary> The value is stored as is - even if null or missing </summary>
		StoredAsIs,
		/// <summary> The value is stored as is - but only if its filled (nonMin / nonempty / nonfalse) </summary>
		StoredWhenFilled
	}

}
