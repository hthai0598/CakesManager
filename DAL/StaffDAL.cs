using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;
using System.Data;

namespace DAL
{
    public class staffDAL
    {

        string query;
        MySqlDataReader reader;
        MySqlConnection conn;
        public staffDAL()
        {
            conn = DBsql.OpenConnection();
        }
        public Staff Staff_Login(string username, string pass)
        {
            // Regex re = new Regex("[a-zA-Z0-9_]");
            // MatchCollection matchCollectionUsername = re.Matches(username);
            // MatchCollection matchCollectionPass = re.Matches(pass);
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            query = @"select * from Staff where StaffID = '" + username + "' and StaffPass= '" + pass + "';";
            MySqlCommand com = new MySqlCommand(query, conn);
            reader = com.ExecuteReader();
            Staff staff = null;
            if (reader.Read())
            {
                staff = new Staff();
                staff.StaffID = reader.GetString("StaffID");
                staff.Staffpass = reader.GetString("StaffPass");
                staff.calamviec = reader.GetString("Calamviec");
                staff.StaffName = reader.GetString("StaffName");
                return staff;
            }
            reader.Close();
            conn.Close();
            return staff;


        }

    }
}    