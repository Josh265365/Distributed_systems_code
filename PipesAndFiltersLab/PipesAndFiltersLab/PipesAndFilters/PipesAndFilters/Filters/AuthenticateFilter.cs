using PipesAndFilters.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace PipesAndFilters.Filters
{

    internal class AuthenticateFilter : IFilter
    {
        

        public IMessage Run(IMessage message)
        {
            if (message.Headers.ContainsKey("User"))
            {
                int userId = int.Parse(message.Headers["User"]);
                ServerEnvironment.SetCurrentUser(userId);
            }
            return message;
        }
    }


    


 }

