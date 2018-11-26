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
		public static string CheckPath(ref string path)
		{
			string answer;

			if (!(Directory.Exists(path)))
			{

				Console.WriteLine($"The path {path} does not exists.");
				Console.WriteLine("Press [R] to enter new path.");
				Console.WriteLine("Press [X] to exit program.");

				answer = Console.ReadLine().ToString();

				switch (answer.ToUpper())
				{
					case "X":
						Console.WriteLine("Exiting program...");
						break;
					case "R":
						path = setCubePath();
						break;
					default:
						Console.WriteLine("Invalid input. Please retry.");
						CheckPath(ref path);
						break;
				}

				return answer.ToUpper();
			}
			else
			{
				return "$";
			}
		}

		public static string setCubePath()
		{
			Console.WriteLine("Enter the cube folder path:");
			string cubePath = Console.ReadLine();

			return cubePath;
		}

		public static string setDacpacPath()
		{
			Console.WriteLine("Enter the dacpac folder path:");
			string dacpacPath = Console.ReadLine();

			return dacpacPath;
		}
	}
}
