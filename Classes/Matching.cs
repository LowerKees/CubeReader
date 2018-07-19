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
                    Database db = new Database();
                    // Compare based on Initial Catalog because
                    // databases have no connection string
                    db = databases.Find(x => x._databaseDs._dsInitCatalog.Equals(ds._dsInitCatalog));
                    Console.WriteLine($"The cube {cube._cubeName} connects to database {db._databaseDs._dsInitCatalog}");
                }
            }
        }
    }
}
