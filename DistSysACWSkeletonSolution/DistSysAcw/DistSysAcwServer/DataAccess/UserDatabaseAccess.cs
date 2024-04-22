using DistSysAcwServer.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace DistSysAcwServer.DataAccess
{
    public class UserDatabaseAccess
    {
        private readonly UserContext _context;
        public UserDatabaseAccess(UserContext context)
        {
            _context = context;

        }


        /// <summary>
        /// create a new user with given username and creating a new GUID which is saved as a strignto the database as the ApiKey
        /// </summary>
        /// <param name="username"></param>
        /// <returns>the user object</returns>

        public string CreateUser(string userName, string role)
        {
            var apiKey = Guid.NewGuid().ToString();
            var user = new User { ApiKey = apiKey, UserName = userName, Role = role };
            _context.Users.Add(user);
            _context.SaveChanges();
            return apiKey;
        }

        /// <summary>
        /// check if user with given API key exists 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns>true or flase</returns>
        public bool CheckUserExists(string username)
        {
            
            return _context.Users.Any(u => u.UserName == username);
            
        }



        /// <summary>
        /// check if user with given API key and username exists
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="username"></param>
        /// <returns>true or false</returns>
        public bool CheckUserNameandAPIExists(string apiKey, string username)
        {
            // using (var context = new UserContext())
            // {
            return _context.Users.Any(u => u.ApiKey == apiKey && u.UserName == username);
            // }
        }


        /// <summary>
        /// Check if user with given API key exists
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns>The exisitng user user</returns>
        public User GetUserByApiKey(string apiKey)
        {
            //using (var context = new UserContext())
            //{
            //    // Querying the database for the user with the given apiKey
            //    var user = context.Users.FirstOrDefault(u => u.ApiKey == apiKey);

            //    return user; // Return the user object if found, or null if not found
            //}
            return _context.Users.FirstOrDefault(u => u.ApiKey == apiKey);


        }


        /// <summary>
        /// Delete the user with the given API key
        /// </summary>
        /// <param name="apiKey"></param>
        public void DeleteUser(string apiKey)
        {
            

              var user = _context.Users.FirstOrDefault(u => u.ApiKey == apiKey);
            //var user = _context.Users.Include(u => u.Logs).FirstOrDefault(u => u.ApiKey == apiKey);
            if (user != null)
            {
                // var archiveLogs = new LogArchive();
                var logs = user.Logs.ToList();

                foreach (var log in logs)
                {
                    //var archiveLogs = new LogArchive(apiKey, log.LogString);
                    var archiveLogs = new LogArchive//fix this
                    {
                        LogString = log.LogString,
                        LogDateTime = log.LogDateTime,
                        UserApiKey = user.ApiKey
                    };

                    //var archiveLogs = new LogArchive(log.LogString, user.ApiKey);

                    _context.LogArchives.Add(archiveLogs);

                }



                _context.Users.Remove(user);
                _context.SaveChanges();

               
            }
        }

        internal int GetTotalUserCount()
        {
            // using (var context = new UserContext())
            // {
            return _context.Users.Count();
            //}

        }



        internal User GetUserByUsernameAndApiKey(string username, string apiKey)
        {
            
            return _context.Users.FirstOrDefault(u => u.UserName == username && u.ApiKey == apiKey);
            

        }

    

        internal void SaveChanges()
        {
           
            _context.SaveChanges();
            

        }

        internal object GetUserByUsername(string username)
        {
          
            return _context.Users.FirstOrDefault(u => u.UserName == username);
           

        }

        internal bool UpdateRole(string userName, string newRole)
        {
           
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                user.Role = newRole;
                _context.SaveChanges();
                return true;
            }
            return false;
            

        }

        // Method to add log to user's collection of logs
        public void AddLogToUser(string apiKey, string logMessage)
        {
            var user = GetUserByApiKey(apiKey);
            if (user != null)
            {
                var log = new Log(logMessage);
                user.Logs.Add(log);
             
                _context.SaveChanges();
            }
        }

        //public void AddLogArchive(string apiKey, string logMessage)
        //{
        //    //var user = GetUserByApiKey(apiKey);
        //    //if (user != null)
        //    //{
        //    //    var log = new LogArchive(logMessage);
        //    //    user.LogArchives.Add(log);
        //    //    _context.SaveChanges();
        //    //}

        //    var log = new LogArchive(logMessage, apiKey);  // Pass the user's API key to the LogArchive constructor
        //    _context.LogArchives.Add(log);
        //    _context.SaveChanges();
        //}


    }
}
