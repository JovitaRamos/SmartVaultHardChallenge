using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using SmartVault.Program.BusinessObjects;
using System.Linq;

namespace SmartVault.Program
{
    partial class Program
    {
        static void Main(string[] args)
        {
            /*if (args.Length == 0)
            {
                return;
            }
            */
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();


            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = configuration["CodeGenerationFilePath"];
            startInfo.Arguments = Directory.GetCurrentDirectory();

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            using (var connection = new SQLiteConnection(string.Format(configuration?["ConnectionStrings:DefaultConnection"] ?? "", configuration?["DatabaseFileName"])))
            {
                connection.Open();

                WriteEveryThirdFileToFile(connection);
                GetAllFileSizes(connection);

                connection.Close();
            }
        }

        private static void GetAllFileSizes(SQLiteConnection connection)
        {
            //dynamic totalDoc = connection.QuerySingleOrDefault($"SELECT SUM(Length) as Total FROM Document;");
            //dynamic totalDoc.Total

            List<Document> totalDocs = connection.Query<Document>($"SELECT Length FROM Document;").ToList();

            Console.WriteLine("Total File Sizes Is: " + 
                (from items in totalDocs select (long)(items.Length))
                .DefaultIfEmpty(0).Sum());
        }

        private static void WriteEveryThirdFileToFile(SQLiteConnection connection)
        {
            List<Document> totalDocs = connection.Query<Document>($@"SELECT FilePath
                                                                        FROM (
                                                                          SELECT FilePath, ROW_NUMBER() OVER (PARTITION BY AccountId ORDER BY Id) AS RowNum
                                                                          FROM Document
                                                                        ) AS Subquery
                                                                        WHERE RowNum % 3 = 0;").ToList();

            File.WriteAllText("SingleDoc.txt", string.Join(", ", totalDocs.Select(x => x.FilePath)));

            Console.WriteLine($"Content of file SingleDoc Written at {new FileInfo("SingleDoc.txt").FullName}");
            Console.WriteLine();
        }
    }
}