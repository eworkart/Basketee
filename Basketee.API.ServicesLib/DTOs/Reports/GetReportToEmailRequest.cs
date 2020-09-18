using System.ComponentModel.DataAnnotations;

namespace Basketee.API.DTOs.Reports
{
    public class GetReportToEmailRequest : AuthBase
    {
        /// <summary>
        /// Report type
        /// 1 - Master data extraction report
        /// 2 - For future reference
        /// </summary>
        [Range(1, 2, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int report_type { get; set; }

    }
}
