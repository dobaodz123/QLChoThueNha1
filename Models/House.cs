using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QlChoThueNha1.Models
{
    public class House
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà là bắt buộc")]
        [MaxLength(200)]
        [Display(Name = "Tên nhà")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [MaxLength(300)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Giá thuê là bắt buộc")]
        [Range(100000, 1000000000, ErrorMessage = "Giá từ 100,000đ - 1,000,000,000đ")]
        [Display(Name = "Giá thuê/tháng")]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Diện tích là bắt buộc")]
        [Range(10, 1000, ErrorMessage = "Diện tích từ 10m² - 1000m²")]
        [Display(Name = "Diện tích (m²)")]
        public double Area { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Available"; // Available | Rented

        [MaxLength(500)]
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }

        // Foreign key
        [Required]
        [Display(Name = "Loại nhà")]
        public int HouseTypeId { get; set; }

        // Navigation property
        [ForeignKey("HouseTypeId")]
        public HouseType? HouseType { get; set; }

        // Navigation property
        public ICollection<RentalRequest>? RentalRequests { get; set; }
    }
}