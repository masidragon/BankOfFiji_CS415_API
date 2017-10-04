using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using BankOfFiji_WebAPI.Aspects;

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

        public static List<Intervals> GetAutoIntervals()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var Intervals = (from all in db.Scheduler
                                 select all).ToList();

                List<Intervals> List = new List<Intervals>();

                foreach(var item in Intervals)
                {
                    Intervals newentry = new Intervals();

                    newentry.IntervalID = item.scheduleId;
                    newentry.IntervalDescription = item.schedule;

                    List.Add(newentry);
                }

                return List;
            }
            catch
            {
                return null;
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
                var FindAllCompanies = (from all in db.Users
                                        where all.roleId == 1001
                                        select all).ToList();

                List<BankAccount> CheckAccs = new List<BankAccount>();
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
                        newentry.Type = result.Users.firstName;

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

        public static List<AutoPayments> GetAutoPayments(int CustID)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            List<AutoPayments> FinalList = new List<AutoPayments>();
            try
            {
                var FindAccs = (from all in db.BankAccount
                               where all.userId == CustID
                               select all.accountNo).ToList();

                foreach(var item in FindAccs)
                {
                    var query = (from all in db.AutoPayments
                                 where (all.scheduleId == 2 || all.scheduleId == 1) && all.sourceAccount == item
                                 select all).ToList();

                    foreach (var autopayment in query)
                    {
                        FinalList.Add(autopayment);
                    }
                }

                return FinalList;
                
            }
            catch
            {
                return null;
            }

        }

        public static string TerminateAutoPayments(int AutoPaymentID)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                var FindAutoPayment = db.AutoPayments.Find(AutoPaymentID);
                FindAutoPayment.State_ID = 4;
                db.SaveChanges();

                return "Scheduled payment from Account: " + FindAutoPayment.sourceAccount + " to Account: " + FindAutoPayment.destinationAccount + " has been terminated!";
            }
            catch
            {
                return "An error as occured Please contact administrators.";
            }

        }


        [NotificationsAspect]
        public static Transfer EnableTransfer(Transfer info)
        {
            try
            {
                BankOfFijiEntities db = new BankOfFijiEntities();

                // Check if decrement is from Savings
                var AccType = (from all in db.BankAccount
                               where info.Acc_ID == all.accountNo
                               select all.accountTypeId).FirstOrDefault();

                if (AccType == 2)
                {
                    var WithdrawlIncrementCounter = db.BankAccount.Find(info.Acc_ID);
                    WithdrawlIncrementCounter.withdrawal_monthly_counter = WithdrawlIncrementCounter.withdrawal_monthly_counter + 1;
                    db.SaveChanges();
                }

                // If Automatic Transfers was passed to API
                if (info.StartDate != "0001-01-01T00:00:00" || info.EndDate != "0001-01-01T00:00:00")
                {
                    var MoneyExists = from all in db.BankAccount
                                      where all.creditBal < info.Trans_Amount && all.accountNo == info.Acc_ID
                                      select all;


                    if (MoneyExists.Any())
                    {
                        info.TransferStatus = "Oh no! You have insufficient funds to make your bill payment!";

                        return info;
                    }

                    AutoPayments SetUpScheduler = new AutoPayments();

                    DateTime Today = DateTime.Now;
                    DateTime Start = DateTime.Parse(info.StartDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                    DateTime End = DateTime.Parse(info.EndDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));

                    SetUpScheduler.transactionTypeId = info.Transac_Type_ID;
                    SetUpScheduler.sourceAccount = info.Acc_ID;
                    SetUpScheduler.destinationAccount = info.TransferAcc_ID;
                    SetUpScheduler.paymentAmount = info.Trans_Amount;
                    SetUpScheduler.scheduleId = info.Interval;
                    SetUpScheduler.startDate = Start;

                    var FindTransType = (from all in db.TransactionType
                                        where all.TransactionTypeId == info.Transac_Type_ID
                                        select all).FirstOrDefault();

                    SetUpScheduler.TransactionType = FindTransType;

                    var FindDaysTillNext = (from all in db.Scheduler
                                            where all.scheduleId == info.Interval
                                            select all.scheduleperiod).FirstOrDefault();


                    SetUpScheduler.nextDate = Start.AddDays(FindDaysTillNext);
                    SetUpScheduler.endDate = End;
                    SetUpScheduler.terminationDate = Start.AddYears(100);

                    var FindStateType = (from all in db.Automatic_Payment_State
                                         where all.State_ID == 2
                                         select all).FirstOrDefault();

                    SetUpScheduler.Automatic_Payment_State = FindStateType;
                    SetUpScheduler.State_ID = 2;

                    if (Start == Today)
                    {

                        // Check withdrawl counter
                        var AutoWithdrawlCounter = (from all in db.BankAccount
                                                    where info.Acc_ID == all.accountNo
                                                    select all.withdrawal_monthly_counter).FirstOrDefault();

                        if (AutoWithdrawlCounter > 1)
                        {
                            // Pay for withdrawl from account
                            var WithdrawlAutoDecrement = db.BankAccount.Find(info.Acc_ID);
                            WithdrawlAutoDecrement.creditBal = WithdrawlAutoDecrement.creditBal + Decimal.Parse("5.00");

                            // Bank income
                            var WithdrawlAutoIncrement = db.BankAccount.Find(10);
                            WithdrawlAutoIncrement.creditBal = WithdrawlAutoIncrement.creditBal + Decimal.Parse("5.00");

                            // Make transaction entry
                            Transactions WithdrawlFee = new Transactions();

                            WithdrawlFee.transcId = Guid.NewGuid();
                            WithdrawlFee.transactionTypeId = 10;
                            WithdrawlFee.transcAmount = Decimal.Parse("5.00");
                            WithdrawlFee.transcDate = DateTime.Now;
                            WithdrawlFee.sourceAccount = info.Acc_ID;
                            WithdrawlFee.destinationAccount = 10;

                            db.Transactions.Add(WithdrawlFee);
                        }
                    }

                    // Decrement from account
                    var AutoDecrement = db.BankAccount.Find(info.Acc_ID);
                    AutoDecrement.creditBal = AutoDecrement.creditBal - info.Trans_Amount;

                    // Increment to account
                    var AutoIncrement = db.BankAccount.Find(info.TransferAcc_ID);
                    AutoIncrement.creditBal = AutoIncrement.creditBal + info.Trans_Amount;

                    // Make transaction entry
                    Transactions AutoNewEntry = new Transactions();

                    AutoNewEntry.transcId = Guid.NewGuid();
                    AutoNewEntry.transactionTypeId = info.Transac_Type_ID;
                    AutoNewEntry.transcAmount = info.Trans_Amount;
                    AutoNewEntry.transcDate = DateTime.Now;
                    AutoNewEntry.sourceAccount = info.Acc_ID;
                    AutoNewEntry.destinationAccount = info.TransferAcc_ID;

                    db.Transactions.Add(AutoNewEntry);

                    db.AutoPayments.Add(SetUpScheduler);
                    db.SaveChanges();

                    var TransferCheckAccountBalance = (from all in db.BankAccount
                                               where all.accountNo == info.Acc_ID
                                               select all.creditBal).FirstOrDefault();

                    if (Convert.ToDouble(TransferCheckAccountBalance) < 10.00)
                    {
                        LowBalance(info);
                    }
                }

                Transactions NewEntry = new Transactions();

                // Check withdrawl counter
                var WithdrawlCounter = (from all in db.BankAccount
                                        where info.Acc_ID == all.accountNo
                                        select all.withdrawal_monthly_counter).FirstOrDefault();

                if (WithdrawlCounter > 1)
                {
                    // Pay for withdrawl from account
                    var WithdrawlAutoDecrement = db.BankAccount.Find(info.Acc_ID);
                    WithdrawlAutoDecrement.creditBal = WithdrawlAutoDecrement.creditBal + Decimal.Parse("5.00");

                    // Bank income
                    var WithdrawlAutoIncrement = db.BankAccount.Find(10);
                    WithdrawlAutoIncrement.creditBal = WithdrawlAutoIncrement.creditBal + Decimal.Parse("5.00");

                    // Make transaction entry
                    Transactions WithdrawlFee = new Transactions();

                    WithdrawlFee.transcId = Guid.NewGuid();
                    WithdrawlFee.transactionTypeId = 10;
                    WithdrawlFee.transcAmount = Decimal.Parse("5.00");
                    WithdrawlFee.transcDate = DateTime.Now;
                    WithdrawlFee.sourceAccount = info.Acc_ID;
                    WithdrawlFee.destinationAccount = 10;

                    db.Transactions.Add(WithdrawlFee);
                }

            
                // Decrement from account
                var Decrement = db.BankAccount.Find(info.Acc_ID);
                Decrement.creditBal = Decrement.creditBal - info.Trans_Amount;

                // Increment to account
                var Increment = db.BankAccount.Find(info.TransferAcc_ID);
                Increment.creditBal = Increment.creditBal + info.Trans_Amount;

                // Make transaction entry
                NewEntry.transcId = Guid.NewGuid();
                NewEntry.transactionTypeId = info.Transac_Type_ID;
                NewEntry.transcAmount = info.Trans_Amount;
                NewEntry.transcDate = DateTime.Now;
                NewEntry.sourceAccount = info.Acc_ID;
                NewEntry.destinationAccount = info.TransferAcc_ID;

                db.Transactions.Add(NewEntry);
                db.SaveChanges();

                var CheckAccountBalance = (from all in db.BankAccount
                                           where all.accountNo == info.Acc_ID
                                           select all.creditBal).FirstOrDefault();

                if (Convert.ToDouble(CheckAccountBalance) < 10.00)
                {
                    LowBalance(info);
                }

                info.TransferStatus = "Yay! You have sucessfully transfered $" + info.Trans_Amount + " to Acc Number: " + info.TransferAcc_ID + " from Acc Number: " + info.Acc_ID;

                return info;

            }
            catch(Exception ex)
            {
                info.TransferStatus = "Oh no! Error:" + ex;

                return info;
            }
        }

        [NotificationsAspect]
        public static Transfer EnableBillPayment(Transfer info)
        {
            try
            {
                BankOfFijiEntities db = new BankOfFijiEntities();

                // Check if decrement is from Savings
                var AccType = (from all in db.BankAccount
                               where info.Acc_ID == all.accountNo
                               select all.accountTypeId).FirstOrDefault();

                if (AccType == 2)
                {
                    var WithdrawlIncrementCounter = db.BankAccount.Find(info.Acc_ID);
                    WithdrawlIncrementCounter.withdrawal_monthly_counter = WithdrawlIncrementCounter.withdrawal_monthly_counter + 1;
                    db.SaveChanges();
                }

                // If Automatic Transfers was passed to API
                if (info.StartDate != "0001-01-01T00:00:00" || info.EndDate != "0001-01-01T00:00:00")
                {
                    var MoneyExists = from all in db.BankAccount
                                      where all.creditBal < info.Trans_Amount && all.accountNo == info.Acc_ID
                                      select all;


                    if (MoneyExists.Any())
                    {
                        info.TransferStatus = "Oh no! You have insufficient funds to make your bill payment!";

                        return info;
                    }

                    AutoPayments SetUpScheduler = new AutoPayments();

                    DateTime Today = DateTime.Now;
                    DateTime Start = DateTime.Parse(info.StartDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                    DateTime End = DateTime.Parse(info.EndDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));

                    SetUpScheduler.transactionTypeId = info.Transac_Type_ID;
                    SetUpScheduler.sourceAccount = info.Acc_ID;
                    SetUpScheduler.destinationAccount = info.TransferAcc_ID;
                    SetUpScheduler.paymentAmount = info.Trans_Amount;
                    SetUpScheduler.scheduleId = info.Interval;
                    SetUpScheduler.startDate = Start;

                    var FindTransType = (from all in db.TransactionType
                                         where all.TransactionTypeId == info.Transac_Type_ID
                                         select all).FirstOrDefault();

                    SetUpScheduler.TransactionType = FindTransType;

                    var FindDaysTillNext = (from all in db.Scheduler
                                            where all.scheduleId == info.Interval
                                            select all.scheduleperiod).FirstOrDefault();


                    SetUpScheduler.nextDate = Start.AddDays(FindDaysTillNext);
                    SetUpScheduler.endDate = End;
                    SetUpScheduler.terminationDate = Start.AddYears(100);

                    var FindStateType = (from all in db.Automatic_Payment_State
                                         where all.State_ID == 2
                                         select all).FirstOrDefault();

                    SetUpScheduler.Automatic_Payment_State = FindStateType;
                    SetUpScheduler.State_ID = 2;

                    if (Start == Today)
                    {

                        // Check withdrawl counter
                        var AutoWithdrawlCounter = (from all in db.BankAccount
                                                    where info.Acc_ID == all.accountNo
                                                    select all.withdrawal_monthly_counter).FirstOrDefault();

                        if (AutoWithdrawlCounter > 1)
                        {
                            // Pay for withdrawl from account
                            var WithdrawlAutoDecrement = db.BankAccount.Find(info.Acc_ID);
                            WithdrawlAutoDecrement.creditBal = WithdrawlAutoDecrement.creditBal + Decimal.Parse("5.00");

                            // Bank income
                            var WithdrawlAutoIncrement = db.BankAccount.Find(10);
                            WithdrawlAutoIncrement.creditBal = WithdrawlAutoIncrement.creditBal + Decimal.Parse("5.00");

                            // Make transaction entry
                            Transactions WithdrawlFee = new Transactions();

                            WithdrawlFee.transcId = Guid.NewGuid();
                            WithdrawlFee.transactionTypeId = 10;
                            WithdrawlFee.transcAmount = Decimal.Parse("5.00");
                            WithdrawlFee.transcDate = DateTime.Now;
                            WithdrawlFee.sourceAccount = info.Acc_ID;
                            WithdrawlFee.destinationAccount = 10;

                            db.Transactions.Add(WithdrawlFee);
                        }
                    }

                    // Decrement from account
                    var AutoDecrement = db.BankAccount.Find(info.Acc_ID);
                    AutoDecrement.creditBal = AutoDecrement.creditBal - info.Trans_Amount;

                    // Increment to account
                    var AutoIncrement = db.BankAccount.Find(info.TransferAcc_ID);
                    AutoIncrement.creditBal = AutoIncrement.creditBal + info.Trans_Amount;

                    // Make transaction entry
                    Transactions AutoNewEntry = new Transactions();

                    AutoNewEntry.transcId = Guid.NewGuid();
                    AutoNewEntry.transactionTypeId = info.Transac_Type_ID;
                    AutoNewEntry.transcAmount = info.Trans_Amount;
                    AutoNewEntry.transcDate = DateTime.Now;
                    AutoNewEntry.sourceAccount = info.Acc_ID;
                    AutoNewEntry.destinationAccount = info.TransferAcc_ID;

                    db.Transactions.Add(AutoNewEntry);

                    db.AutoPayments.Add(SetUpScheduler);
                    db.SaveChanges();

                    var BillCheckAccountBalance = (from all in db.BankAccount
                                               where all.accountNo == info.Acc_ID
                                               select all.creditBal).FirstOrDefault();

                    if (Convert.ToDouble(BillCheckAccountBalance) < 10.00)
                    {
                        LowBalance(info);
                    }
                }

                Transactions NewEntry = new Transactions();

                // Check withdrawl counter
                var WithdrawlCounter = (from all in db.BankAccount
                                        where info.Acc_ID == all.accountNo
                                        select all.withdrawal_monthly_counter).FirstOrDefault();

                if (WithdrawlCounter > 1)
                {
                    // Pay for withdrawl from account
                    var WithdrawlAutoDecrement = db.BankAccount.Find(info.Acc_ID);
                    WithdrawlAutoDecrement.creditBal = WithdrawlAutoDecrement.creditBal + Decimal.Parse("5.00");

                    // Bank income
                    var WithdrawlAutoIncrement = db.BankAccount.Find(10);
                    WithdrawlAutoIncrement.creditBal = WithdrawlAutoIncrement.creditBal + Decimal.Parse("5.00");

                    // Make transaction entry
                    Transactions WithdrawlFee = new Transactions();

                    WithdrawlFee.transcId = Guid.NewGuid();
                    WithdrawlFee.transactionTypeId = 10;
                    WithdrawlFee.transcAmount = Decimal.Parse("5.00");
                    WithdrawlFee.transcDate = DateTime.Now;
                    WithdrawlFee.sourceAccount = info.Acc_ID;
                    WithdrawlFee.destinationAccount = 10;

                    db.Transactions.Add(WithdrawlFee);
                }


                // Decrement from account
                var Decrement = db.BankAccount.Find(info.Acc_ID);
                Decrement.creditBal = Decrement.creditBal - info.Trans_Amount;

                // Increment to account
                var Increment = db.BankAccount.Find(info.TransferAcc_ID);
                Increment.creditBal = Increment.creditBal + info.Trans_Amount;

                // Make transaction entry
                NewEntry.transcId = Guid.NewGuid();
                NewEntry.transactionTypeId = info.Transac_Type_ID;
                NewEntry.transcAmount = info.Trans_Amount;
                NewEntry.transcDate = DateTime.Now;
                NewEntry.sourceAccount = info.Acc_ID;
                NewEntry.destinationAccount = info.TransferAcc_ID;

                db.Transactions.Add(NewEntry);
                db.SaveChanges();

                var CheckAccountBalance = (from all in db.BankAccount
                                           where all.accountNo == info.Acc_ID
                                           select all.creditBal).FirstOrDefault();

                if (Convert.ToDouble(CheckAccountBalance) < 10.00)
                {
                    LowBalance(info);
                }

                info.TransferStatus =  "Yay! You have sucessfully paid your bill of $" + info.Trans_Amount + " to Acc Number: " + info.TransferAcc_ID + " from Acc Number: " + info.Acc_ID;
                return info;
            }
            catch (Exception ex)
            {
                info.TransferStatus = "Oh no! Error: " + ex;

                return info;
            }
        }

        [NotificationsAspect]
        public static Transfer LowBalance(Transfer Acc)
        {
            return Acc;
        }
    }
}