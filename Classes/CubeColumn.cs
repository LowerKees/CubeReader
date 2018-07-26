using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class CubeColumn : Column
    {
        private string cubeColumnName, dataType;
        private int dataTypePrec, dataTypeScale;

        public string _cubeColumnName
        {
            get
            {
                return cubeColumnName;
            }
            set
            {
                cubeColumnName = value;
            }
        }

        public CubeColumn() : base()
        {
        }
    }
}
