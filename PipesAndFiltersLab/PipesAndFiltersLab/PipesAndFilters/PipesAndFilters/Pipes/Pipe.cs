using PipesAndFilters.Filters;
using PipesAndFilters.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipesAndFilters.Pipes
{
    class Pipe : IPipe
    {
        private List<IFilter> filters;

        //add a constructor to Pipe and instantiate Filters to a new List<IFilter>();
        public Pipe()
        {
            filters = new List<IFilter>();
        }

        
        /// <summary>
        /// implement the RegisterFilter method required by the IPipe interface returning void and taking an IFilter as a parameter
        /// </summary>
        /// <param name="filter"></param>
        public void RegisterFilter(IFilter filter)
        {
            filters.Add(filter);
           
        }

        /// <summary>
        /// implement the ProcessMessage method required by the IPipe interface returning an IMessage and taking an IMessage as a parameter
        /// </summary>
        /// <param name="imessages"></param>
        /// <returns></returns>
        public IMessage ProcessMessage(IMessage imessages)
        {

            
            foreach (var filter in filters)
            {
                imessages = filter.Run(imessages);
            }
            
            return imessages;
            

        }

         
        
    }
}
