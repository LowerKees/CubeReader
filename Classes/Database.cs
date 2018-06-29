﻿using System;
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
            databaseDs = getDbDataSource(modelPath);
            databaseTables = getDbTables(modelPath);
        }

        public DataSource _databaseDs { get { return databaseDs; } }
        public List<Table> _databaseTables { get { return databaseTables; } }

        private List<Table> getDbTables(string modelPath)
        {
            List<Table> tables = new List<Table>();

            XmlDocument myDatabase = new XmlDocument();
            myDatabase.Load(modelPath);

            // Determine xpath to search for dsv
            XmlNodeList nodes;
            string xPath = "/~ns~:DataSchemaModel/~ns~:Model/~ns~:Element[@Type='SqlTable']/@Name";

            nodes = ArtifactReader.getArtifactNodes(xPath, myDatabase);

            foreach(XmlNode x in nodes)
            {
                Table table = new Table();
                table._tableName = x.InnerText;

                tables.Add(table);
                Console.WriteLine($"Found table {x.InnerText}");

                string xPathCols = xPath.Replace("]/@Name", $" and @Name='{x.InnerText}']");
                xPathCols += "/~ns~:Relationship/~ns~:Entry/~ns~:Element[@Type='SqlSimpleColumn']/@Name";

                XmlNodeList columns;

                columns = ArtifactReader.getArtifactNodes(xPathCols, myDatabase);

                foreach(XmlNode c in columns)
                {
                    Column column = new Column();
                    column.myColumnName = c.InnerText;
                    table.AddColumn(column);
                    Console.WriteLine($"Found column {c.InnerText} and added it to the table.");
                }
            }

            return tables;
        }

        private DataSource getDbDataSource(string modelPath)
        {
            DataSource dataSource = new DataSource();

            string metadataPath = Path.GetDirectoryName(modelPath) + "\\DacMetadata.xml";

            XmlDocument myDatabase = new XmlDocument();
            myDatabase.Load(metadataPath);

            // Determine xpath to search for dsv
            XmlNodeList nodes;
            string xPath = "/~ns~:DacType/~ns~:Name";

            nodes = ArtifactReader.getArtifactNodes(xPath, myDatabase);

            foreach (XmlNode x in nodes)
            {
                dataSource._dsInitCatalog = x.InnerText;
                Console.WriteLine($"Found node {x.InnerText}");
            }

            return dataSource;
        }
    }
}