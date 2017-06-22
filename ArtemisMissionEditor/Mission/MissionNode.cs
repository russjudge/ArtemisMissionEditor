using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArtemisMissionEditor
{
	/// <summary> 
    /// Stores data about one mission node (Start, Event, Folder or Comment)
    /// </summary>
    [Serializable]
    public abstract class MissionNode : ICloneable
    {
        /// <summary> GUID of the node. Required for linking nodes to folders and specifying background nodes</summary>
        public Guid? ID { get; set; }
        
        /// <summary> GUID of the mission node containing this node.</summary>
        public Guid? ParentID { get; set; }

        /// <summary> Extra attributes of the node. Used to store states such as "collapsed/expanded" state of the folder</summary>
        public List<string> ExtraAttributes { get; set; }

        /// <summary> Wether the node is using it's default name or not.</summary>
        public bool HasDefaultName { get { return _hasDefaultName; } protected set { _hasDefaultName = value; } }
        protected bool _hasDefaultName;
		
        /// <summary> Name of the node. </summary>
        public string Name
		{
			get { return _name; }
            set { _name = value; _hasDefaultName = false; }
		}
        protected string _name;

        /// <summary> List of conditions of this node (if any)</summary>
        public List<MissionStatement> Conditions { get; set; }

        /// <summary> List of actions of this node (if any)</summary>
		public List<MissionStatement> Actions { get; set; }

        #region SHARED
        
        //R. Judge: store item on MissionNode with Missionstatement  <-- WTF?
        /// <summary>
        /// Create and return new MissionNode from an XmlNode. Will set type appropriately.
        /// </summary>
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

            mn.FromXml(item);
            return mn;
        }

        #endregion

        #region INHERITANCE

        /// <summary> Set node's name to default (whatever it is for this kind of node). </summary>
        protected virtual void SetDefaultName() { HasDefaultName = true; }

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public abstract int ImageIndex { get; }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public abstract XmlNode ToXml(XmlDocument xDoc, bool full = false);

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public abstract void FromXml(XmlNode item);

        /// <summary> Clone this MissionNode appropriately</summary>
		public object Clone()
		{
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

        protected MissionNode()
        {
            ID = null;
            ParentID = null;
            SetDefaultName();
            ExtraAttributes = new List<string>();
            Conditions = new List<MissionStatement>();
			Actions = new List<MissionStatement>();
        }

        #endregion
    }

    /// <summary>
    /// Stores data about one commentary at mission node level
    /// </summary>
    [Serializable]
    public sealed class MissionNode_Comment : MissionNode
    {
        #region INHERITANCE

        /// <summary> Set node's name to default (" - - - - - - - - - - "). </summary>
        protected override void SetDefaultName() { Name = " - - - - - - - - - - "; base.SetDefaultName(); }

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public override int ImageIndex { get { return 152; } }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlComment xCom = xDoc.CreateComment(Name);
            return xCom;
        }

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public override void FromXml(XmlNode item)
        {
            Name = item.Value;
        }

        public MissionNode_Comment()
            : base()
        {
            ID = Guid.NewGuid();
            ParentID = null;
            SetDefaultName();
        }

        #endregion
    }

    /// <summary>
    /// Stores data about one event node
    /// </summary>
    [Serializable]
	public sealed class MissionNode_Event : MissionNode
    {
		/// <summary> Wether this event is enabled or not.</summary>
        public bool Enabled;

        /// <summary> Serial number of this event.</summary>
        public int SerialNumber { get; private set; }

        #region INHERITANCE

        /// <summary> Set node's name to default ("Event ##"). </summary>
        protected override void SetDefaultName() { _name = "Event " + SerialNumber; base.SetDefaultName(); }

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public override int ImageIndex { get { return Enabled ? 0 : 107; } }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement(Enabled ? "event" : "disabled_event");
            if (!HasDefaultName || full)
            {
                xAttr = xDoc.CreateAttribute("name");
                xAttr.Value = Name;
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

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public override void FromXml(XmlNode item)
        {
			SerialNumber = Mission.Current.EventCount + 1;
            Enabled = item.Name == "event";
            ID = null;
            ParentID = null;
            Conditions.Clear();
			Actions.Clear();

			List<MissionStatement> commentHolder = new List<MissionStatement>();

            foreach (XmlAttribute att in item.Attributes)
            {
                if (att.Name == "id_arme")
                    ID = Helper.GuidTryRead(att.Value, true);
                if (att.Name == "parent_id_arme")
                    ParentID = Helper.GuidTryRead(att.Value, true);
				if (att.Name == "name" || att.Name == "name")
				{
					Name = att.Value;
					if (Name.Contains("Event") && Helper.IntTryParse(Name.Substring(5, Name.Length - 5)))
						SerialNumber = Helper.StringToInt(Name.Substring(5, Name.Length - 5));
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
			SerialNumber = Mission.Current.EventCount;
            SetDefaultName();
        }

        #endregion
    }

    /// <summary>
    /// Stores data about one folder node
    /// </summary>
    [Serializable]
    public sealed class MissionNode_Folder : MissionNode
    {
        #region INHERITANCE

        /// <summary> Set node's name to default ("Folder"). </summary>
        protected override void SetDefaultName() { Name = "Folder"; base.SetDefaultName(); }

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public override int ImageIndex { get { return 2; } }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement("folder_arme");
            if (!HasDefaultName)
            {
                xAttr = xDoc.CreateAttribute("name");
                xAttr.Value = Name;
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
            foreach (string item in ExtraAttributes)
            {
                xAttr = xDoc.CreateAttribute(item);
                xAttr.Value = "";
                xElem.Attributes.Append(xAttr);
            }

            return xElem;
        }

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public override void FromXml(XmlNode item)
        {
            SetDefaultName();
            ID = null;
            ParentID = null;
            ExtraAttributes.Clear();
            Conditions.Clear();
            Actions.Clear();

            foreach (XmlAttribute att in item.Attributes)
            {
                if (att.Name == "id_arme")
                    ID = Helper.GuidTryRead(att.Value, true);
                else if (att.Name == "parent_id_arme")
                    ParentID = Helper.GuidTryRead(att.Value, true);
                else if (att.Name == "name")
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
            SetDefaultName();
        }

        #endregion
    }

    /// <summary>
    /// Stores data about one start node
    /// </summary>
    [Serializable]
	public sealed class MissionNode_Start : MissionNode
    {
        #region INHERITANCE

        /// <summary> Set node's name to default ("Stat"). </summary>
        protected override void SetDefaultName() { _name = "Start"; base.SetDefaultName(); }

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public override int ImageIndex { get { return 1; } }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlElement xElem;
            XmlAttribute xAttr;
            xElem = xDoc.CreateElement("start");
            if (!HasDefaultName)
            {
                xAttr = xDoc.CreateAttribute("name");
                xAttr.Value = Name;
                xElem.Attributes.Append(xAttr);

            }
            xAttr = xDoc.CreateAttribute("id_arme");
            xAttr.Value = ID.ToString();
            xElem.Attributes.Append(xAttr);
            
            // Start node should not have parent id
            //if (ParentID != null)
            //{
            //    xAttr = xDoc.CreateAttribute("parent_id_arme");
            //    xAttr.Value = ParentID.ToString();
            //    xElem.Attributes.Append(xAttr);
            //}
            
            // Start node should not have conditions
			//foreach (MissionStatement item in Conditions)
			//	xElem.AppendChild(item.ToXml(xDoc));
			foreach (MissionStatement item in Actions)
				xElem.AppendChild(item.ToXml(xDoc,full));

            return xElem;
        }

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public override void FromXml(XmlNode item)
        {
            SetDefaultName();
            ID = null;
            ParentID = null;
            Conditions.Clear();
			Actions.Clear();

			List<MissionStatement> commentHolder = new List<MissionStatement>();

            foreach (XmlAttribute att in item.Attributes)
            {
                if (att.Name == "id_arme")
                    ID = Helper.GuidTryRead(att.Value, true);
                // Start node should not have parent id
                //if (att.Name == "parent_id_arme")
                //    ParentID = new Guid(att.Value);
				if (att.Name == "name" || att.Name == "name")
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
							Log.Add("Found a condition in start block! It was ignored as start nodes are not allowed to have conditions.");	
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
            SetDefaultName();
        }

        #endregion
    }

    /// <summary>
    /// Stores data about one unknown node
    /// </summary>
    [Serializable]
	public sealed class MissionNode_Unknown : MissionNode
    {
        /// <summary> XmlNode containing this node.</summary>
        [NonSerialized]
        private XmlNode _node;

        #region INHERITANCE

        /// <summary> Get Image index for this type of node (to display in the TreeView) </summary>
        public override int ImageIndex { get { return 131; } }

        /// <summary> 
        /// Get XmlNode containing the node.
        /// </summary>
        /// <param name="xDoc">XmlDocument object used to spawn node</param>
        /// <param name="full">If true, all possible attributes will be output. 
        /// If false, only filled and appropriate attributes will be output.</param>
        public override XmlNode ToXml(XmlDocument xDoc, bool full = false)
        {
            XmlNode xNode = xDoc.CreateNode(_node.NodeType, _node.Name, "");
            if (!String.IsNullOrEmpty(_node.InnerText))
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

        /// <summary>
        /// Read node contents from XmlNode. 
        /// </summary>
        /// <param name="item">Source node</param>
        public override void FromXml(XmlNode item) 
        {
            Name = item.Name; 
            
            _node = item;
        }

        #endregion
    }

}
