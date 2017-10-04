using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankOfFiji_WebAPI.Aspects;

namespace BankOfFiji_WebAPI.Aspect
{
    public class TestNotification
    {
        [NotificationsAspect]
        public string Save()
        {
            Console.WriteLine("Hey");
            return "hi";
        }
    }
}