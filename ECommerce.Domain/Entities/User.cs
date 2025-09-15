namespace ECommerce.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string PersonnelNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Salt { get; set; }
        public Guid? AgencyId { get; set; }
        public ICollection<Role> Roles { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
