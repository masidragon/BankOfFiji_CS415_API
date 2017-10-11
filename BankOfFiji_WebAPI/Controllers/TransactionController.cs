using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BankOfFiji_WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public IHttpActionResult CheckTransactions(HistoryHandler HistoryParam)
        {
            List<TransactionHistory> List = new List<TransactionHistory>();

            try
            {
                List = TransactionRepo.CheckStatement(HistoryParam);

                return Ok(List);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        [Route("interestpdf")]
        public IHttpActionResult InterestPDF(HistoryHandler HistoryParam)
        {
            DataTable PDF = new DataTable();

            try
            {
                PDF = TransactionRepo.InterestPDF();

                return Ok(PDF);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("fircapdf")]
        public IHttpActionResult FIRCAPDF(HistoryHandler HistoryParam)
        {
            DataTable PDF = new DataTable();

            try
            {
                PDF = TransactionRepo.FIRCAPDF();

                return Ok(PDF);
            }
            catch
            {
                return NotFound();
            }

        }
    }
}