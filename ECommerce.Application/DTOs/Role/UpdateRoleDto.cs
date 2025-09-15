using ECommerce.Application.Core.Request;

namespace ECommerce.Application.DTOs.Role;
    public class UpdateRoleDto : BaseRequest
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
