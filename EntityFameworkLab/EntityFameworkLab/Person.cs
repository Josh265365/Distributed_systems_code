using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EntityFameworkLab
{
    internal class Person
    {
            public int PersonId { get; set; }
            public string First_Name { get; set; }
            public string Middle_Name { get; set; }
            public string Last_Name { get; set; }
            public DateTime Date_of_Birth { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
            public Person() { }
        
    }
}
