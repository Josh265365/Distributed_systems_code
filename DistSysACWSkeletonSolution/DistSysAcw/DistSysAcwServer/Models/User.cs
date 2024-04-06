using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DistSysAcwServer.Models
{
    /// <summary>
    /// User data class
    /// </summary>
    ///
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        #endregion


        //public Guid ApiKey { get; set; }
        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Role { get;  set; }

        private static string _storedUsername;
        private static string _storedApiKey;

        public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
       // public virtual ICollection<LogArchive> LogArchives { get; set; } = new List<LogArchive>();


        public User()
        {


           
        }
        public static void setUser(string username, string apiKey)
        {
            User user = new User();
            user.UserName = username;
            user.ApiKey = apiKey;
            _storedApiKey = apiKey;
            _storedUsername = username;
            Console.WriteLine("User stored");
        }

        public static string GetStoredUsername()
        {
            return _storedUsername;
        }

        public static string GetStoredApiKey()
        {
           return _storedApiKey;
        }
    }

  

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion


}