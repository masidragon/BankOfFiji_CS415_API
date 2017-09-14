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
                return "Yay! Loan application for $" + info.LoanAmount + " sent succesfully.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "Database query error occured. Contact administrator.";
            }

        }

        public static List<LoanTypes> CheckLoanTypes()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                // Check if password is correct
                var query = (from all in db.LoanType
                            select all).ToList();

                List<LoanTypes> List = new List<LoanTypes>();

                // Username does not exist
                foreach(var item in query)
                {
                    LoanTypes NewEntry = new LoanTypes();
                    NewEntry.LoanID = item.LoanTypeId;
                    NewEntry.LoanDescription = item.LoanTypeDesc;
                    List.Add(NewEntry);
                }
                return List;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }

        }

        public static List<AssetTypes> CheckAssetTypes()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                List<AssetTypes> List = new List<AssetTypes>();
                // Check if password is correct
                var query = (from all in db.AssetType
                             select all).ToList();

                foreach (var item in query)
                {
                    AssetTypes NewEntry = new AssetTypes();
                    NewEntry.AssetID = item.assetTypeId;
                    NewEntry.AssetDescription = item.assetTypeDesc;
                    List.Add(NewEntry);
                }

                // Username does not exist
                return List;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }

        }
    }
}