using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class IntTransferStatesViewModel
    {
        public int SourceAccount { get; set; }
        public int DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; }
    }
}