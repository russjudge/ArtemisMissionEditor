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
    /// This check is for specified point vs name vs gm position in multiple statements that require a position as one of parameters.
    /// </summary>
    public sealed class ExpressionMemberCheck_Point_Name_GM : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            if (!String.IsNullOrEmpty(container.GetAttribute("name")))
                return Choices[2]; // name
            if (container.GetAttribute("use_gm_position") != null)
                return Choices[1]; // use_gm_pos
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
                container.SetAttribute("use_gm_position", null);
                container.SetAttribute("name", null);
            }
            if (value == Choices[1])  //gm_pos
            {
                if (container.GetAttribute("use_gm_position") == null)
                    container.SetAttribute("use_gm_position", "");
                container.SetAttribute("name", null);
            }
            if (value == Choices[2]) // name
            {
                container.SetAttributeIfNull("name", " ");
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for specified point vs name vs gm position in multiple statements that require a position as one of parameters.
        /// </summary>
        /// <param name="unbound">Whether the coordinates are bound by map boundaries.</param>
        public ExpressionMemberCheck_Point_Name_GM(bool unbound = false)
            : base("", ExpressionMemberValueDescriptions.Check_Point_Name_GM)
        {
            List<ExpressionMember> eML;

            eML = this.Add("point"); //Choices[0]
                                     //In case this is point, it looks like this : "... at point (x, y, z)"
            eML.Add(new ExpressionMember("<>", unbound ? ExpressionMemberValueDescriptions.XUnbound : ExpressionMemberValueDescriptions.X, "centerX"));
            eML.Add(new ExpressionMember("<>", unbound ? ExpressionMemberValueDescriptions.YUnbound : ExpressionMemberValueDescriptions.Y, "centerY"));
            eML.Add(new ExpressionMember("<>", unbound ? ExpressionMemberValueDescriptions.ZUnbound : ExpressionMemberValueDescriptions.Z, "centerZ"));

            eML = this.Add("GM position"); //Choices[1]
                                           //In case this is using GM pos, it looks like: "... at GM position"
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.UseGM, "use_gm_position"));

            eML = this.Add("named object"); //Choices[2]
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.NameAll, "name"));
        }
    }
}
