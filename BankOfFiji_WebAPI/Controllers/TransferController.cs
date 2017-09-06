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
        [HttpPost]
        [Route("checkaccs")]
        // POST api/values
        public List<Account> CheckAccounts([FromBody]int custid)
        {
            List<Account> List = new List<Account>();

            List = TransferRepo.CheckBankAccounts(custid);

            return List;

        }

        [HttpPost]
        [Route("getotheracc")]
        // POST api/values
        public List<Account> GetOtherAccounts(Account info)
        {
            List<Account> List = new List<Account>();

            List = TransferRepo.GetOtherAccounts(info);

            return List;

        }

        [HttpPost]
        [Route("transfertoacc")]
        // POST api/values
        public string EnableTransfer([FromBody]Transfer info)
        {
            string message = TransferRepo.EnableTransfer(info);

            return message;

        }
    }
}