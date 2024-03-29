﻿using System.Xml;
using SkylineEngine;

namespace SkylineEngine.StreetMap
{
    public class OSMBounds : OSMBase
    {
        public float MinLat { get; private set; }
        public float MaxLat { get; private set; }
        public float MinLon { get; private set; }
        public float MaxLon { get; private set; }
        public Vector3 Center { get; private set; }

        public OSMBounds(XmlNode node)
        {
            MinLat = GetAttribute<float>("minlat", node.Attributes);
            MaxLat = GetAttribute<float>("maxlat", node.Attributes);
            MinLon = GetAttribute<float>("minlon", node.Attributes);
            MaxLon = GetAttribute<float>("maxlon", node.Attributes);

            float x = (float)(MercatorProjection.lonToX(MaxLon) + MercatorProjection.lonToX(MinLon)) / 2;
            float y = (float)(MercatorProjection.latToY(MaxLat) + MercatorProjection.latToY(MinLat)) / 2;

            Center = new Vector3(x, 0, y);
        }
    }
}
