using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
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
        public int NumberOfPassengers { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Charges { get; set; }
        public string Status { get; set; }
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }
        public TripRequest TripRequest { get; set; }

    }
}
