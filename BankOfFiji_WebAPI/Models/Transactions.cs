//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankOfFiji_WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transactions
    {
        public System.Guid transcId { get; set; }
        public int transactionTypeId { get; set; }
        public System.DateTime transcDate { get; set; }
        public int sourceAccount { get; set; }
        public int destinationAccount { get; set; }
        public decimal transcAmount { get; set; }
    
        public virtual BankAccount BankAccount { get; set; }
        public virtual BankAccount BankAccount1 { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}
