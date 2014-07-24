﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
	
namespace ArtemisMissionEditor
{
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

    public struct ValidateResult
    {
        public bool Valid;
        public bool IsExpression;
        public string WarningText;
        public List<int> ErrorList;
        public List<ValidateResultWarning> WarningList;

        /// <summary> A ValidateResult with "true" result, everything else unset.</summary>
        public static ValidateResult True { get; private set; }
        /// <summary> A ValidateResult with "false" result, everything else unset.</summary>
        public static ValidateResult False { get; private set; }
        public static ValidateResult FromBool(bool valid) { return valid ? True : False; }

        static ValidateResult()
        {
            True = new ValidateResult(true);
            False = new ValidateResult(false);
        }

        public ValidateResult(bool valid, bool isExpression = false, string warningText = "", List<int> errorList = null, List<ValidateResultWarning> warningList = null)
        {
            Valid = valid;
            IsExpression = isExpression;
            WarningText = warningText;
            ErrorList = errorList;
            WarningList = warningList;
        }
    }

	public static class Helper
    {
        private static NumberFormatInfo nfi;

		public static Dictionary<Guid, Guid> GuidDictionary;

		/// <summary> Flag that paste action is in progress, will prevent reading control keys by treeview in case of selection events </summary>
		public static bool PasteInProgress;

