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
	/// Represents a single member in an expression, like the "[variable]" in "If [variable] [comparator] [value]"
	/// </summary>
	public class ExpressionMember
	{
		/// <summary>
		/// Text label of the member, used by members that are static (like intermediate text in expressions) or as a placeholder for null values.
		/// </summary>
        public string Text { get; protected set; }

		/// <summary>
		/// Description of the value of this member - its type, boundaries, etc.
		/// </summary>
		public ExpressionMemberValueDescription ValueDescription
		{
			get { return _valueDescription; }
			protected set { _valueDescription = value; OnValueDescriptionChange(); }
		}
        protected ExpressionMemberValueDescription _valueDescription;
        protected virtual void OnValueDescriptionChange() { }

		/// <summary>
		/// Must this member be non-empty.
		/// </summary>
        public bool Mandatory { get; protected set; }

		/// <summary>
		/// Name of the variable in Xml, like: name, centerX, comparator etc.
		/// </summary>
        public string Name { get; protected set; }

		/// <summary>
		/// Whether line break is required after this member.
		/// </summary>
		public virtual bool RequiresLinebreak { get { return false; } }

		/// <summary>
		/// Get value of current member (internal value, as is in XML)
		/// </summary>
        public virtual string GetValue(ExpressionMemberContainer container)
        { return container.GetAttribute(); }

		/// <summary>
		/// Get value of current member to display in the label (meaning, exactly what will appear in GUI labels)
		/// </summary>
		public virtual string GetValueDisplay(ExpressionMemberContainer container) 
        { return ValueDescription.GetValueDisplay(container); }

		/// <summary>
		/// Get value of current member to display in the text string representing the statement
		/// </summary>
		public virtual string GetValueText(ExpressionMemberContainer container) 
        { return GetValueDisplay(container); }

		/// <summary>
		/// Set value of the current member, including all checks (like, sets to null if this is a "store only if filled" value etc...)
		/// </summary>
		protected virtual void SetValueInternal(ExpressionMemberContainer container, string value) 
        { container.SetAttribute(value); }

		/// <summary>
		/// Sets value of the current member.
		/// </summary>
        public void SetValue(ExpressionMemberContainer container, string value) 
        { SetValueInternal(container, value); container.Statement.Update(); }

		/// <summary>
		/// Routine to be done when label control assigned to this member is clicked
		/// </summary>
		public void OnClick(ExpressionMemberContainer container, NormalLabel l, Point curPos, ExpressionMemberValueEditorActivationMode mode) 
        { ValueDescription.OnClick(container, l, curPos, mode); }

		/// <summary>
		/// Transform display value to xml
		/// </summary>
		/// <param name="value"></param>
		public string ValueToXml(string value) 
        { return ValueDescription.ValueToXml(value); }

		/// <summary>
		/// Transform XML value to display value
		/// </summary>
		/// <param name="value"></param>
		public string ValueToDisplay(string value) 
        { return ValueDescription.ValueToDisplay(value); }

		/// <summary>
        /// Creates a fully functional expression member.
		/// </summary>
		/// <param name="text">Default text (usually displayed when value is null)</param>
		/// <param name="valueDescription">Value description class</param>
		/// <param name="nameXml">Value's name as it appears in the element's attribute</param>
		/// <param name="mandatory">If this value will not be present, crashes or glitches can happen</param>
        public ExpressionMember(string text, ExpressionMemberValueDescription valueDescription, string nameXml = "", bool mandatory = false)
		{
			Text = text;
			_valueDescription = valueDescription;
			Name = nameXml;
			Mandatory = mandatory;
		}

        /// <summary>
		/// Creates a text-only expression member.
		/// </summary>
		public ExpressionMember(string text) : this(text, ExpressionMemberValueDescriptions.Label) { }

        /// <summary>
        /// Creates a blank (invisible) expression member.
        /// </summary>
		public ExpressionMember() : this("", ExpressionMemberValueDescriptions.Blank) { }
	}

}
