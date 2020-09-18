using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetAssignedOrderCountResponse : ResponseDto
    {
        public AssignedOrderCountDto order { get; set; } 
    }
}
