namespace Basketee.API.DTOs.SuperUser
{
    public class LoginRequest
    {
        public string mobile_number { get; set; }
        public string password { get; set; }
        public string app_id { get; set; }
        public string push_token { get; set; }
    }
}
