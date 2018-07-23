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
                // TODO: toevoegen van de juiste namespace en bijbehorende value
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);

                string nsCubeReading = "cubeReading";
                nsmgr.AddNamespace(nsCubeReading, xmlns);

                string ns = "xs";
                xmlns = "http://www.w3.org/2001/XMLSchema";

                nsmgr.AddNamespace(ns, xmlns);

                ns = "msprop";
                xmlns = "urn:schemas-microsoft-com:xml-msprop";

                nsmgr.AddNamespace(ns, xmlns);

                // create correct xPath expression
                if (baseXPath != null)
                {
                    xPath = baseXPath + xPath;
                }
                
                xPath = xPath.Replace("~ns~", nsCubeReading);
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
