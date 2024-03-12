using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.DataAccess;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        readonly MyDataCRUD _myDataCRUDAcce;
        //private MyDataCRUD _myDataCRUDAccess;

        public DataController(MyDataCRUD myDataCRUDAccess)
        {
            _myDataCRUDAcce = myDataCRUDAccess;
        }

        [ActionName("Create")]
        public void CreateData()
        {
            _myDataCRUDAcce.Create();
        }

        // GET: api/<DataController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //string[] myData = new string[] { "zero", "one", "two", "three", "four", "five" };
            // return myData;
            //return new string[] { "value1", "value2" };
            return _myDataCRUDAcce.Read();
        }

        // GET api/<DataController>/5
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            //string[] myData = new string[] { "zero", "one", "two", "three", "four", "five" };

            // return " ";  
            return _myDataCRUDAcce.Read(id);
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "Data not found.";
        }

        // [HttpGet("{action:name}")]
        // [HttpGet("{action}/{name}")]
        [HttpGet("[action]")]
        public string Name([FromQuery] string name)
        {
            return "Your Name is " + name;
        }
        
        



        // POST api/<DataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
