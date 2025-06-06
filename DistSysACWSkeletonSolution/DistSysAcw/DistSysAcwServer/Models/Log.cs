﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace DistSysAcwServer.Models
{
    public class Log
    {
        [Key]
        public int LogId { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }

        public Log()
        {

        }
        public Log(string logString)
        {
            LogDateTime = DateTime.Now;
            LogString = logString;
        }
    }
}
