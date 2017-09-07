using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class Loan
    {
        public int CustID { get; set; }
        public int AccountNo { get; set; }
        public int AssetID { get; set; }
        public int LoanID { get; set; }
        public decimal LoanAmount { get; set; }
    }
}