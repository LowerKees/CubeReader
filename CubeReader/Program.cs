using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Classes;

namespace CubeReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Finding the cubes we're looking for
            string cubePath = setCubePath();
            string dacpacPath = setDacpacPath();

            for (int i = 0; i < 1; i ++)
            {
                if (!Directory.Exists(cubePath))
                {
                    setProgram(ref i, ref cubePath);
                }
                
                if (!Directory.Exists(dacpacPath))
                {
                    setProgram(ref i, ref dacpacPath);
                }

                // Creating cube file list for in-memory storage
                List<string> xmlaFiles = new List<string>();
                xmlaFiles.AddRange(getFileList(cubePath, "*.xmla"));

                List<Cube> compareCubeList = new List<Cube>();

                try
                {
                    foreach (string cubeFile in xmlaFiles)
                    {
                        Cube myCube = new Cube(cubeFile);

                        // Print tables and columns
                        // Disabled getCubeInfo to reduce console output
                        // getCubeInfo(myCube); 

                        // Add the cube to the compare list
                        compareCubeList.Add(myCube);
                    }
                }
                catch (Exception e) 
                {
                    throw;
                }

                // Unpack dacpac files
                List<string> dacpacFileList = new List<string>();
                dacpacFileList.AddRange(getFileList(dacpacPath, "*.dacpac"));
                string unpackingPath = Environment.CurrentDirectory + "\\Unpacking";
                
                // Unpack the dacpac files
                try
                {
                    // Empty target location for each run
                    foreach (string dir in Directory.GetDirectories(unpackingPath))
                    {
                        foreach(string file in Directory.GetFiles(dir))
                        {
                            File.Delete(file);
                        }
                        Directory.Delete(dir);
                    }

                    // Unpack the dacpac
                    foreach (string file in dacpacFileList)
                    {
                        unpackDacpac(file, unpackingPath);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                // Read the model.xml file from the dacpac into memory
                List<Database> compareDatabaseList = new List<Database>();

                try
                {
                    foreach (string dir in Directory.GetDirectories(unpackingPath))
                    {
                        foreach(string file in Directory.GetFiles(dir, "model.xml"))
                        {
                            Database database = new Database(file);

                            // Print info to client
                            // Disabled method to reduce console window output
                            // getDatabaseInfo(database);

                            compareDatabaseList.Add(database);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                // Match cubes and databases in list of matches
                List<Match> matches = new List<Match>();
                try
                {
                    foreach(Cube cube in compareCubeList)
                    {
                        matches.Add(Match.MatchCubeToDatabase(compareDatabaseList, cube));
                    }
                }
                catch (MatchException me)
                {
                    Console.WriteLine(me.Message); 
                }
                catch (Exception e)
                {
                    throw;
                }          

                // Run matching checks
                try
                {
                    foreach (Match match in matches)
                    {
                        // Print out the information 
                        IntroduceCubeChecks(match.MatchingCube._cubeName);
                        // Check if all cube tables are
                        // matched in the database
                        Check.CheckForTables(match);
                        // Check if all cube columns have
                        // corresponding database columns
                        // and that the data type is a match
                        Check.CheckForColumns(match);
                    }
                }
                catch (MatchException me)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine($"ERROR: {me.Message}");
                    Console.WriteLine("An error occured: execution aborted.");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }

                // Run additional info statements
                try
                {
                    Console.WriteLine("\n*******************************************");
                    Console.WriteLine("***** Running additional info statements\n");

                    foreach (Cube cube in compareCubeList)
                    {
                        Information.OutputLogicalColumns(cube);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }

                // TODO: remove debug statement
                Console.ReadKey();
            }
        }

        public static string setCubePath()
        {
            Console.WriteLine("Enter the cube folder path:");
            string cubePath = Console.ReadLine();

            return cubePath;
        }

        private static string setDacpacPath()
        {
            Console.WriteLine("Enter the dacpac folder path:");
            string dacpacPath = Console.ReadLine();

            return dacpacPath;
        }

        public static void getCubeInfo(Cube myCube)
        {
            Console.WriteLine($"Found the cube {myCube._cubeName}");
            foreach (DataSource dataSource in myCube._cubeDs)
            {
                Console.WriteLine($"Found the connection string {dataSource._dsConnString}.");
                Console.WriteLine($"Found the initial catalog {dataSource._dsInitCatalog}");
            }

            foreach (CubeTable cubeTable in myCube._cubeTables)
            {
                Console.WriteLine($"Found the table {cubeTable.CubeTableName} referencing {cubeTable.TableName}");
                Console.WriteLine("Column list:");
                foreach (CubeColumn cubeColumn in cubeTable.ColumnList)
                {
                    Console.WriteLine($"Cube column: {cubeColumn.CubeColumnName} referencing db column {cubeColumn.ColumnName}");
                }
            }
        }

        public static void getDatabaseInfo(Database myDatabase)
        {
            Console.WriteLine($"Found the database {myDatabase._databaseDs._dsInitCatalog}");
            Console.WriteLine($"Found the connection string {myDatabase._databaseDs._dsConnString}.");
            Console.WriteLine($"Found the initial catalog {myDatabase._databaseDs._dsInitCatalog}");
            foreach (Table dbTable in myDatabase._databaseTables)
            {
                Console.WriteLine($"Found the {dbTable.TableType} {dbTable.TableName}");
                Console.WriteLine("Column list:");
                foreach (Column column in dbTable.ColumnList)
                {
                    Console.WriteLine($"Column: {column.ColumnName}");
                }
            }
        }

        private static List<string> getFileList(string path, string extension)
        {
            List<string> xmlaFiles = new List<string>();
            xmlaFiles.AddRange(Directory.GetFiles(path, extension));

            foreach (string xmlaFile in xmlaFiles)
            {
                Console.WriteLine($"Found the file {xmlaFile}");
            }

            return xmlaFiles;
        }

        private static void setProgram(ref int i, ref string path)
        {
            string answer;

            Console.WriteLine($"The path {path} does not exists.");
            Console.WriteLine("Press [R] to enter new path.");
            Console.WriteLine("Press [X] to exit program.");

            answer = Console.ReadLine().ToString();

            if (answer.ToUpper() == "X")
            {
                Console.WriteLine("Exiting program...");
            }
            if (answer.ToUpper() == "R")
            {
                i = -1;
                path = setCubePath();
            }
        }

        public static void unpackDacpac(string dacpacPath, string unpackingPath)
        {
            Console.WriteLine($"Unpacking dacpac from {dacpacPath}");

            unpackingPath += ("\\" + Path.GetFileNameWithoutExtension(dacpacPath));

            // Unzip the dacpac file
            // Create async operation for unzip
            Console.WriteLine($"Unzipping {dacpacPath}...");
            ZipFile.ExtractToDirectory(dacpacPath, unpackingPath);
        }

        private static void IntroduceCubeChecks(string cubeName)
        {
            Console.WriteLine("\n*****************************************************");
            Console.WriteLine($"***** Running checks for {cubeName}...\n");
        }
    }
}
