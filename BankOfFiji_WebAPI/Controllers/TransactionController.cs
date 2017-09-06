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
        [HttpPost]
        [Route("getstatement")]
        // POST api/values
        public List<TransactionHistory> CheckTransactions(HistoryHandler HistoryParam)
        {
            List<TransactionHistory> List = new List<TransactionHistory>();

            List = TransactionRepo.CheckStatement(HistoryParam);

            return List;

        }
    }
}