        /// <summary>
        /// Helper function to calculate angle from point 1 to point 2
        /// </summary>
        /// <param name="px1"></param>
        /// <param name="py1"></param>
        /// <param name="px2"></param>
        /// <param name="py2"></param>
        /// <returns></returns>
        public static double CalcaulateAngle(double px1, double py1, double px2, double py2)
        {
            // Negate X and Y values
            double pxRes = px2 - px1;
            double pyRes = py2 - py1;
            double angle = 0.0;
            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pyRes == 0.0) angle = 0.0;
                else
                    if (pyRes > 0.0) angle = System.Math.PI / 2.0;
                    else angle = System.Math.PI * 3.0 / 2.0;
            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0) angle = 0.0;
                else angle = System.Math.PI;
            }
            else
            {
                if (pxRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else
                    if (pyRes < 0.0) angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);
                    else angle = System.Math.Atan(pyRes / pxRes);
            }

            return angle;
        }

        /// <summary>
        /// Outputs double to string without different stupid shit M$ does by default
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DoubleToString(double input, bool decimalZero = true)
        {
			if (decimalZero)
				return input.ToString("##################################################0.0##################################################", nfi);
			else
				return input.ToString("##################################################0.###################################################", nfi);
        }

        public static string DoubleToPercent(double input)
        {
            return input.ToString("##################################################0.##################################################%", nfi);
        }

        public static string DoubleFormatToString(string format, double input)
        {
            return String.Format(nfi, format, input);
        }

        /// <summary>
        /// Converts string to double, not making trouble when there is a , separator or . separator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double StringToDouble(string input)
        {
            double result;
            if (double.TryParse(input, NumberStyles.Float, nfi, out result))
                return result;

            return double.Parse(input.Replace(',', '.'), nfi);
        }

        /// <summary>
        /// Does double.TryParse without all the stupid shit with , and .
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool DoubleTryParse(string input, out double result)
        {
            result = 0.0;
            if (input == null)
                return false;

            if (double.TryParse(input, NumberStyles.Float, nfi, out result))
                return true;

            return double.TryParse(input.Replace(',', '.'), NumberStyles.Float, nfi, out result);
        }
		public static bool DoubleTryParse(string input)
		{
			double tmp;
			return DoubleTryParse(input, out tmp);
		}

		/// <summary>
		/// Does int.TryParse without going crazy about .0 or ,0 at the end!
		/// </summary>
		/// <param name="input"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool IntTryParse(string input, out int result)
		{
			result = 0;
			if (input == null)
				return false;

			if (int.TryParse(input,  out result))
				return true;

			return int.TryParse(input.Replace(",0", "").Replace(".0", ""),out result);
		}
        public static bool IntTryParse(string input)
        {
            int tmp;
            return IntTryParse(input, out tmp);
        }

		public static int StringToInt(string input)
		{
			int result = 0;
			
			if (int.TryParse(input,  out result))
				return result;

			return int.Parse(input.Replace(",0", "").Replace(".0", ""));
		}

        public static ValidateResult Validate(string value, ExpressionMemberValueDescription description)
		{
			if (value == null)
				return ValidateResult.True;
			bool valid = true;
            string warning = "";
			switch (description.Type)
			{
				case ExpressionMemberValueType.VarBool:
					if (value == "")
					    return ValidateResult.False;
                    return ValidateResult.True;
                case ExpressionMemberValueType.VarInteger:
					int i;
                    if (!Helper.IntTryParse(value, out i))
                        return ValidateExpression(value, description);
                    if (Mission.Current.VariableNamesList.Key.Contains(value))
                        warning = "Warning: variable named \"" + value + "\" is set in the mission script. While this is technically allowed, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \"" + value + "+1\" will not evaluate to " + (StringToDouble(value) + 1.0).ToString() + ", but rather to the current " + value + "'s value plus 1";
                    valid = valid && (description.Min == null || i >= (int)description.Min);
					valid = valid && (description.Max == null || i <= (int)description.Max);
					return new ValidateResult(valid, false, warning);
                case ExpressionMemberValueType.VarDouble:
					double d;
					if (!DoubleTryParse(value, out d))
                        return ValidateExpression(value, description);
                    if (Mission.Current.VariableNamesList.Key.Contains(value))
                        warning = "Warning: variable named \"" + value + "\" is set in the mission script. While this is technically allowed, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \"" + value + "+1\" will not evaluate to " + (StringToDouble(value) + 1.0).ToString() + ", but rather to the current " + value + "'s value plus 1";
                    valid = valid && (description.Min == null || d >= (double)description.Min);
					valid = valid && (description.Max == null || d <= (double)description.Max);
                    return new ValidateResult(valid, false, warning);
                case ExpressionMemberValueType.VarString:
                    return ValidateResult.True;
                case ExpressionMemberValueType.Body:
                    return ValidateResult.True;
                case ExpressionMemberValueType.Nothing:
                    return ValidateResult.True;
				default:
					throw new NotImplementedException("Attempting to validate something unknown!");
			}
		}

        private static int GetOperatorPrecedence(char value)
        {
            if (value == '*' || value == '/')
                return 2;
            if (value == '+' || value == '-')
                return 1;
            return 0;
        }

        private static ValidateResult ValidateExpression(string value, ExpressionMemberValueDescription description)
        {
            bool isValid = true;
            string warnings = "";
            List<ValidateResultWarning> warningList = new List<ValidateResultWarning>();
            List<int> errorList = new List<int>();
            bool nextMustBeOperator = false;
            Tuple<int, string> curItem = null;
            Stack<Tuple<int, string>> operatorStack = new Stack<Tuple<int, string>>();
            List<Tuple<int, string>> parsedExpression = new List<Tuple<int, string>>();
            
            Action<int, string> AddError = (int i, string text) => {
                errorList.Add(i);
                warnings += "ERROR at " + i.ToString("000") + ": " + text + "\r\n";
            };
            Action<int, int, string> AddWarning = (int pos, int len, string text) =>{
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
                int precedence = GetOperatorPrecedence(curChar);
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
                        isValid = false;
                        AddError(i, "Unexpected operator (expected literal or variable)");
                    }
                    nextMustBeOperator = false;
                    while (operatorStack.Count > 0 && GetOperatorPrecedence(operatorStack.Peek().Item2[0]) > GetOperatorPrecedence(curChar))
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
                if (token.Item2.Length == 1 && GetOperatorPrecedence(token.Item2[0]) > 0)
                    continue;
                if (description.Type == ExpressionMemberValueType.VarInteger)
                    if (!IntTryParse(token.Item2) && DoubleTryParse(token.Item2))
                        AddWarning(token.Item1, token.Item2.Length, "Float literal used inside an integer value");
                if ((IntTryParse(token.Item2) || DoubleTryParse(token.Item2)) && variableNames.Contains(token.Item2))
                    AddWarning(token.Item1, token.Item2.Length, "Variable named \"" + token.Item2 + "\" is set in the mission script. While this is technically allowed by the Artemis script parser, you should avoid doing this, because variable names take precedence to literals during expression evalutation. For example, an expression \""+token.Item2+"+1\" will not evaluate to " + (StringToDouble(token.Item2)+1.0).ToString()+", but rather to the current "+token.Item2+"'s value plus 1");
                if (!IntTryParse(token.Item2) && !DoubleTryParse(token.Item2) && !variableNames.Contains(token.Item2))
                    AddWarning(token.Item1, token.Item2.Length, "Variable named \"" + token.Item2 + "\" is never set in the mission script (a typo?)");
            }

            return new ValidateResult(isValid, true, warnings.Length == 0 ? warnings : warnings.Substring(0, warnings.Length - 2), errorList, warningList);
        }

        public static bool AreEqual(string value1, string value2, ExpressionMemberValueDescription description)
		{
			if (value1 == null && value2 == null)
				return true;
			if (value1 == null || value2 == null)
				return false;
			if (!Validate(value1, description).Valid 
                || Validate(value1, description).IsExpression
                || !Validate(value2, description).Valid
                || Validate(value2, description).IsExpression)
				return false;
			switch (description.Type)
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
				case ExpressionMemberValueType.VarString:
					return value1.Trim() == value2.Trim();
				case ExpressionMemberValueType.Body:
					return value1.Trim() == value2.Trim();
				case ExpressionMemberValueType.Nothing:
					throw new ArgumentOutOfRangeException("Attempting to compare two NOTHING values!");
				default:
					throw new NotImplementedException("Attempting to compare something unknown!");
			}
		}

        public static string FixMissionXml(string text)
        {
            text = text.Replace("\"<=\"", "\"LESS_EQUAL\"");
            text = text.Replace("\"=>\"", "\"GREATER_EQUAL\"");
            text = text.Replace("\"!=\"", "\"NOT\"");
            text = text.Replace("\"<\"", "\"LESS\"");
            text = text.Replace("\">\"", "\"GREATER\"");
            text = text.Replace("\"=\"", "\"EQUALS\"");
            return text;
        }

        public static string StringReplaceEx(string original,
                    string pattern, string replacement)
        {
			if (pattern.Length == 0)
				return original;

            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

		private static void TrueClone_private_CloneExpansionStatusAndTags(TreeNode source, TreeNode destination)
		{
			//Expand destination if source is expanded
			if (source.IsExpanded) 
				destination.Expand();
			
			//Clone tag if possible
			if (destination.Tag is MissionNode
				||destination.Tag is MissionNode_Comment
				||destination.Tag is MissionNode_Event
				||destination.Tag is MissionNode_Folder
				||destination.Tag is MissionNode_Start
				||destination.Tag is MissionNode_Unknown)
				destination.Tag = ((MissionNode)destination.Tag).Clone();

			int i;
			for (i = 0; i < source.Nodes.Count; i++)
				TrueClone_private_CloneExpansionStatusAndTags(source.Nodes[i], destination.Nodes[i]);
		}

		/// <summary>
		/// Clones the node all right (retaining the exact Tag value and 
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static TreeNode TrueClone(TreeNode original)
		{
			//Clone the node
			TreeNode clone = (TreeNode)original.Clone();

			//Reset dictionary of guids
			ResetGuidDictionary();

			//Clone the expansion status and tags
			TrueClone_private_CloneExpansionStatusAndTags(original, clone);

			return clone;
		}

		public static void ResetGuidDictionary()
		{
			GuidDictionary.Clear();
		}

		public static string RemoveNodes(string text)
		{
			foreach (string toDel in new string[4] { "<event", "<start", "</event", "</start" })
			{
				string result = "";

				while (text.IndexOf(toDel) != -1)
				{
					result += text.Substring(0, text.IndexOf(toDel));
					text = text.Substring(text.IndexOf(toDel) + toDel.Length, text.Length - (text.IndexOf(toDel) + toDel.Length));
					int i = -1;
					int quotecount = 0;
					while (++i < text.Length)
					{
						if (text[i] == '"')
							quotecount++;
						if (text[i] == '>' && quotecount % 2 == 0)
							break;
					}
					if (i < text.Length)
					{
						text = text.Substring(i + 1, text.Length - i - 1);
					}
				}

				text = result + text;
			}

			return text;
		}

		public static Rectangle MakeDrawNodeRectangle(Rectangle nodeBounds)
		{
			return new Rectangle(nodeBounds.X - 1, nodeBounds.Y, nodeBounds.Size.Width+4, nodeBounds.Size.Height);
		}

		static Helper()
        {
            //R. Judge: Changes Jan 16, 2013, to band-aid up to 1.7
            nfi = CultureInfo.GetCultureInfo("en-US").NumberFormat;
            
            //nfi.NumberDecimalSeparator = ".";
            //R. Judge: End changes Jan 16, 2013.
			GuidDictionary = new Dictionary<Guid, Guid>();
			PasteInProgress = false;
        }
    }
}
