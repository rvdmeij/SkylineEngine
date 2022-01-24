using System;
using System.Xml;

namespace SkylineEngine.StreetMap
{
    public abstract class OSMBase
    {
        protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
        {
            string strValue = attributes[attrName].Value;
            return (T)Convert.ChangeType(strValue, typeof(T));
        }
    }
}
