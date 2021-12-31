using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtemisMissionEditor.Expressions
{	
	/// <summary>
	/// Static class containing all the expressions and the expression constructor
	/// </summary>
    /// <remarks>
    /// This class contains the "schematic" for the expression parsing. 
    /// In the Root member exists the sole check. Every statement is parsed by that check and decided into one of the three.
    /// Then it gets decided further and further (Expression -> Action -> Destroy -> Destroy Nameless -> ...) until everything is parsed
    /// </remarks>
	public static class ExpressionParser
	{
		/// <summary>
		/// Contains all of the possible expressions
		/// </summary>
        public static readonly List<ExpressionMember> Root = new List<ExpressionMember>() { new ExpressionMemberCheck_XmlNode() };

		/// <summary>
		/// Constructs the expression representing the given statement
		/// </summary>
		/// <param name="statement"></param>
		/// <returns></returns>
		public static List<ExpressionMemberContainer> ParseExpression(MissionStatement statement)
		{
			List<ExpressionMemberContainer> newExpression = new List<ExpressionMemberContainer>();

			Append(statement, Root, newExpression);

			return newExpression;
		}

		/// <summary>
		/// Appends expression members from the specified list of ExpressionMemebers to the specified list of ExpressionMemberContainers
		/// </summary>
		/// <param name="statement"></param>
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

				string key = nEMC.Decide();
				List<ExpressionMember> toAppend = ((ExpressionMemberCheck)item).PossibleExpressions[key];
				Append(statement, toAppend, where);
			}
		}
	}
}
