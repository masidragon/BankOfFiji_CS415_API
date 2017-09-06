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
                //DateTime Start = Convert.ToDateTime(info.StartDate);
                //DateTime End = Convert.ToDateTime(info.EndDate);

                var CheckTrans = from all in db.Transactions
                                where all.transcDate >= Start && all.transcDate <= End && all.BankAccount.accountNo == info.AccountNumber
                                select all;

                List<TransactionHistory> newlist = new List<TransactionHistory>();
                foreach (var item in CheckTrans)
                {
                    TransactionHistory newentry = new TransactionHistory();

                    newentry.Amount = item.transcAmount;
                    newentry.Date = item.transcDate.ToShortDateString();
                    newentry.DestinationAccount = item.destinationAccount;
                    newentry.SourceAccount = item.sourceAccount;
                    newentry.TypeOfTrans = item.TransactionType.TransactionTypeDesc;

                    newlist.Add(newentry);
                }

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