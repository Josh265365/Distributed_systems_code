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
        private readonly RSA rsa;

        public ProtectedController(UserDatabaseAccess userDatabaseAccess)
        {
            _userDatabaseAccess = userDatabaseAccess;
            rsa = RSA.Create();
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

       
        [HttpGet("GetPublicKey")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetPublicKey()
        {
            var apiKey = Request.Headers["ApiKey"].ToString();
            var user = _userDatabaseAccess.GetUserByApiKey(apiKey);

            if (user != null)
            {
                // Return the public key
                return Ok(rsa.ToXmlString(false)); // Specify false to exclude private key
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpGet("Sign")]
        [Authorize(Roles = "Admin, User")]

        public IActionResult SignMessage([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "message")] string message)
        {
            // Check if the API Key is in the database
            var user = _userDatabaseAccess.GetUserByApiKey(apiKey);
            if (user == null)
            {
                return NotFound("API Key not found");
            }

            // Convert the message string to bytes
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);

            using (SHA1 sha1 = SHA1.Create())
            {
                // Compute the hash of the message
                byte[] hashBytes = sha1.ComputeHash(messageBytes);

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    
                    byte[] signatureBytes = rsa.SignHash(hashBytes, CryptoConfig.MapNameToOID("SHA1"));

                    // Convert the signature bytes to hexadecimal format with dashes as delimiters
                    StringBuilder signatureHex = new StringBuilder();
                    foreach (byte b in signatureBytes)
                    {
                        signatureHex.AppendFormat("{0:X2}-", b);
                    }

                    // Remove the last dash
                    string signatureHexString = signatureHex.ToString().TrimEnd('-');

                    return Ok(signatureHexString);
                }
            }
        }


    }




}

