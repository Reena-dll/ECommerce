namespace ECommerce.Domain.Entities;
public class ApiLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AuthTokenInfo { get; set; } // JSON string of user info or 'Guest'
    public string UserAgent { get; set; }
    public string UserIp { get; set; }
    public string RequestMethod { get; set; } // GET, POST, PUT, DELETE vs.
    public string RequestUrl { get; set; }
    public int StatusCode { get; set; }
    public string RequestBody { get; set; }
    public string ResponseBody { get; set; }
    public DateTime RequestTime { get; set; }
    public decimal ResponseTimeInSeconds { get; set; }
}

