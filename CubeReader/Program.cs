using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        getCubeInfo(myCube);

                        // Add the cube to the compare list
                        compareCubeList.Add(myCube);
                    }
                }
                catch (Exception e) 
                {
                    Console.WriteLine($"An error occured while reading the cube: {e.Message}");
                }

                // Unpack and read .dacpac files
                List<string> dacpacFileList = new List<string>();
                dacpacFileList.AddRange(getFileList(dacpacPath, "*.dacpac"));
                try
                {
                    foreach(string dacpacFile in dacpacFileList)
                    {
                        Database database = new Database(dacpacFile);

                        // Unpack the dacpac files
                        string unpackingPath = Environment.CurrentDirectory + "\\Unpacking";
                        
                        // Empty target location for each run
                        foreach (string file in Directory.GetFiles(unpackingPath))
                        {
                            File.Delete(file);
                        }
                        database.unpackDacpac(dacpacFile, unpackingPath);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occured while reading dacpac files: {e.Message}");
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
            Console.WriteLine($"Found the connection string {myCube._cubeDs._dsConnString}.");
            Console.WriteLine($"Found the initial catalog {myCube._cubeDs._dsInitCatalog}");
            foreach (CubeTable cubeTable in myCube._cubeTables)
            {
                Console.WriteLine($"Found the table {cubeTable._tableName}");
                Console.WriteLine("Column list:");
                foreach (Column cubeColumn in cubeTable.columnList)
                {
                    Console.WriteLine($"Column: {cubeColumn.myColumnName}");
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
    }
}
