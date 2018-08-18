using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Classes
{
    public class CubeColumn : Column
    {
        public string CubeColumnDataType { get; set; }
        public string CubeColumnDataLength { get; set; }
        public string CubeColumnName { get; set; }

        public CubeColumn() : base()
        {
        }

        public CubeColumn(string columnName, string cubeColumnName, string cubeColumnDataType, 
            string cubeColumnDataLength) : base()
        {
            this.ColumnName = columnName;
            this.CubeColumnName = CubeColumnName;
            this.CubeColumnDataType = cubeColumnDataType;
            this.CubeColumnDataLength = cubeColumnDataLength;
        }

        // Get the data type from the cube
        public static Tuple<string, string> GetCubeColumnDataType(XmlNodeList nodes)
        {
            //
            // Summary:
            //      Finds the cube data type 
            //      and corresponding metadata
            //      such as length
            //

            Tuple<string, string> tuple = null;
            string dataType = null;
            string length = "not applicable";

            // datatype selection is always based on the first node in the NodeList

            // datatypes int, decimal, double, dateTime and boolean are 
            // stored in the "type" attribute
            if (nodes.Item(0).Attributes[$"type"] != null)
            {
                dataType = ArtifactReader.FindXmlAttribute(nodes, "type");
            }
            // string datatype and character length are stored
            // in the child nodes
            else if (nodes.Item(0).HasChildNodes)
            {
                dataType = ArtifactReader.FindXmlAttribute(nodes.Item(0).ChildNodes, "base");
                length = ArtifactReader.FindXmlAttribute(nodes.Item(0).ChildNodes, "value");
            }

            tuple = Tuple.Create(dataType.Replace("xs:", ""), length.Replace("xs:", ""));
            return tuple;
        }

        public static Boolean HandleLogicalColumns(XmlNodeList trueColumns, CubeTable currentTable)
        {
            // 
            // Summary:
            //      Determines if a cube column
            //      from the DSV is based on the
            //      database (non logical) or based
            //      on a calculation (logical). 
            //      Adds the column to the input
            //      table if logical.
            //
            if (trueColumns.Item(0).Attributes["msprop:IsLogical"] != null)
            {
                if (trueColumns.Item(0).Attributes.GetNamedItem("msprop:IsLogical").Value.ToString() == "True")
                {
                    CubeColumn myLogicalColumn = new CubeColumn
                    {
                        CubeColumnName =
                        trueColumns.Item(0).Attributes.GetNamedItem("msprop:DbColumnName").Value.ToString()
                    };
                    currentTable.LogicalColumns.Add(myLogicalColumn);
                    // Return true to indicate the column is logical
                    return true;
                }
            }
            // Return false to indicate the column is not logical
            return false;
        }
    }
}
