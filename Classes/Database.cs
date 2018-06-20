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
        
        public void unpackDacpac(string dacpacPath, string unpackingPath)
        {
            Console.WriteLine($"Unpacking dacpac from {dacpacPath}");

            unpackingPath += ("\\" + Path.GetFileNameWithoutExtension(dacpacPath));
            
            // Unzip the dacpac file
            // Create async operation for unzip
            Console.WriteLine($"Unzipping {dacpacPath}...");
            ZipFile.ExtractToDirectory(dacpacPath, unpackingPath);       
        }
    }
}
