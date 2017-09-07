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
        // GET api/userdetails
        /// <summary>
        /// Verify the user exists in the system and return their details.
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns>The user details for the dashboard view.</returns>
        [HttpGet]
        [Route("userdetails")]
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