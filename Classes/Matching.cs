using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Matching
    {
        public static void matchCubeToDatbase(List<Database> databases, List<Cube> cubes)
        {
            foreach(Cube cube in cubes)
            {
                Console.WriteLine($"Comparing for cube {cube._cubeName}...");

                foreach (DataSource ds in cube._cubeDs)
                {
                    Console.WriteLine("Found connection string {0}", databases.Find(x => x._databaseDs._dsConnString.Equals(ds._dsConnString)));
                }
            }
        }
    }
}
