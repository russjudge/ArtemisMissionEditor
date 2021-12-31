using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	public sealed class DependencyMiscCondition : DependencyCondition
	{
		public override bool IsActionStatisfying(DependencyAction action, ref int seconds, ref double probability)
		{
			return false;
		}
	}

	public sealed class DependencyTimerCondition : DependencyCondition
	{
		public string Name;

		public override bool IsActionStatisfying(DependencyAction action, ref int seconds, ref double probability)
		{
			seconds = 0;

			if (action is DependencyTimerAction)
				return ((DependencyTimerAction)action).Name == Name && Helper.IntTryParse(((DependencyTimerAction)action).Value, out seconds);
			else
				return false;
		}

		public DependencyTimerCondition(MissionStatement statement, string name)
		{
			Statement = statement;
			Name = name;
		}
	}

	public sealed class DependencyValueCondition : DependencyCondition
	{
		public string Name;
		public string Comparator;
		public string Value;

		public override bool IsActionStatisfying(DependencyAction action, ref int seconds, ref double probability)
		{
			probability = 1.0;

			if (action is DependencyValueAction)
			{
				DependencyValueAction va = (DependencyValueAction)action;
				
				if (va.Name != Name)
					return false;
				
				if (!Helper.DoubleTryParse(Value))
					return false;

				if (va.ValueExact == null && va.ValueMinInt != null && va.ValueMaxInt != null)			//random int
				{
					double value = Helper.StringToDouble(Value);
					double minInt = Helper.StringToDouble(va.ValueMinInt);
					double maxInt = Helper.StringToDouble(va.ValueMaxInt);
					//Check if values are integer
					if (value != Math.Round(value))
					{
						if (Comparator == "GREATER")
							value = Math.Truncate(value);
						if (Comparator == "GREATER_EQUAL")
							value = Math.Truncate(value) + 1;
						if (Comparator == "EQUALS")
							return false;
						if (Comparator == "NOT")
							return true;
						if (Comparator == "LESS_EQUAL")
							value = Math.Truncate(value);
						if (Comparator == "LESS")
							value = Math.Truncate(value) + 1;
					}
					if (minInt != Math.Round(minInt))
						return false;
					if (maxInt != Math.Round(maxInt))
						return false;

					switch (Comparator)
					{
						case "GREATER":
							if (value >= maxInt)
								return false;
							if (value < minInt)
								return true;
							probability = (maxInt - value) / (1 + maxInt - minInt);
							return true;
						case "GREATER_EQUAL":
							if (value > maxInt)
								return false;
							if (value <= minInt)
								return true;
							probability = (1 + maxInt - value) / (1 + maxInt - minInt);
							return true;
						case "EQUALS":
							if (value > maxInt)
								return false;
							if (value < minInt)
								return false;
							probability = 1 / (1 + maxInt - minInt);
							return true;
						case "NOT":
							if (value > maxInt)
								return true;
							if (value < minInt)
								return true;
							probability = (maxInt - minInt) / (1 + maxInt - minInt);
							return true;
						case "LESS_EQUAL":
							if (value >= maxInt)
								return true;
							if (value < minInt)
								return false;
							probability = (1 + value - minInt) / (1 + maxInt - minInt);
							return true;
						case "LESS":
							if (value > maxInt)
								return true;
							if (value <= minInt)
								return false;
							probability = (value - minInt) / (1 + maxInt - minInt);
							return true;
						default:
							return false;
					}
				}
				else if (va.ValueExact == null && va.ValueMinFloat != null && va.ValueMaxFloat != null)	//random float
				{
					double value = Helper.StringToDouble(Value);
					double minFloat = Helper.StringToDouble(va.ValueMinFloat);
					double maxFloat = Helper.StringToDouble(va.ValueMaxFloat);

					switch (Comparator)
					{
						case "GREATER":
							if (value >= maxFloat)
								return false;
							if (value < minFloat)
								return true;
							probability = (maxFloat - value) / (maxFloat - minFloat);
							return true;
						case "GREATER_EQUAL":		//Should add minimal possible value 
							if (value > maxFloat)
								return false;
							if (value <= minFloat)
								return true;
							probability = (maxFloat - value) / (maxFloat - minFloat);
							return true;
						case "EQUALS":				//Should add minimal possible value 
							if (value > maxFloat)
								return false;
							if (value < minFloat)
								return false;
							probability = 0 / (1 + maxFloat - minFloat);
							return true;
						case "NOT":
							if (value > maxFloat)
								return true;
							if (value < minFloat)
								return true;
							probability = (1 + maxFloat - minFloat) / (maxFloat - minFloat);
							return true;
						case "LESS_EQUAL":			//Should add minimal possible value 
							if (value > maxFloat)
								return true;
							if (value <= minFloat)
								return false;
							probability = (value - minFloat) / (maxFloat - minFloat);
							return true;
						case "LESS":
							if (value > maxFloat)
								return true;
							if (value <= minFloat)
								return false;
							probability = (value - minFloat) / (maxFloat - minFloat);
							return true;
						default:
							return false;
					}
				}
				else if (va.ValueExact == null || !Helper.DoubleTryParse(va.ValueExact))				//exact value is invalid
				{
					return false;
				}
				else																					//exact value is valid
				{
					switch (Comparator)
					{
						case "GREATER":
							return Helper.StringToDouble(va.ValueExact) >  Helper.StringToDouble(Value);
						case "GREATER_EQUAL":
							return Helper.StringToDouble(va.ValueExact) >= Helper.StringToDouble(Value);
						case "EQUALS":
							return Helper.StringToDouble(va.ValueExact) == Helper.StringToDouble(Value);
						case "NOT":
							return Helper.StringToDouble(va.ValueExact) != Helper.StringToDouble(Value);
						case "LESS_EQUAL":
							return Helper.StringToDouble(va.ValueExact) <= Helper.StringToDouble(Value);
						case "LESS":
							return Helper.StringToDouble(va.ValueExact) <  Helper.StringToDouble(Value);
						default:
							return false;
					}
				}
			}
			else
				return false;
		}

        public DependencyValueCondition(MissionStatement statement, string name, string comparator, string value)
		{
			Statement = statement;
			Name = name;
			Comparator = comparator;
			Value = value;
		}
	}

	public abstract class DependencyCondition
	{
		public List<DependencyPrecursor> Precursors;

		public DependencyPrecursor GetPrecursor(DependencyEvent _event)
		{
			foreach (DependencyPrecursor item in Precursors)
				if (item.Event == _event)
					return item;
			return null;
		}

		/// <summary> Check if action statisfies a condition </summary>
		public abstract bool IsActionStatisfying(DependencyAction action, ref int seconds, ref double probability);
		
		/// <summary> Check if any action from event statisfies the condition, add event as a precursor if it does </summary>
		public int CheckAgainstEvent(DependencyEvent item, ref int seconds, ref double probability)
		{
			double p = 1.0;
			int s = 0;

			foreach (DependencyAction action in item.Actions)
				if (IsActionStatisfying(action, ref s, ref p))
				{
					Precursors.Add(new DependencyPrecursor(item, s, p,0,action.Statement));
					seconds = Math.Max(seconds, s);
					probability *= p;
					return 1;
				}
			
			return 0;
		}

		public MissionStatement Statement;

		public DependencyCondition()
		{
			Precursors = new List<DependencyPrecursor>();
			Statement = null;
		}

		public static DependencyCondition NewFromStatement(MissionStatement item)
		{
			switch (item.Name)
			{
				case "if_variable":
					return new DependencyValueCondition(item, item.GetAttribute("name"), item.GetAttribute("comparator"), item.GetAttribute("value"));
				case "if_timer_finished":
					return new DependencyTimerCondition(item, item.GetAttribute("name"));
				default:
					return new DependencyMiscCondition();
			}
		}
	}
}
