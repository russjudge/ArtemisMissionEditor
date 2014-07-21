using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
	
namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;
	
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

		public static bool Validate(string value, EMVD description)
		{
			if (value == null)
				return true;
			bool valid;
			switch (description.Type)
			{
				case EMVT.VarBool:
					if (value == "")
						return false;
					return true;
				case EMVT.VarInteger:
					int x;
					valid = Helper.IntTryParse(value, out x);
					valid = valid && (description.Min == null || x >= (int)description.Min);
					valid = valid && (description.Max == null || x <= (int)description.Max);
					return valid;
				case EMVT.VarDouble:
					double y;
					valid = DoubleTryParse(value, out y);
					valid = valid && (description.Min == null || y >= (double)description.Min);
					valid = valid && (description.Max == null || y <= (double)description.Max);
					return valid;
				case EMVT.VarString:
					return true;
				case EMVT.Body:
					return true;
				case EMVT.Nothing:
					return true;
				default:
					throw new Exception("FAIL! Attempting to validate something unknown!");
			}
		}

		public static bool AreEqual(string value1, string value2, EMVD description)
		{
			if (value1 == null && value2 == null)
				return true;
			if (value1 == null || value2 == null)
				return false;
			if (!Validate(value1, description) || !Validate(value2, description))
				return false;
			switch (description.Type)
			{
				case EMVT.VarBool:
					return value1 == value2;
				case EMVT.VarInteger:
					int xi = Helper.StringToInt(value1);
					int yi = Helper.StringToInt(value2);
					return xi == yi;
				case EMVT.VarDouble:
					double xd = Helper.StringToDouble(value1);
					double yd = Helper.StringToDouble(value2);
					return xd == yd;
				case EMVT.VarString:
					return value1.Trim() == value2.Trim();
				case EMVT.Body:
					return value1.Trim() == value2.Trim();
				case EMVT.Nothing:
					throw new Exception("FAIL! Attempting to compare two NOTHING values!");
				default:
					throw new Exception("FAIL! Attempting to compare something unknown!");
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
