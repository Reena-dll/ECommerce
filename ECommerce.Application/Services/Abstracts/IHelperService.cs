using ECommerce.Application.Core.DTO;

namespace ECommerce.Application.Services.Abstracts;

public interface IHelperService
{
    Task FillAudit(IEnumerable<BaseDto> entities);
    Task FillAudit(BaseDto entity);
    Task<CustomUser> GetUserFromToken();
}

