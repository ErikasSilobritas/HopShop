using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectQuantityException : Exception
    {
        public IncorrectQuantityException() : base ("The requested quantity cannot be negative")
        {
            
        }
    }
}
