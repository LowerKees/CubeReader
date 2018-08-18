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
    }
}
