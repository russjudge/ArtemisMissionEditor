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

    public sealed class ExpressionMemberCheck_Side : ExpressionMemberCheck
    {
        public override string Decide(ExpressionMemberContainer container)
        {
            if (container.GetAttribute("sideValue") == null)
                return _choices[0]; // point
            else
                return _choices[1]; // use_gm_pos
        }

        public override string GetValue(ExpressionMemberContainer container)
        {
            return base.GetValue(container);
        }

        public override string GetValueDisplay(ExpressionMemberContainer container)
        {
            return base.GetValueDisplay(container);
        }

        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            base.SetValueInternal(container, value);
        }

        public ExpressionMemberCheck_Side()
            : base("", EMVD.GetItem("<blank>"))
        {
            List<ExpressionMember> eML;
            
            eML = this.Add("default"); //_choices[0]
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("default", EMVD.GetItem("sideValue"), "sideValue"));
            eML.Add(new ExpressionMember("side "));

            eML = this.Add("specified"); //_choices[1]
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("side "));
            eML.Add(new ExpressionMember("default", EMVD.GetItem("sideValue"), "sideValue"));
        }
    }
}