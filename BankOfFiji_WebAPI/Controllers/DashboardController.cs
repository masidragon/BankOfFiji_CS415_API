﻿using BankOfFiji_WebAPI.Models;
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
        public IHttpActionResult searchuser(int CustID)
        {
            if (CustID == 0)
            {
                return NotFound();
            }

            try
            {
                var Result = DashboardRepo.Check_UserDetails(CustID);

                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }

        }

        // GET api/userdetails
        /// <summary>
        /// Verify the user exists in the system and return their details.
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns>The user details for the dashboard view.</returns>
        [HttpGet]
        [Route("notifications")]
        public IHttpActionResult getnotifications()
        {
            try
            {
                var Result = DashboardRepo.GetAllNotifications();

                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }

        }

        // POST api/userdetails
        /// <summary>
        /// Verify the user exists in the system and return their details.
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns>The user details for the dashboard view.</returns>
        [HttpGet]
        [Route("getsinglenotifications")]
        public IHttpActionResult getsinglenotifications(int info)
        {
            try
            {
                var Result = DashboardRepo.GetSingleNotifications(info);

                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("setsinglenotifications")]
        public IHttpActionResult setsinglenotifications(Notification info)
        {
            try
            {
                var Result = DashboardRepo.SetSingleNotifications(info);

                return Ok(Result);
            }
            catch
            {
                return NotFound();
            }

        }
    }
}