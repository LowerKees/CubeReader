using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public static class Information
    {
        public static void OutputLogicalColumns(Cube cube)
        {
            foreach (CubeTable table in cube._cubeTables)
            {
                foreach (CubeColumn logicalColumn in table.LogicalColumns)
                {
                    string warning = $"Found logical column {logicalColumn.CubeColumnName} " +
                        $"in cube {cube._cubeName}.";
                    OutputInformation(warning, MessageType.Warning);
                }
            }
        }

        public enum MessageType
        {
            Information = 1,
            Warning = 2,
            Error = 3
        };

        public static void OutputInformation(string message, MessageType messageType)
        {
            string label = null;
            switch (messageType)
            {
                case MessageType.Information:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    label = "Info";
                    break;
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    label = "WARNING";
                    break;
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    label = "ERROR";
                    break;
            }
            Console.WriteLine($"{label}: {message}");
            Console.ResetColor();
        }

		public static void WriteCubeInfo(List<Cube> cubes)
		{
			foreach (Cube cube in cubes)
			{
				Console.WriteLine($"Found the cube {cube._cubeName}");
				foreach (DataSource dataSource in cube._cubeDs)
				{
					Console.WriteLine($"Found the connection string {dataSource._dsConnString}.");
					Console.WriteLine($"Found the initial catalog {dataSource._dsInitCatalog}");
				}

				foreach (CubeTable cubeTable in cube._cubeTables)
				{
					Console.WriteLine($"Found the table {cubeTable.CubeTableName} referencing {cubeTable.TableName}");
					Console.WriteLine("Column list:");
					foreach (CubeColumn cubeColumn in cubeTable.ColumnList)
					{
						Console.WriteLine($"Cube column: {cubeColumn.CubeColumnName} referencing db column {cubeColumn.ColumnName}");
					}
				}
			}
		}

		public static void WriteDatabaseInfo(List<Database> databases)
		{
			foreach (Database database in databases)
			{
				Console.WriteLine($"Found the database {database._databaseDs._dsInitCatalog}");
				Console.WriteLine($"Found the connection string {database._databaseDs._dsConnString}.");
				Console.WriteLine($"Found the initial catalog {database._databaseDs._dsInitCatalog}");
				foreach (Table dbTable in database._databaseTables)
				{
					Console.WriteLine($"Found the {dbTable.TableType} {dbTable.TableName}");
					Console.WriteLine("Column list:");
					foreach (Column column in dbTable.ColumnList)
					{
						Console.WriteLine($"Column: {column.ColumnName}");
					}
				}
			}
		}
	}
}
