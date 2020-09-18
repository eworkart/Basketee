using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class SUserReviewReasonResponse : ResponseDto
    {
        public List<SUserReviewReasonDto> service_reason_rating { get; set; }
    }
}