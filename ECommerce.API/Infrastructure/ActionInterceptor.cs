using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using ECommerce.Application.Core.Request;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using ECommerce.Application.Core.Results;

namespace ECommerce.API.Infrastructure
{
    public class ActionInterceptor(IServiceProvider serviceProvider) : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateRequest(context);
            FillBaseRequest(context);
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is ECommerce.Application.Core.Results.IResult result &&
                !result.IsSuccess)
            {
                if (objectResult.StatusCode != null && objectResult.StatusCode != (int)HttpStatusCode.UnprocessableEntity)
                    context.Result = new ConflictObjectResult(objectResult.Value);
            }
            base.OnResultExecuting(context);
        }

        private void ValidateRequest(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

                var validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                    var result = validator.Validate(validationContext);

                    if (!result.IsValid)
                    {
                        var errors = result.Errors.Select(p => new Error($"{p.PropertyName}Invalid", p.ErrorMessage)).ToList();

                        context.Result = new UnprocessableEntityObjectResult((Result)errors);
                    }
                }
            }
        }

        private void FillBaseRequest(ActionExecutingContext context)
        {
            var baseRequests = context.ActionArguments
                .Where(arg => arg.Value is BaseRequest baseRequest)
                .Select(arg => (BaseRequest)arg.Value);
            foreach (var baseRequest in baseRequests)
            {
                if (baseRequest != null)
                {
                    var authz = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
                    if (authz != null)
                    {
                        var token = authz.Substring("Bearer ".Length).Trim();
                        var handler = new JwtSecurityTokenHandler();
                        if (handler.CanReadToken(token))
                        {
                            var jwtToken = handler.ReadJwtToken(token);
                            var claims = jwtToken.Claims.ToList();
                            baseRequest.CurrentUserId = Guid.Parse(claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier.ToString())?.Value ?? string.Empty);
                            baseRequest.CurrentUserRoleName = claims.FirstOrDefault(p => p.Type == ClaimTypes.Role.ToString())?.Value ?? string.Empty;
                        }
                    }
                }
            }
        }

    }
}
