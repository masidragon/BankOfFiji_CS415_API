using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankOfFiji_WebAPI.Controllers
{
    public class TransactionController : ApiController
    {
        // POST api/values
        /// <summary>
        /// Get all the transactions in the date range specified by the user.
        /// </summary>
        /// <param HistoryHandler="HistoryParam"></param>
        /// <returns>A list of Transactions objects with transaction details.</returns>
        [HttpPost]
        [Route("getstatement")]
        public List<TransactionHistory> CheckTransactions(HistoryHandler HistoryParam)
        {
            List<TransactionHistory> List = new List<TransactionHistory>();

            List = TransactionRepo.CheckStatement(HistoryParam);

            return List;

        }
    }
}