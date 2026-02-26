using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QlChoThueNha1.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        public string? Phone { get; set; }

        public string? Role { get; set; }

        public DateTime? CreatedAt { get; set; }

        public ICollection<RentalRequest>? RentalRequests { get; set; }
    }
}