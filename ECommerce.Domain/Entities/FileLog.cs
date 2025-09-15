namespace ECommerce.Domain.Entities;

public class FileLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Length { get; set; }
    public string? Description { get; set; }
    public byte[] Content { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}
