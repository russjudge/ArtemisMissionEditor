﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is for specified point vs name in [direct] statement.
    /// </summary>
    public sealed class ExpressionMemberCheck_Point_TargetName : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (!String.IsNullOrEmpty(container.GetAttribute("targetName")))
				return Choices[1]; // name
			else
				return Choices[0]; // point
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{

			if (value == Choices[0]) //point
			{
				container.SetAttribute("targetName", null);
			}
			if (value == Choices[1]) // name
			{
				container.SetAttributeIfNull("targetName", " ");
			}

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for specified point vs name in [direct] statement.
        /// </summary>
        public ExpressionMemberCheck_Point_TargetName()
			: base("", ExpressionMemberValueDescriptions.Check_Point_TargetName)
		{
			List<ExpressionMember> eML;

			eML = this.Add("point"); //Choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "pointX"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Y, "pointY"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "pointZ"));

			eML = this.Add("object"); //Choices[1]
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.NameAll, "targetName"));
		}
	}
}