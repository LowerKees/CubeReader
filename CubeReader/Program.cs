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

                Cube myCube = new Cube();

                try
                {
                    // TODO: maak een loop voor xmlaFiles voor het vangen van meerdere cube files
                    myCube._cubeTables.AddRange(myCube.GetCubeTables(xmlaFiles[0]));

                    // Print tables and columns
                    getCubeInfo(myCube._cubeTables);

                    // Get cube connection string
                    myCube.getCubeDsv(xmlaFiles[0]);
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

        public static void getCubeInfo(List<CubeTable> cubeTableList)
        {
            foreach (CubeTable cubeTable in cubeTableList)
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
