using System;
using System.Collections.Generic;
using Persistence;
using System.Text.RegularExpressions;
using DAL;
namespace BL
{   
    public class StaffBL
    {
        
        private staffDAL udal = new staffDAL();
        public Staff Login(string username, string pass)
        {
            // Regex regex = new Regex("[a-zA-Z0-9_]");
            // MatchCollection matchCollectionUsername = regex.Matches(username);
            // MatchCollection matchCollectionPassword = regex.Matches(pass);
            // if(matchCollectionUsername.Count <= 0 || matchCollectionPassword.Count <= 0)
            // {
            //     return null;
            // }
            // return udal.Staff_Login(username,pass);
            return udal.Staff_Login(username,pass);
        }
    }
}