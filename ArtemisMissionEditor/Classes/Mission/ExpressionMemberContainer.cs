using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;

namespace ArtemisMissionEditor
{
	[Serializable]
	public sealed class ExpressionMemberContainer
	{
		public ExpressionMember Member { get; set; }

		public MissionStatement Statement { get; set; }

		public string PreviousCheckValue { get; set; }
		private string _checkValue;
		public string CheckValue { get { return _checkValue; } set { PreviousCheckValue = _checkValue; _checkValue = value; } }

        /// <summary>
        /// Wether this member is not filled correctly
        /// </summary>
        public bool IsInvalid
        {
            get
            {
				if (!Member.ValueDescription.IsSerialized || !Member.ValueDescription.IsDisplayed)
					return false;
				return  !Helper.Validate(GetValue(), Member.ValueDescription) || Member.Mandatory && String.IsNullOrEmpty(GetValue());
            }
        }

		public string Decide()													{ Member.SetValue(this, Member.Decide(this)); return CheckValue; }
		public string GetValue()												{ return Member.GetValue(this); }
		public string GetValueDisplay()											{ return Member.GetValueDisplay(this); }
		public string GetValueText()											{ return Member.GetValueText(this); }
		public void SetValue(string value)										{ Member.SetValue(this, value);	}
		public string ValueToXml(string value)									{ return Member.ValueToXml(value);}
		public string ValueToDisplay(string value)								{ return Member.ValueToDisplay(value); }

		public void SetAttribute(string value)									{ Statement.SetAttribute(Member.Name, value); }
		public void SetAttribute(string name, string value)						{ Statement.SetAttribute(name, value); }
		public void SetAttributeIfNull(string value)							{ Statement.SetAttributeIfNull(Member.Name, value); }
		public void SetAttributeIfNull(string name, string value)				{ Statement.SetAttributeIfNull(name, value); }
		public string GetAttribute()											{ return Statement.GetAttribute(Member.Name, Member.ValueDescription.DefaultIfNull); }
		public string GetAttribute(string name)									{ return Statement.GetAttribute(name); }
		public string GetAttribute(string name, string valueIfNull)				{ return Statement.GetAttribute(name, valueIfNull); }
		//public string GetAttribute(object useMe, string valueIfNull)			{ return Statement.GetAttribute(Member.Name, valueIfNull); }

		public void OnClick(NormalLabel l, Point curPos, EditorActivationMode mode) { Member.OnClick(this, l, curPos, mode); }
		
		public ExpressionMemberContainer(ExpressionMember member, MissionStatement statement)
		{
			Member = member;
			Statement = statement;
		}
	}
}