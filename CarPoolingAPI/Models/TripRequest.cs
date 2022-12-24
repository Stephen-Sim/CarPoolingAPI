using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Models
{
    public class TripRequest
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Rating { get; set; }
        public int? TripId { get; set; }
        public int RequestId { get; set; }
        public Trip? Trip { get; set; }
        public Request Request { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}
