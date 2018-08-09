using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class CubeColumn : Column
    {
        private string _cubeColumnName;

        public string CubeColumnName
        {
            get
            {
                return _cubeColumnName;
            }
            set
            {
                _cubeColumnName = value;
            }
        }

        public CubeColumn() : base()
        {
        }

        public CubeColumn(string columnName, string cubeColumnName) : base()
        {
            this.ColumnName = columnName;
            this._cubeColumnName = CubeColumnName;
        }
    }
}
