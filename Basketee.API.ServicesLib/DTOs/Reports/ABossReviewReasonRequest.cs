using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class ABossReviewReasonRequest : AuthBase
    {
        public int periodical_data { get; set; }
        public int driver_id { get; set; }
    }
}
