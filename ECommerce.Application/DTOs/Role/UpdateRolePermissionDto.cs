namespace ECommerce.Application.DTOs.Role;

public class UpdateRolePermissionDto
{
    public Guid roleId { get; set; }
    public List<Guid> PermissionIds { get; set; }

}

