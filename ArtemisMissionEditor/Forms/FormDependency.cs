using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public partial class FormDependency : FormSerializeableToRegistry
    {
        public FormDependency()
        {
            InitializeComponent();

            _FD_b_Event.Tag = new MissionSearchResult();
            _FD_b_Event.Text = "";
        }

        public void OpenEventDependency(TreeNode node)
        {
            if (node != null)
            {
                _FD_lb_Left.Items.Clear();
                _FD_lb_Right.Items.Clear();

				Mission.Current.RecalculateDependencies();

				DependencyEvent dependencyEvent = Mission.Current.Dependencies.GetEvent(node);
				if (dependencyEvent != null)
				{
					DependencyEvent successor;
						
					#region Fill Precursors

					successor = dependencyEvent;
					List<MissionSearchResult> listPrecursor = new List<MissionSearchResult>();
				
					foreach (DependencyPrecursor item in successor.Precursors)
					{
						string tag;
						bool enoughFull = item.Count == successor.Conditions.Count;
						bool enoughMinus = item.Count + successor.MiscConditionCount == successor.Conditions.Count;
						int strongCount = 0;
						foreach (DependencyCondition condition in successor.Conditions)
							strongCount += condition.GetPrecursor(item.Event) != null && condition.Precursors.Count == 1 ? 1 : 0;

						//[ STR+EN ] [06/12] [12/12] [65s] [100%] 
						if (enoughFull && strongCount > 0)
							tag = "[ STR+EN ]";
						else if (enoughMinus && strongCount > 0)
							tag = "[ STR+EN-]";
						else if (strongCount > 0)
							tag = "[ STRONG ]";
						else if (enoughFull)
							tag = "[ ENOUGH ]";
						else if (enoughMinus)
							tag = "[ ENOUGH-]";
						else
							tag = "[  WEAK  ]";

						tag += "[" + strongCount.ToString("00") + "/" + item.Count.ToString("00") + "/" + (successor.Conditions.Count - successor.MiscConditionCount).ToString("00") + "]";
						tag += "[" + successor.MiscConditionCount.ToString("00") + "]";
						tag += item.Seconds == 0 ? "[----]" : item.Seconds > 999 ? "[>999]" : "[" + item.Seconds.ToString("000") + "s]";
						tag += "[" + string.Format("{0,4}", item.Probability.ToString("##0%")) + "]";

						string suffix = "";
						foreach (DependencyCondition condition in successor.Conditions)
						{
							DependencyPrecursor dp = condition.GetPrecursor(item.Event);
							if (dp != null)
							{
								suffix += "[" + dp.Statement.Text.Replace("Set timer","ST").Replace("Set variable","SV").Replace(" second(s)","s") + "] ";
							}
						}
						suffix = suffix.Length > 2 ? suffix.Substring(0, suffix.Length - 1) : suffix;
						listPrecursor.Add(new MissionSearchResult(tag + " ", " " + suffix, item.Event, strongCount * 1000 * 1000 * 1000 + item.Count * 1000 * 1000 / (successor.Conditions.Count - successor.MiscConditionCount) + item.Count * 1000 + (1000 - item.Seconds)));
					}

					listPrecursor.Sort((MissionSearchResult x, MissionSearchResult y) => -(x.SortOrder - y.SortOrder));

					foreach (MissionSearchResult item in listPrecursor)
						_FD_lb_Left.Items.Add(item);

					#endregion 

					#region Fill Successors

					List<MissionSearchResult> listSuccessor = new List<MissionSearchResult>();

					foreach (DependencyEvent evt in Mission.Current.Dependencies.Events)
					{
						successor = evt;
						DependencyPrecursor item = evt.GetPrecursor(dependencyEvent);
						if (item == null)
							continue;

						string tag;
						bool enoughFull = item.Count == successor.Conditions.Count;
						bool enoughMinus = item.Count + successor.MiscConditionCount == successor.Conditions.Count;
						int strongCount = 0;
						foreach (DependencyCondition condition in successor.Conditions)
							strongCount += condition.GetPrecursor(item.Event) != null && condition.Precursors.Count == 1 ? 1 : 0;

						//[ STR+EN ] [06/12] [12/12] [65s] [100%] 
						if (enoughFull && strongCount > 0)
							tag = "[ STR+EN ]";
						else if (enoughMinus && strongCount > 0)
							tag = "[ STR+EN-]";
						else if (strongCount > 0)
							tag = "[ STRONG ]";
						else if (enoughFull)
							tag = "[ ENOUGH ]";
						else if (enoughMinus)
							tag = "[ ENOUGH-]";
						else
							tag = "[  WEAK  ]";

						tag += "[" + strongCount.ToString("00") + "/" + item.Count.ToString("00") + "/" + (successor.Conditions.Count - successor.MiscConditionCount).ToString("00") + "]";
						tag += "[" + successor.MiscConditionCount.ToString("00") + "]";
						tag += item.Seconds == 0 ? "[----]" : item.Seconds > 999 ? "[>999]" : "[" + item.Seconds.ToString("000") + "s]";
						tag += "[" + string.Format("{0,4}", item.Probability.ToString("##0%")) + "]";

						string suffix = "";
						foreach (DependencyCondition condition in successor.Conditions)
						{
							DependencyPrecursor dp = condition.GetPrecursor(item.Event);
							if (dp != null)
							{
								suffix += "[" + dp.Statement.Text.Replace("Set timer", "ST").Replace("Set variable", "SV").Replace(" second(s)", "s") + "] ";
							}
						}
						suffix = suffix.Length > 2 ? suffix.Substring(0, suffix.Length - 1) : suffix;
						listSuccessor.Add(new MissionSearchResult(tag + " ", " " + suffix, successor, strongCount * 1000 * 1000 * 1000 + item.Count * 1000 * 1000 / (successor.Conditions.Count - successor.MiscConditionCount) + item.Count * 1000 + (1000 - item.Seconds)));
					}

					listSuccessor.Sort((MissionSearchResult x, MissionSearchResult y) => -(x.SortOrder - y.SortOrder));

					foreach (MissionSearchResult item in listSuccessor)
						_FD_lb_Right.Items.Add(item);
					
					#endregion
				}
											
				_FD_tc_Main_tp_1.Text = "Preceeding [" + _FD_lb_Left.Items.Count + "]";
				_FD_tc_Main_tp_2.Text = "Succeeding [" + _FD_lb_Right.Items.Count + "]";

                //Remember the event itself
				if (dependencyEvent != null)
					_FD_b_Event.Tag = new MissionSearchResult("", "", Mission.Current.Dependencies.GetEvent(node));
				else
					_FD_b_Event.Tag = new MissionSearchResult();
                _FD_b_Event.Text = node.Text;
            }

			if (((MissionSearchResult)_FD_b_Event.Tag).Valid)
			{
				Show();
				BringToFront();
			}
			else
			{
				Close();
			}
        }

        private void _E_FD_Load(object sender, EventArgs e)
        {
			_mainSC = null;
            _rightSC = null;
            ID = "FormDependency";
            LoadFromRegistry();
        }

        private void _E_FD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.IsClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void _E_FD_b_Event_Click(object sender, EventArgs e)
        {
			_FD_lb_Left.SelectedItem = null;
			_FD_lb_Right.SelectedItem = null;
            ((MissionSearchResult)((Button)sender).Tag).Activate();
			Program.FormMainInstance.BringToFront();
			Program.FormDependencyInstance.BringToFront();
        }

        private void _E_FD_lb_Left_Click(object sender, EventArgs e)
        {
			if (_FD_lb_Left.SelectedItem != null)
			{
				_FD_lb_Right.SelectedItem = null;
				((MissionSearchResult)_FD_lb_Left.SelectedItem).Activate(((MissionSearchResult)_FD_b_Event.Tag).Event, true);
				Program.FormMainInstance.BringToFront();
				Program.FormDependencyInstance.BringToFront();
			}
        }

        private void _E_FD_lb_Right_Click(object sender, EventArgs e)
        {
			if (_FD_lb_Right.SelectedItem != null)
			{
				_FD_lb_Left.SelectedItem = null;
				((MissionSearchResult)_FD_lb_Right.SelectedItem).Activate(((MissionSearchResult)_FD_b_Event.Tag).Event, false);
				Program.FormMainInstance.BringToFront();
				Program.FormDependencyInstance.BringToFront();
			}
        }

		private void _E_FD_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				Close();
			}
			if (e.KeyData == Keys.F4)
			{
				e.SuppressKeyPress = true;
				Mission.Current.ShowEventDependencyForm(true);
			}
		}

		private void _FormDependency_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;

			MessageBox.Show("Click the button at the top to show the event for which the dependencies are shown.\r\n"
				+ "Click the entry in the list to show the precursor or succeeding node.\r\n"
				+ "\r\n"
				+ "The nodes are marked with the following tags:\r\n"
				+ "[  TYPE  ] - shows the general kind of the link between the nodes,\r\n"
				+ "   - STRONG means this node must happen before the succeeding node can happen, since its the only node in the mission that futfills one of the conditions of the succeeding node\r\n"
				+ "   - ENOUGH means this node is enough for the succeeding node to happen\r\n"
				+ "   - ENOUGH- means this node is enough for the succeeding node to happen if the other conditions (depending on the space map state) will be fulfilled\r\n"
				+ "   - STR+EN means both STRONG and ENOUGH both apply\r\n"
				+ "   - STR+EN- means both STRONG and ENOUGH- both apply\r\n"
				+ "   - WEAK means this node is neither a strong requirement nor enough\r\n"
				+ "[SS/FF/TT] - shows the amount of conditions,\r\n"
				+ "   - SS, first two digits, stand for amount of fulfilled conditions strongly linked in this event\r\n"
				+ "   - FF, second two digits, stand for amount of futfilled conditions in this event\r\n"
				+ "   - TT, third two digits, stand for total amount of conditions in succeeding event (except those that rely on space map state)\r\n"
				+ "[SM] - shows the amount of conditions in succeeding event that rely on space map state\r\n"
				+ "[XXXs] - shows the amount of seconds that must pass before succeeding event will fire\r\n"
				+ "[XXX%] - shows the percent chance for the succeeding event to happen (this can be other than 100% if random variable value is set)"
				,"Dependencies");
		}

		private void _E_FD_lb_Left_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				_E_FD_lb_Left_Click(sender, e);
		}

		private void _E_FD_lb_Right_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				_E_FD_lb_Right_Click(sender, e);
		}

    }

}
