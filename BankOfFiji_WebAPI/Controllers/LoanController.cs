using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankOfFiji_WebAPI.Controllers
{
    public class LoanController : ApiController
    {
        // POST api/values
        /// <summary>
        /// Submission of loan application into the database
        /// </summary>
        /// <param Loan="info"></param>
        /// <returns>A string to inform system of the status of loan submission.</returns>
        [HttpGet]
        [Route("loanapplication")]
        public IHttpActionResult searchuser(Loan info)
        {
            try
            {
                var Result =  LoanRepo.LoanApplication(info);
                return Ok(Result);
            }
            catch
            {
                return Ok("Hm. Seems like something went wrong.");
            }
        }
    }
}