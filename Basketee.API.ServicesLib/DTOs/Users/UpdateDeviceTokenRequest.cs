using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UpdateDeviceTokenRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int user_id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required and cannot be empty")]
        public string auth_token { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required and cannot be empty")]
        public string push_token { get; set; }
    }
}