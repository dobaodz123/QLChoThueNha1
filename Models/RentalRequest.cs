using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QlChoThueNha1.Models
{
    public class RentalRequest
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to House
        public int HouseId { get; set; }

        // Navigation property
        [ForeignKey("HouseId")]
        public House House { get; set; }

        // Foreign key to User (Customer/Tenant)
        public int UserId { get; set; }

        // Navigation property to User
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Thời gian thuê
        [Required]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Ngày gửi yêu cầu")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Trạng thái: Pending / Approved / Rejected
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // Ghi chú từ khách thuê
        [MaxLength(500)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
    }
}