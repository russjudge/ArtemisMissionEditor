using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ArtemisMissionEditor
{
	public partial class FormMissionProperties : ArtemisMissionEditor.FormSerializeableToRegistry
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
			if (!Program.IsClosing)
			{
				e.Cancel = true;
				this.Hide();
			}

			SaveToRegistry();
		}

		private void _FM_b_OK_Click(object sender, EventArgs e)
		{
			try
			{
				string x;
				foreach (string item in Program.FormMissionPropertiesInstance._FMP_tb_Before.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.None))
					x = new XmlDocument().CreateComment(item).OuterXml;

			}
			catch (Exception ee)
			{
				MessageBox.Show("Invalid comment in \"before\" section:\r\n"+ee.Message, "Commentary error");
				return;
			}

			try
			{
				string x;
				foreach (string item in Program.FormMissionPropertiesInstance._FMP_tb_After.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.None))
					x = new XmlDocument().CreateComment(item).OuterXml;
			}
			catch (Exception ee)
			{
				MessageBox.Show("Invalid comment in \"after\" section:\r\n" + ee.Message, "Commentary error");
				return;
			}

			Mission.Current._commentsBefore = Program.FormMissionPropertiesInstance._FMP_tb_Before.Text;
			Mission.Current._commentsAfter = Program.FormMissionPropertiesInstance._FMP_tb_After.Text;
			Mission.Current.BgNode = (TreeNode)_FMP_tb_Node.Tag;
			Mission.Current.RegisterChange("Changed mission properties");
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void _FormMissionProperties_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
			if (e.KeyData == (Keys.Enter | Keys.Shift))
				_FM_b_OK_Click(null, null);
		}

		public void ReadDataFromMission()
		{
			_FMP_tb_Before.Text = Mission.Current._commentsBefore;
			_FMP_tb_After.Text = Mission.Current._commentsAfter;
			_FMP_tb_Node.Tag = Mission.Current.BgNode;
			UpdateBgNode();
		}

		public void UpdateBgNode()
		{
			TreeNode tNode = (TreeNode)_FMP_tb_Node.Tag;
			MissionNode mNode = (MissionNode)tNode.Tag;
			_FMP_tb_Node.Text = tNode.Text + " - " + Mission.Current.CanInvokeSpaceMapCreate(tNode) + " create statement(s)" + "";
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
	}
}
