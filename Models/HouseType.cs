using System.Collections.Generic;

namespace QlChoThueNha1.Models
{
    public class HouseType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<House> Houses { get; set; } = new List<House>();
    }
}