using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	/// <summary>
	/// Static class containing all the expressions and the expression constructor
	/// </summary>
    /// <remarks>
    /// This class contains the "schematic" for the expression parsing. 
    /// In the Root member exists the sole check. Every statement is parsed by that check and decided into one of the three.
    /// Then it gets decided further and further (Expression -> Action -> Destroy -> Destroy Nameless -> ...) until everything is parsed
    /// </remarks>
	public static class Expressions 
	{
        static Expressions() { Root = new List<ExpressionMember>(); Root.Add(new ExpressionMemberCheck_XmlNode_H()); }

		/// <summary>
		/// Root of all evil :) Contains all of the possible expressions
		/// </summary>
		public static List<ExpressionMember> Root;

		/// <summary>
		/// Constructs the expression representing the given statement
		/// </summary>
		/// <param name="statement"></param>
		/// <returns></returns>
		public static List<ExpressionMemberContainer> ConstructExpression(MissionStatement statement)
		{
			List<ExpressionMemberContainer> newExpression = new List<ExpressionMemberContainer>();

			Append(statement, Root, newExpression);

			return newExpression;
		}

		/// <summary>
		/// Appends expression members from the specified list of EM to the specified list of EMC
		/// </summary>
		/// <param name="what"></param>
		/// <param name="where"></param>
		private static void Append(MissionStatement statement, List<ExpressionMember> what, List<ExpressionMemberContainer> where)
		{
			foreach (ExpressionMember item in what)
			{
				ExpressionMemberContainer nEMC = new ExpressionMemberContainer(item, statement);
				where.Add(nEMC);

				if (item as ExpressionMemberCheck == null)
					continue;

				Append(statement, ((ExpressionMemberCheck)item).PossibleExpressions[nEMC.Decide()], where);
			}
		}
	}
}