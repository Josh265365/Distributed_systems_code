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
    public class UserController : ControllerBase 
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
            bool userExists = _userDatabaseAccess.CheckUserExists(UserName);
            if (userExists)
            {
                return Ok("True - User already exists! Did you mean to do a POST to create a new user?");
            }
            else
            {
                return Ok("False - User does not exist! Did you mean to do a POST to create a new user?");
            }

          
        }


        [HttpPost("new")]
       
        public IActionResult CreateUser([FromBody] string username)
        {
          
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


   

        [HttpDelete("RemoveUser")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult RemoveUser([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "username")] string username)
        {
            

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


            _userDatabaseAccess.AddLogToUser(apiKey, $"User requested /User/RemoveUser and removed user {user} ");
          //  _userDatabaseAccess.AddLogArchive(apiKey, $"User requested /User/RemoveUser and removed user {user} ");

           
            

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
        [HttpPost("ChangeRole")]//change to put
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] ChangeUserRole request)
        {
           if(request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.NewRole))
            {
                return BadRequest("Username and new role must be provided.");
            }

           if(!_userDatabaseAccess.CheckUserExists(request.UserName))
            {
                return BadRequest("NOT DONE: Username does not exist.");
            }

            if(request.NewRole != "User" && request.NewRole != "Admin")
            {
                return BadRequest("NOT DONE: Role does not exist.");
            }

            if(_userDatabaseAccess.UpdateRole(request.UserName, request.NewRole))
            {
                _userDatabaseAccess.AddLogToUser(apiKey, $"user requested /User/ChangeRole and changed {request.UserName}'s role to {request.NewRole}");
                return Ok("DONE");
            }
            else
            {
                return BadRequest("NOT DONE: An error occurred.");
            }

        }
    }
}
