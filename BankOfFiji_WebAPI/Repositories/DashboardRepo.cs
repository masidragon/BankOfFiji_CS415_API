using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Repositories
{
    public class DashboardRepo
    {
        public static UserDetails Check_UserDetails(int custID)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();
            UserDetails Data = new UserDetails();
            List<Account> List = new List<Account>();
            List<TransactionHistory> Interest = new List<TransactionHistory>();
            List<TransactionHistory> MonthlyFee = new List<TransactionHistory>();

            try
            {
                var FindProfile = from Object in db.CustomerProfile
                                  where custID == Object.customerId
                                  select Object;

                var ListAccount = (from all in db.BankAccount
                                   where all.userId == custID
                                   select all).ToList();

                var ListInterestEarned = (from all in db.Transactions
                                          where all.transactionTypeId == 6
                                          select all).ToList();

                var ListMonthlyFee = (from all in db.Transactions
                                          where all.transactionTypeId == 8
                                          select all).ToList();

                foreach (var item in ListAccount)
                {
                    Account newentry = new Account();

                    newentry.AccountBalance = (item.creditBal - item.debitBal);
                    newentry.AccountStatus = item.AccountStatus.accountStatusDesc;
                    newentry.Date = item.startDate.ToShortDateString();
                    newentry.ID = item.accountNo;
                    newentry.TotalInterest = item.totalInterest;
                    newentry.Type = item.AccountType.accountTypeDesc;
                    newentry.UserID = item.userId;

                    List.Add(newentry);
                }

                foreach (var item in ListInterestEarned)
                {
                    TransactionHistory newentry = new TransactionHistory();

                    newentry.Adjustment = "CR";
                    newentry.Amount = item.transcAmount;
                    newentry.Balance = 0;
                    newentry.Date = item.transcDate.ToShortDateString();
                    newentry.Particulars = item.TransactionType.TransactionTypeDesc;
                    newentry.AccountRequested = item.destinationAccount;

                    Interest.Add(newentry);
                }

                foreach (var item in ListMonthlyFee)
                {
                    TransactionHistory newentry = new TransactionHistory();

                    newentry.Adjustment = "DR";
                    newentry.Amount = item.transcAmount;
                    newentry.Balance = 0;
                    newentry.Date = item.transcDate.ToShortDateString();
                    newentry.Particulars = item.TransactionType.TransactionTypeDesc;
                    newentry.AccountRequested = item.sourceAccount;

                    MonthlyFee.Add(newentry);
                }


                if (FindProfile.Any())
                {
                    var obj = FindProfile.FirstOrDefault();
                    Data.FirstName = obj.Users.firstName;
                    Data.LastName = obj.Users.lastName;
                    Data.CustomerID = obj.customerId;
                    Data.DOB = obj.dateOfBirth.ToString();
                    Data.FNPFNumber = obj.FNPFNumber;
                    Data.ResidentialStatus = obj.residentialType;
                    Data.HomeAddress = obj.address;
                    Data.PostalAddress = obj.postal;
                    Data.AccountList = List;
                    Data.InterestEarned = Interest;
                    Data.MonthlyFees = MonthlyFee;
                    return Data;
                }

                return Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Data;
            }
        }

        public static List<Notification> GetAllNotifications()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            return (from all in db.Notification select all).ToList();
        } 
    }
}