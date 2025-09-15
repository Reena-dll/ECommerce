using ECommerce.Application.Core.DTO;

namespace ECommerce.Application.DTOs.Permission;

public class PermissionDto:BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid RoleId { get; set; }
    }
