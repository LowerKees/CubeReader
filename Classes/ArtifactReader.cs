using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Classes
{
    class ArtifactReader
    {
        public ArtifactReader(){}

        public static XmlNodeList getArtifactNodes(string xPath, XmlDocument xml, string baseXPath = null)
        {
            // Determine xpath to search for dsv
            XmlNodeList nodes;
            XmlNode root = xml.DocumentElement;

            if (root.Attributes["xmlns"] != null)
            {
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);

                string xmlnsName = "cubeReading";
                nsmgr.AddNamespace(xmlnsName, xmlns);

                // create correct xPath expression
                xPath = baseXPath + xPath;
                xPath = xPath.Replace("~ns~", xmlnsName);

                nodes = root.SelectNodes(xPath, nsmgr);
            }
            else
            {
                xPath = xPath.Replace("~ns~:", string.Empty);
                nodes = root.SelectNodes(xPath);
            }

            return nodes;
        }
    }
}
