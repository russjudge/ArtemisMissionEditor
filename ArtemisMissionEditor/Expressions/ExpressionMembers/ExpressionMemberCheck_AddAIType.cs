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
    /// This check is for "type" from [add_ai], is hidden since add ai statement has type as a member.
	/// </summary>
	public sealed class ExpressionMemberCheck_AddAIType : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("type");

			switch (type)
			{
				case "TRY_TO_BECOME_LEADER":return "<NOTHING>";
				case "CHASE_PLAYER":		return "<CHASE>";
				case "CHASE_AI_SHIP":		return "<CHASE>";
				case "CHASE_STATION":		return "<CHASE_NO_NEBULA>";
				case "CHASE_WHALE":			return "<CHASE_NO_NEBULA>";
				case "AVOID_WHALE":			return "<AVOID>";
				case "AVOID_BLACK_HOLE":	return "<AVOID>";
				case "CHASE_ANGER":			return "<NOTHING>";
				case "CHASE_FLEET":			return "CHASE_FLEET";
				case "FOLLOW_LEADER":		return "<NOTHING>";
				case "FOLLOW_COMMS_ORDERS": return "<NOTHING>";
				//case "FOLLOW_NEUTRAL_PATH": return "<NOTHING>";
				case "LEADER_LEADS":		return "<NOTHING>";
				case "SPCL_AI":			return "<NOTHING>";
				case "DIR_THROTTLE":		return "DIR_THROTTLE";
				case "POINT_THROTTLE":		return "POINT_THROTTLE";
				case "TARGET_THROTTLE":		return "TARGET_THROTTLE";
				case "ATTACK":				return "ATTACK";
				case "DEFEND":				return "DEFEND";
				case "PROCEED_TO_EXIT":		return "<NOTHING>";
				case "FIGHTER_BINGO":		return "<NOTHING>";
				case "LAUNCH_FIGHTERS":		return "LAUNCH_FIGHTERS";
				case "GUARD_STATION":       return "GUARD_STATION";
				default:
					return "<INVALID_TYPE>";//This must be further converted in SetValue to some valid one, and type must be set there as well
			}
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == "<INVALID_TYPE>")
			{
				value = "<NOTHING>";
				container.SetAttribute("type", "PROCEED_TO_EXIT");
			}

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Adds "(type) "
		/// </summary>
		private void ____Add_Type(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("\""));
			eML.Add(new ExpressionMember("<type>", ExpressionMemberValueDescriptions.TypeAI, "type"));
		}

		/// <summary>
		/// Adds "for object [selected by gm or named]"
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML, ExpressionMemberValueDescription name = null)
		{
            name = name ?? ExpressionMemberValueDescriptions.NameAll;

			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM(name));
		}
        
        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition. 
        /// This check is for "type" from [add_ai], is hidden since add ai statement has type as a member.
        /// </summary>
		public ExpressionMemberCheck_AddAIType()
			: base()
		{
			List<ExpressionMember> eML;

			#region <NOTHING>		(TRY_TO_BECOME_LEADER, CHASE_ANGER, FOLLOW_LEADER, FOLLOW_COMMS_ORDERS, FOLLOW_NEUTRAL_PATH, LEADER_LEADS, SPCL_AI, PROCEED_TO_EXIT)

			eML = this.Add("<NOTHING>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region <CHASE>		(CHASE_PLAYER, CHASE_NEUTRAL, CHASE_ENEMY)

			eML = this.Add("<CHASE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNebula());
			____Add_Name(eML);

			#endregion

			#region <CHASE_NO_NEBULA>		(CHASE_STATION, CHASE_WHALE)

			eML = this.Add("<CHASE_NO_NEBULA>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadius, "value1"));
			____Add_Name(eML);

			#endregion

			#region <AVOID>

			eML = this.Add("<AVOID>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadius, "value1"));
			____Add_Name(eML);

			#endregion

			#region CHASE_FLEET

			eML = this.Add("CHASE_FLEET");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region DIR_THROTTLE

			eML = this.Add("DIR_THROTTLE");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Angle, "value1"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueT, "value2"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region POINT_THROTTLE

			eML = this.Add("POINT_THROTTLE");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("towards "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "value1"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Y, "value2"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "value3"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueT, "value4"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region TARGET_THROTTLE

			eML = this.Add("TARGET_THROTTLE");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("towards "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "targetName"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueT, "value1"));
			eML.Add(new ExpressionMember("\" ")); 
			____Add_Name(eML);

			#endregion

			#region ATTACK

			eML = this.Add("ATTACK");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "targetName"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueT, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region DEFEND

			eML = this.Add("DEFEND");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("ally "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadius, "value2"));
			eML.Add(new ExpressionMember(", "));
			eML.Add(new ExpressionMember("engaging "));
			eML.Add(new ExpressionMember("anyone "));
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("range "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion

			#region LAUNCH_FIGHTERS

			eML = this.Add("LAUNCH_FIGHTERS");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("player "));
			eML.Add(new ExpressionMember("ship "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadius, "value1"));
			____Add_Name(eML);

			#endregion

			#region GUARD_STATION

			eML = this.Add("GUARD_STATION");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("magic "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML);

			#endregion
		}
	}
}