using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{



    public class ABossReviewReportRequest : AuthBase
    {

        /// <summary>
        /// 1 - Month
        /// 2 - Week
        /// </summary>
        [Range(1, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int periodical_data { get; set; }

        /// <summary>
        /// Id of the driver
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int driver_id { get; set; }
    }
}
