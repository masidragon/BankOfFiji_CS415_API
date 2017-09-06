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
        [HttpPost]
        [Route("authenticate")]
        // POST api/values
        public string validate(Login info)
        {
            if (info == null)
            {
                return "Invalid Data Entry";
            }

            return LoginRepo.Check_Credentials(info);
        }

        [HttpPost]
        [Route("idrequest")]
        // POST api/values
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