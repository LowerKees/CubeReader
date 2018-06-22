using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Classes
{
    public class Database
    {
        private DataSource databaseDs;
        private List<Table> databaseTables;

        public Database(string modelPath)
        {
            databaseDs = null;
            databaseTables = getDbTables(modelPath);
        }     

        private List<Table> getDbTables(string modelPath)
        {
            List<Table> tables = new List<Table>();

            XmlDocument myDatabase = new XmlDocument();
            myDatabase.Load(modelPath);

            // Determine xpath to search for dsv
            XmlNodeList nodes;
            string xPath = "/~ns~:DataSchemaModel/~ns~:Model/~ns~:Element[@Type='SqlTable']/@Name";
            XmlNode root = myDatabase.DocumentElement;

            if (root.Attributes["xmlns"] != null)
            {
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(myDatabase.NameTable);

                string xmlnsName = "cubeReading";
                nsmgr.AddNamespace(xmlnsName, xmlns);

                // create correct xPath expression
                xPath = xPath.Replace("~ns~", xmlnsName);

                nodes = root.SelectNodes(xPath, nsmgr);
            }
            else
            {
                xPath = xPath.Replace("~ns~:", string.Empty);
                nodes = root.SelectNodes(xPath);
            }

            foreach(XmlNode x in nodes)
            {
                Console.WriteLine($"Found node {x.InnerText}");
            }

            return tables;
        }
    }
}
