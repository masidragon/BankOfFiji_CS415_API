using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankOfFiji_WebAPI.Models
{
    public class Transfer
    {
        public int TransferAcc_ID { get; set; }
        public int Transac_Type_ID { get; set; }
        public decimal Trans_Amount { get; set; }
        public int Acc_ID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Interval { get; set; }
        public string TransferStatus { get; set; }
    }
}