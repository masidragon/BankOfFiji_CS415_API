using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class TransactionHistory
    {
        public string Date { get; set; }
        public int DestinationAccount { get; set; }
        public int SourceAccount { get; set; }
        public decimal Amount { get; set; }
        public string Adjustment { get; set; }
        public string TypeOfTrans { get; set; }
    }
}