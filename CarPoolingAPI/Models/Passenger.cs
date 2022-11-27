namespace CarPoolingAPI.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string RememberToken { get; set; } = string.Empty;
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}
