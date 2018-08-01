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
                    
                    if(db == null)
                    {
                        MatchException matchException = new MatchException($"The cube {cube._cubeName} has no corresponding databases.");
                        throw matchException;
                    }
                    
                    Console.WriteLine($"The cube {cube._cubeName} connects to database {db._databaseDs._dsInitCatalog}");
                    match.matchedDatabases.Add(db);
                }

                matches.Add(match);
            }

            return matches;
        }

        public void checkForTables(Matching match)
        {
            string checkName = "100. Cube vs Database table test";
            Console.WriteLine($"Running check {checkName}: \nChecking if all cube tables have corresponding database tables.");
            List<CubeTable> nonMatchedTables = new List<CubeTable>();

            foreach (CubeTable cubeTable in match.matchingCube._cubeTables)
            {
                Boolean isPresent = false;

                foreach(Database db in match.matchedDatabases)
                {
                    foreach(Table table in db._databaseTables)
                    {
                        if(table._tableName.ToUpper().Equals(cubeTable._tableName.ToUpper()))
                        {
                            isPresent = true;
                        }
                    }

                    foreach(Table view in db._databaseViews)
                    {
                        if(view._tableName.ToUpper().Equals(cubeTable._tableName.ToUpper()))
                        {
                            isPresent = true;
                        }
                    }
                }

                if (!isPresent)
                {
                    nonMatchedTables.Add(cubeTable);
                }
            }

            if (nonMatchedTables.Count > 0)
            {
                MatchException matchException = new MatchException($"Error: One or more cube tables cannot be found in the dacpac. Checkname: {checkName}");
                Console.WriteLine($"For cube {match.matchingCube._cubeName} the following tables cannot be found in the dacpacs:");
                foreach (Table table in nonMatchedTables)
                {
                    Console.WriteLine($"\n{table._tableName}");
                }

                throw matchException;
            }
            else
            {
                showTestSuccess(checkName);
            }
        }

        public void checkForColumns(Matching match)
        {
            string checkName = "101. Cube vs Database column test";
            Console.WriteLine($"Running check {checkName}: \nChecking if all cube table columns have corresponding database columns.");
            List<CubeTable> nonPresentColumnTables = new List<CubeTable>();

            foreach(CubeTable cubeTable in match.matchingCube._cubeTables)
            {
                foreach(Database database in match.matchedDatabases)
                {
                    // Find all tables in matched databases
                    // that correspond with the cube table
                    var tableFound = from table in database._databaseTables
                                     where table._tableName.ToUpper().Equals(cubeTable._tableName.ToUpper())
                                     select table;

                    // Find all views in matched databases
                    var viewFound = from view in database._databaseViews
                                    where view._tableName.ToUpper().Equals(cubeTable._tableName.ToUpper())
                                    select view;

                    if(tableFound.Count() > 0)
                    {
                        foreach(CubeColumn cubeColumn in cubeTable.columnList)
                        {
                            foreach(Table table in tableFound)
                            {
                                var columnNotFound = from column in table.columnList
                                                     where !(column._ColumnName.ToUpper().Equals(cubeColumn._ColumnName.ToUpper()))
                                                     select column;

                                foreach(var column in columnNotFound)
                                {
                                    CubeTable notFoundTable = new CubeTable();
                                    notFoundTable.columnList.Add(column);
                                    notFoundTable._cubeTableName = cubeTable._cubeTableName;

                                    nonPresentColumnTables.Add(notFoundTable);
                                }
                            }
                        }
                    }

                    if(viewFound.Count() > 0)
                    {
                        foreach (CubeColumn cubeColumn in cubeTable.columnList)
                        {
                            foreach (Table table in viewFound)
                            {
                                var columnNotFound = from column in table.columnList
                                                     where !(column._ColumnName.Equals(cubeColumn._ColumnName))
                                                     select column;

                                foreach (var column in columnNotFound)
                                {
                                    CubeTable notFoundTable = new CubeTable();
                                    notFoundTable.columnList.Add(column);
                                    notFoundTable._cubeTableName = cubeTable._cubeTableName;

                                    nonPresentColumnTables.Add(notFoundTable);
                                }
                            }
                        }
                    }
                }
            }

            if (nonPresentColumnTables.Count() == 0)
            {
                showTestSuccess(checkName);
            }
            else
            {
                foreach (CubeTable table in nonPresentColumnTables)
                {
                    foreach (Column column in table.columnList)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Cube table {table._tableName}'s column {column._ColumnName} " +
                            $"cannot be matched with the presented databases.");
                    }
                }
            }

            

        }

        public static void showTestSuccess(string checkName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nCheck {checkName} passed!");
            Console.ResetColor();
        }
    }
}
