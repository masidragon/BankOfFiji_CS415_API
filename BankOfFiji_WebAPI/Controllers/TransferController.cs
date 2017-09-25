using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BankOfFiji_WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransferController : ApiController
    {
        // POST api/accscount
        /// <summary>
        /// Get the count of accounts the logged on user has.
        /// </summary>
        /// <param int="custid"></param>
        /// <returns>Count of accounts owned by logged on user.</returns>
        [HttpPost]
        [Route("accscount")]
        public IHttpActionResult CheckAccountsCount([FromBody]int custid)
        {
            try
            {
                var Result = TransferRepo.CheckAccountsCount(custid);

                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }
            
        }


        // POST/checkaccs
        /// <summary>
        /// Get the account details of the logged on user has.
        /// This populates a drop down menu in the Transfers page
        /// </summary>
        /// <param int="custid"></param>
        /// <returns>A list of accounts owned by the logged on user.</returns>
        [HttpPost]
        [Route("checkaccs")]
        // POST api/values
        public IHttpActionResult CheckAccounts([FromBody]int custid)
        {
            List<Account> List = new List<Account>();

            try
            {
                List = TransferRepo.CheckBankAccounts(custid);
                return Ok(List);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/getotheracc
        /// <summary>
        /// Upon selecting one value in drop down, this will give give other possible options.
        /// </summary>
        /// <param Account="info"></param>
        /// <returns>A list of accounts owned by the logged on user.</returns>
        [HttpPost]
        [Route("getotheracc")]
        public IHttpActionResult GetOtherAccounts(Account info)
        {
            List<Account> List = new List<Account>();

            try
            {
                List = TransferRepo.GetOtherAccounts(info);
                return Ok(List);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/getcompanyaccs
        /// <summary>
        /// Get a list of all company accounts registered in Bank of Fiji 
        /// </summary>
        /// <param Account="info"></param>
        /// <returns>A list of company accounts.</returns>
        [HttpPost]
        [Route("getcompanyaccs")]
        public IHttpActionResult GetCompanyAccs(Account info)
        {
            List<Account> List = new List<Account>();
            try
            {
                List = TransferRepo.GetCompanyAccounts(info);

                return Ok(List);
            }
            catch
            {
                return NotFound();
            }

        }

        // POST api/transfertoacc
        /// <summary>
        /// Makes a new transfer entry in the database
        /// </summary>
        /// <param Transfer="info"></param>
        /// <returns>String to state the status of the transaction</returns>
        [HttpPost]
        [Route("transfertoacc")]
        public IHttpActionResult EnableTransfer([FromBody]Transfer info)
        {
            try
            {
                string message = TransferRepo.EnableTransfer(info);
                return Ok(message);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/values
        /// <summary>
        /// Retrieves all possible automatic payment intervals for view
        /// </summary>
        /// <returns>A list of Scheduler objects for the view</returns>
        [HttpGet]
        [Route("getintervals")]
        public IHttpActionResult GetIntervals()
        {
            try
            {
                var message = TransferRepo.GetAutoIntervals();
                return Ok(message);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/values
        /// <summary>
        /// Retrieves all scheduled payments for user view
        /// </summary>
        /// /// <param int="CustID"></param>
        /// <returns>A list of AutoPayments objects for the view</returns>
        [HttpPost]
        [Route("getautopayments")]
        public IHttpActionResult GetAllAutoPayments(int CustID)
        {
            try
            {
                var message = TransferRepo.GetAutoPayments(CustID);
                return Ok(message);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/values
        /// <summary>
        /// Terminate schduled tasks from database
        /// </summary>
        /// /// <param int="AutoPaymentID"></param>
        /// <returns>A string notifying users of the status of the termination</returns>
        [HttpPost]
        [Route("terminateautopayments")]
        public IHttpActionResult Terminate(int AutoPaymentID)
        {
            try
            {
                var message = TransferRepo.TerminateAutoPayments(AutoPaymentID);
                return Ok(message);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}