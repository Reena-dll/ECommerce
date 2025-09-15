namespace ECommerce.Application.DTOs.Authentication
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
