using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace BankOfFiji_WebAPI.Repositories
{
	public class TransferRepo
	{
        public static List<Account> CheckBankAccounts(int info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var CheckAccs = from all in db.BankAccount
                                where all.userId == info
                                select all;

                List<Account> newlist = new List<Account>();
                foreach (var item in CheckAccs)
                {
                    Account newentry = new Account();

                    newentry.ID = item.accountNo;
                    newentry.Type = item.AccountType.accountTypeDesc;

                    newlist.Add(newentry);
                }

                return newlist;
            }
            catch
            {
                List<Account> ViewContent = null;
                return ViewContent;
            }
        }

        public static int CheckAccountsCount(int info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var CheckAccs = from all in db.BankAccount
                                where all.userId == info
                                select all;

                return CheckAccs.Count();
            }
            catch
            {
                return 0;
            }
        }

        public static List<Account> GetOtherAccounts(Account info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var CheckAccs = (from all in db.BankAccount
                                where all.userId == info.UserID && all.accountNo != info.ID
                                select all).ToList();

                List<Account> newlist = new List<Account>();
                foreach (var item in CheckAccs)
                {
                    Account newentry = new Account();

                    newentry.ID = item.accountNo;
                    newentry.Type = item.AccountType.accountTypeDesc;

                    newlist.Add(newentry);
                }

                return newlist;
            }
            catch
            {
                List<Account> ViewContent = null;
                return ViewContent;
            }
        }


        public static List<Account> GetCompanyAccounts(Account info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var FindAllCompanies =  (from all in db.Users
                                        where all.roleId == 1001
                                        select all).ToList();

                List <BankAccount> CheckAccs = new List<BankAccount>();
                List<Account> newlist = new List<Account>();

                foreach (var item in FindAllCompanies)
                {
                    CheckAccs = (from all in db.BankAccount
                                     where all.userId == item.userId
                                     select all).ToList();
                    
                    foreach (var result in CheckAccs)
                    {
                        Account newentry = new Account();

                        newentry.ID = result.accountNo;
                        newentry.Type = result.AccountType.accountTypeDesc;

                        newlist.Add(newentry);
                    }
                }

                return newlist;
            }
            catch
            {
                List<Account> ViewContent = null;
                return ViewContent;
            }
        }

        public static string EnableTransfer(Transfer info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var MoneyExists = from all in db.BankAccount
                                    where all.creditBal <= info.Trans_Amount && all.accountNo == info.Acc_ID
                                    select all;

                if (MoneyExists.Any())
                {
                    return "Oh no! You have insufficient funds to transfer!";
                }
                BankAccount AdjustValues = new BankAccount();



                Transactions NewEntry = new Transactions();

                var LastID = from all in db.Transactions
                             select all.transcId;
                NewEntry.transcId = Guid.NewGuid();
                NewEntry.transactionTypeId = info.Transac_Type_ID;
                NewEntry.transcAmount = info.Trans_Amount;
                NewEntry.transcDate = DateTime.Now;
                NewEntry.sourceAccount = info.Acc_ID;
                NewEntry.destinationAccount = info.TransferAcc_ID;

                db.Transactions.Add(NewEntry);
                db.SaveChanges();

                return "Yay! You have sucessfully transfered $" + info.Trans_Amount + " to Acc Number:" + info.TransferAcc_ID + " from Acc Number:" + info.Acc_ID;
            }
            catch
            {
                return "Oh no! Something seems to have gone wrong.";
            }
        }
    }
}