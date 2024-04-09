using Application.Commands;
using Common.Utils;
using FluentValidation;

namespace WebApi.Filters;

public class AlperCmdFilter<TCommand, TValidator> : IEndpointFilter
    where TValidator : AbstractValidator<TCommand>, new()
    where TCommand : AlperCmd
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        TValidator validator = new();
        try
        {
            var cmd = context.GetArgument<TCommand>(0);

           
                var email = context.HttpContext.User.Claims.FirstOrDefault(claim =>
                    claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                if (email is null)
                {
                    return Results.BadRequest("Sisteme giriş yapılmalıdır.");
                }

                if (string.IsNullOrWhiteSpace(email.Value))
                {
                    return Results.BadRequest("Sisteme giriş yapılmalıdır.");
                }

                cmd.IslemYapanKullanici = email.Value;
           
                cmd.IslemYapanKullanici = string.Empty;
            

            var validationResult = await validator.ValidateAsync(cmd);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(AlperResult<TCommand>.Validation(validationResult.Errors));
            }

        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message.Contains("Object reference not set to an instance of an object.")
                ? AlperResult<TCommand>.Validation($"{typeof(TCommand).Name} boş olamaz.")
                : AlperResult<TCommand>.Validation(e.Message));
        }

        return await next(context);
    }
}

