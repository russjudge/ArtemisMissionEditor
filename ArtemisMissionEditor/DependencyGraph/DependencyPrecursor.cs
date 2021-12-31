using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public sealed class DependencyPrecursor
	{
		public DependencyEvent Event;
		
		public int Seconds;

		public double Probability;

		/// <summary> Count of conditions this event statisfied </summary>
		public int Count;

		public MissionStatement Statement;

		public DependencyPrecursor(DependencyEvent _event, int seconds = 0, double probability = 1.0, int count = 0, MissionStatement statement = null)
		{
			Event = _event;
			Seconds = seconds;
			Probability = probability;
			Count = count;
			Statement = statement;
		}
	}
}