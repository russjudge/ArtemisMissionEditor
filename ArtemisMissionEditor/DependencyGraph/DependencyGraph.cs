using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{	
	public sealed class DependencyGraph
	{
		public List<DependencyEvent> Events;

		private bool _valid;
		public bool Valid { get { return _valid; } }

		/// <summary> Mark dependency graph as requiring recalculation </summary>
		public void Invalidate()
		{
			_valid = false;
		}

		public void Recalculate(Mission mission)
		{
			if (Valid)
				return;
			_valid = true;

			//Get all events
			Events.Clear();
			foreach (TreeNode node in mission.GetNodes())
				if (node.Tag is MissionNode_Event|| node.Tag is MissionNode_Start)
					Events.Add(new DependencyEvent(node));

			//Fill all events
			foreach (DependencyEvent a in Events)
				foreach (DependencyEvent b in Events)
					if (a != b)
						a.Fill(b);
		}

		public DependencyEvent GetEvent(TreeNode node)
		{
			foreach (DependencyEvent item in Events)
				if (item.Node == node)
					return item;
			return null;
		}

		public DependencyGraph()
		{
			_valid = false;
			Events = new List<DependencyEvent>();
		}
	}


}
