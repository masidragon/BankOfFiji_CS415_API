using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class TransactionDetailsReply
    {
        public class TransactionDetails
        {
            string TransactionDate { get; set; }
            decimal Value { get; set; }
            string Adjustment { get; set; }
            int DestinationAccount { get; set; }
        }

        public List<TransactionDetails> TransactionHistoryDetails { get; set; }
        public int OriginalBalance { get; set; }
    }
}