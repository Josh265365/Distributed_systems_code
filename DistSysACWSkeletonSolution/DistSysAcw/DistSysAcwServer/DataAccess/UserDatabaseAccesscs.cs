using DistSysAcwServer.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace DistSysAcwServer.DataAccess
{
    public class UserDatabaseAccesscs
    {
        public UserDatabaseAccesscs()
        {

        }

        
        /// <summary>
        /// create a new user with given username and creating a new GUID which is saved as a strignto the database as the ApiKey
        /// </summary>
        /// <param name="username"></param>
        /// <returns>the user object</returns>
        public User CreateUser(string username)
        {
            using (var context = new UserContext())
            {
                User user = new User();
                user.UserName = username;
                user.ApiKey = Guid.NewGuid().ToString();
                user.Role = "User";

                context.Users.Add(user); // Add the new user to the database
                context.SaveChanges(); // Save changes to the database

                return user; // Return the user object
            }
        }

        
        /// <summary>
        /// check if user with given API key exists 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns>true or flase</returns>
        public bool CheckUserExists(string apiKey)
        {
            using (var context = new UserContext())
            {
                return context.Users.Any(u => u.ApiKey == apiKey);
            }
        }


          
        /// <summary>
        /// check if user with given API key and username exists
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="username"></param>
        /// <returns>true or false</returns>
        public bool CheckUserNameandAPIExists(string apiKey, string username)
        {
            using (var context = new UserContext())
            {
                return context.Users.Any(u => u.ApiKey == apiKey && u.UserName == username);
            }
        }

      
        /// <summary>
        /// Check if user with given API key exists
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns>The exisitng user user</returns>
        public User GetUser(string apiKey)
        {
            using (var context = new UserContext())
            {
                // Querying the database for the user with the given apiKey
                var user = context.Users.FirstOrDefault(u => u.ApiKey == apiKey);

                return user; // Return the user object if found, or null if not found
            }
        }


        /// <summary>
        /// Delete the user with the given API key
        /// </summary>
        /// <param name="apiKey"></param>
        public void DeleteUser(string apiKey)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(u => u.ApiKey == apiKey);
                if (user != null)
                {
                    context.Users.Remove(user); // Marking the user for deletion
                    context.SaveChanges(); // Committing the deletion to the database
                }
            }
        }




    }
}
