using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Column : IEqualityComparer<Column>
    {
        private string columnName;

        public Column(string columnName = null, string dataType = null, int Precision = 0, int Scale = 0)
        {
            this.columnName = columnName;
        }

        public string _ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        // Implementation of IEqualityComparer interface
        bool IEqualityComparer<Column>.Equals(Column x, Column y)
        {
            if(x == null || y == null)
                return false;

            if (x.columnName == y.columnName)
                return true;
            else
                return false;
        }

        int IEqualityComparer<Column>.GetHashCode(Column obj)
        {
            return obj.columnName.ToLower().GetHashCode();
        }
    }
}
