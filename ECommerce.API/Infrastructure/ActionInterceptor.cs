using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;

namespace ECommerce.API.Infrastructure
{
    public class ActionInterceptor(IServiceProvider serviceProvider) : ActionFilterAttribute
    {
       

       
    }
}
