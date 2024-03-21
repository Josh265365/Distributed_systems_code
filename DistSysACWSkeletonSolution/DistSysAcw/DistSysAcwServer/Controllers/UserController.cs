using DistSysAcwServer.DataAccess;
using DistSysAcwServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using DistSysAcwServer.Controllers;
using System.Data;

namespace DistSysAcwServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase //: BaseController
    {
        // public UserController(Models.UserContext dbcontext) : base(dbcontext) { }
        private readonly UserDatabaseAccess _userDatabaseAccess;

        public UserController(UserDatabaseAccess userDatabaseAccess)
        {
            _userDatabaseAccess = userDatabaseAccess;
        }

        [HttpGet("new")]
        public IActionResult CheckUserName([FromQuery(Name = "username")] string UserName)
        {
            // Check if the user already exists in the database and return "Username already exists!" if it does
            //if (DbContext.Users.Any(u => u.UserName == UserName))
            //{
            //    return Ok("Ture - User Does exists! Didi you mean to do a POST to create a new user");
            //}
            //else
            //{
            //    return Ok("False - User Does not exists! Did you mean to do a POST to create a new user?");
            //}
            if (_userDatabaseAccess.CheckUserExists(UserName))
            {
                return Ok("True - User already exists! Did you mean to do a POST to create a new user?");
            }
            else
            {
                return Ok("False - User does not exist! Did you mean to do a POST to create a new user?");
            }
        }


        [HttpPost("new")]
        //create a new user with given username and creating a new GUID which is saved as a strignto the database as the ApiKey and return the api key to the user with a status code of ok 200.If this is the first user created, the user should be given the role of Admin, all other users should be given the role of User.
        public IActionResult CreateUser([FromBody] string username)
        {
            //if (DbContext.Users.Any(u => u.UserName == username))
            //{
            //    return BadRequest("Oops.This Username is already in use. Please try again with a new username");
            //}
            //if (username == null)
            //{
            //    return StatusCode(403, "Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
            //}
            //else
            //{
            //    Models.User user = new Models.User();
            //    user.UserName = username;
            //    user.ApiKey = Guid.NewGuid().ToString();
            //    if (DbContext.Users.Count() == 0)
            //    {
            //        user.Role = "Admin";
            //    }
            //    else
            //    {
            //        user.Role = "User";
            //    }
            //    DbContext.Users.Add(user);
            //    DbContext.SaveChanges();
            //    return Ok(user.ApiKey);
            //}
            if (_userDatabaseAccess.CheckUserExists(username))
            {
                return BadRequest("Oops. This username is already in use. Please try again with a new username");
            }
            if (username == null)
            {
                return StatusCode(403, "Oops. Make sure your body contains a string with your username and your Content-Type is application/json");
            }

            var user = new User
            {
                UserName = username,
                ApiKey = Guid.NewGuid().ToString(),
                Role = _userDatabaseAccess.GetTotalUserCount() == 0 ? "Admin" : "User"
            };

            _userDatabaseAccess.CreateUser(username, user.Role);
            //_userDatabaseAccess.AddUser(user);
            // _userDatabaseAccess.SaveChanges();
            return Ok(user.ApiKey);

        }


        //[HttpDelete("RemoveUser")]
        //public IActionResult RemoveUser([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name ="username")] string username)
        //{
        //    if (DbContext.Users.Any(u => u.ApiKey == apiKey))
        //    {
        //        var user = DbContext.Users.FirstOrDefault(u => u.ApiKey == apiKey);
        //        if (user != null && user.UserName != username)
        //        {
        //            DbContext.Users.Remove(user);
        //            DbContext.SaveChanges();
        //        }

        //        return Ok(true);
        //    }
        //    return Ok(false);
        //}

        [HttpDelete("RemoveUser")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult RemoveUser([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "username")] string username)
        {
            //if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(username))
            //{
            //    return BadRequest("API Key and username must be provided.");
            //}

            //var user = DbContext.Users.FirstOrDefault(u => u.UserName == username && u.ApiKey == apiKey);

            //if (user == null && apiKey==null)
            //{
            //    return Ok(false); // User not found or not authorized to delete
            //}

            //DbContext.Users.Remove(user);
            //DbContext.SaveChanges();

            //return Ok(true); // User deleted successfully

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(username))
            {
                return BadRequest("API Key and username must be provided.");
            }

            var user = _userDatabaseAccess.GetUserByUsernameAndApiKey(username, apiKey);

            if (user == null)
            {
                return Ok(false); // User not found or not authorized to delete
            }

            //_userDatabaseAccess.RemoveUser(user);
            _userDatabaseAccess.DeleteUser(apiKey);

            return Ok(true); // User deleted successfull
        }


        //[HttpPut("ChangeRole")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult ChangeRole([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] string username, [FromBody] string newRole)
        //{
        //    var currentUser = _userDatabaseAccess.GetUserByApiKey(apiKey);

        //    if (currentUser != null && currentUser.Role == "Admin")
        //    {
        //        var userToChange = _userDatabaseAccess.GetUserByUsername(username);

        //        if (userToChange == null)
        //        {
        //            return BadRequest("NOT DONE: Username does not exist");
        //        }

        //        if (newRole != "user" && newRole != "Admin")
        //        {
        //            return BadRequest("NOT DONE: Role does not exist");
        //        }

        //        userToChange.Role = newRole;


        //        bool roleChanged = _userDatabaseAccess.UpdateUser(userToChange);

        //        if (roleChanged)
        //        {
        //            return Ok("DONE");
        //        }
        //        else
        //        {
        //            return BadRequest("NOT DONE: An error occurred");
        //        }
        //    }
        //    else
        //    {
        //        return Forbid();


        //    }

        //}
        /// <summary>
        /// changes the role of a user to either "User" or "Admin" depending on the newRole parameter given in the request body
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="request"></param>
        /// <returns>new role</returns>
        [HttpPut("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] ChangeUserRole request)
        {
           if(request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.NewRole))
            {
                return BadRequest("Username and new role must be provided.");
            }

           if(_userDatabaseAccess.CheckUserExists(request.UserName))
            {
                return BadRequest("NOT DONE: Username does not exist.");
            }

            if(request.NewRole != "User" && request.NewRole != "Admin")
            {
                return BadRequest("NOT DONE: Role does not exist.");
            }

            if(_userDatabaseAccess.UpdateRole(request.UserName, request.NewRole))
            {
                return Ok("DONE");
            }
            else
            {
                return BadRequest("NOT DONE: An error occurred.");
            }

        }
    }
}
