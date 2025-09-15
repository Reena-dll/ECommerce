namespace ECommerce.Application.DTOs.Authentication
{
    public class LoginResponseDto
    {
        public string FullName { get; set; } = null!;
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpireTime { get; set; }
        public List<string> RightList { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
    }
}
