using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Requests
{
    public class AssignItemToShop
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
    }
}
