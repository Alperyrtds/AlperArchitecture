using Application.Commands;
using Common.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class AlperCmdFilter<TCommand, TValidator> : IAsyncActionFilter, IFilterMetadata
    where TValidator : AbstractValidator<TCommand>, new()
    where TCommand : AlperCmd
{
    //public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    //{
    //    TValidator validator = new();
    //    try
    //    {
    //        var cmd = context.GetArgument<TCommand>(0);


    //        var email = context.HttpContext.User.Claims.FirstOrDefault(claim =>
    //            claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

    //        if (email is null)
    //        {
    //            return Results.BadRequest("Sisteme giriş yapılmalıdır.");
    //        }

    //        if (string.IsNullOrWhiteSpace(email.Value))
    //        {
    //            return Results.BadRequest("Sisteme giriş yapılmalıdır.");
    //        }

    //        cmd.IslemYapanKullanici = email.Value;

    //        //cmd.IslemYapanKullanici = string.Empty;


    //        var validationResult = await validator.ValidateAsync(cmd);
    //        if (!validationResult.IsValid)
    //        {
    //            return Results.BadRequest(AlperResult<TCommand>.Validation(validationResult.Errors));
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        return Results.BadRequest(e.Message.Contains("Object reference not set to an instance of an object.")
    //            ? AlperResult<TCommand>.Validation($"{typeof(TCommand).Name} boş olamaz.")
    //            : AlperResult<TCommand>.Validation(e.Message));
    //    }

    //    return await next(context);
    //}

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        TValidator validator = new();
        try
        {
            // Komutu ActionArguments üzerinden alıyoruz
            if (context.ActionArguments.TryGetValue("qry", out var commandObj) && commandObj is TCommand cmd)
            {
                // Kullanıcı emailini alıyoruz
                var email = context.HttpContext.User.Claims.FirstOrDefault(claim =>
                    claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                if (email == null || string.IsNullOrWhiteSpace(email.Value))
                {
                    context.Result = new BadRequestObjectResult("Sisteme giriş yapılmalıdır.");
                    return;
                }

                var property = cmd.GetType().GetProperty("IslemYapanKullanici");

                cmd.IslemYapanKullanici = email.Value;
                //if (property != null)
                //{
                //    property.SetValue(cmd, email.Value);
                //}
                //else
                //{
                //    // Eğer 'IslemYapanKullanici' adında bir property bulamazsak
                //    context.Result = new BadRequestObjectResult("Komutun 'IslemYapanKullanici' özelliği bulunamadı.");
                //    return;
                //}

                // Validasyon kontrolü
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(AlperResult<TCommand>.Validation(validationResult.Errors));
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Komut verisi bulunamadı.");
                return;
            }
        }
        catch (Exception e)
        {
            context.Result = new BadRequestObjectResult(e.Message.Contains("Object reference not set to an instance of an object.")
                ? AlperResult<TCommand>.Validation($"{typeof(TCommand).Name} boş olamaz.")
                : AlperResult<TCommand>.Validation(e.Message));
            return;
        }

        await next();
    }

}