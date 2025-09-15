using ECommerce.Application.Core.Request;

namespace ECommerce.Application.DTOs.User;

public class AddUserDto : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public string PersonnelNumber { get; set; }
    public List<Guid> RoleId { get; set; }

}

