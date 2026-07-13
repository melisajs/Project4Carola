using CarolaEnditiyLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Carola.DataAccessLayer.Concrete
{
    public class CaraloContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use local SQLEXPRESS connection string with TrustServerCertificate to avoid self-signed SSL errors.
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=CarolaRentDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<WeAreTheBest> WeAreTheBests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys that don't match default EF naming conventions
            modelBuilder.Entity<Category>().HasKey(c => c.CarCategoryId);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CarCategoryId = 1, CategoryName = "SUV" },
                new Category { CarCategoryId = 2, CategoryName = "Sedan" },
                new Category { CarCategoryId = 3, CategoryName = "Truck" },
                new Category { CarCategoryId = 4, CategoryName = "Spor" },
                new Category { CarCategoryId = 5, CategoryName = "Ekonomik" }
            );

            // Seed Brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, BrandName = "Volkswagen", LogoUrl = "/carola/assets/images/brands/volkswagen.png", Status = true },
                new Brand { BrandId = 2, BrandName = "Toyota", LogoUrl = "/carola/assets/images/brands/toyota.png", Status = true },
                new Brand { BrandId = 3, BrandName = "Peugeot", LogoUrl = "/carola/assets/images/brands/peugeot.png", Status = true },
                new Brand { BrandId = 4, BrandName = "Renault", LogoUrl = "/carola/assets/images/brands/renault.png", Status = true },
                new Brand { BrandId = 5, BrandName = "Porsche", LogoUrl = "/carola/assets/images/brands/porsche.png", Status = true },
                new Brand { BrandId = 6, BrandName = "BMW", LogoUrl = "/carola/assets/images/brands/bmw.png", Status = true },
                new Brand { BrandId = 7, BrandName = "Mercedes-Benz", LogoUrl = "/carola/assets/images/brands/mercedes.png", Status = true }
            );

            // Seed Locations (Branches)
            modelBuilder.Entity<Location>().HasData(
                new Location { LocationId = 1, LocationName = "İstanbul Havalimanı", City = "İstanbul", Address = "İstanbul Havalimanı Gelen Yolcu Katı", AuthorizedPerson = "Mehmet Demir" },
                new Location { LocationId = 2, LocationName = "Antalya Havalimanı", City = "Antalya", Address = "Antalya Havalimanı Dış Hatlar", AuthorizedPerson = "Ahmet Yılmaz" },
                new Location { LocationId = 3, LocationName = "İzmir Havalimanı", City = "İzmir", Address = "Adnan Menderes Havalimanı İç Hatlar", AuthorizedPerson = "Elif Kaya" },
                new Location { LocationId = 4, LocationName = "Antalya Konyaaltı Şubesi", City = "Antalya", Address = "Konyaaltı Arapsuyu Caddesi No:12", AuthorizedPerson = "Can Yıldız" }
            );

            // Seed Cars
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    CarId = 1,
                    BrandId = 6, // BMW
                    Model = "320i",
                    ModelYear = 2022,
                    PlateNumber = "07 ABC 123",
                    DailyPrice = 2500,
                    SeatCount = 5,
                    LuggageCapacity = 450,
                    Mileage = 155551,
                    IsAvailable = false, // Kırada/Bakımda
                    FuelType = "Benzin",
                    TransmissionType = "Otomatik",
                    CategoryId = 4, // Spor
                    LocationId = 2, // Antalya Havalimanı
                    ImageUrl = "/carola/assets/images/cars/bmw_320i.png"
                },
                new Car
                {
                    CarId = 2,
                    BrandId = 7, // Mercedes-Benz
                    Model = "C180",
                    ModelYear = 2021,
                    PlateNumber = "35 DEF 456",
                    DailyPrice = 2800,
                    SeatCount = 5,
                    LuggageCapacity = 480,
                    Mileage = 22000,
                    IsAvailable = true,
                    FuelType = "Benzin",
                    TransmissionType = "Otomatik",
                    CategoryId = 4, // Spor
                    LocationId = 1, // İstanbul Havalimanı
                    ImageUrl = "/carola/assets/images/cars/mercedes_c180.png"
                },
                new Car
                {
                    CarId = 3,
                    BrandId = 4, // Renault
                    Model = "Clio",
                    ModelYear = 2020,
                    PlateNumber = "06 GHI 789",
                    DailyPrice = 1500,
                    SeatCount = 5,
                    LuggageCapacity = 320,
                    Mileage = 45000,
                    IsAvailable = false, // Kırada/Bakımda
                    FuelType = "Dizel",
                    TransmissionType = "Manuel",
                    CategoryId = 1, // SUV (based on clio categorized as SUV in screenshots)
                    LocationId = 1, // İstanbul Havalimanı
                    ImageUrl = "/carola/assets/images/cars/renault_clio.png"
                },
                new Car
                {
                    CarId = 4,
                    BrandId = 5, // Porsche
                    Model = "911 Carrera",
                    ModelYear = 2024,
                    PlateNumber = "07 POR 001",
                    DailyPrice = 7800,
                    SeatCount = 2,
                    LuggageCapacity = 200,
                    Mileage = 5000,
                    IsAvailable = true,
                    FuelType = "Benzin",
                    TransmissionType = "Otomatik",
                    CategoryId = 4, // Spor
                    LocationId = 4, // Antalya Konyaaltı
                    ImageUrl = "/carola/assets/images/cars/porsche_911.png"
                },
                new Car
                {
                    CarId = 5,
                    BrandId = 2, // Toyota
                    Model = "Corolla",
                    ModelYear = 2022,
                    PlateNumber = "07 TOY 005",
                    DailyPrice = 1500,
                    SeatCount = 5,
                    LuggageCapacity = 0,
                    Mileage = 22000,
                    IsAvailable = true,
                    FuelType = "Hibrit",
                    TransmissionType = "Otomatik",
                    CategoryId = 5, // Ekonomik
                    LocationId = 3, // İzmir Havalimanı
                    ImageUrl = "/carola/assets/images/cars/toyota_corolla.png"
                },
                new Car
                {
                    CarId = 6,
                    BrandId = 7, // Mercedes-Benz
                    Model = "C 200",
                    ModelYear = 2022,
                    PlateNumber = "07 ABC 891",
                    DailyPrice = 2500,
                    SeatCount = 5,
                    LuggageCapacity = 450,
                    Mileage = 15000,
                    IsAvailable = true,
                    FuelType = "Benzin",
                    TransmissionType = "Otomatik",
                    CategoryId = 2, // Sedan
                    LocationId = 4, // Antalya Konyaaltı
                    ImageUrl = "/carola/assets/images/cars/mercedes_c200.png"
                },
                new Car
                {
                    CarId = 7,
                    BrandId = 2, // Toyota
                    Model = "Hilux",
                    ModelYear = 2024,
                    PlateNumber = "07 TRK 007",
                    DailyPrice = 3500,
                    SeatCount = 5,
                    LuggageCapacity = 1000,
                    Mileage = 12000,
                    IsAvailable = true,
                    FuelType = "Dizel",
                    TransmissionType = "Otomatik",
                    CategoryId = 3, // Truck
                    LocationId = 2, // Antalya Havalimanı
                    ImageUrl = "/carola/assets/images/cars/toyota_hilux.png"
                },
                new Car
                {
                    CarId = 8,
                    BrandId = 5, // Porsche
                    Model = "911 Carrera",
                    ModelYear = 2017,
                    PlateNumber = "06 GHI 789",
                    DailyPrice = 9500,
                    SeatCount = 3,
                    LuggageCapacity = 320,
                    Mileage = 40000,
                    IsAvailable = false, // Kırada/Bakımda
                    FuelType = "Hibrit",
                    TransmissionType = "Manuel",
                    CategoryId = 4, // Spor
                    LocationId = 1, // İstanbul Havalimanı
                    ImageUrl = "/carola/assets/images/cars/porsche_911_yellow.png"
                }
            );

            // Seed Slider
            modelBuilder.Entity<Slider>().HasData(
                new Slider
                {
                    SliderId = 1,
                    Title = "Drive Easy. Your Ride, Your Rules.",
                    Description = "Flexible booking options, affordable prices, and reliable service make your journey easier. Enjoy full control of your travel experience.",
                    ImageUrl = "/carola/assets/images/slider/hero_bg.jpg",
                    Status = true
                }
            );

            // Seed WeAreTheBest
            modelBuilder.Entity<WeAreTheBest>().HasData(
                new WeAreTheBest
                {
                    WeAreTheBestId = 1,
                    Title = "Explore The World With Your Own Way Of Driving",
                    Description = "We offer the best car rental services with clean vehicles, round-the-clock support, and convenient branch pick-ups.",
                    Feature1Title = "Free Pick Up & Drop",
                    Feature1Description = "Your convenience matter. Complimentary pick-up and drop-off service for any your vehicle, a stress-free experience.",
                    Feature2Title = "24/7 Road Assistant",
                    Feature2Description = "No matter the time or place, and our 24/7 roadside assistance ensures you're never stranded. Drive confidently with support."
                }
            );
        }
    }
}
