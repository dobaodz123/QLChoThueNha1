using QlChoThueNha1.Models;

public class HouseType
{
    public int Id { get; set; }
    public string Name { get; set; } // Phòng trọ, Căn hộ...

    public ICollection<House> Houses { get; set; }
    public string Description { get; internal set; }
}
