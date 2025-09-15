using ECommerce.Application.Core.DTO;
using ECommerce.Application.DTOs.Role;

namespace ECommerce.Application.DTOs.User;

public class UserDto : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Username { get; set; }
    public string Email { get; set; }
    public string PersonnelNumber { get; set; }
    public List<RoleForUser> RoleList { get; set; }
}

