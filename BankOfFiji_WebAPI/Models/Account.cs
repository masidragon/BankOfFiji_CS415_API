using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class Account
    {
        public int ID { get; set; }//AccountID
        public int UserID { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal TotalInterest { get; set; }
        public string AccountStatus { get; set; }
    }
}