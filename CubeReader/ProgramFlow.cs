using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeReader
{
	class ProgramFlow
	{
		public static Dictionary<PathType, string> UserInput()
		{
			bool NeedsRefresh = false;

			Dictionary<PathType, string> Paths = new Dictionary<PathType, string>
			{
				{ PathType.cube, SetPath(PathType.cube) },
				{ PathType.dacpac, SetPath(PathType.dacpac) }
			};

			foreach(KeyValuePair<PathType, string> path in Paths)
			{
				// See if the path exists
				string answer = CheckPath(path.Value, path.Key);
				// See if the user wants to exit
				ExitProgram(answer);
				// Assign new path values if user wants to
				// by rerunnig the method
				if(answer == "R")
				{
					if (!NeedsRefresh)
					{
						NeedsRefresh = true;
					}
				}				
			}

			if(NeedsRefresh)
			{
				Paths = UserInput();
			}

			return Paths;
		}

		private static string CheckPath(string path, PathType pathType)
		{
			string answer;

			if (!(Directory.Exists(path)))
			{

				Console.WriteLine($"The path {path} does not exists.");
				Console.WriteLine("Press [R] to reenter the paths.");
				Console.WriteLine("Press [X] to exit program.");

				answer = Console.ReadLine().ToString();
			}
			else
			{
				answer = "$";
			}
			return answer.ToUpper();
		}

		private static void ExitProgram(string answer)
		{
			if (answer == "X")
			{
				Console.WriteLine("Exiting program...");
				Environment.Exit(0);
			}
		}

		private static string SetPath(PathType pathType)
		{
			Console.WriteLine($"Enter the {pathType} folder path:");
			string path = Console.ReadLine();

			return path;
		}
	}

	public enum PathType { cube, dacpac }
}
