using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class CubeTable : Table
    {
        public CubeTable() : base()
        {
            _logicalColumns = new List<CubeColumn>();
        }

        private List<CubeColumn> _logicalColumns;

        public string CubeTableName
        {
            get;
            set;
        }

        public List<CubeColumn> LogicalColumns
        {
            get
            {
                return _logicalColumns;
            }
            set
            {
                _logicalColumns = value;
            }
        }
    }
}
