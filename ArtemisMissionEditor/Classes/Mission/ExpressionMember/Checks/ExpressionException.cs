using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace ArtemisMissionEditor.Classes.Mission.ExpressionMember.Checks
{
    [Serializable]
    public class ExpressionException : Exception
    {
        public ExpressionException() : base() { }
        public ExpressionException(string message) : base(message) { }

    }
}
