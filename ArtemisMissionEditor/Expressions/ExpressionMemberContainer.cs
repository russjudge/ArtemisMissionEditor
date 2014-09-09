using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;

namespace ArtemisMissionEditor.Expressions
{
	/// <summary>
	/// Links together MissionSatement with an ExpressionMember. Also contains check information for checks.
	/// </summary>
    [Serializable]
	public sealed class ExpressionMemberContainer
	{
		/// <summary>
		/// ExpressionMember linked by this container.
		/// </summary>
        public ExpressionMember Member { get; private set; }
        
        /// <summary>
        /// MissionStatement linked by this container.
        /// </summary>
        public MissionStatement Statement { get; private set; }

        /// <summary>
        /// Remembers previous CheckValue of this Check, required in order to see if the expression has changed.
        /// </summary>
        public string PreviousCheckValue { get; private set; }
        
        /// <summary>
        /// CheckValue of this check, which is what the Decide() method has decided.
        /// </summary>
        public string CheckValue { get { return _checkValue; } set { PreviousCheckValue = _checkValue; _checkValue = value; } }
        private string _checkValue;
		
        /// <summary>
        /// Wether this member of the statement is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
				if (!Member.ValueDescription.IsSerialized || !Member.ValueDescription.IsDisplayed)
					return true;
                return Member.ValueDescription.Validate(GetValue()).Valid && !(Member.Mandatory && String.IsNullOrEmpty(GetValue()));
            }
        }

        /*
         * Following functions call respective ExpressionMember functions and pass this Container.
         * This is done to invoke logic provided by ExpressionMember class on the data located in Container or MissionStatement class.
         * Maybe there is a better pattern to be used here but I was not aware of it at the time of writing the editor.
         */

        /// <summary>  
        /// Get value of current member (internal value, as is in XML)  
        /// </summary>
        public string GetValue()                                                
        { return Member.GetValue(this); }

        /// <summary> 
        /// Get value of current member to display in the label (meaning, exactly what will appear in GUI labels) 
        /// </summary>
        public string GetValueDisplay()											
        { return Member.GetValueDisplay(this); }

        /// <summary>
        /// Get value of current member to display in the text string representing the statement
        /// </summary>
		public string GetValueText()											
        { return Member.GetValueText(this); }

        /// <summary>
        /// Sets value of the current member.
        /// </summary>
		public void SetValue(string value)										
        { Member.SetValue(this, value);	}
        
        /// <summary>
        /// Call routine to be done when label control assigned to this member is clicked.
        /// </summary>
        public void OnClick(NormalLabel l, Point curPos, ExpressionMemberValueEditorActivationMode mode) 
        { Member.OnClick(this, l, curPos, mode); }

        /// <summary> 
        /// Make Decision for this check. 
        /// Should only be called for containers that contain checks.
        /// </summary>
        public string Decide() 
        { Member.SetValue(this, ((ExpressionMemberCheck)Member).Decide(this)); return CheckValue; }

        /*
         * Following functions call respective MissionStatement functions.
         * This is done to invoke logic provided by MissionStatement class that sometimes requires data from ExpressionMember class (namely, attribute name).
         * Maybe there is a better pattern to be used here but I was not aware of it at the time of writing the editor.
         */

        /// <summary> 
        /// Sets the value for the attribute associated with this ExpressionMember.
        /// </summary>
        /// <param name="value">Attribute value, as should appear in XML</param>
        public void SetAttribute(string value) 
        { Statement.SetAttribute(Member.Name, value); }

        /// <summary> 
        /// Sets the attribute's value.
        /// </summary>
        /// <param name="name">Attribute name, as in XML</param>
        /// <param name="value">Attribute value, as should appear in XML</param>
        public void SetAttribute(string name, string value) 
        { Statement.SetAttribute(name, value); }

        /// <summary> 
        /// Sets the value for the attribute associated with this ExpressionMember if its current value is null.
        /// </summary>
        /// <param name="name">Attribute name, as in XML</param>
        /// <param name="value">Attribute value, as should appear in XML</param>
        public void SetAttributeIfNull(string value) 
        { Statement.SetAttributeIfNull(Member.Name, value); }

        /// <summary> 
        /// Sets the attribute's value if its current value is null.
        /// </summary>
        /// <param name="name">Attribute name, as in XML</param>
        /// <param name="value">Attribute value, as should appear in XML</param>
        public void SetAttributeIfNull(string name, string value) 
        { Statement.SetAttributeIfNull(name, value); }

        /// <summary> 
        /// Gets the value of the attribute associated with this ExpressionMember.
        /// </summary>
        /// <returns>Value if attribute exists, null if doesnt</returns>
		public string GetAttribute()											
        { return Statement.GetAttribute(Member.Name, Member.ValueDescription.DefaultIfNull); }

        /// <summary> 
        /// Gets the attribute value.
        /// </summary>
        /// <param name="name">Name of the value, as in XML</param>
        /// <returns>Value if attribute exists, null if doesnt</returns>
		public string GetAttribute(string name)									
        { return Statement.GetAttribute(name); }

        /// <summary> 
        /// Gets the attribute value 
        /// </summary>
        /// <param name="name">Name of the value, as in XML</param>
        /// <param name="valueIfNull">Set value to this if its null</param>
        /// <returns>Value if attribute exists, null if doesnt</returns>
        public string GetAttribute(string name, string valueIfNull)				
        { return Statement.GetAttribute(name, valueIfNull); }


        public ExpressionMemberContainer(ExpressionMember member, MissionStatement statement) { Member = member; Statement = statement; }
	}
}