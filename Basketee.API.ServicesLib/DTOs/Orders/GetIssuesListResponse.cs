using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetIssuesListResponse : ResponseDto
    {
        public OrderCountDto orders { get; set; }
        public IssueDetailsDto[] order_details { get; set; }
         
    }
    
}
