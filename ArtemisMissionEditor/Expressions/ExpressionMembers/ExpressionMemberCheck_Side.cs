using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is for default/specified side for [create] statement.
    /// </summary>
    public sealed class ExpressionMemberCheck_Side : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            if (container.GetAttribute("sideValue") == null)
                return Choices[0]; // "... on default side"
            else
                return Choices[1]; // "... on side #"
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for default/specified side for [create] statement.
        /// </summary>
        public ExpressionMemberCheck_Side()
            : base("", ExpressionMemberValueDescriptions.Blank)
        {
            List<ExpressionMember> eML;
            
            eML = this.Add("default"); //Choices[0]
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("default", ExpressionMemberValueDescriptions.SideValue, "sideValue"));
            eML.Add(new ExpressionMember("side "));

            eML = this.Add("specified"); //Choices[1]
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("side "));
            eML.Add(new ExpressionMember("default", ExpressionMemberValueDescriptions.SideValue, "sideValue"));
        }
    }
}