using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class SUserReviewReportResponse : ResponseDto
    {
        public List<SUserReviewReportDto> service_rating { get; set; }
    }
}
