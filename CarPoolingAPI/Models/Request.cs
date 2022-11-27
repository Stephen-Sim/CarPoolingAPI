﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Models
{
    public class Request
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
        public string Status { get; set; }
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }
        public ICollection<TripRequest> TripRequests { get; set; }

    }
}
