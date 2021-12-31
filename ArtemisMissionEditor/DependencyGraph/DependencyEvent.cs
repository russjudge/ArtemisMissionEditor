using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public sealed class DependencyEvent
	{
		/// <summary>
		/// Node that this class item represents
		/// </summary>
		public TreeNode Node;

		public int MiscConditionCount;

		public List<DependencyCondition> Conditions;

		public List<DependencyAction> Actions;

		public List<DependencyPrecursor> Precursors;

		public DependencyPrecursor GetPrecursor(DependencyEvent _event)
		{
			foreach (DependencyPrecursor item in Precursors)
				if (item.Event == _event)
					return item;
			return null;
		}

		/// <summary> Fill this event's list of precursors with other event where event actions are applying </summary>
		public void Fill(DependencyEvent item)
		{
			double p = 1.0;
			int s = 0;
			int statisfied = 0;

			foreach (DependencyCondition condition in Conditions)
				statisfied += condition.CheckAgainstEvent(item, ref s, ref p);

			if (statisfied > 0)
				Precursors.Add(new DependencyPrecursor(item, s, p, statisfied));

		}

		private void Conditions_Add(MissionStatement item)
		{
			DependencyCondition dc = DependencyCondition.NewFromStatement(item);

			MiscConditionCount += dc is DependencyMiscCondition ? 1 : 0;
			
			Conditions.Add(dc);
		}

		private void Actions_Add(MissionStatement item)
		{
			DependencyAction da = DependencyAction.NewFromStatement(item);

			//Remove duplicate if present
			for (int i = Actions.Count-1;i>=0;i--)
				if (Actions[i].IsDuplicate(da))
					Actions.RemoveAt(i);

			Actions.Add(da);
		}

		public DependencyEvent(TreeNode node)
		{
			Node = node;
			MiscConditionCount = 0;
			Conditions = new List<DependencyCondition>();
			Actions = new List<DependencyAction>();
			Precursors = new List<DependencyPrecursor>();

			foreach(MissionStatement item in ((MissionNode)Node.Tag).Conditions)
				if (item.Kind == MissionStatementKind.Condition)
					Conditions_Add(item);
				
			foreach(MissionStatement item in ((MissionNode)Node.Tag).Actions)
				if (item.Kind == MissionStatementKind.Action)
					Actions_Add(item);
		}
	}
}
