namespace CarPoolingAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlatNo { get; set; }
        public string Color { get; set; }
        public int Capacity { get; set; }
        public int DriverId{ get; set; }
        public bool IsActive { get; set; } = true;
        public Driver Driver { get; set; }
        public ICollection<Trip> Trips { get; set; }
    }
}
