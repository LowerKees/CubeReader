using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Column : IEqualityComparer<Column>
    {
        private string _columnName;

        public Column(string columnName = null, string dataType = null, int Precision = 0, int Scale = 0)
        {
            this._columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
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
    }
}
