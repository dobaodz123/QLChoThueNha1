using System;

namespace QlChoThueNha1.Models
{
    public class DanhGia
    {
        public int Id { get; set; }

        public int NhaId { get; set; }

        public string NoiDung { get; set; }

        public int SoSao { get; set; }

        public string NguoiDanhGia { get; set; }

        public DateTime NgayDanhGia { get; set; }
            = DateTime.Now;
    }
}