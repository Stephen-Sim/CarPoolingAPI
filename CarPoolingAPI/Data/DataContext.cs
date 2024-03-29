﻿using CarPoolingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarPoolingAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripRequest> TripRequests { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>()
                .HasIndex(x => x.PhoneNo)
                .IsUnique();
            modelBuilder.Entity<Driver>()
                .HasIndex(x => x.Username)
                .IsUnique();
            modelBuilder.Entity<Vehicle>()
                .HasIndex(x => x.PlatNo)
                .IsUnique();

            modelBuilder.Entity<Passenger>()
               .HasIndex(x => x.PhoneNo)
               .IsUnique();
            modelBuilder.Entity<Passenger>()
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasOne(x => x.Driver)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.DriverId);

            modelBuilder.Entity<Trip>()
                .HasOne(x => x.Vehicle)
                .WithMany(x => x.Trips)
                .HasForeignKey(x => x.VehicleId);

            modelBuilder.Entity<TripRequest>()
                .HasOne(x => x.Trip)
                .WithMany(x => x.TripRequests)
                .HasForeignKey(x => x.TripId);

            modelBuilder.Entity<Chat>()
                .HasOne(x => x.TripRequest)
                .WithMany(x => x.Chats)
                .HasForeignKey(x => x.TripRequestId);

            modelBuilder.Entity<Request>()
                .HasOne(x => x.TripRequest)
                .WithOne(x => x.Request)
                .HasForeignKey<TripRequest>(x => x.RequestId);
            
            modelBuilder.Entity<Request>()
                .HasOne(x => x.Passenger)
                .WithMany(x => x.Requests)
                .HasForeignKey(x => x.PassengerId);
        }
    }
}
