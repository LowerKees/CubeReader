using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Classes
{
	public static class UnpackDacpac
	{
		private static readonly string unpackingPath = String.Concat(Environment.CurrentDirectory,
			"\\Unpacking");

		public static List<Database> UnpackDacpacs(List<string> dacpacFileList)
		{
			try
			{
				DeleteUnpackingPath();
				UnzipDacpac(dacpacFileList);
				List<Database> databases = DacpacToDatabase(unpackingPath);

				return databases;
			}
			catch (IOException ex)
			{
				// TODO: create logger
				throw ex;
			}
			catch (Exception ex)
			{
				// TODO: create logger
				throw ex;
			}
		}

		public static void DeleteFolder(string path)
		{
			foreach (string dir in Directory.GetDirectories(path))
			{
				foreach (string file in Directory.GetFiles(dir))
				{
					File.Delete(file);
				}
				Directory.Delete(dir);
			}
		}

		public static void DeleteUnpackingPath()
		{
			DeleteFolder(unpackingPath);
		}

		public static void UnzipDacpac(List<string> dacpacPaths, string unpackingPath)
		{
			// Unzip the dacpac file
			foreach (string path in dacpacPaths)
			{
				string landingArea = String.Concat(unpackingPath, "\\",
					Path.GetFileNameWithoutExtension(path));
				ZipFile.ExtractToDirectory(path, landingArea);
			}
		}

		public static void UnzipDacpac(List<string> dacpacFileList)
		{
			UnzipDacpac(dacpacFileList, unpackingPath);
		}

		public static List<Database> DacpacToDatabase(string unpackingPath)
		{
			List<Database> databases = new List<Database>();

			foreach (string file in Directory.GetFiles(unpackingPath, "model.xml", 
				SearchOption.AllDirectories))
			{
				Database database = new Database(file);
				databases.Add(database);
			}

			return databases;
		}
	}
}