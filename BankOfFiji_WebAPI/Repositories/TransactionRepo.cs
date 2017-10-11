using BankOfFiji_WebAPI.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
                                            orderby all.transcDate ascending
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
                                                 orderby all.transcDate ascending
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
                        newentry.Particulars = item.TransactionType.TransactionTypeDesc + " to  Account: "  + item.destinationAccount.ToString();
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
                        newentry.Particulars = item.TransactionType.TransactionTypeDesc + " to  Account: " + item.destinationAccount.ToString();
                        newentry.Balance = CheckCurrentBalance;
                        newlist.Add(newentry);
                    }
                }

                TransactionHistory ClosingBalanceEntry = new TransactionHistory();
                ClosingBalanceEntry.Adjustment = "CR";
                ClosingBalanceEntry.Amount = ClosingBalance;
                ClosingBalanceEntry.Balance = CheckCurrentBalance;
                ClosingBalanceEntry.Date = End.ToShortDateString();
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

        public static DataTable InterestPDF()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            DataTable newpdf = new DataTable();

            newpdf.Columns.Add("Particulars");
            newpdf.Columns.Add("Interest Income/Expense (Current)");

            var InterestIncomeCurrent = from all in db.Transactions
                                        where all.destinationAccount == 10 && (all.transactionTypeId == 8 || all.transactionTypeId == 10 || all.transactionTypeId == 13) && DateTime.Now.Year == all.transcDate.Year
                                        orderby all.transcDate descending
                                        select all;


            var InterestExpenseCurrent = from all in db.Transactions
                                        where all.sourceAccount == 10 && all.transactionTypeId == 6 && DateTime.Now.Year == all.transcDate.Year
                                        orderby all.transcDate descending
                                        select all;


            decimal InterestIncomeCurrentSum = 0;
            decimal InterestExpenseCurrentSum = 0;

            foreach (var item in InterestIncomeCurrent)
            {
                InterestIncomeCurrentSum = item.transcAmount + InterestIncomeCurrentSum;
            }

            foreach (var item in InterestExpenseCurrent)
            {
                InterestExpenseCurrentSum = item.transcAmount + InterestExpenseCurrentSum;
            }


            newpdf.Rows.Add("Interest Income", InterestIncomeCurrentSum.ToString());
            newpdf.Rows.Add("Interest Expense", InterestExpenseCurrentSum.ToString());
            newpdf.Rows.Add("Net Interest Income", (InterestIncomeCurrentSum - InterestExpenseCurrentSum).ToString());

           
            return newpdf;
        }

        public static DataTable FIRCAPDF()
        {
            BankOfFijiEntities db = new BankOfFijiEntities();

            DataTable newpdf = new DataTable();

            newpdf.Columns.Add("Customer");
            newpdf.Columns.Add("Account");
            newpdf.Columns.Add("Non Residential Tax - " + DateTime.Now.Year);

            var AllNonResidents = (from all in db.BankAccount
                                  where all.taxId == 2
                                  select all.userId).Distinct();

            foreach(var user in AllNonResidents)
            {
                decimal UserSum = 0;
                var Name = (from all in db.Users
                            where all.userId == user
                            select all).FirstOrDefault();

                string CustomerName = Name.firstName + " " + Name.lastName;

                var FindAllAccounts = from all in db.BankAccount
                                      where all.userId == user
                                      select all;
                decimal Sum = 0;

                foreach (var account in FindAllAccounts)
                {
                    var TotalNRT = from all in db.Transactions
                                   where all.destinationAccount == account.accountNo && all.transcDate.Year == DateTime.Now.Year && all.transactionTypeId == 9
                                   select all;

                    Sum = 0;

                    foreach (var result in TotalNRT)
                    {
                        Sum = Sum + result.transcAmount;
                    }

                    UserSum = Sum + UserSum;

                    newpdf.Rows.Add(CustomerName, (account.AccountType.accountTypeDesc + " - " + account.accountNo.ToString()), Sum.ToString());
                }
                newpdf.Rows.Add("Total for: " + CustomerName, "", UserSum.ToString());
            }

            return newpdf;
        }
    }
}