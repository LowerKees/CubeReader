using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Classes
{
    public class Cube
    {
        private List<CubeTable> cubeTables;
        private DataSource cubeDs;
        private string cubeName;

        public Cube(string cubePath)
        {
            this.cubeTables = GetCubeTables(cubePath);
            this.cubeDs = getCubeDataSource(cubePath);
            this.cubeName = getCubeName(cubePath);
        }

        public string _cubeName { get; }

        public List<CubeTable> _cubeTables
        {
            get
            {
                return cubeTables;
            }
        }

        public DataSource _cubeDs
        {
            get
            {
                return cubeDs;
            }
        }

        private List<CubeTable> GetCubeTables(string cubePath)
        {
            List<CubeTable> foundTables = new List<CubeTable>();

            // Create, configure and load xml items and values
            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube = loadCube(cubePath);
            XmlNode root = myXmlCube.DocumentElement;

            // Determine xpath to search for tables
            XmlNodeList nodes;
            string xDimPath = "/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database/~ns~:Dimensions/~ns~:Dimension";
            string xPath = "/~ns~:Attributes/~ns~:Attribute/~ns~:KeyColumns/~ns~:KeyColumn/~ns~:Source";

            if (root.Attributes["xmlns"] != null)
            {
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(myXmlCube.NameTable);

                string xmlnsName = "cubeReading";
                nsmgr.AddNamespace(xmlnsName, xmlns);

                // create correct xPath expression
                xPath = xDimPath + xPath;
                xPath = xPath.Replace("~ns~", xmlnsName);
                
                nodes = root.SelectNodes(xPath, nsmgr);
            }
            else
            {
                xPath = xPath.Replace("~ns~:", string.Empty);
                nodes = root.SelectNodes(xPath);
            }

            string checkNode = null;
            foreach (XmlNode node in nodes)
            {
                CubeTable currentTable = foundTables.Find(x => x._tableName.Equals(node.FirstChild.InnerText));
                // Check if a new table is presented
                if (currentTable is null)
                {
                    currentTable = new CubeTable();
                    currentTable._tableName = node.FirstChild.InnerText;
                    checkNode = currentTable._tableName;

                    // Add table to output
                    foundTables.Add(currentTable);
                }

                // Add columns to table
                CubeColumn myColumn = new CubeColumn();
                myColumn.myColumnName = node.LastChild.InnerText;
                currentTable.AddColumn(myColumn);
            }
            return foundTables;
        }

        private DataSource getCubeDataSource(string cubePath)
        {
            DataSource myDataSource = new DataSource();

            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube = loadCube(cubePath);

            // Determine xpath to search for dsv
            XmlNodeList nodes;
            string xBasePath = "/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database";
            string xPath = "/~ns~:DataSources/~ns~:DataSource/~ns~:ConnectionString";
            XmlNode root = myXmlCube.DocumentElement;

            if (root.Attributes["xmlns"] != null)
            {
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(myXmlCube.NameTable);

                string xmlnsName = "cubeReading";
                nsmgr.AddNamespace(xmlnsName, xmlns);

                // create correct xPath expression
                xPath = xBasePath + xPath;
                xPath = xPath.Replace("~ns~", xmlnsName);

                nodes = root.SelectNodes(xPath, nsmgr);
            }
            else
            {
                xPath = xPath.Replace("~ns~:", string.Empty);
                nodes = root.SelectNodes(xPath);
            }

            // TODO: Check for multiple connection strings
            foreach(XmlNode node in nodes)
            {
                myDataSource._dsConnString = node.InnerText;
            }

            myDataSource._dsInitCatalog = getCubeInitialCatalog(myDataSource);
            
            return myDataSource;
        }

        private string getCubeName(string cubePath)
        {
            // Create, configure and load xml items and values
            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube = loadCube(cubePath);
            XmlNode root = myXmlCube.DocumentElement;

            // Determine xpath to search for cube name
            XmlNode nameNode;
            string xNamePath = "/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database/~ns~:Name";

            if (root.Attributes["xmlns"] != null)
            {
                string xmlns = root.Attributes["xmlns"].Value;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(myXmlCube.NameTable);

                string xmlnsName = "cubeReading";
                nsmgr.AddNamespace(xmlnsName, xmlns);

                xNamePath = xNamePath.Replace("~ns~", xmlnsName);
                nameNode = root.SelectSingleNode(xNamePath, nsmgr);
            }
            else
            {
                xNamePath = xNamePath.Replace("~ns~:", string.Empty);
                nameNode = root.SelectSingleNode(xNamePath);
            }

            return nameNode.InnerText;
        }

        private string getCubeInitialCatalog(DataSource datasource)
        { 
            // Method performs text search for initial catalog value
            string initCat;
            string searchString = "Initial Catalog=";
            int startIndex = datasource._dsConnString.IndexOf(searchString) + (searchString).Length;
            int lengthIndex;
            string remainingString = datasource._dsConnString.Substring(startIndex, datasource._dsConnString.Length - startIndex);

            if (remainingString.Contains(";"))
            {
                lengthIndex = remainingString.IndexOf(";");
            }
            else
            {
                lengthIndex = remainingString.Length;
            }

            initCat = datasource._dsConnString.Substring(startIndex, lengthIndex);

            return initCat;
        }

        // TODO: method slaat nergens op
        private static XmlDocument loadCube(string cubePath)
        {
            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube.Load(cubePath);
            return myXmlCube;
        }
    }
}
