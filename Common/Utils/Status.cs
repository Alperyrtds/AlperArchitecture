using Common.Enums;
using System.Text;
using NUlid;

namespace Common.Utils;

public sealed class Status()
{
    public ResultCodes Code { get; init; } = ResultCodes.Unknown;
    public string Message { get; set; } = string.Empty;
    public string ErrorId { get; init; } = Ulid.NewUlid().ToString();

    private readonly IEnumerable<ErrorDetail> _errorDetails = new List<ErrorDetail>();

    public IEnumerable<ErrorDetail> ErrorDetails() => _errorDetails;

    private Status(ResultCodes resultCode, string message) : this()
    {
        Code = resultCode;
        Message = message;
        _errorDetails = new List<ErrorDetail>();
    }

    private Status(ResultCodes resultCode, string message, IEnumerable<ErrorDetail> errorDetails) : this()
    {
        Code = resultCode;
        Message = message;
        _errorDetails = errorDetails;
    }

    public static Status Olustur(ResultCodes resultCode, string message) => new(resultCode, message);

    public static Status Olustur(ResultCodes resultCode, string message, IEnumerable<ErrorDetail> errorDetails) =>
        new(resultCode, message, errorDetails);
}

public sealed record ErrorDetail(string ClassName, string MethodName, string LineNumber)
{
    public static ErrorDetail Olustur(string className, string methodName, string lineNumber) =>
        new(className, methodName, lineNumber);
}

public static class StatusExt
{
    public static string FlatAll(this IEnumerable<Status> errors)
    {
        var builder = new StringBuilder(string.Empty);

        foreach (var error in errors)
        {
            builder.Append($"{error.Code}: {error.Message}\n");
        }

        return builder.ToString();
    }
}

