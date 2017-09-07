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
        [HttpGet]
        [Route("loanapplication")]
        // POST api/values
        public string searchuser(Loan info)
        {
            try
            {
                return LoanRepo.LoanApplication(info);
            }
            catch
            {
                return "Hm. Seems like something went wrong.";
            }
        }
    }
}