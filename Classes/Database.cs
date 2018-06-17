using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    class Database
    {
        private DataSource databaseDs;
        private List<Table> databaseTables;

        public Database(string dacpacPath)
        {
            databaseDs = null;
            databaseTables = null;
        }       
    }
}
