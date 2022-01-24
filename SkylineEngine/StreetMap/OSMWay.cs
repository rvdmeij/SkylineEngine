using System.Collections.Generic;
using System.Xml;

namespace SkylineEngine.StreetMap
{
    public class OSMWay : OSMBase
    {
        public List<ulong> NodeIds { get; private set; }
        public ulong Id { get; private set; }
        public bool Visible { get; private set; }                
        public float Height { get; private set; }
        public bool IsBoundary { get; private set; }
        public bool IsBuilding { get; private set; }
        public bool IsRoad { get; private set; }
        public bool IsRail { get; private set; }
        public int Lanes { get; private set; }
        public int Levels { get; private set; }
        public string Name { get; private set; }
        public bool OneWay { get; private set; }
        public bool Sidewalk { get; private set; }

        public OSMWay(XmlNode node)
        {
            Levels = 1;
            Height = 3.0f;
            NodeIds = new List<ulong>();

            Id = GetAttribute<ulong>("id", node.Attributes);
            //Visible = GetAttribute<bool>("visible", node.Attributes);
            Visible = true;

            XmlNodeList nds = node.SelectNodes("nd");

            foreach (XmlNode n in nds)
            {
                ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
                NodeIds.Add(refNo);
            }

            if(NodeIds.Count > 1)
            {
                IsBoundary = NodeIds[0] == NodeIds[NodeIds.Count - 1];
            }

            XmlNodeList tags = node.SelectNodes("tag");

            foreach (XmlNode t in tags)
            {
                string key = GetAttribute<string>("k", t.Attributes);

                if (key == "building")
                {
                    //TODO: building things
                    Height = 3.0f;
                    Levels = 1;
                    
                    IsBuilding = true;
                }
                else if (key == "building:levels")
                {
                    //TODO: building things
                    Levels = GetAttribute<int>("v", t.Attributes);
                    Height = 3.0f * Levels;
                    
                    IsBuilding = true;
                }
                else if (key == "height")
                {
                    //TODO: building things
                    Height = 0.3048f * GetAttribute<float>("v", t.Attributes);
                }

                else if (key == "highway")
                {
                    //TODO: highway things
                    IsRoad = true;
                }
                else if (key == "lanes")
                {
                    Lanes = GetAttribute<int>("v", t.Attributes);
                }
                else if (key == "name")
                {
                    Name = GetAttribute<string>("v", t.Attributes);
                }
                else if (key == "sidewalk")
                {
                    Sidewalk = GetAttribute<string>("v", t.Attributes) == "yes";
                }
                else if(key == "oneway")
                {
                    OneWay = GetAttribute<string>("v", t.Attributes) == "yes";
                }

                else if (key == "railway")
                {
                    //TODO: railway things
                    IsRail = true;
                }
            }
        }
    }
}
