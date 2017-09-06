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

            try
            {
                var FindProfile = from Object in db.CustomerProfile
                                  where custID == Object.customerId
                                  select Object;

                var ListAccount = (from all in db.BankAccount
                                   where all.userId == custID
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
    }
}