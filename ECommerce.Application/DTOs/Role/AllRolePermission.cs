namespace ECommerce.Application.DTOs.Role;
public class AllRolePermission
{
    public string RoleName { get; set; }
    public IEnumerable<string> RightList { get; set; }
    public Guid RoleId { get; set; }
    public List<Guid> PermissionIds { get; set; }
}