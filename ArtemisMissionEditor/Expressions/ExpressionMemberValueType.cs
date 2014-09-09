using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Type of the value (defines what kind of values are allowed)
    /// </summary>
	public enum ExpressionMemberValueType
	{
		/// <summary>
		/// Means it doesnt have a value assigned to it
		/// </summary>
		Nothing,
		VarInteger,
		VarBool,
		VarDouble,
		VarString,
		/// <summary>
		/// Means its a multiline string
		/// </summary>
		Body
	}
}