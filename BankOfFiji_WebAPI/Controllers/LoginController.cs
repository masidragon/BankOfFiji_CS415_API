using BankOfFiji_WebAPI.Models;
using BankOfFiji_WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BankOfFiji_WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        // POST api/values
        /// <summary>
        /// Validate the user's existance in the system
        /// </summary>
        /// <param Login="info"></param>
        /// <returns>A string mentioning the result of validation.</returns>
        [HttpPost]
        [Route("authenticate")]
        public string validate(Login info)
        {
            if (info == null)
            {
                return "Invalid Data Entry";
            }

            return LoginRepo.Check_Credentials(info);
        }

        // POST api/values
        /// <summary>
        /// Retrieves all the ID associated with the logged on user.
        /// </summary>
        /// <param Login="info"></param>
        /// <returns>A list of IDs for the user</returns>
        [HttpPost]
        [Route("idrequest")]
        public UserDetails retrieve(Login info)
        {
            if (info == null)
            {
                return null;
            }

            return LoginRepo.Get_UserIDs(info);
        }
    }
}