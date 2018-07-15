using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.IO;

namespace DAL
{
    public class DBsql
    {
        private static string CONNECTION_STRING = "server=localhost;user id=root;password=anhthai123;port=3306;database=CakeManager;SslMode=None";
        public static MySqlConnection OpenDefaultConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = CONNECTION_STRING
                };
                connection.Open();
                return connection;
            }
            catch
            {
                return null;
            }
        }

        public static MySqlConnection OpenConnection()
        {
            // try{
            //     string connectionString;
            //     FileStream fileStream = new FileStream("ConnectionString.txt", FileMode.Open);
            //     using (StreamReader reader = new StreamReader(fileStream))
            //     {
            //         connectionString = reader.ReadLine();
            //     }
            //     return OpenConnection(connectionString);
            // }catch{
            //     return null;
            // }
            try
            {
                if (CONNECTION_STRING == null)
                {
                    using (FileStream fileStream = File.OpenRead("ConnectionString.txt"))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            CONNECTION_STRING = reader.ReadLine();
                        }
                    }
                }
                return OpenConnection(CONNECTION_STRING);
            }
            catch
            {
                return null;
            }
        }

        public static MySqlConnection OpenConnection(string connectionString)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = connectionString
                };
                connection.Open();
                return connection;
            }
            catch
            {
                return null;
            }
        }
    }
}