using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.IO.Compression;
	
namespace ArtemisMissionEditor
{
    /// <summary>
    /// Static helper class containing all useful functions that do not belong to specific classes.
    /// </summary>
    public static class Helper
    {
        private static NumberFormatInfo nfi;

        /// <summary> Used during cloning ob objects in order to provide proper links.</summary>
		public static Dictionary<Guid, Guid> GuidDictionary;

		/// <summary> Flag that paste action is in progress, will prevent reading control keys by treeview in case of selection events </summary>
		public static bool PasteInProgress;

        /// <summary>
        /// Calculates angle of a vector from point 1 to point 2
        /// </summary>
        /// <param name="px1">Point 1's X</param>
        /// <param name="py1">Point 1's Y</param>
        /// <param name="px2">Point 2's X</param>
        /// <param name="py2">Point 2's Y</param>
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
        /// Outputs double to string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="decimalZero">Whether a decimal zero must be present.</param>
        public static string DoubleToString(double input, bool decimalZero = true)
        {
			if (decimalZero)
				return input.ToString("##################################################0.0##################################################", nfi);
			else
				return input.ToString("##################################################0.###################################################", nfi);
        }

        /// <summary>
        /// Outputs double value as percentage (where a value of 1.0 is 1%, not 100%)
        /// </summary>
        public static string DoubleToPercent(double input)
        {
            return input.ToString("##################################################0.##################################################%", nfi);
        }

        /// <summary>
        /// Converts double to string according to specified format.
        /// </summary>
        public static string DoubleFormatToString(string format, double input)
        {
            return String.Format(nfi, format, input);
        }

        /// <summary>
        /// Converts string to double no matter what is used as a separator.
        /// </summary>
        public static double StringToDouble(string input)
        {
            double result;
            if (double.TryParse(input, NumberStyles.Float, nfi, out result))
                return result;

            return double.Parse(input.Replace(',', '.'), nfi);
        }

        /// <summary>
        /// Does double.TryParse no matter what is used as a separator.
        /// </summary>
        public static bool DoubleTryParse(string input, out double result)
        {
            result = 0.0;
            if (input == null)
                return false;

            if (double.TryParse(input, NumberStyles.Float, nfi, out result))
                return true;

            return double.TryParse(input.Replace(',', '.'), NumberStyles.Float, nfi, out result);
        }

        /// <summary>
        /// Does double.TryParse no matter what is used as a separator, 
        /// and without requiring an out parameter.
        /// </summary>
        public static bool DoubleTryParse(string input)
		{
			double tmp;
			return DoubleTryParse(input, out tmp);
		}

		/// <summary>
		/// Does int.TryParse without complaining about decimal zero (.0 or ,0 at the end).
		/// </summary>
		public static bool IntTryParse(string input, out int result)
		{
			result = 0;
			if (input == null)
				return false;

			if (int.TryParse(input,  out result))
				return true;

			return int.TryParse(input.Replace(",0", "").Replace(".0", ""),out result);
		}

        /// <summary>
        /// Does int.TryParse without complaining about decimal zero (.0 or ,0 at the end), 
        /// and without requiring an out parameter.
        /// </summary>
        public static bool IntTryParse(string input)
        {
            int tmp;
            return IntTryParse(input, out tmp);
        }

        /// <summary>
        /// Does Guid.TryParse without reqiring an out parameter.
        /// </summary>
        public static bool GuidTryParse(string input)
        {
            Guid tmp;
            return Guid.TryParse(input, out tmp);
        }

        /// <summary>
        /// Try reading Guid from input. If failed, generate new guid instead.
        /// </summary>
        /// <param name="verbose">If true, whine to the log about failed parsing.</param>
        public static Guid GuidTryRead(string input, bool verbose = false)
        {
            Guid guid;
            if (Helper.GuidTryParse(input))
                guid = new Guid(input);
            else
            {
                guid = Guid.NewGuid();
                Log.Add("Found a bad GUID value: \"" + input + "\".");
            }
            return guid;
        }

