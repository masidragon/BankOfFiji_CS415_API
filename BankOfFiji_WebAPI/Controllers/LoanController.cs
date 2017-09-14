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
        [HttpPost]
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

        // POST api/values
        /// <summary>
        /// Accumulate all available loan types
        /// </summary>
        /// <returns>A list of all avaialble loan types.</returns>
        [HttpGet]
        [Route("loantype")]
        public IHttpActionResult getloantype()
        {
            List<LoanTypes> List = new List<LoanTypes>();

            try
            {
                List = LoanRepo.CheckLoanTypes();
                return Ok(List);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST api/values
        /// <summary>
        /// Accumulate all available asset types
        /// </summary>
        /// <returns>A list of all avaialble asset types.</returns>
        [HttpGet]
        [Route("assettypes")]
        public IHttpActionResult getassettype()
        {
            List<AssetTypes> List = new List<AssetTypes>();

            try
            {
                List = LoanRepo.CheckAssetTypes();
                return Ok(List);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}