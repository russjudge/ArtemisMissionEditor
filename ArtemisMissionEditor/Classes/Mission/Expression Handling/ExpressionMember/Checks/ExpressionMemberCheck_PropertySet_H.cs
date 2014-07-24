using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	/// <summary>
	/// Add ai (type) check, is hidden since add ai statement has type as a member
	/// </summary>
	public sealed class ExpressionMemberCheck_PropertySet_H : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("property", ExpressionMemberValueDescription.GetItem("property").DefaultIfNull);

			switch (type)
			{
				//EVERYTHING
				case "positionX":				return "<FLT0...100K>";
				case "positionY": 				return "<FLT-100K...100K>";
				case "positionZ": 				return "<FLT0...100K>";
				case "deltaX": 					return "<FLT-100K...100K>";
				case "deltaY": 					return "<FLT-100K...100K>";
				case "deltaZ": 					return "<FLT-100K...100K>";
				case "angle": 					return "<DEFAULT>";
				case "pitch": 					return "<DEFAULT>";
				case "roll": 					return "<DEFAULT>";
                case "sideValue": return "<DEFAULT>";
				//VALUES FOR GENERIC MESHES		
				case "blocksShotFlag": 			return "<BOOLYESNO>";
				case "pushRadius": 				return "<FLT-+INF>";
				case "pitchDelta": 				return "<DEFAULT>";
				case "rollDelta": 				return "<DEFAULT>";
				case "angleDelta": 				return "<DEFAULT>";
				case "artScale": 				return "<DEFAULT>";
				//VALUES FOR STATIONS			
				case "shieldState": 			return "<FLT-+INF>";
				case "canBuild": 				return "<BOOLYESNO>";
				case "missileStoresHoming": 	return "<INT0...+INF>";
				case "missileStoresNuke": 		return "<INT0...+INF>";
				case "missileStoresMine": 		return "<INT0...+INF>";
				case "missileStoresECM": 		return "<INT0...+INF>";
				//VALUES FOR SHIELDED SHIPS		
				case "throttle": 				return "<DEFAULT>";
				case "steering": 				return "<DEFAULT>";
				case "topSpeed": 				return "<DEFAULT>";
				case "turnRate": 				return "<DEFAULT>";
				case "shieldStateFront": 		return "<INT-+INF>";
				case "shieldMaxStateFront": 	return "<INT-+INF>";
				case "shieldStateBack": 		return "<INT-+INF>";
				case "shieldMaxStateBack": 		return "<INT-+INF>";
				case "shieldsOn": 				return "<BOOLYESNO>";
				case "triggersMines": 			return "<BOOLYESNO>";
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
				case "targetPointX": 			return "<FLT0...100K>";
				case "targetPointY": 			return "<FLT-100K...100K>";
				case "targetPointZ": 			return "<FLT0...100K>";
				case "hasSurrendered": 			return "<BOOLYESNO>";
				case "eliteAIType": 			return "<ELITEAITYPE>";
				case "eliteAbilityBits": 		return "<ELITEABILITYBITS>";
				case "eliteAbilityState": 		return "<DEFAULT>";
				case "surrenderChance":			return "<INT0...100>";
				//VALUES FOR NEUTRALS			
				case "exitPointX": 				return "<FLT0...100K>";
				case "exitPointY": 				return "<FLT-100K...100K>";
				case "exitPointZ": 				return "<FLT0...100K>";
				//VALUES FOR PLAYERS			
				case "countHoming": 			return "<INT0...+INF>";
				case "countNuke": 				return "<INT0...+INF>";
				case "countMine": 				return "<INT0...+INF>";
				case "countECM": 				return "<INT0...+INF>";
				case "energy": 					return "<INT0...+INF>";
				case "warpState":				return "<INT0...4>";
				case "currentRealSpeed":		return "<READ_ONLY>";
                case "totalCoolant":            return "<INT0...+INF>";
				//DEFAULT CASE
				default:
					return "<UNKNOWN_PROPERTY>";
			}
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
            if (value == "<BOOLYESNO>")
            {
                string flag = container.GetAttribute("value");
                if (flag == null || !Helper.IntTryParse(flag))
                    container.SetAttribute("value","0");
				else if (Helper.StringToInt(flag) == 1)
					container.SetAttribute("value", "1");
				else
					container.SetAttribute("value", "0");
            }
            //if (value == "<INVALID_PROPERTY>")
			//{
			//    value = "<DEFAULT>";
			//    container.SetAttribute("property", "positionX");
			//}
			if (Mission.Current.Loading && value == "<READ_ONLY>")
				Log.Add("Warning! Attempt to set read-only property " + container.GetAttribute("property") + " detected in event: " + container.ParentStatement.Parent.Name + "!");
			if (Mission.Current.Loading && value == "<UNKNOWN_PROPERTY>")
				Log.Add("Warning! Unknown property " + container.GetAttribute("property") + " detected in event: "+container.ParentStatement.Parent.Name+"!");

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
			eML.Add(new ExpressionMember("<property>", "property", "property"));
		}

		/// <summary>
		/// Adds "
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", "name_all", "name"));
			//eML.Add(new ExpressionMemberCheck_Name_x_GMSel());
		}



		public ExpressionMemberCheck_PropertySet_H()
			: base()
		{
			List<ExpressionMember> eML;

            #region <INT-+INF>    (push radius)

            eML = this.Add("<INT-+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "int-+inf", "value"));
            ____Add_Name(eML);

            #endregion

            #region <FLT-+INF>    (push radius)

            eML = this.Add("<FLT-+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "flt-+inf", "value"));
            ____Add_Name(eML);

            #endregion

			#region <INT0...+INF>    (amount of missiles)

			eML = this.Add("<INT0...+INF>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", "int0...+inf", "value"));
			____Add_Name(eML);

			#endregion

			#region <FLT0...+INF>    (coordinate x-z)

			eML = this.Add("<FLT0...+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "flt0...+inf", "value"));
            ____Add_Name(eML);

            #endregion

            #region <INT0...100>    (surrenderChance)

            eML = this.Add("<INT0...100>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "int0...100", "value"));
            ____Add_Name(eML);

            #endregion

			#region <INT0...4>    (warpState)

            eML = this.Add("<INT0...4>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "int0...4", "value"));
            ____Add_Name(eML);

            #endregion
			
			#region <FLT0...100K>    (coordinate x-z)

			eML = this.Add("<FLT0...100K>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "flt0...100k", "value"));
            ____Add_Name(eML);

            #endregion

            #region <FLT-100K...100K>    (coordinate y, delta)

			eML = this.Add("<FLT-100K...100K>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", "flt-100k...100k", "value"));
            ____Add_Name(eML);

            #endregion

            #region <BOOLYESNO>    (coordinate y, delta)

            eML = this.Add("<BOOLYESNO>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "boolyesno", "value"));
            ____Add_Name(eML);

            #endregion

            #region <ELITEAITYPE> (eliteAIType)

            eML = this.Add("<ELITEAITYPE>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "eliteaitype", "value"));
            ____Add_Name(eML);

            #endregion

            #region <ELITEABILITYBITS> (eliteAbilityBits)

            eML = this.Add("<ELITEABILITYBITS>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "eliteabilitybits", "value"));
            ____Add_Name(eML);

            #endregion

            #region <DEFAULT>    (...)

			eML = this.Add("<DEFAULT>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", "value_f", "value"));
			____Add_Name(eML);

			#endregion

			#region <READ_ONLY>

			eML = this.Add("<READ_ONLY>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", "value_f", "value"));
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS READ ONLY)"));

			#endregion
			
			#region <UNKNOWN_PROPERTY>  

			eML = this.Add("<UNKNOWN_PROPERTY>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", "value_f", "value"));
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));

			#endregion
		}
	}
}