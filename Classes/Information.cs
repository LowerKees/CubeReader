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

        enum MessageType
        {
            Information = 1,
            Warning = 2,
            Error = 3
        };

        private static void OutputInformation(string message, MessageType messageType)
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

    }
}
