using BankOfFiji_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Repositories
{
    public class TransactionRepo
    {
        public static List<TransactionHistory> CheckStatement(HistoryHandler info)
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            try
            {
                DateTime Start = DateTime.Parse(info.StartDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                DateTime End = DateTime.Parse(info.EndDate, System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                DateTime Now = DateTime.Now.Date;

                var CheckCurrentBalance = (from all in db.BankAccount
                                          where all.accountNo == info.AccountNumber
                                          select all.creditBal).FirstOrDefault();

                var LastBatchTransactions = from all in db.Transactions
                                            where (all.transcDate < Now && all.transcDate > End) && (all.sourceAccount == info.AccountNumber || all.destinationAccount == info.AccountNumber)
                                            select all;

                foreach(var item in LastBatchTransactions)
                {
                    // If DR transaction - BACKTRACK 
                    if(item.sourceAccount == info.AccountNumber)
                    {
                        CheckCurrentBalance = CheckCurrentBalance + item.transcAmount;
                    }
                    // If CR transaction - BACKTRACK
                    else
                    {
                        CheckCurrentBalance = CheckCurrentBalance - item.transcAmount;
                    }
                }

                // CheckCurrentBalance = Closing Balance
                decimal ClosingBalance = CheckCurrentBalance;

                var StatementBatchTransactions = from all in db.Transactions
                                                where (all.transcDate < End && all.transcDate > Start) && (all.sourceAccount == info.AccountNumber || all.destinationAccount == info.AccountNumber)
                                                select all;

                List<TransactionHistory> newlist = new List<TransactionHistory>();

                foreach (var item in StatementBatchTransactions)
                {
                    // If DR transaction - BACKTRACK 
                    if (item.sourceAccount == info.AccountNumber)
                    {
                        CheckCurrentBalance = CheckCurrentBalance + item.transcAmount;
                    }
                    // If CR transaction - BACKTRACK
                    else
                    {
                        CheckCurrentBalance = CheckCurrentBalance - item.transcAmount;
                    }
                }

                // CheckCurrentBalance = Opening Balance
                TransactionHistory OpeningBalance = new TransactionHistory();
                OpeningBalance.Adjustment = "CR";
                OpeningBalance.Amount = CheckCurrentBalance;
                OpeningBalance.Balance = CheckCurrentBalance;
                OpeningBalance.Date = Start.ToShortDateString();
                OpeningBalance.Particulars = "Balance B/F";
                newlist.Add(OpeningBalance);

                foreach (var item in StatementBatchTransactions)
                {
                    // If DR transaction
                    if (item.sourceAccount == info.AccountNumber)
                    {
                        CheckCurrentBalance = CheckCurrentBalance - item.transcAmount;
                        TransactionHistory newentry = new TransactionHistory();
                        newentry.Adjustment = "DR";
                        newentry.Amount = item.transcAmount;
                        newentry.Date = item.transcDate.ToShortDateString();
                        newentry.Particulars = item.TransactionType.TransactionTypeDesc;
                        newentry.Balance = CheckCurrentBalance;
                        newlist.Add(newentry);
                    }
                    // If CR transaction
                    else
                    {
                        CheckCurrentBalance = CheckCurrentBalance + item.transcAmount;
                        TransactionHistory newentry = new TransactionHistory();
                        newentry.Adjustment = "CR";
                        newentry.Amount = item.transcAmount;
                        newentry.Date = item.transcDate.ToShortDateString();
                        newentry.Particulars = item.TransactionType.TransactionTypeDesc;
                        newentry.Balance = CheckCurrentBalance;
                        newlist.Add(newentry);
                    }
                }

                TransactionHistory ClosingBalanceEntry = new TransactionHistory();
                ClosingBalanceEntry.Adjustment = "CR";
                ClosingBalanceEntry.Amount = ClosingBalance;
                ClosingBalanceEntry.Balance = CheckCurrentBalance;
                ClosingBalanceEntry.Date = Start.ToShortDateString();
                ClosingBalanceEntry.Particulars = "Balance C/F";
                newlist.Add(ClosingBalanceEntry);

                return newlist;
            }
            catch
            {
               List<TransactionHistory> ViewContent = null;
                return ViewContent;
            }
        }
    }
}