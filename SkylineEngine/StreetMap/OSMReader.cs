using System;
using System.Collections.Generic;
using System.Xml;

namespace SkylineEngine.StreetMap
{
    public class OSMReader
    {
        Dictionary<ulong, OSMNode> nodes;
        List<OSMWay> ways;
        OSMBounds bounds;
        XmlDocument document;
        string xml;

        public Dictionary<ulong, OSMNode> Nodes
        {
            get { return nodes; }
        }

        public List<OSMWay> Ways
        {
            get { return ways; }
        }

        public OSMBounds Bounds
        {
            get { return bounds; }
        }

        public XmlDocument Document
        {
            get { return document; }
        }

        public string Xml
        {
            get { return xml; }
        }

        public OSMReader(string path)
        {
            nodes = new Dictionary<ulong, OSMNode>();
            ways = new List<OSMWay>();
            Read(path);
        }

        public OSMReader()
        {
            nodes = new Dictionary<ulong, OSMNode>();
            ways = new List<OSMWay>();
        }

        public void ReadFromString(string text)
        {
            try
            {
                xml = text;
                document = new XmlDocument();
                document.LoadXml(xml);
                SetBounds(document.SelectSingleNode("/osm/bounds"));
                GetNodes(document.SelectNodes("/osm/node"));
                GetWays(document.SelectNodes("/osm/way"));
            }
            catch (Exception ex)
            {
                xml = "Error: Could not read file: " + ex.Message;
            }
        }

        private void Read(string path)
        {
            try
            {
                xml = System.IO.File.ReadAllText(path);
                document = new XmlDocument();
                document.LoadXml(xml);
                SetBounds(document.SelectSingleNode("/osm/bounds"));
                GetNodes(document.SelectNodes("/osm/node"));
                GetWays(document.SelectNodes("/osm/way"));
            }
            catch (Exception ex)
            {
                xml = "Error: Could not read file: " + ex.Message;
            }
        }

        private void SetBounds(XmlNode node)
        {
            bounds = new OSMBounds(node);
        }

        private void GetNodes(XmlNodeList nodeList)
        {
            foreach (XmlNode n in nodeList)
            {
                OSMNode node = new OSMNode(n);
                nodes[node.Id] = node;
            }
        }

        private void GetWays(XmlNodeList nodeList)
        {
            foreach (XmlNode n in nodeList)
            {
                OSMWay way = new OSMWay(n);
                ways.Add(way);
            }
        }
    }
}
