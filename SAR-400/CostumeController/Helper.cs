using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CostumeController
{
    public static class ATHelperXML
    {
        public static XElement[] XElementsByName(XDocument doc, string name)
        {
            if (doc == null)
                return new XElement[0];

            List<XElement> retVal = new List<XElement>();
            foreach (var item in doc.Root.Elements())
            {
                if (item.Name == name)
                    retVal.Add(item);
            }

            return retVal.ToArray();
        }

        public static XElement[] XElementsByName(XElement element, string name)
        {
            if (element == null)
                return new XElement[0];

            List<XElement> retVal = new List<XElement>();
            foreach (var item in element.Elements())
            {
                if (item.Name == name)
                    retVal.Add(item);
            }

            return retVal.ToArray();
        }

        public static XElement XElementByName(XDocument doc, string name)
        {
            if (doc == null)
                return null;

            XElement retVal = null;
            if (doc.Root.Name == name)
                return doc.Root;

            foreach (var item in doc.Root.Elements())
            {
                if (item.Name == name)
                {
                    retVal = item;
                    break;
                }
            }

            return retVal;
        }

        public static XElement XElementByName(XElement element, string name)
        {
            if (element == null)
                return null;

            XElement retVal = null;
            foreach (var item in element.Elements())
            {
                if (item.Name == name)
                {
                    retVal = item;
                    break;
                }
            }

            return retVal;
        }

        public static string XElementInChildreValueByName(XElement element, string name)
        {
            var e = XElementByName(element, name);
            return e == null ? "" : e.Value;
        }

        public static string XElementAttributeValueByName(XElement element, string attributeName)
        {
            if (element == null)
                return "";
            var attr = element.Attribute(attributeName);
            if (attr == null)
                return "";
            return attr.Value;
        }
    }
}
