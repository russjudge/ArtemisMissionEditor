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
    /// </summary>
    public abstract class ExpressionMemberCheck : ExpressionMember
	{
        /// <summary>
        /// Get value of current member (check value)
        /// </summary>
        public override string GetValue(ExpressionMemberContainer container)
        {
            return container.CheckValue;
        }

        /// <summary>
        /// Expressions nested within a Check
        /// </summary>
        public Dictionary<string, List<ExpressionMember>> PossibleExpressions { get; protected set; }
                
        /// <summary> List of choices available for the check. </summary>
        public List<string> Choices { get; protected set; }
		
		/// <summary> Indicates after which item ID's we need to insert separators. </summary>
		protected List<int> Separators;

		/// <summary> Indicates after which item ID's we need to insert separators. </summary>
		public bool SeparatorAt(int position) { return Separators.Contains(position); }

		/// <summary> Adds separator after last added choice </summary>
		protected void AddSeparator()
		{
			Separators.Add(Choices.Count-1);
		}
		
		protected override void OnValueDescriptionChange()
		{
			if (_valueDescription != null && _valueDescription.IsSerialized)
				throw new Exception("A check member has been assigned a serialized xml value! This should never be possible");
		}
		
		/// <summary>
		/// Adds new possible expression to the list of check possibilities
		/// </summary>
		protected List<ExpressionMember> Add(string key)
		{
			Choices.Add(key);
			List<ExpressionMember> nList = new List<ExpressionMember>();
			PossibleExpressions.Add(key, nList);
			return nList;
		}
		
		/// <summary>
		/// This function is called when check needs to decide which list of ExpressionMembers to output. 
		/// After it is called, SetValue will be called, to allow for error correction. 
		/// </summary>
		/// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public abstract string Decide(ExpressionMemberContainer container);

		/// <summary>
		/// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
		/// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
		/// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
		/// </summary>
		protected override void SetValueInternal(ExpressionMemberContainer container, string value) { container.CheckValue = value; }

        protected ExpressionMemberCheck(string text, ExpressionMemberValueDescription valueDescription, string nameXml = "", bool mandatory = false)
			: base(text, valueDescription, nameXml, mandatory)
		{
			Separators = new List<int>();
			Choices = new List<string>();
			
            PossibleExpressions = new Dictionary<string, List<ExpressionMember>>();
		}

        protected ExpressionMemberCheck() : this("", ExpressionMemberValueDescriptions.Blank) { }
	}
}
