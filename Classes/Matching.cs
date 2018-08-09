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
                Database db = new Database();
                // Compare based on Initial Catalog because
                // databases have no connection string

                db = databases.Find(x =>
                    x._databaseDs._dsInitCatalog.Equals(ds._dsInitCatalog));

                if (db == null)
                {
                    MatchException matchException = new MatchException($"The cube " +
                        $"{cube._cubeName} has no corresponding databases.");
                    throw matchException;
                }

                Console.WriteLine($"The cube {cube._cubeName} connects to database " +
                    $"{db._databaseDs._dsInitCatalog}");
                match.MatchedDatabases.Add(db);
            }
            return match;
        }

        private static Tuple<CubeTable, Table> MatchTables(CubeTable cubeTable, 
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

        public void CheckForTables(Match match)
        {
            //
            // Summary: 
            //      Checks for each cube table
            //      if it is represented in the
            //      corresponding database. If not
            //      it will generate warnings and
            //      throw an exception.
            //
            string checkName = "100. Cube vs Database table test";
            Console.WriteLine($"Running check {checkName}: \nChecking if all cube tables have " +
                $"corresponding database tables.");

            List<CubeTable> nonMatchedTables = new List<CubeTable>();

            foreach (CubeTable cubeTable in match.MatchingCube._cubeTables)
            {
                foreach(Database db in match.MatchedDatabases)
                {
                    // See if the cubeTable is found in the database table list
                    Boolean isTable = db._databaseTables.Any(x => x.TableName.ToLower() == cubeTable.TableName.ToLower());

                    // See if the cubeTable is found in the database view list
                    Boolean isView = db._databaseViews.Any(x => x.TableName.ToLower() == cubeTable.TableName.ToLower());

                    // If the cube table is not within the database table or view
                    // list, add it to the non matched table list.
                    if (!(isTable || isView))
                        nonMatchedTables.Add(cubeTable);
                }
            }

            // If there are non matched tables, write
            // out their names to the console. Else
            // write out success.
            if (nonMatchedTables.Count > 0)
            {
                MatchException matchException = new MatchException($"Error: One or more cube " +
                    $"tables cannot be found in the dacpac. Checkname: {checkName}");
                Console.WriteLine($"For cube {match.MatchingCube._cubeName} the following " +
                    $"tables cannot be found in the dacpacs:");
                foreach (Table table in nonMatchedTables)
                {
                    Console.WriteLine($"\n{table.TableName}");
                }

                // TODO: opnieuw aanzetten exception. Staat nu uit voor debugging.
                //throw matchException;
            }
            else
            {
                WriteTestSuccess(checkName);
            }
        }

        public void CheckForColumns(Match match)
        {
            // Summary: 
            //      checks if every column in the
            //      matched cube is represented in the
            //      matched database(s).
            //
            // Parameters: 
            //      match: 
            //          Reference type containing one Cube
            //          and a list of Databases

            string checkName = "101. Cube vs Database column test";
            Console.WriteLine($"Running check {checkName}: \nChecking if all cube table columns " +
                $"have corresponding database columns.");
            List<CubeTable> nonPresentColumnTables = new List<CubeTable>();

            foreach(CubeTable cubeTable in match.MatchingCube._cubeTables)
            {
                // Match the cube table to the database table
                Tuple<CubeTable, Table> tuple = MatchTables(cubeTable, match.MatchedDatabases);

                // Create a list to accomodate all unfound
                // columns.
                List<Column> unfoundColumns = new List<Column>();

                foreach(CubeColumn cubeColumn in tuple.Item1.ColumnList)
                {
                    // Search for the cube column in 
                    // every matched database's table
                    // column list.
                    Boolean isFound = 
                        tuple.Item2.ColumnList.Any(
                            x => x.ColumnName.ToLower() == cubeColumn.ColumnName.ToLower());

                    // If the cube column is not found
                    // add it to the unfound list.
                    if(!isFound)
                    {
                        unfoundColumns.Add(cubeColumn);
                    }
                }

                // Collect all the columns that cannot be
                // matched with a database column
                if(unfoundColumns.Count > 0)
                {
                    CubeTable tableWithMissingColumns = new CubeTable();
                    tableWithMissingColumns.TableName = cubeTable.TableName;
                    tableWithMissingColumns.ColumnList = unfoundColumns;

                    nonPresentColumnTables.Add(tableWithMissingColumns);
                }
            }
            // Print results back to user
            // First; do the non statisfied conditions
            if (nonPresentColumnTables.Count > 0)
            {
                foreach(CubeTable cubeTable in nonPresentColumnTables)
                {
                    foreach(CubeColumn cubeColumn in cubeTable.ColumnList)
                    {
                        string message = $"The cube column {cubeColumn.CubeColumnName}" +
                            $" from cube table {cubeTable.TableName} cannot be found in" +
                            $" it's corresponding databases.";
                        WriteFailedConditions(message);
                    }
                }

                // Next; throw an exception to
                // halt the execution of the program
                //throw new MatchException($"{checkName} failed");
            }
            // If everything looks fine, pass the check!
            else
            {
                WriteTestSuccess(checkName);
            }
        }

        public static void WriteTestSuccess(string checkName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nCheck {checkName} passed!");
            Console.ResetColor();
        }

        public static void WriteFailedConditions(string failedCondition)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {failedCondition}");
            Console.ResetColor();
        }
    }
}