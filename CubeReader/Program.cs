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
			List<Cube> compareCubeList = new List<Cube>();

			// Get user input
			Dictionary<PathType, string> paths = ProgramFlow.UserInput();

			// Find cubes and databases
			List<string> xmlaFileList = GetFileList(paths[PathType.cube], "*.xmla");
			List<string> dacpacFileList = GetFileList(paths[PathType.dacpac], "*.dacpac");

			// Extract databases and cubes into memory
			List<Database> databases = UnpackDacpac.UnpackDacpacs(dacpacFileList);
			List<Cube> cubes = UnpackCube.UnpackCubes(xmlaFileList);

			// Match cubes and databases in list of matches
			List<Match> matches = new List<Match>();
			try
			{
				foreach (Cube cube in compareCubeList)
				{
					matches.Add(Match.MatchCubeToDatabase(databases, cube));
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



		private static List<string> GetFileList(string path, string extension)
		{
			List<string> xmlaFiles = new List<string>();
			xmlaFiles.AddRange(Directory.GetFiles(path, extension));

			foreach (string xmlaFile in xmlaFiles)
			{
				Console.WriteLine($"Found the file {xmlaFile}");
			}

			return xmlaFiles;
		}



		private static void IntroduceCubeChecks(string cubeName)
		{
			Console.WriteLine("\n*****************************************************");
			Console.WriteLine($"***** Running checks for {cubeName}...\n");
		}
	}
}
