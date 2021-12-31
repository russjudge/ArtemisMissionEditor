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
        public static readonly Dictionary<string,string> AITypeDictionary = new Dictionary<string,string>()
        {
            { "ATTACK",  				"<SHIP_ATTACK>" },
            { "AVOID_BLACK_HOLE",    	"<SHIP_AVOID>" },
            { "AVOID_SIGNAL",           "<MONSTER_AVOID_SIGNAL>" },
            { "AVOID_WHALE",  			"<SHIP_AVOID>" },
            { "CHASE_AI_SHIP",    		"<AI_CHASE>" },
            { "CHASE_ANGER",  			"<AI_NOTHING>" },
            { "CHASE_ENEMY",            "<SHIP_CHASE_ENEMY_OBSOLETE>" },
            { "CHASE_FLEET",            "<SHIP_CHASE_FLEET>" },
            { "CHASE_MONSTER",          "<MONSTER_CHASE_NO_NEBULA>" },
            { "CHASE_NEUTRAL",          "<SHIP_CHASE_NEUTRAL_OBSOLETE>" },
            { "CHASE_OTHER_MONSTERS",   "<SHIP_CHASE_NO_NEBULA>" },
            { "CHASE_PLAYER",    		"<AI_CHASE>" },
            { "CHASE_SIGNAL",    		"<MONSTER_CHASE_SIGNAL>" },
            { "CHASE_STATION",    		"<AI_CHASE_NO_NEBULA>" },
            { "CHASE_WHALE",  			"<SHIP_CHASE_NO_NEBULA>" },
            { "DEFEND",  				"<SHIP_DEFEND>" },
            { "DIR_THROTTLE",    		"<AI_DIR_THROTTLE>" },
            { "DRAGON_NEST",     		"<MONSTER_NOTHING>" },
            { "ELITE_AI",               "<SHIP_ELITE_AI_OBSOLETE>" },
            { "FIGHTER_BINGO",    		"<SHIP_NOTHING>" },
            { "FOLLOW_COMMS_ORDERS",    "<SHIP_NOTHING>" },
            { "FOLLOW_LEADER",    		"<SHIP_NOTHING>" },
            { "FRENZY_ATTACK",    		"<MONSTER_NOTHING>" },
            { "GO_TO_HOLE",  		    "<MONSTER_GO_TO_HOLE>" },
            { "GUARD_STATION",          "<SHIP_GUARD_STATION>" },
            { "LAUNCH_FIGHTERS",  		"<SHIP_LAUNCH_FIGHTERS>" },
            { "LEADER_LEADS",    		"<SHIP_NOTHING>" },
            { "MOVE_WITH_GROUP",  		"<MONSTER_MOVE_WITH_GROUP>" },
            { "PLAY_IN_ASTEROIDS",     	"<MONSTER_NOTHING>" },
            { "POINT_THROTTLE",  		"<AI_POINT_THROTTLE>" },
            { "PROCEED_TO_EXIT",  		"<SHIP_NOTHING>" },
            { "RANDOM_PATROL",    	    "<MONSTER_RANDOM_PATROL>" },
            { "RELEASE_PIRANHAS",    	"<MONSTER_RELEASE_PIRANHAS>" },
            { "SPCL_AI",  			    "<SHIP_NOTHING>" },
            { "STAY_CLOSE",  			"<MONSTER_STAY_CLOSE>" },
            { "TARGET_THROTTLE",  		"<AI_TARGET_THROTTLE>" },
            { "TRY_TO_BECOME_LEADER",   "<SHIP_NOTHING>" }
        };

        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("type");

            if (!String.IsNullOrEmpty(type) && AITypeDictionary.ContainsKey(type))
                return AITypeDictionary[type];

			return "<UNKNOWN_TYPE>";
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == "<UNKNOWN_TYPE>")
                Log.Add("Warning! Unknown AI command " + container.GetAttribute("type") + " detected in event: " + container.Statement.Parent.Name + "!");                   
            else if (value == "<SHIP_ELITE_AI_OBSOLETE>")
            {
                // Convert ELITE_AI to SPCL_AI.
                value = "<SHIP_NOTHING>";
                container.SetAttribute("type", "SPCL_AI");
            }
            else if (value == "<SHIP_CHASE_ENEMY_OBSOLETE>" || value == "<SHIP_CHASE_NEUTRAL_OBSOLETE>")
            {
                // Convert to CHASE_AI_SHIP.
                value = "<AI_CHASE>";
                container.SetAttribute("type", "CHASE_AI_SHIP");
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
            name = name ?? ExpressionMemberValueDescriptions.NameAIShipOrMonster;

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

			#region <AI_NOTHING>		(CHASE_ANGER)

			eML = this.Add("<AI_NOTHING>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region <SHIP_NOTHING>		(FIGHTER_BINGO, FOLLOW_COMMS_ORDERS, FOLLOW_LEADER, LEADER_LEADS, PROCEED_TO_EXIT, SPCL_AI, TRY_TO_BECOME_LEADER)

			eML = this.Add("<SHIP_NOTHING>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <MONSTER_NOTHING>		(DRAGON_NEST, FRENZY_ATTACK, PLAY_IN_ASTEROIDS)

			eML = this.Add("<MONSTER_NOTHING>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <AI_CHASE>		(CHASE_PLAYER, CHASE_AI_SHIP)

			eML = this.Add("<AI_CHASE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNebula());
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region <MONSTER_CHASE_NO_NEBULA>		(CHASE_MONSTER)

			eML = this.Add("<MONSTER_CHASE_NO_NEBULA>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNoNebula());
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <SHIP_CHASE_NO_NEBULA>		(CHASE_OTHER_MONSTERS, CHASE_WHALE)

			eML = this.Add("<SHIP_CHASE_NO_NEBULA>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNoNebula());
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <AI_CHASE_NO_NEBULA>		(CHASE_STATION)

			eML = this.Add("<AI_CHASE_NO_NEBULA>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNoNebula());
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region <SHIP_AVOID>

			eML = this.Add("<SHIP_AVOID>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <MONSTER_AVOID_SIGNAL>

			eML = this.Add("<MONSTER_AVOID_SIGNAL>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("magic "));
			eML.Add(new ExpressionMember("value "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <MONSTER_CHASE_SIGNAL>

			eML = this.Add("<MONSTER_CHASE_SIGNAL>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("magic "));
			eML.Add(new ExpressionMember("value "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <MONSTER_RANDOM_PATROL>

			eML = this.Add("<MONSTER_RANDOM_PATROL>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("magic "));
			eML.Add(new ExpressionMember("value "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <SHIP_CHASE_FLEET>

			eML = this.Add("<SHIP_CHASE_FLEET>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_DistanceNoNebula());
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <AI_DIR_THROTTLE>

			eML = this.Add("<AI_DIR_THROTTLE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Angle, "value1"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value2"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region <MONSTER_GO_TO_HOLE>

            eML = this.Add("<MONSTER_GO_TO_HOLE>");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("value1 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value1"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("value2 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value2"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("value3 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value3"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("value4 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value4"));
            eML.Add(new ExpressionMember("\" "));
            ____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <MONSTER_MOVE_WITH_GROUP>

            eML = this.Add("<MONSTER_MOVE_WITH_GROUP>");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("at "));
            eML.Add(new ExpressionMember("throttle "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value1"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("value2 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value2"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("value3 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value3"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("value4 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value4"));
            eML.Add(new ExpressionMember("\" "));
            ____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <AI_POINT_THROTTLE>

			eML = this.Add("<AI_POINT_THROTTLE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("towards "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "value1"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Y, "value2"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "value3"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value4"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region <MONSTER_RELEASE_PIRANHAS>

            eML = this.Add("<MONSTER_RELEASE_PIRANHAS>");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Count, "value3"));
            eML.Add(new ExpressionMember("piranhas "));
            eML.Add(new ExpressionMember("in "));
            eML.Add(new ExpressionMember("pod "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.PodNumber, "value4"));
            eML.Add(new ExpressionMember("if "));
            eML.Add(new ExpressionMember("something "));
            eML.Add(new ExpressionMember("not "));
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("side "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SideValue, "value1"));
            eML.Add(new ExpressionMember("comes "));
            eML.Add(new ExpressionMember("closer "));
            eML.Add(new ExpressionMember("than "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value2", true));
            eML.Add(new ExpressionMember("\" "));
            ____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <MONSTER_STAY_CLOSE>

            eML = this.Add("<MONSTER_STAY_CLOSE>");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("pod "));
            eML.Add(new ExpressionMember("within "));
            eML.Add(new ExpressionMember("range "));
            eML.Add(new ExpressionMember("of "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadius, "value1"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value2"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("value3 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value3"));
            eML.Add(new ExpressionMember(", "));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("value4 "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueFQ, "value4"));
            eML.Add(new ExpressionMember("\" "));
            ____Add_Name(eML, ExpressionMemberValueDescriptions.NameMonster);

			#endregion

			#region <AI_TARGET_THROTTLE>

			eML = this.Add("<AI_TARGET_THROTTLE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("heading "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value1"));
			eML.Add(new ExpressionMember(" towards "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "targetName"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Bool_Do_Dont, "value2"));
			eML.Add(new ExpressionMember("treat "));
			eML.Add(new ExpressionMember("it "));
			eML.Add(new ExpressionMember("as "));
			eML.Add(new ExpressionMember("friendly\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);

			#endregion

			#region ATTACK

			eML = this.Add("<SHIP_ATTACK>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "targetName"));
			eML.Add(new ExpressionMember("moving "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Throttle, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <SHIP_DEFEND>

			eML = this.Add("<SHIP_DEFEND>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("ally "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
			eML.Add(new ExpressionMember(", "));
			eML.Add(new ExpressionMember("engaging "));
			eML.Add(new ExpressionMember("anyone "));
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("range "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value2"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <SHIP_LAUNCH_FIGHTERS>

			eML = this.Add("<SHIP_LAUNCH_FIGHTERS>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("if "));
			eML.Add(new ExpressionMember("player "));
			eML.Add(new ExpressionMember("ship "));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <SHIP_GUARD_STATION>

			eML = this.Add("<SHIP_GUARD_STATION>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("magic "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShip);

			#endregion

			#region <UNKNOWN_TYPE>

			eML = this.Add("<UNKNOWN_TYPE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("\" "));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAIShipOrMonster);
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN AI COMMAND NAME)"));

			#endregion
		}
	}
}
