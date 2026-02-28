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
        [Required]
        public int HouseId { get; set; }

        [ForeignKey("HouseId")]
        public House House { get; set; }

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        // Thời gian thuê
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Ngày gửi yêu cầu")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Trạng thái
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // Ghi chú
        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Note { get; set; }
    }
}