        /// <summary>
        /// Does int.Parse without complaining about decimal zero (.0 or ,0 at the end), 
        /// </summary>
		public static int StringToInt(string input)
		{
			int result = 0;
			
			if (int.TryParse(input,  out result))
				return result;

			return int.Parse(input.Replace(",0", "").Replace(".0", ""));
		}

        /// <summary>
        /// Fixes typical errors in Xml that are allowed by Artemis mission engine, 
        /// but not allowed by Xml specification (and therefore not loaded by .Net class).
        /// </summary>
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

        /// <summary>
        /// Proper replacement function that replaces as long as there is something to replace.
        /// </summary>
        /// <param name="original">Source string.</param>
        /// <param name="pattern">Pattern to look for.</param>
        /// <param name="replacement">Replacement string for the pattern.</param>
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

		/// <summary>
		/// Clones the node property (retaining the exact Tag value)
		/// </summary>
		public static TreeNode CloneProperly(TreeNode original)
		{
			//Clone the node
			TreeNode clone = (TreeNode)original.Clone();

			//Reset dictionary of guids
			ResetGuidDictionary();

			//Clone the expansion status and tags
			CloneProperly_CloneExpansionStatusAndTags(original, clone);

			return clone;
		}

        /// <summary>
        /// Inner subroutine that clones expansion status and other tags, called from CloneProperly.
        /// </summary>
        private static void CloneProperly_CloneExpansionStatusAndTags(TreeNode source, TreeNode destination)
        {
            //Expand destination if source is expanded
            if (source.IsExpanded)
                destination.Expand();

            //Clone tag if possible
            if (destination.Tag is MissionNode
                || destination.Tag is MissionNode_Comment
                || destination.Tag is MissionNode_Event
                || destination.Tag is MissionNode_Folder
                || destination.Tag is MissionNode_Start
                || destination.Tag is MissionNode_Unknown)
                destination.Tag = ((MissionNode)destination.Tag).Clone();

            int i;
            for (i = 0; i < source.Nodes.Count; i++)
                CloneProperly_CloneExpansionStatusAndTags(source.Nodes[i], destination.Nodes[i]);
        }

        /// <summary>
        /// Resets the GUID dictionary.
        /// </summary>
		public static void ResetGuidDictionary()
		{
			GuidDictionary.Clear();
		}

        /// <summary>
        /// Removes nodes from the string. 
        /// Used when pasting in order to remove node text and leave only statements.
        /// </summary>
		public static string RemoveNodes(string text)
		{
			foreach (string toDel in new string[] { "<event", "<start", "</event", "</start", "<folder" })
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

        /// <summary>
        /// Gets bounding rectangle for a node.
        /// </summary>
		public static Rectangle MakeDrawNodeRectangle(Rectangle nodeBounds)
		{
			return new Rectangle(nodeBounds.X - 1, nodeBounds.Y, nodeBounds.Size.Width+4, nodeBounds.Size.Height);
		}

        /// <summary>
        /// Compresses the string into byte array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static byte[] StringToCompressedByte(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return gZipBuffer;
        }

        /// <summary>
        /// Decompresses the string from a byte array.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string StringFromCompressedByte(byte[] gZipBuffer)
        {
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// Embeds string array into a single string line (line breaks replaced with "\")
        /// </summary>
        public static string StringArrayToLine(string[] arr)
        {
            string output = "";
            for (int i = 0; i < arr.Length; i++)
            {
                output += (i == 0 ? "" : "\\") + arr[i].Replace("\\", "\\\\");
            }

            return output;
        }

        /// <summary>
        /// Gets string array from single line (line breaks replaced with "\")
        /// </summary>
        public static string[] StringArrayFromLine(string str)
        {
            List<string> output = new List<string>();

            int curPos = -1;

            while (++curPos < str.Length)
            {
                if (str[curPos] != '\\')
                    continue;
                    
                if (curPos + 1 < str.Length && str[curPos + 1] == '\\')
                {
                    str = str.Substring(0, curPos + 1) + str.Substring(curPos + 2);
                    continue;
                }

                output.Add(str.Substring(0, curPos));
                str = str.Substring(curPos + 1);
                curPos = -1;
            }
            output.Add(str);

            return output.ToArray();
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
