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
            string cubePath;
            cubePath = setCubePath();

            for (int i = 0; i < 1; i ++)
            {
                string answer = "";

                if (!Directory.Exists(cubePath))
                {
                    Console.WriteLine($"The path {cubePath} does not exists.");
                    Console.WriteLine("Press [R] to enter new path.");
                    Console.WriteLine("Press [X] to exit program.");
                    answer = Console.ReadLine().ToString();
                }

                if (answer.ToUpper() == 'X'.ToString())
                { 
                    Console.WriteLine("Exiting program...");
                    continue;
                }
                if (answer.ToUpper() == 'R'.ToString())
                {
                    i = -1;
                    cubePath = setCubePath();
                    continue;
                }

                // Retrieving cube content
                Console.WriteLine($"Getting file from {cubePath}...");

                // Creating file list
                List<string> xmlaFiles = new List<string>();
                xmlaFiles.AddRange(getFileList(cubePath));

                // TODO: lijst van cubes maken
                List<Cube> compareCubeList = new List<Cube>();

                try
                {
                    foreach (string cubeFile in xmlaFiles)
                    {
                        Cube myCube = new Cube();

                        myCube._cubeTables.AddRange(myCube.GetCubeTables(cubeFile));

                        // Get cube connection string
                        myCube._cubeDs = myCube.getCubeDataSource(cubeFile);

                        // Get cube name
                        myCube._cubeName = myCube.getCubeName(cubeFile);

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

        public static void getCubeInfo(Cube myCube)
        {
            Console.WriteLine($"Found the cube {myCube._cubeName}");
            Console.WriteLine($"Found the connection string {myCube._cubeDs._dsConnString}.");
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

        private static List<string> getFileList(string cubePath)
        {
            List<string> xmlaFiles = new List<string>();
            xmlaFiles.AddRange(Directory.GetFiles(cubePath, "*.xmla"));

            foreach (string xmlaFile in xmlaFiles)
            {
                Console.WriteLine($"Found the file {xmlaFile}");
            }

            return xmlaFiles;
        }
    }
}
