using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CubeColumn(string columnName, string cubeColumnName) : base()
        {
            this.ColumnName = columnName;
            this.CubeColumnName = CubeColumnName;
        }
    }
}
