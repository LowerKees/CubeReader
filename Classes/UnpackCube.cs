using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
	public static class UnpackCube
	{
		public static List<Cube> UnpackCubes(List<string> cubeFileList)
		{
			List<Cube> cubeList = new List<Cube>();

			try
			{
				foreach (string cube in cubeFileList)
				{
					cubeList.Add(new Cube(cube));
				}
				return cubeList;
			}
			catch (Exception e)
			{
				Information.OutputInformation($"Failed to add a cube to the program",
					Information.MessageType.Error);
				throw e;
			}
		}
	}
}
