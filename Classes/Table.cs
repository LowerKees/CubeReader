using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Table : IEqualityComparer<Table>
    {
        private List<Column> _columnList;

        public string TableType { get; set; }
        public string TableName { get; set; }
        public List<Column> ColumnList
        {
            get
            {
                return _columnList;
            }
            set
            {
                if(value is List<Column>)
                {
                    _columnList = value;
                }
                else
                {
                    // TODO: implement exception
                }
            }
        }

        public Table()
        {
            this._columnList = new List<Column>();
        }

        public void AddColumn(Column myColumn)
        {
            if(myColumn != null)
            {
                _columnList.Add(myColumn);
            }
            else
            {
                Console.WriteLine("Please enter a valid column.");
            }
        }

        // Implementation of IEqualityComparer interface
        bool IEqualityComparer<Table>.Equals(Table x, Table y)
        {
            if (x.TableName == null || y.TableName == null)
                return false;

            if (x.TableName.ToLower() == y.TableName.ToLower())
                return true;
            else
                return false;
        }

        int IEqualityComparer<Table>.GetHashCode(Table obj)
        {
            return obj.TableName.ToLower().GetHashCode();
        }

        public bool Equals(Table other)
        {
            if (this.TableName == null || other.TableName == null)
                return false;

            if (this.TableName.ToLower() == other.TableName.ToLower())
                return true;
            else
                return false;
        }
    }
}
