using ECommerce.Application.Core.Request;

namespace ECommerce.Application.DTOs.Authentication
{
    public class LoginDto : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
