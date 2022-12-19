﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolingAPI.Dto
{
    public class TripDto
    {
        public int? Id { get; set; }
        public string? TripNumber { get; set; }
        public decimal? FromLatitude { get; set; }
        public decimal? FromLongitude { get; set; }
        public string? FromAddress { get; set; }
        public decimal? ToLatitude { get; set; }
        public decimal? ToLongitude { get; set; }
        public string? ToAddress { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public int? VehicleId { get; set; }
    }
}
