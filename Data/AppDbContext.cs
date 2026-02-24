using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Models;

namespace QlChoThueNha1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<HouseType> HouseTypes { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Seed HouseType
            // =========================
            modelBuilder.Entity<HouseType>().HasData(
                new HouseType
                {
                    Id = 1,
                    Name = "Phòng trọ",
                    Description = "Phòng trọ giá rẻ, phù hợp sinh viên"
                },
                new HouseType
                {
                    Id = 2,
                    Name = "Chung cư",
                    Description = "Căn hộ chung cư hiện đại"
                },
                new HouseType
                {
                    Id = 3,
                    Name = "Nhà nguyên căn",
                    Description = "Nhà riêng biệt, không gian rộng rãi"
                }
            );

            // =========================
            // Seed User (Admin + Customer)
            // =========================
            _ = modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "123456", // Trong thực tế nên hash
                    Email = "admin@gmail.com",
                    FullName = "Quản trị viên",
                    Role = "Admin"
                },
                new User
                {
                    UserId = 2,
                    Username = "customer1",
                    Password = "123456",
                    Email = "customer1@gmail.com",
                    FullName = "Nguyễn Văn A",
                   
                    Role = "Customer"
                },
                new User
                {
                    UserId = 3,
                    Username = "customer2",
                    Password = "123456",
                    Email = "customer2@gmail.com",
                    FullName = "Trần Thị B",
                    
                    Role = "Customer"
                }
            );

            // =========================
            // Seed House
            // =========================
            modelBuilder.Entity<House>().HasData(
                new House
                {
                    Id = 1,
                    Name = "Phòng trọ Quận 1",
                    Address = "123 Lê Lợi, Quận 1, TP.HCM",
                    Description = "Phòng trọ đẹp, gần chợ Bến Thành, đầy đủ tiện nghi",
                    Price = 3000000,
                    Area = 25,
                    ImageUrl = "/images/house1.jpg",
                    Status = "Available",
                    HouseTypeId = 1
                },
                new House
                {
                    Id = 2,
                    Name = "Chung cư Quận 3",
                    Address = "45 Nguyễn Thị Minh Khai, Quận 3, TP.HCM",
                    Description = "Căn hộ 2 phòng ngủ, view đẹp, an ninh tốt",
                    Price = 8000000,
                    Area = 60,
                    ImageUrl = "/images/house2.jpg",
                    Status = "Available",
                    HouseTypeId = 2
                },
                new House
                {
                    Id = 3,
                    Name = "Nhà nguyên căn Quận 7",
                    Address = "89 Huỳnh Tấn Phát, Quận 7, TP.HCM",
                    Description = "Nhà 1 trệt 2 lầu, 4 phòng ngủ, có sân vườn",
                    Price = 15000000,
                    Area = 120,
                    ImageUrl = "/images/house3.jpg",
                    Status = "Available",
                    HouseTypeId = 3
                },
                new House
                {
                    Id = 4,
                    Name = "Phòng trọ Quận Bình Thạnh",
                    Address = "234 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM",
                    Description = "Gần trường đại học, tiện cho sinh viên",
                    Price = 2500000,
                    Area = 20,
                    ImageUrl = "/images/house4.jpg",
                    Status = "Available",
                    HouseTypeId = 1
                },
                new House
                {
                    Id = 5,
                    Name = "Chung cư Quận 2",
                    Address = "567 Mai Chí Thọ, Quận 2, TP.HCM",
                    Description = "Căn hộ cao cấp, đầy đủ nội thất",
                    Price = 12000000,
                    Area = 80,
                    ImageUrl = "/images/house5.jpg",
                    Status = "Rented",
                    HouseTypeId = 2
                }
            );

            // =========================
            // Seed RentalRequest (Mẫu)
            // =========================
            modelBuilder.Entity<RentalRequest>().HasData(
                new RentalRequest
                {
                    Id = 1,
                    HouseId = 5,
                    UserId = 2,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    RequestDate = new DateTime(2023, 12, 15),
                    Status = "Approved",
                    Note = "Thuê dài hạn 1 năm"
                }
            );

            // =========================
            // Configure Relationships
            // =========================
            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.House)
                .WithMany(h => h.RentalRequests)
                .HasForeignKey(r => r.HouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.User)
                .WithMany(u => u.RentalRequests)

                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<House>()
                .HasOne(h => h.HouseType)
                .WithMany(ht => ht.Houses)
                .HasForeignKey(h => h.HouseTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}