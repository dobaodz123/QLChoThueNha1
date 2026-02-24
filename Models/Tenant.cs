namespace QlChoThueNha1.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<RentalRequest> RentalRequests { get; set; }
    }

}
