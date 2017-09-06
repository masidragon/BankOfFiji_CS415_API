using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankOfFiji_WebAPI.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet]
        [Route("userdetails")]
        // POST api/values
        public UserDetails searchuser(int CustID)
        {
            if (CustID == 0)
            {
                UserDetails NullHandler = new UserDetails();
                NullHandler.FirstName = "NULL";
                return NullHandler;
            }

            return DashboardRepo.Check_UserDetails(CustID);
        }
    }
}