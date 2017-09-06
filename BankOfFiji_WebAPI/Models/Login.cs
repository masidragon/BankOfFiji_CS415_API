using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class Login
    {
        public string Username { get; set; }
        public int RoleID { get; set; }
        public int CustomerID { get; set; }
        public string Password { get; set; }
    }
}