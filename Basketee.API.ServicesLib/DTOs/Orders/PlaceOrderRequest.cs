using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class PlaceOrderRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int user_id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required and cannot be empty")]
        public string auth_token { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int address_id { get; set; }

        [Required]
        //[Range(typeof(DateTime), "1/1/2017", "1/1/2050")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime delivery_date { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short time_slot_id { get; set; }

        public ProductsDto[] products { get; set; }
        public ExchangeDto[] exchange { get; set; }

        [Display(Name = "has_exchange")]
        [Range(typeof(bool), "false", "true", ErrorMessage = "Value for {0} must be between either {1} and {2}.")]
        public bool has_exchange { get; set; }
    }
}