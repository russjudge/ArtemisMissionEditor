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
	/// Base class to be inherited by all checks
	/// </summary>
	public class ExpressionMemberCheck : ExpressionMember
	{
		/// <summary> List of choices available at the check if its a GUI check </summary>
		protected List<string> _choices;

		/// <summary> List of choices available at the check if its a GUI check </summary>
		public List<string> Choices { get { return _choices; } }

		/// <summary> Automatically fills _choices, useful when they are mapped 1 to 1 </summary>
		protected bool _autoFillChoices;

		/// <summary> Indicates after which item ID's we need to insert separators </summary>
		protected List<int> _separators;

		/// <summary> Indicates after which item ID's we need to insert separators </summary>
		public bool SeparatorAt(int position) { return _separators.Contains(position); }

		/// <summary> Adds separator after last added choice </summary>
		protected void AddSeparator()
		{
			_separators.Add(_choices.Count-1);
		}
		

		protected override void OnValueDescriptionChange()
		{
			if (_valueDescription != null && _valueDescription.IsSerialized)
				throw new Exception("FAIL! A check member has been assigned a serialized xml value!");
		}
		
		public override bool IsCheck { get { return true; } }

		/// <summary>
		/// Adds new possible expression to the list of check possibilities
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected List<ExpressionMember> Add(string key)
		{
			_choices.Add(key);
			List<ExpressionMember> nList = new List<ExpressionMember>();
			PossibleExpressions.Add(key, nList);
			return nList;
		}
		
		/// <summary>
		/// This function is called when check needs to decide which list of ExpressionMembers to output. 
		/// After it is called, SetValue will be called, to allow error correction. 
		/// </summary>
		/// <example>If input is wrong, decide will choose something, and then the input will be corrected in SetValue function</example>
		/// <param name="container"></param>
		/// <returns></returns>
		public override string Decide(ExpressionMemberContainer container) { throw new Exception("FAIL! Decide was on-overriden in check class"); }

		/// <summary>
		/// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through GUI.
		/// For checks, SetValue must change the attributes/etc of the statement so it fits the newly chosen value
		/// <example>If you chose "use_gm_..", SetValue will set "use_gm_..." attribute to ""</example>
		/// </summary>
		/// <param name="container"></param>
		/// <param name="value"></param>
		protected override void SetValueInternal(ExpressionMemberContainer container, string value) { container.CheckValue = value; }

		public ExpressionMemberCheck(string text, ExpressionMemberValueDescription valueDescription, string nameXml = "", bool mandatory = false)
			: base(text, valueDescription, nameXml, mandatory)
		{
			_separators = new List<int>();
			_choices = new List<string>();
			_autoFillChoices = valueDescription.IsDisplayed; 
		}

		public ExpressionMemberCheck()
			: base()
		{
			_separators = new List<int>();
			_choices = new List<string>();
			_autoFillChoices = false;
		}
	}
}
