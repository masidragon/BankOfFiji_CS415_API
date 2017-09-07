using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankOfFiji_WebAPI.Controllers
{
    public class TransferController : ApiController
    {
        // GET: Transfer
        /// <summary>
        /// Get the count of accounts the logged on user has.
        /// </summary>
        /// <param int="custid"></param>
        /// <returns>Count of accounts owned by logged on user.</returns>
        [HttpPost]
        [Route("accscount")]
        // POST api/values
        public int CheckAccountsCount([FromBody]int custid)
        {
            return TransferRepo.CheckAccountsCount(custid);
        }


        // GET: Transfer
        /// <summary>
        /// Get the account details of the logged on user has.
        /// This populates a drop down menu in the Transfers page
        /// </summary>
        /// <param int="custid"></param>
        /// <returns>A list of accounts owned by the logged on user.</returns>
        [HttpPost]
        [Route("checkaccs")]
        // POST api/values
        public List<Account> CheckAccounts([FromBody]int custid)
        {
            List<Account> List = new List<Account>();

            List = TransferRepo.CheckBankAccounts(custid);

            return List;

        }

        // POST api/values
        /// <summary>
        /// Upon selecting one value in drop down, this will give give other possible options.
        /// </summary>
        /// <param Account="info"></param>
        /// <returns>A list of accounts owned by the logged on user.</returns>
        [HttpPost]
        [Route("getotheracc")]
        public List<Account> GetOtherAccounts(Account info)
        {
            List<Account> List = new List<Account>();

            List = TransferRepo.GetOtherAccounts(info);

            return List;

        }

        // POST api/values
        /// <summary>
        /// Get a list of all company accounts registered in Bank of Fiji 
        /// </summary>
        /// <param Account="info"></param>
        /// <returns>A list of company accounts.</returns>
        [HttpPost]
        [Route("getcompanyaccs")]
        public List<Account> GetCompanyAccs(Account info)
        {
            List<Account> List = new List<Account>();

            List = TransferRepo.GetCompanyAccounts(info);

            return List;
        }

        // POST api/values
        /// <summary>
        /// Makes a new transfer entry in the database
        /// </summary>
        /// <param Transfer="info"></param>
        /// <returns>String to state the status of the transaction</returns>
        [HttpPost]
        [Route("transfertoacc")]
        public string EnableTransfer([FromBody]Transfer info)
        {
            string message = TransferRepo.EnableTransfer(info);

            return message;

        }
    }
}