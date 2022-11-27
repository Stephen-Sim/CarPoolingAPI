namespace CarPoolingAPI.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public int TripRequestId { get; set; }
        public TripRequest TripRequest { get; set; }
    }
}
