using System;

namespace QlChoThueNha1.Models
{
    public class ThanhToan
    {
        public int Id { get; set; }

        public int NhaId { get; set; }

        public string TenNguoiThanhToan { get; set; }

        public decimal SoTien { get; set; }

        public string PhuongThuc { get; set; }

        public DateTime NgayThanhToan { get; set; }
            = DateTime.Now;
        public string TrangThai { get; set; } = "Chờ duyệt";
    }
}