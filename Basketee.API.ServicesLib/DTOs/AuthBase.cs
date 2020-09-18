using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs
{
    /// <summary>
    /// Base class for authentication
    /// </summary>
    public abstract class AuthBase
    {
        [Range(0, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int user_id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required and cannot be empty")]
        public string auth_token { get; set; }
    }
}
