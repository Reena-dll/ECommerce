namespace ECommerce.Application.Core.DTO;

public class BaseFilterDto
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}