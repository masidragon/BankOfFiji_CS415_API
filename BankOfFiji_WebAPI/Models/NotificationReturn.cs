using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankOfFiji_WebAPI.Models
{
    public class NotificationReturn
    {
        public int NotificationID { get; set; }
        public string NotificationType { get; set; }
        public IList<SelectListItem> NotificationStatus { get; set; }
    }
}