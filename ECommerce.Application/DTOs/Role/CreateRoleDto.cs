using ECommerce.Application.Core.Request;

namespace ECommerce.Application.DTOs.Role;

public class CreateRoleDto : BaseRequest
{
    public string RoleName { get; set; }
    public List<Guid> PermissionIds { get; set; }
}

