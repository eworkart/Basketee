using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
   public class GetIssueDetailsResponse:ResponseDto
    {
        public IssueDetailsDto issue_details { get; set; }
        public ProductDetailsDto[] product_details { get; set; }
        public AgencyDetailsDto agency_details { get; set; }
    }
}
