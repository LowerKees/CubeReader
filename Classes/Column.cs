using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Column
    {
        private string columnName, dataType;
        private int dataTypePrec, dataTypeScale;

        public Column(string columnName = null, string dataType = null, int Precision = 0, int Scale = 0)
        {
            this.columnName = columnName;
            this.dataType = dataType;
            this.dataTypePrec = Precision;
            this.dataTypeScale = Scale;
        }

        public string _ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public string myDataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        
        public int myDataTypePrec
        {
            get { return dataTypePrec; }
            set { dataTypePrec = value; }
        }

        public int myDataTypeScale
        {
            get { return dataTypeScale; }
            set { dataTypeScale = value; }
        }
    }
}
