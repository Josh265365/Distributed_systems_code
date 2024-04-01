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
        //public User CreateUser(string username)
        //{
        //    using (var context = new UserContext())
        //    {
        //        User user = new User();
        //        user.UserName = username;
        //        user.ApiKey = Guid.NewGuid().ToString();
        //        user.Role = "User";

        //        context.Users.Add(user); // Add the new user to the database
        //        context.SaveChanges(); // Save changes to the database

        //        return user; // Return the user object
        //    }
        //}
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
            //using (var context = new UserContext())
            //{
               // return _context.Users.Any(u => u.ApiKey == apiKey);
            return _context.Users.Any(u => u.UserName == username);
            //}
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
            //using (var context = new UserContext())
            //{
            //    var user = context.Users.FirstOrDefault(u => u.ApiKey == apiKey);
            //    if (user != null)
            //    {
            //        context.Users.Remove(user); // Marking the user for deletion
            //        context.SaveChanges(); // Committing the deletion to the database
            //    }
            //}
            var user = _context.Users.FirstOrDefault(u => u.ApiKey == apiKey);
            if (user != null)
            {
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

        //internal void AddUser(User user)
        //{
        //   // using (var context = new UserContext())
        //   // {
        //        _context.Users.Add(user);
        //        _context.SaveChanges();
        //   // }
            
        //}

        internal User GetUserByUsernameAndApiKey(string username, string apiKey)
        {
           // using (var context = new UserContext())
           // {
                return _context.Users.FirstOrDefault(u => u.UserName == username && u.ApiKey == apiKey);
           // }
           
        }

        //internal void RemoveUser(object user)
        //{
        //   // using (var context = new UserContext())
        //   // {
        //        _context.Users.Remove((User)user);
        //        _context.SaveChanges();
        //    //}
            
        //}

        internal void SaveChanges()
        {
            //using (var context = new UserContext())
           // {
                _context.SaveChanges();
            //}
            
        }

        internal object GetUserByUsername(string username)
        {
            //using (var context = new UserContext())
            //{
                return _context.Users.FirstOrDefault(u => u.UserName == username);
            //}
            
        }

        internal bool UpdateRole(string userName, string newRole)
        {
            //using (var context = new UserContext())
            //{
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
            {
                    user.Role = newRole;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            //}
            
        }

        //public bool deleteUser(string apiKey)
        //{
        //    using (var context = new UserContext())
        //    {
        //        var user = context.Users.FirstOrDefault(u => u.ApiKey == apiKey);
        //        if (user != null)
        //        {
        //            context.Users.Remove(user); // Marking the user for deletion
        //            context.SaveChanges(); // Committing the deletion to the database
        //            return true;
        //        }
        //        return false;
        //    }
        //}



    }
}
