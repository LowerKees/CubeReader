using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Classes;

namespace Classes
{
    public class Cube
    {
        private List<CubeTable> cubeTables;
        private List<DataSource> cubeDs;
        private string cubeName;

        public Cube(string cubePath)
        {
            this.cubeTables = GetCubeTables(cubePath);
            this.cubeDs = getCubeDataSource(cubePath);
            this.cubeName = getCubeName(cubePath);
        }

        public string _cubeName
        {
            get
            {
                return cubeName;
            }
        }

        public List<CubeTable> _cubeTables
        {
            get
            {
                return cubeTables;
            }
        }

        public List<DataSource> _cubeDs
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

            nodes = ArtifactReader.getArtifactNodes(xPath, myXmlCube, xDimPath);

            string checkNode = null;
            foreach (XmlNode node in nodes)
            {
                CubeTable currentTable = foundTables.Find(x => x._cubeTableName.Equals(node.FirstChild.InnerText));
                // Check if a new table is presented
                if (currentTable is null)
                {
                    // Add cube table
                    currentTable = new CubeTable();
                    currentTable._cubeTableName = node.FirstChild.InnerText;
                    checkNode = currentTable._cubeTableName;

                    // Add db ref table
                    
                    // Find db table name and schema
                    XmlNodeList trueTables;
                    xPath = $"/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database/~ns~:DataSourceViews/~ns~:DataSourceView/~ns~:Schema/xs:schema/xs:element/xs:complexType/xs:choice/xs:element[@name=\'{node.FirstChild.InnerText}\']";
                    trueTables = ArtifactReader.getArtifactNodes(xPath, myXmlCube);

                    // Add findings to property list
                    string trueTableName, trueSchemaName;
                    trueTableName = trueTables.Item(0).Attributes.GetNamedItem("msprop:DbTableName").Value.ToString();
                    trueSchemaName = trueTables.Item(0).Attributes.GetNamedItem("msprop:DbSchemaName").Value.ToString();
                    currentTable._tableName = trueSchemaName + "." + trueTableName;
                    
                    // Add table to output
                    foundTables.Add(currentTable);
                }
                
                // Add cube columns to table
                CubeColumn myColumn = new CubeColumn();
                myColumn._cubeColumnName = node.LastChild.InnerText;
                
                // Find matching db column name
                XmlNodeList trueColumns;
                xPath = $"/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database/~ns~:DataSourceViews/~ns~:DataSourceView/~ns~:Schema/xs:schema/xs:element/xs:complexType/xs:choice/xs:element[@name=\'{currentTable._cubeTableName}\']/xs:complexType/xs:sequence/xs:element[@name=\'{node.LastChild.InnerText}\']";
                trueColumns = ArtifactReader.getArtifactNodes(xPath, myXmlCube);
                string trueColumnName = trueColumns.Item(0).Attributes.GetNamedItem("msprop:DbColumnName").Value.ToString();

                myColumn._ColumnName = trueColumnName;
                currentTable.AddColumn(myColumn);
            }
            return foundTables;
        }

        private List<DataSource> getCubeDataSource(string cubePath)
        {
            List<DataSource> myDataSources = new List<DataSource>();

            // Load current cube
            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube = loadCube(cubePath);

            //// Determine xpath to search for dsv
            XmlNodeList nodes;
            string xBasePath = "/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database";
            string xPath = "/~ns~:DataSources/~ns~:DataSource/~ns~:ConnectionString";

            // Return nodes with data source(s)
            nodes = ArtifactReader.getArtifactNodes(xPath, myXmlCube, xBasePath);

            // TODO: Check for multiple connection stringsB
            foreach (XmlNode node in nodes)
            {
                DataSource dataSource = new DataSource();
                dataSource._dsConnString = node.InnerText;
                dataSource._dsInitCatalog = getCubeInitialCatalog(dataSource);

                myDataSources.Add(dataSource);
            }
            
            return myDataSources;
        }

        private string getCubeName(string cubePath)
        {
            // Create, configure and load xml items and values
            XmlDocument myXmlCube = new XmlDocument();
            myXmlCube = loadCube(cubePath);
            XmlNode root = myXmlCube.DocumentElement;

            // Determine xpath to search for cube name
            XmlNodeList nameNodes;
            string xNamePath = "/~ns~:Batch/~ns~:Alter/~ns~:ObjectDefinition/~ns~:Database/~ns~:Name";

            nameNodes = ArtifactReader.getArtifactNodes(xNamePath, myXmlCube);

            if(nameNodes.Count == 1)
            {
                return nameNodes.Item(0).InnerText.ToString();
            }
            else
            {
                // TODO: throw error
                Console.WriteLine("Multiple names where found for a cube. No name was passed to the cube property.");
                return null;
            }
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
