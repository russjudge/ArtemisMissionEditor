using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtemisMissionEditor
{
	public enum MissionStatementType
	{
		Commentary,
		Statement,
		MAX
	}

	public enum MissionStatementKind
	{
		Condition,
		Action,
		Commentary,
		MAX
	}

	[Serializable]
	public sealed class MissionStatement
    {
		public string SourceXML { get; set; }

		/// <summary>
		/// Parent node of this statement
		/// </summary>
		public MissionNode Parent { get; set; }

		/// <summary> Xml name of this statement - like if_variable or create </summary>
		public string Name { get; set; }

		/// <summary> Body of the node - text for comment, or body for those elements that use it </summary>
		public string Body { get; set; }

		/// <summary> Text representation of this statement </summary>
		private string _text;
		/// <summary> Text representation of this statement for general public access </summary>
		public string Text { get { return _text; } }
		
		/// <summary> Label belonging to this statement requires update of text </summary>
		private bool _invalidatedLabel;
		/// <summary> Label belonging to this statement requires update of text </summary>
		public bool InvalidatedLabel { get { return _invalidatedLabel; } }

		/// <summary> Expression belonging to this statement requires update </summary>
		private bool _invalidatedExpression;
		/// <summary> Expression belonging to this statement requires update </summary>
		public bool InvalidatedExpression { get { return _invalidatedExpression; } }

		/// <summary> Statement requires some sort of update </summary>
		public bool Invalidated { get { return _invalidatedLabel || _invalidatedExpression; } }

		/// <summary> Type of this statement: Comment or Statement </summary>
		public MissionStatementType Type { get; set; }

        /// <summary> What kind of icon should this statement show in the treeview</summary>
        public int ImageIndex { get {  return IsGreen() ? ImageIndexOK : ImageIndexBad; } }

        private int ImageIndexOK { get { return 4 + (int)Kind * 2; } }
        private int ImageIndexBad { get { return 4 + (int)Kind * 2 + 1; } }

		/// <summary> If the item is all valid (all changeable expression members are green, not red) </summary>
		public bool IsGreen()
		{
			bool allGreen = true; 
			foreach (ExpressionMemberContainer item in Expression) 
				allGreen = allGreen && !item.IsInvalid;
			return allGreen;
		}

		/// <summary>Guess if this statement is a Condition or Action, based on its name</summary>
		private MissionStatementKind GuessKind
		{
			get
			{
				if (Name.Substring(0, 3) == "if_")
					return MissionStatementKind.Condition;
				else
					return MissionStatementKind.Action;
			}
		}

		/// <summary>
		/// Statement's kind - Commentary, Condition or Action, based on its position inside parent (or guessing by name if its not yet added to parent)
		/// </summary>
		public MissionStatementKind Kind
		{
			get
			{
				if (Type == MissionStatementType.Commentary)
					return MissionStatementKind.Commentary;
				if (!Parent.Conditions.Contains(this) && !Parent.Actions.Contains(this))
					return GuessKind;
				if (Parent.Conditions.Contains(this))
					return MissionStatementKind.Condition;
				else
					return MissionStatementKind.Action;
			}
		}

        public bool IsCreateStatement()
        {
            return IsCreateNamedStatement() || IsCreateNamelessStatement();
        }

        private string[] _listNamedTypes = new string[9] { "anomaly", "blackHole", "neutral", "enemy", "monster", "player", "station", "genericMesh", "whale" };
        private string[] _listNamelessTypes = new string[3] { "asteroids", "nebulas", "mines" };

        public bool IsCreateNamedStatement()
        {
			return (Name == "create") && _listNamedTypes.Contains(GetAttribute("type")) && GetAttribute("use_gm_position") == null;
        }

        public bool IsCreateNamelessStatement()
        {
			return (Name == "create") && _listNamelessTypes.Contains(GetAttribute("type")) && GetAttribute("use_gm_position") == null;
        }

        public bool IsSpaceMapEditableStatement()
        {
			return ((Name == "destroy_near") && GetAttribute("name") == null && GetAttribute("use_gm_position") == null)
				|| ((Name == "add_ai") && GetAttribute("type") == "POINT_THROTTLE")
				|| ((Name == "direct")  && GetAttribute("targetName") == null)
                || ((Name == "if_inside_box"))
                || ((Name == "if_inside_sphere"))
                || ((Name == "if_outside_box"))
                || ((Name == "if_outside_sphere"));
        }

		/// <summary> Attributes of this statement (contains single one named "text" in case of comment) </summary>
		private Dictionary<string, string> Attributes;

		public KeyValuePair<string, string>[] GetAttributes()
		{
			return Attributes.ToArray();
		}

		/// <summary> Gets the attribute value </summary>
		/// <param name="name">Name of the value, as in XML</param>
		/// <returns>Value if attribute exists, null if doesnt</returns>
		public string GetAttribute(string name)
		{
			if (name == "")
				throw new Exception("FAIL! Attribute cannot have a blank name!");

			//Try reading directly
			if (Attributes.ContainsKey(name))
				return Attributes[name];

			//Try finding by case-unsensitive
			string ci_name = null;
			foreach (string key in Attributes.Keys)
				if (key.ToLower() == name.ToLower())
					ci_name = key;

			//If found by case-unsensitive search - change key and then return
			if (ci_name != null)
			{
				Attributes.Add(name, Attributes[ci_name]);
				Attributes.Remove(ci_name);
				return Attributes[name];
			}

			//Try finding by another name
			string pseudonym = null;

			foreach (string[] arr in _linkedAttributes)
				if (arr.Contains(name))
					foreach (string item in arr)
						if (name != item && Attributes.ContainsKey(item))
							pseudonym = item;

			//If found by another name - change key and then return
			if (pseudonym != null)
			{
				Attributes.Add(name, Attributes[pseudonym]);
				Attributes.Remove(pseudonym);
				return Attributes[name];
			}

			//Tell them it doesnt exist
			return null;
		}
		/// <summary> 
		/// Gets the attribute value 
		/// </summary>
		/// <param name="name">Name of the value, as in XML</param>
		/// <param name="valueIfNull">Set value to this if its null</param>
		/// <returns>Value if attribute exists, null if doesnt</returns>
		public string GetAttribute(string name, string valueIfNull)
		{
			string result = GetAttribute(name);
			
			if (result != null || valueIfNull == null)
				return result;
			
			SetAttribute(name, valueIfNull);
			
			return valueIfNull;
		}

		/// <summary>
		/// Adds, sets or removes attribute, as nessecary
		/// </summary>
		/// <param name="name">Name of the attribute</param>
		/// <param name="value">If value=null, attribute will be removed, if present. If value!=null, attribute will be set, if present, or added, if not present</param>
		private void AttribuesAddSetRemove(string name, string value)
		{
			if (Attributes.ContainsKey(name))
			{
				if (value != null)
					Attributes[name] = value;
				else
					Attributes.Remove(name);
			}
			else
				if (value != null)
					Attributes[name] = value;
		}

		/// <summary> 
		/// Sets the attribute' value 
		/// </summary>
		/// <param name="name">Attribute name, as in XML</param>
		/// <param name="value">Attribute value, as should appear in XML</param>
		public void SetAttribute(string name, string value)
		{
			//First see if there is already a variable with a shared name and remove it
			foreach (string[] arr in _linkedAttributes)
				if (arr.Contains(name))
					foreach (string item in arr)
						if (name != item)
							AttribuesAddSetRemove(item, null);

			//Then set the attribute
			AttribuesAddSetRemove(name, value);

			//Then check for bound attributes
			if (value != null)
			{
				string low = null, high = null;
				foreach (string[] arr in _boundAttributes)
					if (arr[0] == name || arr[1] == name)
					{
						low = arr[0];
						high = arr[1];
					}

				if (low != null)
				{
					string lowSValue = GetAttribute(low);
					string highSValue = GetAttribute(high);
					double lowValue, highValue;
					if (Helper.DoubleTryParse(lowSValue == null ? null : lowSValue, out lowValue) && Helper.DoubleTryParse(highSValue == null ? null : highSValue, out highValue))
						if (lowValue > highValue)
						{
							AttribuesAddSetRemove(low, highSValue);
							AttribuesAddSetRemove(high, lowSValue);
						}
				}
			}
		}

		/// <summary> 
		/// Sets attribute value if it is null 
		/// </summary>
		/// <param name="name">Attribute name, as in XML</param>
		/// <param name="value">Attribute value, as should appear in XML</param>
		public void SetAttributeIfNull(string name, string value)
		{
			if (value == null)
				return;
			GetAttribute(name, value);
		}

		/// <summary> Contains arrays of attribute names that share the same value, like, x/startX/centerX/... </summary>
		private static string[][] _linkedAttributes;

		/// <summary> Contains arrays of attribute names that signify boundaries, like, randomIntLow/randomIntHigh, and thus the first one must be less than the second </summary>
		private static string[][] _boundAttributes;
		
		static MissionStatement()
		{
			string[] tmp;

			_linkedAttributes = new string[6][];
			
			tmp = new string[4] { "x", "startX", "centerX", "pointX" };
			_linkedAttributes[0] = tmp;
			
			tmp = new string[4] { "y", "startY", "centerY", "pointY" };
			_linkedAttributes[1] = tmp;
			
			tmp = new string[4] { "z", "startZ", "centerZ", "pointZ" };
			_linkedAttributes[2] = tmp;
			
			tmp = new string[2] { "use_gm_position", "use_gm_selection" };
			_linkedAttributes[3] = tmp;
			
			tmp = new string[2] { "name", "name1" };
			_linkedAttributes[4] = tmp;
			
			tmp = new string[2] { "name2", "targetName" };
			_linkedAttributes[5] = tmp;
			
			_boundAttributes = new string[4][];

			tmp = new string[2] { "randomIntLow", "randomIntHigh" };
			_boundAttributes[0] = tmp;

			tmp = new string[2] { "randomFloatLow", "randomFloatHigh" };
			_boundAttributes[1] = tmp;

			tmp = new string[2] { "leastX", "mostX" };
			_boundAttributes[2] = tmp;

			tmp = new string[2] { "leastZ", "mostZ" };
			_boundAttributes[3] = tmp;

		}

		/// <summary> The exression that currently represents the statement </summary>
		public List<ExpressionMemberContainer> Expression;

		public static MissionStatement NewFromXML(XmlNode item, MissionNode parent)
        {
            MissionStatement mn = new MissionStatement(parent);

			mn.FromXml(item);

			mn.Update();

            return mn;
        }

		/// <summary> Prevent nesting updates inside updates </summary>
		private bool _updating;

		/// <summary>
		/// Updates the item's string representation and expression
		/// </summary>
		public void Update()
		{
			if (_updating)
				return;
			_updating = true;
			
			bool expressionChanged;
			
			List<ExpressionMemberContainer> newExpr = Expressions.ConstructExpression(this);

			expressionChanged = newExpr.Count != Expression.Count;
			for (int i = 0; i < newExpr.Count && !expressionChanged; i++)
				expressionChanged = expressionChanged || newExpr[i].CheckValue != Expression[i].PreviousCheckValue;

			if (expressionChanged)
				Expression = newExpr;

			//TODO: Correctly update string representation <-- WTF does it mean?
			_text = "";

			foreach (ExpressionMemberContainer item in Expression)
			{
				_text += item.GetValueText();
			}
			
			_text = _text.Trim();

			if (true)				_invalidatedLabel = true;
			if (expressionChanged)	_invalidatedExpression = true;

			_updating = false;
		}

		public void ConfirmUpdate()
		{
			_invalidatedExpression = false;
			_invalidatedLabel = false;
		}

        //TO XML
		public XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
			switch (Type)
			{
				case MissionStatementType.Commentary:
					XmlComment cResult = xDoc.CreateComment((Body.Length == 0 || Body[Body.Length - 1] != '-' ? Body : Body + " ").Replace("--", "-"));
					return cResult;
				case MissionStatementType.Statement:
					XmlElement eResult = xDoc.CreateElement(Name);
					if (full || (Expression.Count >0 && Expression[Expression.Count-1].Member is ExpressionMember_Unknown))
						foreach (KeyValuePair<string, string> kvp in Attributes)
						{
							XmlAttribute cAtt = xDoc.CreateAttribute(kvp.Key);
							cAtt.Value = kvp.Value;
							eResult.Attributes.Append(cAtt);
						}
					else foreach (ExpressionMemberContainer container in Expression)
						{
							if (!container.Member.ValueDescription.IsSerialized)
								continue;
							XmlAttribute cAtt = xDoc.CreateAttribute(container.Member.Name);
							cAtt.Value = container.GetAttribute();
							if (container.Member.ValueDescription.BehaviorInXml == ExpressionMemberValueBehaviorInXml.AsIs || !String.IsNullOrEmpty(cAtt.Value))
								eResult.Attributes.Append(cAtt);
						}
					if (Body != "")
						eResult.InnerText = Body;
					return eResult;
			}
			return null; 
        }

        //FROM XML
        public void FromXml(XmlNode item)
        {
			Attributes.Clear();
			Name = "";
			Body = "";

			switch (item.GetType().ToString())
			{
				case "System.Xml.XmlComment":
					Type = MissionStatementType.Commentary;
					Name = "Commentary";
					Body = item.InnerText;
					break;
				case "System.Xml.XmlElement":
					Type = MissionStatementType.Statement;
					Name = item.Name;
					foreach (XmlAttribute att in item.Attributes)
						SetAttribute(att.Name, att.Value);
					if (item.InnerText != "")
						Body = item.InnerText;
					break;
			}

			SourceXML = string.IsNullOrEmpty(SourceXML) ? item.OuterXml : SourceXML;
        }

		public MissionStatement(MissionNode parent)
		{
			Parent = parent;
			Name = "";
			Body = "";
			Attributes = new Dictionary<string, string>();
			Expression = new List<ExpressionMemberContainer>();
			_invalidatedLabel = true;
			_invalidatedExpression = true;
			_updating = false;
		}


    }
}
