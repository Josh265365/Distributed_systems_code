using PipesAndFilters.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipesAndFilters.Filters
{
    internal class TimestampFilter: IFilter
    {
        //The Run method should add a header to the IMessage containing the key: “Timestamp” and the current DateTime.Now.ToString() value
        public IMessage Run(IMessage message)
        {
            message.Headers.Add("Timestamp", DateTime.Now.ToString());
            return message;
        }

       
    }
}
