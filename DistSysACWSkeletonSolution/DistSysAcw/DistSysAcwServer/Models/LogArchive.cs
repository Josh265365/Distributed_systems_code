using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace DistSysAcwServer.Models
{
    public class LogArchive
    {
        [Key]
        public int LogId { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }

       public string UserApiKey { get; set; }


        public LogArchive()
        {

        }

        public LogArchive(string Logstring, string userApiKey)
        {

            LogDateTime = DateTime.Now;
            LogString = Logstring;
            UserApiKey = userApiKey;

        }
    }
}
