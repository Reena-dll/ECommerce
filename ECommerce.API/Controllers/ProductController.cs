using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("test");
    }
}

