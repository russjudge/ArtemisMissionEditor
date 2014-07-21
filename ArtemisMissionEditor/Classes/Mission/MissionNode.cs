using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArtemisMissionEditor
{
	/// <summary> Contains data about one mission node (Start, Event, Folder or Comment)</summary>
    [Serializable]
    public abstract class MissionNode : ICloneable
    {
        public Guid? ID { get; set; }
        public Guid? ParentID { get; set; }
		public List<string> ExtraAttributes { get; set; }
		protected int _defaultName;
		public int DefaultName { get { return _defaultName; } }
		protected string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; _defaultName = 0; }
		}

        public List<MissionStatement> Conditions { get; set; }
		public List<MissionStatement> Actions { get; set; }

        #region SHARED
        //R. Judge: store item on MissionNode with Missionstatement
        public static MissionNode NewFromXML(XmlNode item)
        {
            MissionNode mn = null;

            switch (item.Name)
            {
                case "event":
                    mn = new MissionNode_Event();
                    break;
				case "disabled_event":
					mn = new MissionNode_Event();
					break;
				case "start":
                    mn = new MissionNode_Start();
                    break;
                case "#comment":
                    mn = new MissionNode_Comment();
                    break;
                case "folder_arme":
                    mn = new MissionNode_Folder();
                    break;
                default:
                    mn = new MissionNode_Unknown();
                    break;
            }

            mn.FromXml(item,true);
            return mn;
        }

        #endregion

        #region INHERITANCE

        public abstract int ImageIndex { get; }

        //TO XML
        public abstract XmlNode ToXml(XmlDocument xDoc, bool full = false);
        
        //FROM XML
        public abstract void FromXml(XmlNode item, bool countPlusOne = false);

		public object Clone()
		{
			//Code to serialize generally anything, but it doesnt preserve links!
			//MemoryStream ms = new MemoryStream();
			//BinaryFormatter bf = new BinaryFormatter();
			//bf.Serialize(ms, this);
			//ms.Position = 0;
			//object obj = bf.Deserialize(ms);
			//ms.Close();
			//return obj;

			//Copy mission node
			MissionNode result = MissionNode.NewFromXML(this.ToXml(new XmlDocument(), true));

			//Exchange IDs
			if (result.ID.HasValue)
				if (Helper.GuidDictionary.ContainsKey(result.ID.Value))
					result.ID = Helper.GuidDictionary[result.ID.Value];
				else
					Helper.GuidDictionary.Add(result.ID.Value, (result.ID = Guid.NewGuid()).Value);
			if (result.ParentID.HasValue)
				if (Helper.GuidDictionary.ContainsKey(result.ParentID.Value))
					result.ParentID = Helper.GuidDictionary[result.ParentID.Value];
				else
					Helper.GuidDictionary.Add(result.ParentID.Value, (result.ParentID = Guid.NewGuid()).Value);

			return result;
		}

        public MissionNode()
        {
            ID = null;
            ParentID = null;
            _name = "";
			_defaultName = 0;
			ExtraAttributes = new List<string>();
            Conditions = new List<MissionStatement>();
			Actions = new List<MissionStatement>();
        }

        #endregion
    }

	[Serializable]
	public sealed class MissionNode_Event : MissionNode
    {
		public bool Enabled;

        #region INHERITANCE

        public override int ImageIndex { get { return Enabled ? 0 : 107; } }

		public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement(Enabled ? "event" : "disabled_event");
            if (_defaultName == 0 || full)
            {
                xAttr = xDoc.CreateAttribute("name_arme");
                xAttr.Value = _name;
                xElem.Attributes.Append(xAttr);

            }
            xAttr = xDoc.CreateAttribute("id_arme");
            xAttr.Value = ID.ToString();
            xElem.Attributes.Append(xAttr);
            if (ParentID != null)
            {
                xAttr = xDoc.CreateAttribute("parent_id_arme");
                xAttr.Value = ParentID.ToString();
                xElem.Attributes.Append(xAttr);
            }

            foreach (MissionStatement item in Conditions)
                xElem.AppendChild(item.ToXml(xDoc,full));
			foreach (MissionStatement item in Actions)
				xElem.AppendChild(item.ToXml(xDoc,full));

            return xElem;
        }

		public override void FromXml(XmlNode item, bool countPlusOne = false)
        {
			_defaultName = (Mission.Current.EventCount + (countPlusOne ? 1 : 0));
            _name = "Event " + DefaultName;
			Enabled = item.Name == "event";
            ID = null;
            ParentID = null;
            Conditions.Clear();
			Actions.Clear();

			List<MissionStatement> commentHolder = new List<MissionStatement>();

            foreach (XmlAttribute att in item.Attributes)
            {
                if (att.Name == "id_arme")
                    ID = new Guid(att.Value);
                if (att.Name == "parent_id_arme") 
                    ParentID = new Guid(att.Value);
				if (att.Name == "name_arme" || att.Name == "name")
				{
					Name = att.Value;
					if (Name.Contains("Event") && Helper.IntTryParse(Name.Substring(5, Name.Length - 5)))
						_defaultName = Helper.StringToInt(Name.Substring(5, Name.Length - 5));
				}
            }

            if (ID == null)
                ID = Guid.NewGuid();

			foreach (XmlNode xNode in item.ChildNodes)
			{
				MissionStatement nStatement = MissionStatement.NewFromXML(xNode,this);
				switch (nStatement.Kind)
				{
					case MissionStatementKind.Action:
						{
							foreach (MissionStatement mS in commentHolder)
								Actions.Add(mS);
							commentHolder.Clear();
							Actions.Add(nStatement);
						}
						break;
					case MissionStatementKind.Condition:
						{
							foreach (MissionStatement mS in commentHolder)
								Conditions.Add(mS);
							commentHolder.Clear();
							Conditions.Add(nStatement);
						}
						break;
					case MissionStatementKind.Commentary:
						commentHolder.Add(nStatement);
						break;
				}
			}

			foreach (MissionStatement mS in commentHolder)
				Actions.Add(mS);

			
        }

        public MissionNode_Event()
            : base()
        {
            ID = Guid.NewGuid();
            ParentID = null;
			Enabled = true;
			_name = "Event " + Mission.Current.EventCount;
        }
        #endregion
    }

	[Serializable]
	public sealed class MissionNode_Start : MissionNode
    {
        #region INHERITANCE

        public override int ImageIndex { get { return 1; } }

		public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement("start");
            if (_defaultName == 0)
            {
                xAttr = xDoc.CreateAttribute("name_arme");
                xAttr.Value = _name;
                xElem.Attributes.Append(xAttr);

            }
            //xAttr = xDoc.CreateAttribute("id_arme");
            //xAttr.Value = ID.ToString();
            //xElem.Attributes.Append(xAttr);
            //if (ParentID != null)
            //{
            //    xAttr = xDoc.CreateAttribute("parent_id_arme");
            //    xAttr.Value = ParentID.ToString();
            //    xElem.Attributes.Append(xAttr);
            //}

			//foreach (MissionStatement item in Conditions)
			//	xElem.AppendChild(item.ToXml(xDoc));
			foreach (MissionStatement item in Actions)
				xElem.AppendChild(item.ToXml(xDoc,full));

            return xElem;
        }

		public override void FromXml(XmlNode item, bool countPlusOne = false)
        {
            _defaultName = 1;
            _name = "Start";
            ID = null;
            ParentID = null;
            Conditions.Clear();
			Actions.Clear();

			List<MissionStatement> commentHolder = new List<MissionStatement>();

            foreach (XmlAttribute att in item.Attributes)
            {
                //if (att.Name == "id_arme")
                //    ID = new Guid(att.Value);
                //if (att.Name == "parent_id_arme") // shouldnt it never happen to have one?
                //    ParentID = new Guid(att.Value);
				if (att.Name == "name_arme" || att.Name == "name")
                    Name = att.Value;
            }


			foreach (XmlNode xNode in item.ChildNodes)
			{
				MissionStatement nStatement = MissionStatement.NewFromXML(xNode, this);
				switch (nStatement.Kind)
				{
					case MissionStatementKind.Action:
						{
							foreach (MissionStatement mS in commentHolder)
								Actions.Add(mS);
							commentHolder.Clear();
							Actions.Add(nStatement);
						}
						break;
					case MissionStatementKind.Condition:
						{
							Log.Add("Found a condition in start block! It was ignored");	
						}
						break;
					case MissionStatementKind.Commentary:
						commentHolder.Add(nStatement);
						break;
				}
			}

			foreach (MissionStatement mS in commentHolder)
				Actions.Add(mS); 
        }

        public MissionNode_Start()
            : base()
        {
            ID = Guid.NewGuid();
            ParentID = null;
            _name = "Start";
        }

        #endregion
    }

	[Serializable]
	public sealed class MissionNode_Comment : MissionNode
    {
        #region INHERITANCE

        public override int ImageIndex { get { return 152; } }

		public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlComment xCom = xDoc.CreateComment(_name);
            return xCom;
        }

		public override void FromXml(XmlNode item, bool countPlusOne = false)
        {
            _name = item.Value;
        }

        public MissionNode_Comment()
            : base()
        {
            ID = Guid.NewGuid();
            ParentID = null;
            _name = "";
        }

        #endregion
    }

	[Serializable]
	public sealed class MissionNode_Folder : MissionNode
    {
        #region INHERITANCE

        public override int ImageIndex { get { return 2; } }

		public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement("folder_arme");
            if (_defaultName == 0)
            {
                xAttr = xDoc.CreateAttribute("name_arme");
                xAttr.Value = _name;
                xElem.Attributes.Append(xAttr);
            
            }
            xAttr = xDoc.CreateAttribute("id_arme");
            xAttr.Value = ID.ToString();
            xElem.Attributes.Append(xAttr);
            if (ParentID != null)
            {
                xAttr = xDoc.CreateAttribute("parent_id_arme");
                xAttr.Value = ParentID.ToString();
                xElem.Attributes.Append(xAttr);
            }
			foreach(string item in ExtraAttributes)
			{
				xAttr = xDoc.CreateAttribute(item);
				xAttr.Value = "";
				xElem.Attributes.Append(xAttr);
			}

            return xElem;
        }

		public override void FromXml(XmlNode item, bool countPlusOne = false)
        {
            _name = "Folder";
            ID = null;
            ParentID = null;
			ExtraAttributes.Clear();
			Conditions.Clear();
			Actions.Clear();

            foreach (XmlAttribute att in item.Attributes)
            {
				if (att.Name == "id_arme")
					ID = new Guid(att.Value);
				else if (att.Name == "parent_id_arme")
					ParentID = new Guid(att.Value);
				else if (att.Name == "name_arme")
					Name = att.Value;
				else
					ExtraAttributes.Add(att.Name);
            }

            if (ID == null)
                ID = Guid.NewGuid();
        }

        public MissionNode_Folder()
            : base()
        {
            ID = Guid.NewGuid();
            ParentID = null;
            _name = "Folder";
        }

        #endregion
    }

	[Serializable]
	public sealed class MissionNode_Unknown : MissionNode
    {
        [NonSerialized]
        private XmlNode _node;

        #region INHERITANCE

        public override int ImageIndex { get { return 131; } }

		public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlNode xNode = xDoc.CreateNode(_node.NodeType, _node.Name, "");
            if (_node.InnerText != "")
                xNode.InnerText = _node.InnerText;
            if (_node.Attributes != null)
                foreach (XmlAttribute att in _node.Attributes)
                {
                    XmlAttribute att2 = xDoc.CreateAttribute(att.Name);
                    att2.Value = att.Value;
                    xNode.Attributes.Append(att2);
                }

            return xNode;
        }

		public override void FromXml(XmlNode item, bool countPlusOne = false) 
        {
            _name = item.Name; 
            
            _node = item;
        }

        #endregion
    }

}
