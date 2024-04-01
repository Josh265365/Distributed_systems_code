using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DistSysAcwServer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DistSysAcwServer.Controllers
{
    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly UserDatabaseAccess _userDatabaseAccess;

        public ProtectedController(UserDatabaseAccess userDatabaseAccess)
        {
            _userDatabaseAccess = userDatabaseAccess;
        }

        [HttpGet("Hello")]
       // [Authorize(Roles = "Admin, User")]

        //cerate a method that returns a string "Hello <username from database>"with status code 200
        public IActionResult GetHello()
        {
            //var apiKey = Request.Headers["ApiKey"].ToString();
            //var username = _userDatabaseAccess.GetUserByApiKey(apiKey);
            //if (username != null)
            //{
            //    return Ok("Hello " + username);
            //}
            //else
            //{
            //    return NotFound("User not found");
            //}
            var apiKey = Request.Headers["ApiKey"].ToString();
            var user = _userDatabaseAccess.GetUserByApiKey(apiKey);

            if (user != null)
            {
                var username = user.UserName;
                return Ok("Hello " + username);
            }
            else
            {
                return NotFound("User not found");
            }
        }



        //public IActionResult GetSha1([FromQuery(Name = "message")] string message)
        //{
        //    if (message == null)
        //    {
        //        return BadRequest("Bad Request");
        //    }
        //    else
        //    {
        //        using (SHA1 sha1 = SHA1.Create())
        //        {
        //            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));
        //            StringBuilder sb = new StringBuilder();
        //            for (int i = 0; i < hashBytes.Length; i++)
        //            {
        //                sb.Append(hashBytes[i].ToString("X2"));
        //            }
        //            return Ok(sb.ToString());
        //        }
        //    }

        //}
        [HttpGet("sha1")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> GetSha1([FromQuery(Name = "message")] string message)
        {
            if (message == null)
            {
                return BadRequest("Bad Request");
            }
            else
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return Ok(sb.ToString());
                }
            }
        }




        [HttpGet("sha256")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> GetSha256([FromQuery(Name = "message")] string message)
        {
            if (message == null)
            {
                return BadRequest("Bad Request");
            }
            else
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return Ok(sb.ToString());
                }
            }
        }
    }


    

}

