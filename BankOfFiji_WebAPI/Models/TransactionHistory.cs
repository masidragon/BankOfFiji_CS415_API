using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class TransactionHistory
    {
        public int AccountRequested { get; set; }
        public string Date { get; set; }
        public decimal Balance { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
        public string Adjustment { get; set; }
    }
}