using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Matching
    {
        private Cube _matchingCube;
        public Cube matchingCube
        {
            get
            {
                return _matchingCube;
            }
            set
            {
                _matchingCube = value;
            }
        }

        private List<Database> _matchedDatabases;
        public List<Database> matchedDatabases
        {
            get
            {
                return _matchedDatabases;
            }
            set
            {
                _matchedDatabases = value;
            }
        }
        
        public Matching()
        {
            matchingCube = null;
            matchedDatabases = new List<Database>();
        }
            
        public static List<Matching> matchCubeToDatabase(List<Database> databases, List<Cube> cubes)
        {
            List<Matching> matches = new List<Matching>();

            foreach(Cube cube in cubes)
            {
                Matching match = new Matching();
                Console.WriteLine($"Comparing for cube {cube._cubeName}...");
                match.matchingCube = cube;

                foreach (DataSource ds in cube._cubeDs)
                {
                    Database db = new Database();
                    // Compare based on Initial Catalog because
                    // databases have no connection string
                    db = databases.Find(x => x._databaseDs._dsInitCatalog.Equals(ds._dsInitCatalog));
                    Console.WriteLine($"The cube {cube._cubeName} connects to database {db._databaseDs._dsInitCatalog}");
                    match.matchedDatabases.Add(db);
                }

                matches.Add(match);
            }

            return matches;
        }
    }
}
