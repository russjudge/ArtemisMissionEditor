using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	/// <summary>
	/// Create (type) check, is hidden since create statement has type as a member
	/// </summary>
	public sealed class ExpressionMemberCheck_CreateType_H : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("type");

			switch (type)
			{
				case "anomaly":		return "<NAMED_MAP_OBJECT>";
				case "blackHole":	return "<NAMED_MAP_OBJECT>";
				case "player":		return "player";
				case "whale":		return "whale";
				case "monster":		return "monster";
				case "neutral":		return "neutral";
				case "station":		return "station";
				case "enemy":		return "enemy";
				case "genericMesh":	return "genericMesh";
				case "nebulas":		return "<NAMELESS_MAP_OBJECT>";
				case "asteroids":	return "<NAMELESS_MAP_OBJECT>";
				case "mines":		return "<NAMELESS_MAP_OBJECT>";
				case null:			return "<NULL>";
				default:
					return "<INVALID_TYPE>";//This must be further converted in SetValue to some valid one, and type must be set there as well
			}
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == "<NULL>")
			{
				container.SetAttribute("type", "enemy");
				value = "enemy";
			}
			if (value == "<INVALID_TYPE>")
			{
				Log.Add("Warning! Unknown create type " + container.GetAttribute("type") + " detected in event: " + container.ParentStatement.Parent.Name + "!");
			}

			base.SetValueInternal(container, value);
		}

		//It starts with
		//       "Create "
		/////////////////////////////////////////////////////

		/// <summary>
		/// Adds "(type) "
		/// </summary>
		private void ____Add_Type(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<type>", EMVD.GetItem("type"), "type"));
		}

		/// <summary>
		/// Adds "at point (x, y, z) " or " GM position "
		/// </summary>
		private void ____Add_Point_x_GMPos(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMemberCheck_Point_x_GMPos());
		}

		/// <summary>
		/// Adds "bearing (angle) degrees "
		/// </summary>
		private void ____Add_Angle(List<ExpressionMember> eML, bool nameMandatory = false)
		{
			eML.Add(new ExpressionMember("bearing "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("angle"), "angle"));
			eML.Add(new ExpressionMember("degrees "));
		}

		/// <summary>
		/// Adds "with name (name) "
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML, bool nameMandatory = false, string name = "name")
		{
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem(name), "name", nameMandatory));
		}
      
		/// <summary>
		/// Adds "and hull ID (hullID) " or "and race/hull keys ("raceKeys") ("hullKeys") "
		/// </summary>
		private void ____Add_HullID_x_HullRaceKeys(List<ExpressionMember> eML, bool keysMandatory = true)
		{
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMemberCheck_HullID_x_HullRaceKeys(keysMandatory));
		}

		public ExpressionMemberCheck_CreateType_H()
			: base()
		{
			List<ExpressionMember> eML;

			#region <NAMED_MAP_OBJECT>		(Anomaly / Black Hole)

			eML = this.Add("<NAMED_MAP_OBJECT>");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Name(eML);

			#endregion

			#region <NAMED_MAP_OBJECT_AN>		(Monster / Player / Whale)

			eML = this.Add("monster");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML);

			eML = this.Add("player");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML, false, "name_createplayer");
            eML.Add(new ExpressionMemberCheck_Side());

			eML = this.Add("whale");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("pod "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("podnumber"), "podnumber"));

			#endregion

			#region neutral				(Neutral)

			eML = this.Add("neutral");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			____Add_HullID_x_HullRaceKeys(eML);
            eML.Add(new ExpressionMemberCheck_Side());

			#endregion

			#region station				(Station)

			eML = this.Add("station");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML,true);
			____Add_HullID_x_HullRaceKeys(eML, false);
            eML.Add(new ExpressionMemberCheck_Side());

			#endregion

			#region enemy				(Enemy)

			eML = this.Add("enemy");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			____Add_HullID_x_HullRaceKeys(eML);
            eML.Add(new ExpressionMemberCheck_Side());
            eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("fleet "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("fleetnumber"), "fleetnumber"));
            
			#endregion

			#region genericMesh			(Generic Mesh)

			eML = this.Add("genericMesh");
			____Add_Type(eML);
			____Add_Point_x_GMPos(eML);
			____Add_Angle(eML);
			____Add_Name(eML);
			eML.Add(new ExpressionMember("using "	));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMember("mesh "	));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("meshFileName"), "meshFileName", true));
			eML.Add(new ExpressionMember("with "	));
			eML.Add(new ExpressionMember("texture "	));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("textureFileName"), "textureFileName", true));
			eML.Add(new ExpressionMember("having "	));
			eML.Add(new ExpressionMember("hull "	));
			eML.Add(new ExpressionMember("race "	));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("hullRace"), "hullRace"));
			eML.Add(new ExpressionMember("and "		));
			eML.Add(new ExpressionMember("type "	));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("hullType"), "hullType"));
			eML.Add(new ExpressionMember("having "	));
			eML.Add(new ExpressionMemberCheck_FakeShields());
			eML.Add(new ExpressionMember("and "		));
			eML.Add(new ExpressionMember("color "	));
			eML.Add(new ExpressionMember("RGB "		));
			eML.Add(new ExpressionMember("of "		));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("colorR"), "colorRed"));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("colorG"), "colorGreen"));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("colorB"), "colorBlue"));

			#endregion
           
			#region <NAMELESS_MAP_OBJECT>		(Nebulas / Asteroids / Mines)

			eML = this.Add("<NAMELESS_MAP_OBJECT>");
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("count"), "count"));
			____Add_Type(eML);
			eML.Add(new ExpressionMember("on "		));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMemberCheck_Line_x_Circle());
			eML.Add(new ExpressionMember("spread "	));
			eML.Add(new ExpressionMember("randomly ")); 
			eML.Add(new ExpressionMember("in "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("range "	));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("randomRange"), "randomRange"));
			eML.Add(new ExpressionMember("with "		));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("random "	));
			eML.Add(new ExpressionMember("seed "	));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>",		EMVD.GetItem("randomSeed"), "randomSeed"));

			eML = this.Add("<INVALID_TYPE>");
			____Add_Type(eML);
			eML.Add(new ExpressionMember("(WARNING! INVALID CREATE TYPE DETECTED)"));

			#endregion

		}
	}
}