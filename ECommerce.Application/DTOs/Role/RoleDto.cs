using ECommerce.Application.Core.DTO;

namespace ECommerce.Application.DTOs.Role;
public class RoleDto : BaseDto
{
    public string RoleName { get; set; }
    public string Actions { get; set; }
}