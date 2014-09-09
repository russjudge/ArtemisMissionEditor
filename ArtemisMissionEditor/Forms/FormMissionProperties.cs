using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ArtemisMissionEditor.Forms
{
	public partial class FormMissionProperties : FormSerializeableToRegistry
	{
		public FormMissionProperties()
		{
			InitializeComponent();
		}

		private void _FormMissionProperties_Load(object sender, EventArgs e)
		{
			_mainSC = null;
			_rightSC = null;
			ID = "FormMissionProperties";
			LoadFromRegistry();
		}

		private void _FormMissionProperties_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
            else
                SaveToRegistry();
		}

		private void _FM_b_OK_Click(object sender, EventArgs e)
		{
            try
            {
                string justSoThatOuterXmlGetLogicIsCalled;
                foreach (string item in Program.FormMissionPropertiesInstance._FMP_tb_Before.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.None))
                    // Miscrosoft.Preformance is so helpful! Yeah sure, if I could just call OuterXml without assigning it, I'd never do this!
                    justSoThatOuterXmlGetLogicIsCalled = new XmlDocument().CreateComment(item).OuterXml;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid comment in \"before\" section:\r\n" + ex.Message + " " + ex.StackTrace, "Commentary error");
                return;
            }

            try
            {
                string x;
                foreach (string item in Program.FormMissionPropertiesInstance._FMP_tb_After.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.None))
                    x = new XmlDocument().CreateComment(item).OuterXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid comment in \"after\" section:\r\n" + ex.Message + " " + ex.StackTrace, "Commentary error");
                return;
            }

            Mission.Current.CommentsBeforeRoot =_FMP_tb_Before.Text;
			Mission.Current.CommentsAfterRoot = _FMP_tb_After.Text;
            Mission.Current.VersionNumber = _FMP_tb_Version.Text;
			Mission.Current.BackgroundNode = (TreeNode)_FMP_tb_Node.Tag;
            Mission.Current.PlayerShipNames = _FMP_tb_PN.Lines;
			Mission.Current.RegisterChange("Changed mission properties");
            Hide();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            Hide();
		}

		private void _FormMissionProperties_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
                Hide();
			if (e.KeyData == (Keys.Enter | Keys.Shift))
				_FM_b_OK_Click(null, null);
		}

		public void ReadDataFromMission()
		{
			_FMP_tb_Before.Text = Mission.Current.CommentsBeforeRoot;
			_FMP_tb_After.Text = Mission.Current.CommentsAfterRoot;
            _FMP_tb_Version.Text = Mission.Current.VersionNumber;
			_FMP_tb_Node.Tag = Mission.Current.BackgroundNode;
            _FMP_tb_PN.Lines = Mission.Current.PlayerShipNames;
			UpdateBgNode();
		}

		public void UpdateBgNode()
		{
			TreeNode tNode = (TreeNode)_FMP_tb_Node.Tag;
			//MissionNode mNode = (MissionNode)tNode.Tag;
			_FMP_tb_Node.Text = tNode.Text + " - " + Mission.Current.GetAmountOfCreateActionsInStatement(tNode) + " create statement(s)" + "";
		}

		private void button3_Click(object sender, EventArgs e)
		{
			_FMP_tb_Node.Tag = Mission.Current.StartNode;

			UpdateBgNode();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			_FMP_tb_Node.Tag = Mission.Current.GetSelectedNode();

			UpdateBgNode();
        }

        private void FormMissionProperties_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                SaveToRegistry();
                Program.ShowMainFormIfRequired();
            }
        }
	}
}
