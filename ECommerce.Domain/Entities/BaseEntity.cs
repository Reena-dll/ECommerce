namespace ECommerce.Domain.Entities;

public interface IBaseEntity;
public class BaseEntity : IBaseEntity
{
    //Guid
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateDate { get; set; } = GetTurkeyTime(DateTime.UtcNow);
    public Guid CreatedBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }


    public static DateTime GetTurkeyTime(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
    }
}
