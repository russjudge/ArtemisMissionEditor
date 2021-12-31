using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// The exppression member that represents a commentary statement
    /// </summary>
    public class ExpressionMember_Commentary : ExpressionMember
	{
        /// <summary>
        /// Get value of current member (Xml element's body).
        /// </summary>
        public override string GetValue(ExpressionMemberContainer container)
		{
			return container.Statement.Body;
		}

        /// <summary>
        /// Set value of the current member, including all checks (like, sets to null if this is a "store only if filled" value etc...)
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			string old = container.Statement.Body;
			if (value != null)
				container.Statement.Body = value;
			else
				container.Statement.Body = "";
			if (old != container.Statement.Body)
			{
				try
				{
					// Miscrosoft.Preformance is so helpful! Yeah sure, if I could just call OuterXml without assigning it, I'd never do this!
                    string justSoThatOuterXmlGetLogicIsCalled = container.Statement.ToXml(new XmlDocument()).OuterXml;
				}
				catch
				{
					container.Statement.Body = old;
				}
			}

		}

        /// <summary>
        /// Get value of current member to display in the label (meaning, exactly what will appear in GUI labels)
        /// </summary>
        public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			string result = GetValue(container);
			return string.IsNullOrWhiteSpace(result) ? " - - - - - - - - - - " : result;
		}

        public ExpressionMember_Commentary() : base("", ExpressionMemberValueDescriptions.Commentary) { }
	}

}