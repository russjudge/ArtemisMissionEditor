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
    /// This check is providing of an option to convert a deprecated [direct] statement into [add_ai] statement.
    /// </summary>
    public sealed class ExpressionMemberCheck_DirectConversion : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			return Choices[0];
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[1]) //convert
			{
				string msg = "Direct statement (name: \"" + (container.GetAttribute("name") ?? "<null>") + "\" ";

				value = "add_ai";
				container.Statement.Name = value;
				if (!String.IsNullOrEmpty(container.GetAttribute("targetName")))
				{
					//ATTACK
					container.SetAttribute("type", "TARGET_THROTTLE");
					container.SetAttribute("value1", container.GetAttribute("scriptThrottle"));
					msg += "targetName: \"" + container.GetAttribute("targetName") + "\" scriptThrottle \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"TARGET_THROTTLE\" statement";
				}
				else
				{
					//POINT_THROTTLE
					container.SetAttribute("type", "POINT_THROTTLE");
					container.SetAttribute("value1", container.GetAttribute("pointX"));
					container.SetAttribute("value2", container.GetAttribute("pointY"));
					container.SetAttribute("value3", container.GetAttribute("pointZ"));
					container.SetAttribute("value4", container.GetAttribute("scriptThrottle"));
					msg += "target point: \"" + container.GetAttribute("pointX") + ", " + container.GetAttribute("pointY") + ", " + container.GetAttribute("pointZ") + "\" scriptThrottle: \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"POINT_THROTTLE\" statement";
				}

				Log.Add(msg);
			}

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is providing of an option to convert a deprecated [direct] statement into [add_ai] statement.
        /// </summary>
        public ExpressionMemberCheck_DirectConversion()
			: base("", ExpressionMemberValueDescriptions.Check_ConvertDirect)
		{
			List<ExpressionMember> eML;
			eML = this.Add("Do nothing"); //Choices[0]
			eML = this.Add("Convert to add_ai"); //Choices[1]
		}

    }
    public sealed class ExpressionMemberCheck_WhaleConversion : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            return Choices[0];
        }

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            if (value == Choices[1]) //convert
            {
                string msg = "Whale statement (name: \"" + (container.GetAttribute("name") ?? "<null>") + "\" ";

                value = "Monster";
                container.Statement.Name = value;
                if (!String.IsNullOrEmpty(container.GetAttribute("targetName")))
                {
                    //ATTACK
                    container.SetAttribute("type", "TARGET_THROTTLE");
                    container.SetAttribute("value1", container.GetAttribute("scriptThrottle"));
                    msg += "targetName: \"" + container.GetAttribute("targetName") + "\" scriptThrottle \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"TARGET_THROTTLE\" statement";
                }
                else
                {
                    //POINT_THROTTLE
                    container.SetAttribute("type", "POINT_THROTTLE");
                    container.SetAttribute("value1", container.GetAttribute("pointX"));
                    container.SetAttribute("value2", container.GetAttribute("pointY"));
                    container.SetAttribute("value3", container.GetAttribute("pointZ"));
                    container.SetAttribute("value4", container.GetAttribute("scriptThrottle"));
                    msg += "target point: \"" + container.GetAttribute("pointX") + ", " + container.GetAttribute("pointY") + ", " + container.GetAttribute("pointZ") + "\" scriptThrottle: \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"POINT_THROTTLE\" statement";
                }

                Log.Add(msg);
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is providing of an option to convert a deprecated [direct] statement into [add_ai] statement.
        /// </summary>
        //public ExpressionMemberCheck_DirectConversion()
        //    : base("", ExpressionMemberValueDescriptions.Check_ConvertDirect)
        //{
        //    List<ExpressionMember> eML;
        //    eML = this.Add("Do nothing"); //Choices[0]
        //    eML = this.Add("Convert to add_ai"); //Choices[1]
        //}

    }

}