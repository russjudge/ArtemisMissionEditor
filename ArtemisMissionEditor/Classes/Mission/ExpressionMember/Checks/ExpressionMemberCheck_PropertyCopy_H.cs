﻿using System;
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
	/// Add ai (type) check, is hidden since add ai statement has type as a member
	/// </summary>
	public sealed class ExpressionMemberCheck_PropertyCopy_H : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("property", EMVD.GetItem("property").DefaultIfNull);

			switch (type)
			{
				//EVERYTHING
				case "positionX":				return "<DEFAULT>";
				case "positionY": 				return "<DEFAULT>";
				case "positionZ": 				return "<DEFAULT>";
				case "deltaX": 					return "<DEFAULT>";
				case "deltaY": 					return "<DEFAULT>";
				case "deltaZ": 					return "<DEFAULT>";
				case "angle": 					return "<DEFAULT>";
				case "pitch": 					return "<DEFAULT>";
				case "roll": 					return "<DEFAULT>";
                case "sideValue": return "<DEFAULT>";
				//VALUES FOR GENERIC MESHES		
				case "blocksShotFlag": 			return "<DEFAULT>";
				case "pushRadius": 				return "<DEFAULT>";
				case "pitchDelta": 				return "<DEFAULT>";
				case "rollDelta": 				return "<DEFAULT>";
				case "angleDelta": 				return "<DEFAULT>";
				case "artScale": 				return "<DEFAULT>";
				//VALUES FOR STATIONS			
				case "shieldState": 			return "<DEFAULT>";
				case "canBuild": 				return "<DEFAULT>";
				case "missileStoresHoming": 	return "<DEFAULT>";
				case "missileStoresNuke": 		return "<DEFAULT>";
				case "missileStoresMine": 		return "<DEFAULT>";
				case "missileStoresECM": 		return "<DEFAULT>";
				//VALUES FOR SHIELDED SHIPS		
				case "throttle": 				return "<DEFAULT>";
				case "steering": 				return "<DEFAULT>";
				case "topSpeed": 				return "<DEFAULT>";
				case "turnRate": 				return "<DEFAULT>";
				case "shieldStateFront": 		return "<DEFAULT>";
				case "shieldMaxStateFront": 	return "<DEFAULT>";
				case "shieldStateBack": 		return "<DEFAULT>";
				case "shieldMaxStateBack": 		return "<DEFAULT>";
				case "shieldsOn": 				return "<DEFAULT>";
				case "triggersMines": 			return "<DEFAULT>";
				case "systemDamageBeam": 		return "<DEFAULT>";
				case "systemDamageTorpedo": 	return "<DEFAULT>";
				case "systemDamageTactical": 	return "<DEFAULT>";
				case "systemDamageTurning": 	return "<DEFAULT>";
				case "systemDamageImpulse": 	return "<DEFAULT>";
				case "systemDamageWarp": 		return "<DEFAULT>";
				case "systemDamageFrontShield":	return "<DEFAULT>";
				case "systemDamageBackShield": 	return "<DEFAULT>";
				case "shieldBandStrength0": 	return "<DEFAULT>";
				case "shieldBandStrength1": 	return "<DEFAULT>";
				case "shieldBandStrength2": 	return "<DEFAULT>";
				case "shieldBandStrength3": 	return "<DEFAULT>";
				case "shieldBandStrength4": 	return "<DEFAULT>";
				//VALUES FOR ENEMIES			
				case "targetPointX": 			return "<DEFAULT>";
				case "targetPointY": 			return "<DEFAULT>";
				case "targetPointZ": 			return "<DEFAULT>";
				case "hasSurrendered": 			return "<DEFAULT>";
				case "eliteAIType": 			return "<DEFAULT>";
				case "eliteAbilityBits": 		return "<DEFAULT>";
				case "eliteAbilityState": 		return "<DEFAULT>";
				case "surrenderChance":			return "<DEFAULT>";
				//VALUES FOR NEUTRALS			
				case "exitPointX": 				return "<DEFAULT>";
				case "exitPointY": 				return "<DEFAULT>";
				case "exitPointZ": 				return "<DEFAULT>";
				//VALUES FOR PLAYERS			
				case "countHoming": 			return "<DEFAULT>";
				case "countNuke": 				return "<DEFAULT>";
				case "countMine": 				return "<DEFAULT>";
				case "countECM": 				return "<DEFAULT>";
				case "energy": 					return "<DEFAULT>";
				case "warpState":				return "<DEFAULT>";
				case "currentRealSpeed":		return "<READ_ONLY>";
				//DEFAULT CASE
				default:
					return "<UNKNOWN_PROPERTY>";
			}
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			//if (value == "<INVALID_PROPERTY>")
			//{
			//    value = "<DEFAULT>";
			//    container.SetAttribute("property", "positionX");
			//}
			if (Mission.Current.Loading && value == "<READ_ONLY>")
				Log.Add("Warning! Attempt to copy read-only property " + container.GetAttribute("property") + " detected in event: " + container.ParentStatement.Parent.Name + "!");
			if (Mission.Current.Loading && value == "<UNKNOWN_PROPERTY>")
				Log.Add("Warning! Unknown property " + container.GetAttribute("property") + " detected in event: " + container.ParentStatement.Parent.Name + "!");

			base.SetValueInternal(container, value);
		}

		//It starts with
		//       "Create "
		/////////////////////////////////////////////////////

		/// <summary>
		/// Adds "(type) "
		/// </summary>
		private void ____Add_Property(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<property>", EMVD.GetItem("property"), "property"));
		}

		/// <summary>
		/// Adds "
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_all"), "name1"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_all"), "name2"));
			//eML.Add(new ExpressionMemberCheck_Name_x_GMSel());
		}

		public ExpressionMemberCheck_PropertyCopy_H()
			: base()
		{
			List<ExpressionMember> eML;

			#region <DEFAULT>    (...)

			eML = this.Add("<DEFAULT>");
			____Add_Property(eML);
			____Add_Name(eML);

			#endregion

			#region <READ_ONLY>

			eML = this.Add("<READ_ONLY>");
			____Add_Property(eML);
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS READ ONLY!)"));

			#endregion

			#region <UNKNOWN_PROPERTY>

			eML = this.Add("<UNKNOWN_PROPERTY>");
			____Add_Property(eML);
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));

			#endregion
		}
	}
}