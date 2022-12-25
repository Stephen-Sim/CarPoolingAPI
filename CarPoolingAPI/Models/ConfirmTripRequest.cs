namespace CarPoolingAPI.Models
{
    public class ConfirmTripRequest
    {
        public int TripId { get; set; }
        public List<int> RequestsId { get; set; }
    }
}
