using ECommerce.Application.Core.DTO;

namespace ECommerce.Application.DTOs.Role;
public class RoleFilterDto : BaseFilterDto
{
    public Guid? RoleId { get; set; }
}
