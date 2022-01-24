using System.Xml;
using SkylineEngine;

namespace SkylineEngine.StreetMap
{
    public class OSMNode : OSMBase
    {
        public ulong Id { get; private set; }
        public float Latitude { get; private set; }
        public float Longitude { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public OSMNode(XmlNode node)
        {
            Id = GetAttribute<ulong>("id", node.Attributes);
            Latitude = GetAttribute<float>("lat", node.Attributes);
            Longitude = GetAttribute<float>("lon", node.Attributes);
            X = (float)MercatorProjection.lonToX(Longitude);
            Y = (float)MercatorProjection.latToY(Latitude);
        }

        public static implicit operator Vector3(OSMNode node)
        {
            return new Vector3(node.X, 0, node.Y);
        }
    }
}
