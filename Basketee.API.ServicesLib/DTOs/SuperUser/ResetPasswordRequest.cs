namespace Basketee.API.DTOs.SuperUser
{
    public class ResetPasswordRequest
    {
     
        public string mobile_number { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
    }
}
