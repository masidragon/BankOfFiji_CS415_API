﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankOfFiji_WebAPI.Models
{
    public class HistoryHandler
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AccountNumber { get; set; }
    }
}