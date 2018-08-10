using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Classes
{
    public class Column : IEqualityComparer<Column>
    {
        // Simple constructor
        public Column()
        {
        }

        // Constructor to fill data types 
        public Column(string columnName = null, string dataType = null, 
            int numericPrecision = 0, int numericScale = 0, int stringLength = 0)
        {
            this.ColumnName = columnName;
            this.DataType = dataType;
            this.NumericPrecision = numericPrecision;
            this.NumericScale = numericScale;
            this.StringLength = stringLength;
        }

        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public int StringLength { get; set; }

        // Implementation of IEqualityComparer interface
        bool IEqualityComparer<Column>.Equals(Column x, Column y)
        {
            if(x == null || y == null)
                return false;

            if (x.ColumnName.ToLower() == y.ColumnName.ToLower())
                return true;
            else
                return false;
        }

        int IEqualityComparer<Column>.GetHashCode(Column obj)
        {
            return obj.ColumnName.ToLower().GetHashCode();
        }

        // Get the data type from the cube
        public static Tuple<string, string> GetCubeColumnDataType(XmlNodeList nodes)
        {
            Tuple<string, string> tuple = null;
            string dataType = null;
            string length = "not applicable";
            
            // datatype selection is always based on the first node in the NodeList

            // datatypes int, decimal, double, dateTime and boolean are 
            // stored in the "type" attribute
            if (nodes.Item(0).Attributes[$"type"] != null)
            {
                dataType = FindXmlAttribute(nodes, "type");
            }
            // string datatype and character length are stored
            // in the child nodes
            else if (nodes.Item(0).HasChildNodes)
            {
                dataType = FindXmlAttribute(nodes.Item(0).ChildNodes, "base");
                length = FindXmlAttribute(nodes.Item(0).ChildNodes, "value");
            }

            // TODO: default values aanpassen van "" en 0 naar iets zinnigs.
            tuple = Tuple.Create(dataType.Replace("xs:",""), length.Replace("xs:",""));
            return tuple;
        }

        // Recursive node reader
        private static string FindXmlAttribute(XmlNodeList xmlNodeList, string attributeName)
        {
            string attributeFound = null;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes[$"{attributeName}"] != null)
                {
                    attributeFound = xmlNode.Attributes.GetNamedItem($"{attributeName}").Value.ToString();
                }
                else
                {
                    if(xmlNode.HasChildNodes)
                    {
                        XmlNodeList myNodes = xmlNode.ChildNodes;
                        attributeFound = FindXmlAttribute(myNodes, attributeName);
                    }
                }
            }
            return attributeFound;
        }
    }
}
