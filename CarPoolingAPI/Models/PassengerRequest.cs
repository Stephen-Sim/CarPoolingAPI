using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Models
{
    public class PassengerRequest
    {
        public int PassengerId { get; set; }
        public int RequestId { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Charges { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; }
        public Passenger Passenger { get; set; }
        public Request Request { get; set; }
    }
}
