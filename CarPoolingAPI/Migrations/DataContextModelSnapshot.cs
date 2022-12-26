﻿// <auto-generated />
using System;
using CarPoolingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarPoolingAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CarPoolingAPI.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripRequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TripRequestId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RememberToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNo")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Passenger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("RememberToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNo")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Charges")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FromLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("FromLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<int>("NumberOfPassengers")
                        .HasColumnType("int");

                    b.Property<int>("PassengerId")
                        .HasColumnType("int");

                    b.Property<string>("RequestNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ToLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("ToLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.HasKey("Id");

                    b.HasIndex("PassengerId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FromLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("FromLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ToLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("ToLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("TripNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.TripRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<int?>("TripId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestId")
                        .IsUnique();

                    b.HasIndex("TripId");

                    b.ToTable("TripRequests");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlatNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("PlatNo")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Chat", b =>
                {
                    b.HasOne("CarPoolingAPI.Models.TripRequest", "TripRequest")
                        .WithMany("Chats")
                        .HasForeignKey("TripRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TripRequest");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Request", b =>
                {
                    b.HasOne("CarPoolingAPI.Models.Passenger", "Passenger")
                        .WithMany("Requests")
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Trip", b =>
                {
                    b.HasOne("CarPoolingAPI.Models.Vehicle", "Vehicle")
                        .WithMany("Trips")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.TripRequest", b =>
                {
                    b.HasOne("CarPoolingAPI.Models.Request", "Request")
                        .WithOne("TripRequest")
                        .HasForeignKey("CarPoolingAPI.Models.TripRequest", "RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarPoolingAPI.Models.Trip", "Trip")
                        .WithMany("TripRequests")
                        .HasForeignKey("TripId");

                    b.Navigation("Request");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Vehicle", b =>
                {
                    b.HasOne("CarPoolingAPI.Models.Driver", "Driver")
                        .WithMany("Vehicles")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Driver", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Passenger", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Request", b =>
                {
                    b.Navigation("TripRequest")
                        .IsRequired();
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Trip", b =>
                {
                    b.Navigation("TripRequests");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.TripRequest", b =>
                {
                    b.Navigation("Chats");
                });

            modelBuilder.Entity("CarPoolingAPI.Models.Vehicle", b =>
                {
                    b.Navigation("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
