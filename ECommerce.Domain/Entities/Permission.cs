namespace ECommerce.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Role> Roles { get; set; }

}
