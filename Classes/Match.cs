using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Match
    {
        
        public Match()
        {
            MatchingCube = null;
            MatchedDatabases = new List<Database>();
        }

        public Match(Cube matchingCube)
        {
            MatchingCube = matchingCube;
            MatchedDatabases = new List<Database>();
        }
            
        private Cube _matchingCube;
        public Cube MatchingCube
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
        public List<Database> MatchedDatabases
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

        public static Match MatchCubeToDatabase(List<Database> databases, Cube cube)
        {
            //
            // Summary:
            //      Based on the connection strings and their initial
            //      catalog values found in the cube, the corresponding 
            //      databases are matched and returned to the caller.
            //

            Match match = new Match(cube);
            Console.WriteLine($"Comparing for cube {cube._cubeName}...");

            foreach (DataSource ds in cube._cubeDs)
            {
                // Compare based on Initial Catalog because
                // databases have no connection string
                Database db = databases.Find(x =>
                    x._databaseDs._dsInitCatalog.Equals(ds._dsInitCatalog));

                if (db == null)
                {
                    // Throw exception if no database is found
                    HandleNoDatabases(ds, cube);
                }
                else
                {
                    // Report success for found databases
                    Console.WriteLine($"The cube {cube._cubeName} connects to database " +
                    $"{db._databaseDs._dsInitCatalog}");
                    // Add it to the match
                    match.MatchedDatabases.Add(db);
                }
            }
            return match;
        }

        public static Tuple<CubeTable, Table> MatchTables(CubeTable cubeTable, 
            List<Database> databases)
        {
            //
            // Summary:
            //      Returns matched cube and database
            //      tables or view for futher analysis.

            Tuple<CubeTable, Table> tableTuple = null;
            Tuple<CubeTable, Table> viewTuple = null;

            foreach (Database database in databases)
            {
                // Find all corresponding tables based
                // on the table name and schema name 
                // of both ref types
                foreach (Table table in database._databaseTables)
                {
                    if (table.Equals(cubeTable))
                    {
                        tableTuple = Tuple.Create(cubeTable, table);
                    }
                }

                // Find all corresponding views based
                // on the table name and schema name of 
                // both ref types
                foreach (Table table in database._databaseViews)
                {
                    if (table.Equals(cubeTable))
                    {
                        viewTuple = Tuple.Create(cubeTable, table);
                    }
                }
            }
            // Database name spaces do not allow
            // tables or views with identical names within a 
            // single database. So no check is installed.
            if(tableTuple != null)
            {
                return tableTuple;
            }
            return viewTuple;
        }

        private static void HandleNoDatabases(Object obj, Cube cube)
        {
            if(obj is Database)
            {
                Database database = (Database)obj;

                MatchException matchException = new MatchException($"The cube " +
                    $"{cube._cubeName} has no corresponding databases.");
                throw matchException;
            }
            else
            {
                Exception exception = new Exception("Input object is not a database.");
                throw exception;
            }
        }
    }
}