using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests
{
    public class GetShop
    {  
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
