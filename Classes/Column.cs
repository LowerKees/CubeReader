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
        private string _columnName;
        private string _dataType;
        private int _numericPrecision;
        private int _numericScale;
        private int _stringLength;

        // Simple constructor
        public Column()
        {
        }

        // Constructor to fill data types 
        public Column(string columnName = null, string dataType = null, 
            int numericPrecision = 0, int numericScale = 0, int stringLength = 0)
        {
            this._columnName = columnName;
            this._dataType = dataType;
            this._numericPrecision = numericPrecision;
            this._numericScale = numericScale;
            this._stringLength = stringLength;
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        
        public string DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                if (value is string)
                {
                    _dataType = value;
                }
                else
                {
                    Console.WriteLine("Value provided is not a string.");
                }
            }
        }

        // Implementation of IEqualityComparer interface
        bool IEqualityComparer<Column>.Equals(Column x, Column y)
        {
            if(x == null || y == null)
                return false;

            if (x._columnName.ToLower() == y._columnName.ToLower())
                return true;
            else
                return false;
        }

        int IEqualityComparer<Column>.GetHashCode(Column obj)
        {
            return obj._columnName.ToLower().GetHashCode();
        }

        // Get the data type from the cube
        public static Tuple<string, string> GetCubeColumnDataType(XmlNodeList nodes)
        {
            Tuple<string, string> tuple;
            string dataType = null;
            string length = null;

            foreach (XmlNode node in nodes)
            {
                // datatypes int, decimal, double, dateTime and boolean are 
                // stored in the "type" attribute
                if (node.Attributes["type"] != null)
                {
                    dataType = node.Attributes.GetNamedItem("type").Value.ToString().Replace("xs:","");
                }
                // string datatype and character length are stored
                // in the child nodes
                else if (node.HasChildNodes)
                {
                    dataType = FindXmlAttribute(node.ChildNodes, "base");
                    length = FindXmlAttribute(node.ChildNodes, "value");
                }
            }

            // TODO: default values aanpassen van "" en 0 naar iets zinnigs.
            tuple = Tuple.Create(dataType, length);
            return tuple;
        }

        // Recursive node reader
        private static string FindXmlAttribute(XmlNodeList xmlNodeList, string attributeName)
        {
            string attributeFound = null;
            foreach(XmlNode xmlNode in xmlNodeList)
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
                        FindXmlAttribute(myNodes, attributeName);
                    }
                }
            }
            return attributeFound;
        }
    }
}
