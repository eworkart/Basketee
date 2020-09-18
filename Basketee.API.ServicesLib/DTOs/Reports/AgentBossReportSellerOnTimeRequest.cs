using Basketee.API.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{

    public class AgentBossReportSellerOnTimeRequest : AuthBase {
        /// <summary>
        /// 1 - Month
        /// 2 - Week
        /// </summary>
        [Range(1, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int periodical_data { get; set; }

        /// <summary>
        /// Id of the driver to group report for
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int driver_id { get; set; }

    }

    public class SuperUserReportSellerOnTimeRequest : AuthBase
    {
        /// <summary>
        /// 1 - Month
        /// 2 - Week
        /// </summary>
        [Range(1, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int periodical_data { get; set; }

        /// <summary>
        /// Id of the agency to group report for
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int agency_id { get; set; }

    }

    public class SellerReportOnTimeRequest
    {

        /// <summary>
        /// 1 - Month
        /// 2 - Week
        /// </summary>
        [Range(1, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int periodical_data { get; set; }

        /// <summary>
        /// Id of an entity to group report for
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int entity_id { get; set; }

        [Required]
        public UserType for_role { get; set; }


    }
}
