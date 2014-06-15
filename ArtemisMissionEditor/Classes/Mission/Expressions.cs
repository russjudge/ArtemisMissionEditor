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
	public static partial class Expressions //Everything except constructor
	{
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
			List<ExpressionMemberContainer> newExp = new List<ExpressionMemberContainer>();

			Append(statement, Root, newExp);

			return newExp;
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

				if (!item.IsCheck)
					continue;

				Append(statement, item.PossibleExpressions[nEMC.Decide()], where);
			}
		}
	}

	public static partial class Expressions 
	{
		static Expressions() { Root = new List<ExpressionMember>(); Root.Add(new ExpressionMemberCheck_XmlNode_H()); }
	}
}