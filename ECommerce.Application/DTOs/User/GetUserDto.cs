using ECommerce.Application.Core.DTO;

namespace ECommerce.Application.DTOs.User;

public class GetUserDto : BaseFilterDto
{
    public string? Name { get; set; }
}
