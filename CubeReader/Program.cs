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
			List<string> xmlaFileList = ProgramFlow.GetFileList(paths[PathType.cube], "*.xmla");
			List<string> dacpacFileList = ProgramFlow.GetFileList(paths[PathType.dacpac], "*.dacpac");

			// Extract databases and cubes into memory
			List<Database> databases = UnpackDacpac.UnpackDacpacs(dacpacFileList);
			List<Cube> cubes = UnpackCube.UnpackCubes(xmlaFileList);

			// Match cubes and databases in list of matches
			List<Match> matches = Match.CreateMatchList(databases, cubes);

			// Run matching checks
			Check.RunChecks(matches);

			// Run additional info checks
			Check.RunAdditionalCubeChecks(cubes);

			// TODO: remove debug statement
			Console.ReadKey();
		}
	}
}
