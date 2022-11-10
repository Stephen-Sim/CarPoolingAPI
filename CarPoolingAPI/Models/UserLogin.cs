namespace CarPoolingAPI.Models
{
    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsDriver { get; set; }
    }
}