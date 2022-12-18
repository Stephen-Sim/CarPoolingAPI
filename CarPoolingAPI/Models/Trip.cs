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
        public decimal FromLongitude { get; set; }
        public string FromAddress { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal ToLatitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal ToLongitude { get; set; }
        public string ToAddress { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public ICollection<TripRequest> TripRequests { get; set; }
    }
}
