using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Table
    {
        public List<Column> columnList;

        public string _tableName { get; set; }

        public Table()
        {
            this.columnList = new List<Column>();
        }

        public void AddColumn(Column myColumn)
        {
            if(myColumn != null)
            {
                columnList.Add(myColumn);
            }
            else
            {
                Console.WriteLine("Please enter a valid column.");
            }
        }
    }
}
