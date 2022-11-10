namespace CarPoolingAPI.Models
{
    public class UserRegister
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsDriver { get; set; }
    }
}
