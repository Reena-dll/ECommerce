using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class Role:BaseEntity
{
    public string RoleName { get; set; }

    [JsonIgnore]
    public ICollection<User> Users { get; set; }

    [JsonIgnore]
    public ICollection<Permission> Permissions { get; set; }
}
