using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InsufficientItemQuantityException : Exception
    {
        public InsufficientItemQuantityException() : base("There are not enough items in the store")
        {
            
        }
    }
}
