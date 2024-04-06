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
             //rsa.ToXmlString(true);
            //rsa.FromXmlString("<RSAKeyValue><Modulus>0QZz");
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
                _userDatabaseAccess.AddLogToUser(apiKey, "User requested /Protected/Hello");

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
                _userDatabaseAccess.AddLogToUser(apiKey, "User requested /Protected/GetPublicKey");
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

                    _userDatabaseAccess.AddLogToUser(apiKey, "User requested /Protected/Sign");

                    return Ok(signatureHexString);
                }
            }
        }

        [HttpGet("Mashify")]
        [Authorize(Roles = "Admin")]
        public IActionResult Mashify([FromHeader(Name = "ApiKey")] string apiKey,
                                     [FromQuery(Name = "encryptedString")] string encryptedStringHex,
                                     [FromQuery(Name = "encryptedSymKey")] string encryptedSymmetricKeyHex,
                                     [FromQuery(Name = "encryptedIV")] string encryptedIVHex)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(encryptedStringHex) ||
                string.IsNullOrEmpty(encryptedSymmetricKeyHex) || string.IsNullOrEmpty(encryptedIVHex))
            {
                return BadRequest("API Key, encrypted string, symmetric key, and IV must be provided.");
            }

            var user = _userDatabaseAccess.GetUserByApiKey(apiKey);

            if (user == null)
            {
                return NotFound("User not found or not authorized.");
            }

            try
            {
                // Remove dashes from hexadecimal strings

                encryptedStringHex = RemoveDashes(encryptedStringHex);
                encryptedSymmetricKeyHex = RemoveDashes(encryptedSymmetricKeyHex);
                encryptedIVHex = RemoveDashes(encryptedIVHex);

                // Decrypting the parameters using the server's private RSA key
                byte[] encryptedStringBytes = ConvertHexStringToBytes(encryptedStringHex);
                byte[] encryptedSymmetricKeyBytes = ConvertHexStringToBytes(encryptedSymmetricKeyHex);
                byte[] encryptedIVBytes = ConvertHexStringToBytes(encryptedIVHex);

                byte[] decryptedStringBytes = rsa.Decrypt(encryptedStringBytes, RSAEncryptionPadding.OaepSHA1);
                byte[] decryptedSymmetricKeyBytes = rsa.Decrypt(encryptedSymmetricKeyBytes, RSAEncryptionPadding.OaepSHA1);
                byte[] decryptedIVBytes = rsa.Decrypt(encryptedIVBytes, RSAEncryptionPadding.OaepSHA1);

                string decryptedString = Encoding.UTF8.GetString(decryptedStringBytes);
                string decryptedSymmetricKey = Encoding.UTF8.GetString(decryptedSymmetricKeyBytes);
                string decryptedIV = Encoding.UTF8.GetString(decryptedIVBytes);

                // Mashify the decrypted string
                string mashedString = MashifyString(decryptedString);

                // Encrypt the mashed string using the client's symmetric (AES) key and IV
                byte[] encryptedMashedBytes = EncryptStringWithAES(mashedString, decryptedSymmetricKey, decryptedIV);

                // Convert the encrypted mashed string to hexadecimal format
                string encryptedMashedHex = BitConverter.ToString(encryptedMashedBytes).Replace(" ", "-");

                return Ok(encryptedMashedHex);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Error: {ex.Message}");
            }
        }



        private string MashifyString(string input)
        {
            StringBuilder mashedBuilder = new StringBuilder();
            foreach (char c in input.Reverse())
            {
                if ("aeiouAEIOU".Contains(c))
                {
                    mashedBuilder.Append('X');
                }
                else
                {
                    mashedBuilder.Append(c);
                }
            }
            return mashedBuilder.ToString();
        }

        private byte[] EncryptStringWithAES(string input, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = Convert.FromBase64String(iv);

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream
                            swEncrypt.Write(input);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        private byte[] ConvertHexStringToBytes(string hexString)
        {
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        private string RemoveDashes(string hexString)
        {
            return hexString.Replace("-", "");
        }


    }




}

