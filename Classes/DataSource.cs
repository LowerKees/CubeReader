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

        public DataSource()
        {
            this.dsConnString = null;
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
    }
}
