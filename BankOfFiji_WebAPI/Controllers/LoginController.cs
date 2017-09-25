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
        public IHttpActionResult validate(Login info)
        {
            if (info == null)
            {
                return NotFound();
            }

            try
            {
                var Result = LoginRepo.Check_Credentials(info);

                return Ok(Result);
            }
            catch
            {
                return Ok("a system error occured. Please contact administrator.");
            }
        }

        // POST api/values
        /// <summary>
        /// Retrieves all the ID associated with the logged on user.
        /// </summary>
        /// <param Login="info"></param>
        /// <returns>A list of IDs for the user</returns>
        [HttpPost]
        [Route("idrequest")]
        public IHttpActionResult retrieve(Login info)
        {
            if (info == null)
            {
                return NotFound();
            }

            try
            {
                var Result = LoginRepo.Get_UserIDs(info);
                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }
            
        }
    }
}