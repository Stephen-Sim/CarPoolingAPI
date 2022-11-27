using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal FromLatitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal FromLongtitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal ToLatitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal ToLongtitude { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public ICollection<TripRequest> TripRequests { get; set; }
    }
}
