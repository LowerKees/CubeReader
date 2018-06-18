using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Database
    {
        private DataSource databaseDs;
        private List<Table> databaseTables;

        public Database(string dacpacPath)
        {
            databaseDs = null;
            databaseTables = null;
        }     
        
        public void unpackDacpac(string dacpacPath)
        {
            Console.WriteLine($"Unpacking dacpac from {dacpacPath}");
            string unpackingPath = Path.Combine(Environment.CurrentDirectory, @"\Unpacking");

            // Empty target location for each run
            foreach(string file in Directory.GetFiles(unpackingPath))
            {
                File.Delete(file);
            }

            // Unzip the dacpac file
            // Create async operation for unzip
            foreach(string file in Directory.GetFiles(dacpacPath, "*.dacpac"))
            {
                Console.WriteLine($"Unzipping {file}...");
                ZipFile.ExtractToDirectory(file, unpackingPath);
            }
                    
        }
    }
}
