using System.Text.Json.Serialization;

namespace ECommerce.Application.Core.Request;
public class BaseRequest
{
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }
    [JsonIgnore]
    public string? CurrentUserRoleName { get; set; }
}
