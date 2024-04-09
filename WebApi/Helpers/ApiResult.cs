using Common.Enums;
using Common.Utils;

namespace WebApi.Helpers;

public static class ApiResult<T>
{
    public static IResult GetHttpResult(AlperResult<T> result)
    {
        return result.ResultCode switch
        {
            ResultCodes.Success => Results.Ok(result),
            ResultCodes.NotFound => Results.NotFound(result),
            ResultCodes.BadRequest => Results.BadRequest(result),
            ResultCodes.InternalServerError => Results.BadRequest(SistemHatasiGizle(result)),
            _ => Results.BadRequest(result)
        };
    }
    private static AlperResult<T> SistemHatasiGizle(AlperResult<T> response)
    {
        foreach (var error in response.Errors)
        {
            error.Message = "Sistem hatası. Lütfen hata numarasını sistem yöneticisine bildiriniz.";
        }

        return response;
    }
}

