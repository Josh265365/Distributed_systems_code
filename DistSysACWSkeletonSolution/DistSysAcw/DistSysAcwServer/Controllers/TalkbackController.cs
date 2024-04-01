using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DistSysAcwServer.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TalkbackController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkbackController(Models.UserContext dbcontext) : base(dbcontext) { }

        #region TASK1
        //    TODO: add api/talkback/hello response
        #endregion
        /// <summary>
        /// gets a response of "Hello World" when the api/talkback/hello endpoint is called
        /// </summary>
        /// <returns>hello world</returns>
        [HttpGet("hello")]
        public async Task<IActionResult> GetHello()
        {
            return await Task.FromResult( Ok("Hello World"));
        }
        #region TASK1
        #endregion

        #region TASK1
        //    TODO:
        //       add a parameter to get integers from the URI query
        //       sort the integers into ascending order
        //       send the integers back as the api/talkback/sort response
        //       conform to the error handling requirements in the spec
        //       if there are no integers submitted, the result should be [] with a status code of OK (200
        #endregion

        /// <summary>
        /// Takes in integers as strings from the URI query, parse them into integers, sort them into ascending order and return them as the response
        /// </summary>
        /// <param name="integers"></param>
        /// <returns>sorted list of integers</returns>
        [HttpGet("sort")]
        public async Task<IActionResult>  GetSort([FromQuery(Name = "integers")] IEnumerable<string> integers)
        {
            List<int> parsedIntegers = new List<int>();

            foreach (var str in integers)
            {
                if (int.TryParse(str, out int result))
                {
                    parsedIntegers.Add(result);
                }
                else
                {
                    return BadRequest("Bad Request"); 
                }
            }

            if (parsedIntegers.Count == 0)
            {
                return Ok(new int[] { }); // Return an empty array with status code 200 if no valid integers are provided
            }

            parsedIntegers.Sort();
            return await Task.FromResult( Ok(parsedIntegers));

        }


    }
}
