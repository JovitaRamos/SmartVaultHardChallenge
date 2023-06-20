using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartVault.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

namespace SmartVault.DataGeneration
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            SQLiteConnection.CreateFile(configuration["DatabaseFileName"]);
            File.WriteAllText("TestDoc.txt", $"This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}");

            using (var connection = new SQLiteConnection(string.Format(configuration?["ConnectionStrings:DefaultConnection"] ?? "", configuration?["DatabaseFileName"])))
            {
                var files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");
                for (int i = 0; i <= 2; i++)
                {
                    var serializer = new XmlSerializer(typeof(BusinessObject));
                    var businessObject = serializer.Deserialize(new StreamReader(files[i])) as BusinessObject;
                    connection.Execute(businessObject?.Script);

                }
                var documentNumber = 0;
                var documentPath = new FileInfo("TestDoc.txt").FullName;
                long documentLength = new FileInfo(documentPath).Length;
                var randomDayIterator = RandomDay().GetEnumerator();

                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        command.Parameters.Add("@Id", DbType.Int32);
                        command.Parameters.Add("@FirstName", DbType.String);
                        command.Parameters.Add("@LastName", DbType.String);
                        command.Parameters.Add("@DateOfBirth", DbType.String);
                        command.Parameters.Add("@UserAccountId", DbType.Int32);
                        command.Parameters.Add("@Username", DbType.String);
                        command.Parameters.Add("@Password", DbType.String);
                        
                        command.Parameters.Add("@AccountName", DbType.String);
                        command.Parameters.Add("@AccountId", DbType.Int32);
                        
                        command.Parameters.Add("@DocumentId", DbType.Int32);
                        command.Parameters.Add("@DocumentAccountId", DbType.Int32);
                        command.Parameters.Add("@DocumentName", DbType.String);
                        command.Parameters.Add("@FilePath", DbType.String);
                        command.Parameters.Add("@Length", DbType.Int32);

                        for (int i = 0; i < 100; i++)
                        {
                            randomDayIterator.MoveNext();

                            command.CommandText = "INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password) VALUES(@Id, @FirstName, @LastName, @DateOfBirth, @AccountId, @Username, @Password);";
                            command.Parameters["@Id"].Value = i;
                            command.Parameters["@FirstName"].Value = $"FName{i}";
                            command.Parameters["@LastName"].Value = $"LName{i}";
                            command.Parameters["@DateOfBirth"].Value = randomDayIterator.Current.ToString("yyyy-MM-dd");
                            command.Parameters["@UserAccountId"].Value = i;
                            command.Parameters["@Username"].Value = $"UserName-{i}";
                            command.Parameters["@Password"].Value = "e10adc3949ba59abbe56e057f20f883e";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO Account (Id, Name) VALUES(@AccountId, @AccountName);";
                            command.Parameters["@AccountId"].Value = i;
                            command.Parameters["@AccountName"].Value = $"Account{i}";
                            command.ExecuteNonQuery();

                            for (int d = 0; d < 10000; d++, documentNumber++)
                            {
                                command.CommandText = "INSERT INTO Document (Id, Name, FilePath, Length, AccountId)  VALUES(@DocumentId, @DocumentName, @FilePath, @Length, @DocumentAccountId);";
                                command.Parameters["@DocumentId"].Value = documentNumber;
                                command.Parameters["@DocumentName"].Value = $"Document{i}-{d}.txt";
                                command.Parameters["@FilePath"].Value = documentPath;
                                command.Parameters["@Length"].Value = documentLength;
                                command.Parameters["@DocumentAccountId"].Value = i;
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }
                var accountData = connection.Query("SELECT COUNT(*) FROM Account;");
                Console.WriteLine($"AccountCount: {JsonConvert.SerializeObject(accountData)}");
                var documentData = connection.Query("SELECT COUNT(*) FROM Document;");
                Console.WriteLine($"DocumentCount: {JsonConvert.SerializeObject(documentData)}");
                var userData = connection.Query("SELECT COUNT(*) FROM User;");
                Console.WriteLine($"UserCount: {JsonConvert.SerializeObject(userData)}");

                connection.Close();
            }
        }

        static IEnumerable<DateTime> RandomDay()
        {
            DateTime start = new DateTime(1985, 1, 1);
            Random gen = new Random();
            int range = (DateTime.Today - start).Days;
            while (true)
                yield return start.AddDays(gen.Next(range));
        }
    }
}