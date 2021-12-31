using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
	
namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Contains information about location and length of an entity that caused a warning during validation.
    /// </summary>
    public struct ValidateResultWarning
    {
        public int Position;
        public int Length;

        public ValidateResultWarning(int position, int length)
        {
            Position = position;
            Length = length;
        }
    }

    /// <summary>
    /// Output of the Validate method of the ExpressionMemberValueDescription class.
    /// </summary>
    public struct ValidateResult
    {
        public bool Valid;
        public bool IsExpression;
        public string WarningText;
        public List<int> ErrorList;
        public List<ValidateResultWarning> WarningList;

        /// <summary> A ValidateResult with "true" result, everything else unset.</summary>
        public static readonly ValidateResult True = new ValidateResult(true);
        /// <summary> A ValidateResult with "false" result, everything else unset.</summary>
        public static readonly ValidateResult False = new ValidateResult(false);

        public static ValidateResult FromBool(bool valid) { return valid ? True : False; }

        public ValidateResult(bool valid, bool isExpression = false, string warningText = "", List<int> errorList = null, List<ValidateResultWarning> warningList = null)
        {
            Valid = valid;
            IsExpression = isExpression;
            WarningText = warningText;
            ErrorList = errorList;
            WarningList = warningList;
        }
    }
	
    /// <summary>
    /// Defines everything about the value of the expression member
    /// </summary>
    /// <remarks>
    /// This class defines everything about an expression member's value:
    /// - what data type is it?
    /// - is this value saved into mission XML file or not
    /// - how is this value saved (always or only when filled)
    /// - what editor is used for the value (like, damcon count, consoles list, generic integer input?)
    /// 
    /// We need to create another ExpressionMemberValueDescription if we need to add another different kind of value 
    /// (like, "captainType" which has specific preset text values to choose from), or we need to have a value 
    /// that works a bit differently than any other currently used value 
    /// (for example, have a colon after it when put in the expression, or offer dropdown menu of some kind).
    ///
    /// Otherwise, we can reuse an existing description.
    /// 
    /// All the different ExpressionMemberValueDescriptions are added into the static list in this class, and then they are got via GetItem
    /// </remarks>
    public sealed class ExpressionMemberValueDescription
    {
          /// <summary> What is added to the left of the member value</summary>
        private string QuoteLeft;
        
        /// <summary> What is added to the right of the member value</summary>
		private string QuoteRight;

		/// <summary> Type of this member's value (like int, string, double...) </summary>
        public ExpressionMemberValueType Type { get; private set; }

		/// <summary> Behavior of this value when stored in XML </summary>
        public ExpressionMemberValueBehaviorInXml BehaviorInXml { get; private set; }
		
		/// <summary>
        /// Editor used for this value (usually defines what values to offer for picking, like, available hullid, property, variable, timer,...)
        /// </summary>
        public ExpressionMemberValueEditor Editor { get; private set; }
		
        /// <summary>
        /// Minimal inclusive boundary, null if value has no minimal boundary, used as displayed false value for bool type
        /// </summary>
        public object Min { get; private set; }

        /// <summary>
		/// Maximal inclusive boundary, null if value has no maximal boundary, used as displayed true value for bool type
        /// </summary>
        public object Max { get; private set; }

		/// <summary>
		/// Default value, to be substituted in case the value of the attribute is null
		/// </summary>
        public string DefaultIfNull { get; private set; }

		/// <summary>
		/// This value is displayed in the GUI
		/// </summary>
		public bool IsDisplayed { get { return Editor.IsDisplayed; } }

		/// <summary>
		/// This value is interactive (can be focused and clicked and does something)
		/// </summary>
		/// <returns></returns>
		public bool IsInteractive { get { return Editor.IsInteractive; } }

		/// <summary>
		/// This value is serialized (saved to xml)
		/// </summary>
		/// <returns></returns>
		public bool IsSerialized { get { return BehaviorInXml != ExpressionMemberValueBehaviorInXml.NotStored; } }

		/// <summary>
		/// Convert display value to xml value
		/// </summary>
		public string ValueToXml(string value) { return Editor.ValueToXml(value, Type, Min, Max); }

		/// <summary>
		/// Convert xml value to display value
		/// </summary>
		public string ValueToDisplay(string value) { return Editor.ValueToDisplay(value, Type, Min, Max); }

		/// <summary>
		/// Takes value from XML (GetValue), returns value fit for displaying on-screen.
		/// </summary>
		public string GetValueDisplay(ExpressionMemberContainer container)
		{
			if (Editor == ExpressionMemberValueEditors.Nothing)
				return null;

			string result = Editor == ExpressionMemberValueEditors.Label ? container.Member.Text : container.GetValue();

			result = ValueToDisplay(result);

			//TODO: Maybe some values need to be displayed differently when null or empty?
			if (String.IsNullOrEmpty(result))
				result = container.Member.Text;

			result = QuoteLeft + result + QuoteRight;

			return result;
		}

		/// <summary>
		/// Expression member that's using our descriptor was clicked.
		/// </summary>
		/// <param name="container"></param>
		public void OnClick(ExpressionMemberContainer container, NormalLabel l, Point curPos, ExpressionMemberValueEditorActivationMode mode)
		{
			if (   mode == ExpressionMemberValueEditorActivationMode.Plus01
				|| mode == ExpressionMemberValueEditorActivationMode.Plus
				|| mode == ExpressionMemberValueEditorActivationMode.Plus10
				|| mode == ExpressionMemberValueEditorActivationMode.Plus100
				|| mode == ExpressionMemberValueEditorActivationMode.Plus1000
				|| mode == ExpressionMemberValueEditorActivationMode.Minus01
				|| mode == ExpressionMemberValueEditorActivationMode.Minus
				|| mode == ExpressionMemberValueEditorActivationMode.Minus10
				|| mode == ExpressionMemberValueEditorActivationMode.Minus100
				|| mode == ExpressionMemberValueEditorActivationMode.Minus1000)
			{
                if ((container.Member as ExpressionMemberCheck == null) 
                    && (Type == ExpressionMemberValueType.VarDouble 
                     || Type == ExpressionMemberValueType.VarInteger 
                     || Type == ExpressionMemberValueType.VarBool))
				{
					bool changed = false;
					double delta = 0.0;
					delta = mode == ExpressionMemberValueEditorActivationMode.Plus01		? 0.1	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Plus		? 1		: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Plus10		? 10	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Plus100	? 100	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Plus1000	? 1000	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Minus01	? -0.1	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Minus		? -1	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Minus10	? -10	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Minus100	? -100	: delta;
					delta = mode == ExpressionMemberValueEditorActivationMode.Minus1000	? -1000	: delta;

					switch (Type)
					{
                        case ExpressionMemberValueType.VarBool:
							int tmpBool;
							if (Helper.IntTryParse(container.GetValue(), out tmpBool))
							{
								if (tmpBool == 0)
									tmpBool = 1;
								else
									tmpBool = 0;
								container.SetValue(tmpBool.ToString());
								changed = true;
							}
							break;
                        case ExpressionMemberValueType.VarDouble:
							double tmpDouble;
							if (Helper.DoubleTryParse(container.GetValue(), out tmpDouble))
							{
								tmpDouble+=delta;
								if (Min != null && tmpDouble < (double)Min)
										tmpDouble = Max != null ? (double)Max : (double)Min;
								if (Max != null && tmpDouble > (double)Max)
									tmpDouble = Min != null ? (double)Min : (double)Max;
								container.SetValue(Helper.DoubleToString(tmpDouble));
								changed = true;
							}
							break;
                        case ExpressionMemberValueType.VarInteger:
							int tmpInt;
							if (Helper.IntTryParse(container.GetValue(), out tmpInt))
							{
								tmpInt += (int)Math.Round(delta);
								if (Min != null && tmpInt < (int)Min)
										tmpInt = Max != null ? (int)Max : (int)Min;
								if (Max != null && tmpInt > (int)Max)
									tmpInt = Min != null ? (int)Min : (int)Max;
								container.SetValue(tmpInt.ToString());
								changed = true;
							}
							break;
					}

					if (changed)
					{
						Mission.Current.UpdateStatementTree();
						Mission.Current.RegisterChange("Expression member value changed");
					}
					return;
				}
				else
				{
					return;
				}
			}
			
			ContextMenuStrip curCMS;
			if ((curCMS = Editor.PrepareContextMenuStrip(container, mode)) != null)
			{
				if (mode == ExpressionMemberValueEditorActivationMode.NextMenuItem)
				{
					if ((bool)curCMS.Tag)
					{
						ShowEditingDialog(container);
						return;
					}

					bool next = false;
					foreach (ToolStripItem item in curCMS.Items)
						if (OnClick_RecursivelyActivate(item, ref next, true))
						{
							curCMS.Close();
							return;
						}
					if (curCMS.Items[0] is ToolStripMenuItem && ((ToolStripMenuItem)curCMS.Items[0]).DropDownItems.Count > 0)
						((ToolStripMenuItem)curCMS.Items[0]).DropDownItems[0].PerformClick();
					else
						curCMS.Items[0].PerformClick();
					return;
				}
				if (mode == ExpressionMemberValueEditorActivationMode.PreviousMenuItem)
				{
					if ((bool)curCMS.Tag)
					{
						ShowEditingDialog(container);
						return;
					}

					bool next = false;
					for(int i=curCMS.Items.Count-1;i>=0;i--)
						if (OnClick_RecursivelyActivate(curCMS.Items[i], ref next, false))
						{
							curCMS.Close();
							return;
						}
					if (curCMS.Items[curCMS.Items.Count - 1] is ToolStripMenuItem && ((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).DropDownItems.Count > 0)
						((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).
                            DropDownItems[((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).
                            DropDownItems.Count - 1].
                            PerformClick();
					else
						curCMS.Items[curCMS.Items.Count - 1].PerformClick();
					curCMS.Close();
					return;
				}
				curCMS.Show(curPos);
			}
			else
				ShowEditingDialog(container);
		}

        /// <summary>
        /// Activates correct menu item by recursively going through the whole menu structure. Called from OnClick and recursively calls itself.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="next"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        private bool OnClick_RecursivelyActivate(ToolStripItem item, ref bool next, bool forward)
        {
            if (!(item is ToolStripMenuItem))
                return false;

            if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
            {
                for (int i = 0; i < ((ToolStripMenuItem)item).DropDownItems.Count; i++)

                    if (OnClick_RecursivelyActivate(((ToolStripMenuItem)item).DropDownItems[forward ? i
                        : ((ToolStripMenuItem)item).DropDownItems.Count - 1 - i], ref next, forward))
                        return true;
            }
            else
            {
                if (next)
                {
                    item.PerformClick();
                    return true;
                }
                next = item.Selected || next;
            }
            return false;
        }
		
        /// <summary>
        /// Shows dialog window when chosen from context menu. 
        /// To be assigned as event handler for context menu item being clicked.
        /// </summary>
		public void ShowEditingDialog(ExpressionMemberContainer container)
		{
			Editor.ShowEditingDialog(container, this, DefaultIfNull);
		}

        /// <summary>
        /// Validate the value.
        /// </summary>
        public ValidateResult Validate(string value)
        {
            if (value == null)
                return ValidateResult.True;
            bool valid = true;
            string warning = "";
            switch (Type)
            {
                case ExpressionMemberValueType.VarBool:
                    if (String.IsNullOrEmpty(value))
                        return ValidateResult.False;
                    return ValidateResult.True;
                case ExpressionMemberValueType.VarInteger:
                    int i;
                    if (!Helper.IntTryParse(value, out i))
                        return ValidateExpression(value);
                    if (Mission.Current.VariableNamesList.Key.Contains(value))
                        warning = "Warning: variable named \"" + value + "\" is set in the mission script. While this is technically allowed, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \"" + value + "+1\" will not evaluate to " + (Helper.StringToDouble(value) + 1.0).ToString() + ", but rather to the current " + value + "'s value plus 1";
                    valid = valid && (Min == null || i >= (int)Min);
                    valid = valid && (Max == null || i <= (int)Max);
                    return new ValidateResult(valid, false, warning);
                case ExpressionMemberValueType.VarDouble:
                    double d;
                    if (!Helper.DoubleTryParse(value, out d))
                        return ValidateExpression(value);
                    if (Mission.Current.VariableNamesList.Key.Contains(value))
                        warning = "Warning: variable named \"" + value + "\" is set in the mission script. While this is technically allowed, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \"" + value + "+1\" will not evaluate to " + (Helper.StringToDouble(value) + 1.0).ToString() + ", but rather to the current " + value + "'s value plus 1";
                    valid = valid && (Min == null || d >= (double)Min);
                    valid = valid && (Max == null || d <= (double)Max);
                    return new ValidateResult(valid, false, warning);
                case ExpressionMemberValueType.VarString:
                    return ValidateResult.True;
                case ExpressionMemberValueType.VarEnumString:
                    valid = Editor.XmlValueToDisplay.ContainsKey(value);
                    return new ValidateResult(valid, false, warning);
                case ExpressionMemberValueType.Body:
                    return ValidateResult.True;
                case ExpressionMemberValueType.Nothing:
                    return ValidateResult.True;
                default:
                    throw new NotImplementedException("Attempting to validate something unknown!");
            }
        }

        /// <summary>
        /// Validate an expresion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ValidateResult ValidateExpression(string value)
        {
            bool isValid = true;
            string warnings = "";
            List<ValidateResultWarning> warningList = new List<ValidateResultWarning>();
            List<int> errorList = new List<int>();
            bool nextMustBeOperator = false;
            Tuple<int, string> curItem = null;
            Stack<Tuple<int, string>> operatorStack = new Stack<Tuple<int, string>>();
            List<Tuple<int, string>> parsedExpression = new List<Tuple<int, string>>();

            Action<int, string> AddError = (int i, string text) =>
            {
                errorList.Add(i);
                warnings += "ERROR at " + i.ToString("000") + ": " + text + "\r\n";
            };
            Action<int, int, string> AddWarning = (int pos, int len, string text) =>
            {
                warningList.Add(new ValidateResultWarning(pos, len));
                warnings += "WARNING at " + pos.ToString("000") + ": " + text + "\r\n";
            };

            for (int i = 0; i < value.Length; i++)
            {
                char curChar = value[i];

                // If it's a space - add previous item to stack
                if (curChar == ' ')
                {
                    if (curItem != null)
                    {
                        nextMustBeOperator = true;
                        parsedExpression.Add(curItem);
                        curItem = null;
                    }
                    continue;
                }

                // If it's an operator
                int precedence = ValidateExpression_GetOperatorPrecedence(curChar);
                if (precedence != 0)
                {
                    if (curItem != null)
                    {
                        nextMustBeOperator = true;
                        parsedExpression.Add(curItem);
                        curItem = null;
                    }
                    if (!nextMustBeOperator)
                    {
                        // Allow the unary '-' operator.
                        if (curChar != '-')
                        {
                            isValid = false;
                            AddError(i, "Unexpected operator (expected literal or variable)");
                        }
                    }
                    nextMustBeOperator = false;
                    while (operatorStack.Count > 0 && ValidateExpression_GetOperatorPrecedence(operatorStack.Peek().Item2[0]) > ValidateExpression_GetOperatorPrecedence(curChar))
                        parsedExpression.Add(operatorStack.Pop());
                    operatorStack.Push(new Tuple<int, string>(i, curChar.ToString()));
                    continue;
                }

                // If it's a bracket
                if (curChar == '(' || curChar == ')')
                {
                    if (curItem != null)
                    {
                        nextMustBeOperator = true;
                        parsedExpression.Add(curItem);
                        curItem = null;
                    }

                    if (curChar == '(')
                    {
                        if (nextMustBeOperator)
                        {
                            isValid = false;
                            AddError(i, "Unexpected opening bracket (expected operator)");
                            nextMustBeOperator = false;
                        }
                        operatorStack.Push(new Tuple<int, string>(i, curChar.ToString()));
                    }
                    else
                    {
                        if (!nextMustBeOperator)
                        {
                            isValid = false;
                            AddError(i, "Unexpected closing bracket (expected literal or variable)");
                            nextMustBeOperator = true;
                        }
                        while (operatorStack.Count > 0 && operatorStack.Peek().Item2[0] != '(')
                            parsedExpression.Add(operatorStack.Pop());
                        if (operatorStack.Count == 0)
                        {
                            isValid = false;
                            AddError(i, "Unexpected closing bracket (no matching opening bracket exists)");
                        }
                        else
                            operatorStack.Pop();
                    }
                    continue;
                }

                //If we reached here, it's a letter or a number
                {
                    if (curItem == null)
                    {
                        if (nextMustBeOperator)
                        {
                            nextMustBeOperator = false;
                            isValid = false;
                            AddError(i, "Unexpected symbol (expected operator)");
                        }
                        curItem = new Tuple<int, string>(i, curChar.ToString());
                    }
                    else
                        curItem = new Tuple<int, string>(curItem.Item1, curItem.Item2 + curChar.ToString());
                }
            }

            if (curItem != null)
            {
                nextMustBeOperator = true;
                parsedExpression.Add(curItem);
                curItem = null;
            }
            else
            {
                if (!nextMustBeOperator && (parsedExpression.Count > 0 || operatorStack.Count > 0))
                {
                    isValid = false;
                    AddError(value.Length, "Unexpected end of expression (expected literal or variable)");
                }
            }
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek().Item2[0] == '(')
                {
                    isValid = false;
                    AddError(operatorStack.Pop().Item1, "Matching closing bracket not found");

                }
                else
                    parsedExpression.Add(operatorStack.Pop());
            }

            List<string> variableNames = Mission.Current.VariableNamesList.Key;
            foreach (Tuple<int, string> token in parsedExpression)
            {
                if (token.Item2.Length == 1 && ValidateExpression_GetOperatorPrecedence(token.Item2[0]) > 0)
                    continue;
                if (Type == ExpressionMemberValueType.VarInteger)
                    if (!Helper.IntTryParse(token.Item2) && Helper.DoubleTryParse(token.Item2))
                        AddWarning(token.Item1, token.Item2.Length, "Float literal used inside an integer value");
                if ((Helper.IntTryParse(token.Item2) || Helper.DoubleTryParse(token.Item2)) && variableNames.Contains(token.Item2))
                    AddWarning(token.Item1, token.Item2.Length, "Variable named \"" + token.Item2 + "\" is set in the mission script. While this is technically allowed by the Artemis script parser, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \"" + token.Item2 + "+1\" will not evaluate to " + (Helper.StringToDouble(token.Item2) + 1.0).ToString() + ", but rather to the current " + token.Item2 + "'s value plus 1");
                if (!Helper.IntTryParse(token.Item2) && !Helper.DoubleTryParse(token.Item2) && !variableNames.Contains(token.Item2))
                    AddWarning(token.Item1, token.Item2.Length, "Variable named \"" + token.Item2 + "\" is never set in the mission script (a typo?)");
            }

            return new ValidateResult(isValid, true, warnings.Length == 0 ? warnings : warnings.Substring(0, warnings.Length - 2), errorList, warningList);
        }

        /// <summary>
        /// Get operator precedence for the value expression validation algorithm. Called from ValidateExpression.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int ValidateExpression_GetOperatorPrecedence(char value)
        {
            if (value == '*' || value == '/')
                return 2;
            if (value == '+' || value == '-')
                return 1;
            return 0;
        }
        
        /// <summary>
        /// Check two values for equality, taking type etc. into account.
        /// </summary>
        public bool AreEqual(string value1, string value2)
        {
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;
            if (!Validate(value1).Valid
                || Validate(value1).IsExpression
                || !Validate(value2).Valid
                || Validate(value2).IsExpression)
                return false;
            switch (Type)
            {
                case ExpressionMemberValueType.VarBool:
                    return value1 == value2;
                case ExpressionMemberValueType.VarInteger:
                    int xi = Helper.StringToInt(value1);
                    int yi = Helper.StringToInt(value2);
                    return xi == yi;
                case ExpressionMemberValueType.VarDouble:
                    double xd = Helper.StringToDouble(value1);
                    double yd = Helper.StringToDouble(value2);
                    return xd == yd;
                case ExpressionMemberValueType.VarEnumString:
                case ExpressionMemberValueType.VarString:
                case ExpressionMemberValueType.Body:
                    return value1.Trim() == value2.Trim();
                case ExpressionMemberValueType.Nothing:
                    throw new ArgumentException("description", "Attempting to compare ValueType.NOTHING values!");
                default:
                    throw new NotImplementedException("Attempting to compare something unknown!");
            }
        }

        public ExpressionMemberValueDescription(ExpressionMemberValueType type, 
            ExpressionMemberValueBehaviorInXml behavior, ExpressionMemberValueEditor editor, 
            object min = null, object max = null, 
            string quoteLeft = "", string quoteRight = " ", 
            string defaultIfNull = null)
		{
			Type			= type;
			BehaviorInXml	= behavior;
			Editor			= editor;
			Min				= min;
			Max				= max;
			QuoteLeft		= quoteLeft;
			QuoteRight		= quoteRight;
			DefaultIfNull	= defaultIfNull;
		}
    }

}
