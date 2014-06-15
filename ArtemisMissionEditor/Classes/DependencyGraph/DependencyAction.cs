using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public sealed class DependencyMiscAction : DependencyAction
	{
		public override bool IsDuplicate(DependencyAction item)
		{
			return false;
		}
	}

	public sealed class DependencyTimerAction : DependencyAction
	{
		public string Name;
		public string Value;

		public override bool IsDuplicate(DependencyAction item)
		{
			if (item is DependencyTimerAction)
				return ((DependencyTimerAction)item).Name == Name;
			else
				return false;
		}

		public DependencyTimerAction(MissionStatement statement, string name, string value)
		{
			Statement = statement;
			Name = name;
			Value = value;
		}
	}

	public sealed class DependencyValueAction : DependencyAction
	{
		public string Name;
		public string ValueExact;
		public string ValueMinInt;
		public string ValueMaxInt;
		public string ValueMinFloat;
		public string ValueMaxFloat;

		public override bool IsDuplicate(DependencyAction item)
		{
			if (item is DependencyValueAction)
				return ((DependencyValueAction)item).Name == Name;
			else
				return false;
		}

		public DependencyValueAction(MissionStatement statement, string name, string value, string valueMinInt, string valueMaxInt, string valueMinFloat, string valueMaxFloat)
		{
			Statement = statement;
			Name = name;
			ValueExact = value;
			ValueMinInt = valueMinInt;
			ValueMaxInt = valueMaxInt;
			ValueMinFloat = valueMinFloat;
			ValueMaxFloat = valueMaxFloat;
		}
	}
	
	public abstract class DependencyAction
	{
		/// <summary> If action is a duplicate of existing action </summary>
		public abstract bool IsDuplicate(DependencyAction item);

		public MissionStatement Statement;

		public DependencyAction()
		{
			Statement = null;
		}

		public static DependencyAction NewFromStatement(MissionStatement item)
		{
			switch (item.Name)
			{
				case "set_variable":
					if (item.GetAttribute("value") == null && (item.GetAttribute("randomIntLow") != null || item.GetAttribute("randomIntHigh") != null))
						return new DependencyValueAction(item, item.GetAttribute("name"), null, item.GetAttribute("randomIntLow"), item.GetAttribute("randomIntHigh"), null, null);
					if (item.GetAttribute("value") == null && (item.GetAttribute("randomFloatLow") != null || item.GetAttribute("randomFloatHigh") != null))
						return new DependencyValueAction(item, item.GetAttribute("name"), null, null, null, item.GetAttribute("randomFloatLow"), item.GetAttribute("randomFloatHigh"));
					else
						return new DependencyValueAction(item, item.GetAttribute("name"), item.GetAttribute("value"), null, null, null, null);
				case "set_timer":
					return new DependencyTimerAction(item, item.GetAttribute("name"), item.GetAttribute("seconds"));
				default:
					return new DependencyMiscAction();
			}
		}
	}
}
