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
	/// This check is for "type" from [create], it is hidden since create statement has type as a member
	/// </summary>
	public sealed class ExpressionMemberCheck_CreateType : ExpressionMemberCheck
	{
		/// <summary>
		/// This function is called when check needs to decide which list of ExpressionMembers to output.
		/// After it is called, SetValue will be called, to allow for error correction.
		/// </summary>
		/// <example>If input is wrong, Decide will choose something, and then the input will be corrected in the SetValue function</example>
		public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("type");
			if (type != null)
			{
				type = type.ToLower();
			}

			string angle = container.GetAttribute("angle");

			switch (type)
			{
				case "anomaly": if (container.GetAttribute("pickupType") == "8")
						return "Beacon";
					else
						return "Anomaly";
				case "blackhole": return "<NAMED_MAP_OBJECT>";
				case "player": return "player";
				case "whale": return "whale";
				case "monster": if (container.GetAttribute("monsterType") == "1" ||
									container.GetAttribute("monsterType") == "4")
						return "PodMonster";
					else
						return "monster";
				case "neutral": return "neutral";
				case "station": if (angle == null)
						return "station";
					else
						return "StationWithDeprecatedAngle";
				case "enemy": return "enemy";
				case "genericmesh": if (angle == null)
						return "genericMesh";
					else
						return "GenericMeshWithDeprecatedAngle";
				case "nebulas": return "nebulas";
				case "asteroids": return "<NAMELESS_MAP_OBJECT>";
				case "mines": return "<NAMELESS_MAP_OBJECT>";
				case "monstertype": return "Classic";
				case "anomalytype": return "Energy";
				case null: return "<NULL>";
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
			if (value == "<NULL>")
			{
				container.SetAttribute("type", "enemy");
				value = "enemy";
			}
			if (value == "<INVALID_TYPE>")
			{
				Log.Add("Warning! Unknown create type " + container.GetAttribute("type") + " detected in event: " + container.Statement.Parent.Name + "!");
			}

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Adds "(type) "
		/// </summary>
		private void ____Add_Type(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<type>", ExpressionMemberValueDescriptions.Type, "type"));
		}

		/// <summary>
		/// Adds "at point (x, y, z) " or " GM position "
		/// </summary>
		private void ____Add_Check_Point_GM(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMemberCheck_Point_GM());
		}

		/// <summary>
		/// Adds "bearing (angle) degrees "
		/// </summary>

		private void ____Add_Monster(List<ExpressionMember> eML, bool nameMandatory = true)
		{
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("type "));
			eML.Add(new ExpressionMember("<SELECT>", ExpressionMemberValueDescriptions.MonsterType, "monsterType"));
			eML.Add(new ExpressionMember(" "));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("age "));
			eML.Add(new ExpressionMember("<SELECT>", ExpressionMemberValueDescriptions.MonsterAge, "age"));
		}

		private void ____Add_Angle(List<ExpressionMember> eML, bool nameMandatory = false)
		{
			eML.Add(new ExpressionMember("bearing "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Angle, "angle"));
			eML.Add(new ExpressionMember("degrees "));
		}

		private void ____Add_AngleDeprecated(List<ExpressionMember> eML, bool nameMandatory = false)
		{
			eML.Add(new ExpressionMember("bearing "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.AngleDeprecated, "angle"));
			eML.Add(new ExpressionMember("degrees "));
		}

		/// <summary>
		/// Adds "with name (name) "
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML, bool nameMandatory = false, ExpressionMemberValueDescription name = null)
		{
			name = name ?? ExpressionMemberValueDescriptions.Name;

			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", name, "name", nameMandatory));
		}

		/// <summary>
		/// Adds "and hull ID (hullID) " or "and race/hull keys ("raceKeys") ("hullKeys") "
		/// </summary>
		private void ____Add_Check_HullID_HullRaceKeys(List<ExpressionMember> eML, bool keysMandatory = true)
		{
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMemberCheck_HullID_HullRaceKeys(keysMandatory));
		}
		private void ____Add_Check_HullID_HullRaceKeysP(List<ExpressionMember> eML, bool keysMandatory = false)
		{
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMemberCheck_HullID_HullRaceKeys(keysMandatory));
		}

		private void ____Add_GenericMeshAttributes(List<ExpressionMember> eML)
		{
			____Add_Type(eML);
			____Add_Check_Point_GM(eML);
			____Add_AngleDeprecated(eML);
			____Add_Name(eML);
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("mesh "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MeshFileName, "meshFileName", true));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("texture "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.TextureFileName, "textureFileName", true));
			eML.Add(new ExpressionMember("having "));
			eML.Add(new ExpressionMember("hull "));
			eML.Add(new ExpressionMember("race "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HullRace, "hullRace"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("type "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HullType, "hullType"));
			eML.Add(new ExpressionMember("having "));
			eML.Add(new ExpressionMemberCheck_FakeShields());
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("color "));
			eML.Add(new ExpressionMember("RGB "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ColorR, "colorRed"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ColorG, "colorGreen"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ColorB, "colorBlue"));
		}

		private void ____Add_StationAttributes(List<ExpressionMember> eML)
		{
			____Add_Type(eML);
			____Add_Check_Point_GM(eML);
			____Add_AngleDeprecated(eML);
			____Add_Name(eML, true);
			____Add_Check_HullID_HullRaceKeys(eML);
			eML.Add(new ExpressionMember("on "));
			eML.Add(new ExpressionMemberCheck_Side());
		}

		/// <summary>
		/// Represents a single member in an expression, which provides branching via checking a condition.
		/// This check is for "type" from [create], it is hidden since create statement has type as a member
		/// </summary>
		public ExpressionMemberCheck_CreateType()
			: base()
		{
			List<ExpressionMember> eML;

			#region <NAMED_MAP_OBJECT>		(Anomaly / Black Hole)

			eML = this.Add("<NAMED_MAP_OBJECT>");
			____Add_Type(eML);
            ____Add_Check_Point_GM(eML);
			____Add_Name(eML);

            #endregion

            eML = this.Add("Anomaly");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("of "));
            eML.Add(new ExpressionMember("type "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.AnomalyType, "pickupType"));
            eML.Add(new ExpressionMember(" "));
            ____Add_Check_Point_GM(eML);
            ____Add_Name(eML);

            eML = this.Add("Beacon");
            ____Add_Type(eML);
            eML.Add(new ExpressionMember("of "));
            eML.Add(new ExpressionMember("type "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.AnomalyType, "pickupType"));
            eML.Add(new ExpressionMember(" "));
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("effect "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.BeaconEffect, "beaconEffect"));
            eML.Add(new ExpressionMember("<SELECT>", ExpressionMemberValueDescriptions.MonsterType, "beaconMonsterType"));
            eML.Add(new ExpressionMember(" "));
            ____Add_Check_Point_GM(eML);
            ____Add_Name(eML);

            #region <NAMED_MAP_OBJECT_AN>		(Monster / Player / Whale)

            #region Monster
            eML = this.Add("monster");
			____Add_Type(eML);
            ____Add_Monster(eML);
            ____Add_Check_Point_GM(eML);
			____Add_Angle(eML);
			____Add_Name(eML);

            eML = this.Add("PodMonster");
			____Add_Type(eML);
            ____Add_Monster(eML);
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("pod "));
            eML.Add(new ExpressionMember("number "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.PodNumber, "podnumber"));
            ____Add_Check_Point_GM(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
            #endregion Monster

            eML = this.Add("player");
			____Add_Type(eML);
            eML.Add(new ExpressionMember("in slot "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.player_slot, "player_slot"));
            ____Add_Check_Point_GM(eML);
			____Add_Angle(eML);
			____Add_Name(eML, false, ExpressionMemberValueDescriptions.NamePlayer);
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMemberCheck_Side());
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("accent color "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.accent_color, "accent_color"));
            ____Add_Check_HullID_HullRaceKeysP(eML);
            eML.Add(new ExpressionMember(". Ship uses "));
            eML.Add(new ExpressionMemberCheck_DriveType());
            eML.Add(new ExpressionMember("drive."));

            eML = this.Add("whale");
            ____Add_Type(eML);
            ____Add_Check_Point_GM(eML);
            ____Add_Angle(eML);
            ____Add_Name(eML);
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("pod "));
            eML.Add(new ExpressionMember("number "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.PodNumber, "podnumber"));
            eML.Add(new ExpressionMember(" THIS STILL WORKS BUT HAS BEEN DEPRECATED AS OF 2.2.0 USE MONSTER"));

            #endregion

            #region neutral				(Neutral)

            eML = this.Add("neutral");
			____Add_Type(eML);
			____Add_Check_Point_GM(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			____Add_Check_HullID_HullRaceKeys(eML);
			eML.Add(new ExpressionMember("on "));
			eML.Add(new ExpressionMemberCheck_Side());
			eML.Add(new ExpressionMember("with "    ));
			eML.Add(new ExpressionMember("fleet "   ));
			eML.Add(new ExpressionMember("number "  ));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FleetNumberOrNone, "fleetnumber"));

			#endregion

			#region station				(Station)

			eML = this.Add("station");
			____Add_StationAttributes(eML);

			eML = this.Add("StationWithDeprecatedAngle");
			____Add_StationAttributes(eML);
			eML.Add(new ExpressionMember("(THE ANGLE ATTRIBUTE IS OBSOLETE) "));
			// We require the user to click to convert since we can't cover cases where
			// the value is an expression with a variable.
			eML.Add(new ExpressionMemberCheck_AngleConversion());

			#endregion

			#region enemy				(Enemy)

			eML = this.Add("enemy");
			____Add_Type(eML);
			____Add_Check_Point_GM(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			____Add_Check_HullID_HullRaceKeys(eML);
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMemberCheck_Side());
            eML.Add(new ExpressionMember("with "    ));
			eML.Add(new ExpressionMember("fleet "   ));
			eML.Add(new ExpressionMember("number "  ));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FleetNumberOrNone, "fleetnumber"));

			#endregion

			#region genericMesh			(Generic Mesh)

			eML = this.Add("genericMesh");
			____Add_GenericMeshAttributes(eML);

			eML = this.Add("GenericMeshWithDeprecatedAngle");
			____Add_GenericMeshAttributes(eML);
			eML.Add(new ExpressionMember("(THE ANGLE ATTRIBUTE IS OBSOLETE) "));
			// We require the user to click to convert since we can't cover cases where
			// the value is an expression with a variable.
			eML.Add(new ExpressionMemberCheck_AngleConversion());

			#endregion

			#region <NAMELESS_MAP_OBJECT>		(Asteroids / Mines)

			eML = this.Add("<NAMELESS_MAP_OBJECT>");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Count, "count"));
			____Add_Type(eML);
			eML.Add(new ExpressionMember("on "		));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMemberCheck_Line_Circle());
			eML.Add(new ExpressionMember("spread "	));
			eML.Add(new ExpressionMember("randomly "));
			eML.Add(new ExpressionMember("in "      ));
			eML.Add(new ExpressionMember("the "     ));
			eML.Add(new ExpressionMember("range "	));
			eML.Add(new ExpressionMember("of "      ));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.RandomRange, "randomRange"));
			eML.Add(new ExpressionMember("with "	));
			eML.Add(new ExpressionMember("the "     ));
			eML.Add(new ExpressionMember("random "	));
			eML.Add(new ExpressionMember("seed "	));
			eML.Add(new ExpressionMember("of "      ));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.RandomSeed, "randomSeed"));

			#endregion

			#region nebulas                     (Nebulas)

			eML = this.Add("nebulas");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Count, "count"));
			____Add_Type(eML);
			eML.Add(new ExpressionMember("of "	));
			eML.Add(new ExpressionMember("type "	));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.NebulaType, "nebType"));
			eML.Add(new ExpressionMember("on "		));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMemberCheck_Line_Circle());
			eML.Add(new ExpressionMember("spread "	));
			eML.Add(new ExpressionMember("randomly "));
			eML.Add(new ExpressionMember("in "      ));
			eML.Add(new ExpressionMember("the "     ));
			eML.Add(new ExpressionMember("range "	));
			eML.Add(new ExpressionMember("of "      ));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.RandomRange, "randomRange"));
			eML.Add(new ExpressionMember("with "	));
			eML.Add(new ExpressionMember("the "     ));
			eML.Add(new ExpressionMember("random "	));
			eML.Add(new ExpressionMember("seed "	));
			eML.Add(new ExpressionMember("of "      ));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.RandomSeed, "randomSeed"));

			#endregion

			#region <INVALID_TYPE>

			eML = this.Add("<INVALID_TYPE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("(WARNING! INVALID CREATE TYPE DETECTED)"));

			#endregion

		}
	}
}
