using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Repositories
{
    public class LoanRepo
    {
        public static string LoanApplication(Loan info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                // Check if password is correct
                LoanApplications NewEntry = new LoanApplications();
                NewEntry.monthlyRent_MortageAmt = 0;
                NewEntry.accountNo = info.AccountNo;
                NewEntry.applicationStatusId = 1;
                NewEntry.assetTypeId = info.AssetID;
                NewEntry.desiredLoanAmt = info.LoanAmount;
                NewEntry.loanTypeId = info.LoanID;

                db.LoanApplications.Add(NewEntry);
                db.SaveChanges();

                // Username does not exist
                return "Loan application for $" + info.LoanAmount + " sent succesfully.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "Database query error occured. Contact administrator.";
            }

        }
    }
}