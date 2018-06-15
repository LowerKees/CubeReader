using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class DataSource
    {
        private string dsConnString;
        private string dsInitCatalog;

        public DataSource()
        {
            this.dsConnString = null;
            this.dsInitCatalog = null;
        }

        public string _dsConnString
        {
            get
            {
                return dsConnString;
            }
            set
            {
                if (value is string)
                {
                    dsConnString = value;
                }
            }
        }

        public string _dsInitCatalog
        {
            get
            {
                return dsInitCatalog;
            }
            set
            {
                if (value is string)
                {
                    dsInitCatalog = value;
                }
            }
        }
    }
}
