using Common.Enums;
using System.Diagnostics;
using System.Text;
using FluentValidation.Results;

namespace Common.Utils;

public sealed class AlperResult<TValue>()
{
    public TValue Result { get; init; } = default!;
    public ResultCodes ResultCode { get; init; } = ResultCodes.Unknown;
    public List<Status> Errors { get; init; } = [];
    private bool IsError { get; init; }

    public AlperResult(TValue result, int resultCode = 200) : this()
    {
        ResultCode = (ResultCodes)resultCode;
        IsError = resultCode != 200;
        Result = result;
        Errors = [];
    }

    private AlperResult(List<Status> errors) : this()
    {
        if (errors.Count != 0)
        {
            ResultCode = errors.Count > 1
                ? ResultCodes.MultipleChoices
                : errors.ElementAt(0).Code;
        }
        else
        {
            ResultCode = ResultCodes.Unknown;
        }

        IsError = true;
        Result = default!;
        Errors = errors;
    }
    private AlperResult(string error) : this()
    {
        IsError = true;
        Result = default!;
        ResultCode = ResultCodes.BadRequest;
        Errors = [Status.Olustur(ResultCode, error)];
    }
    private AlperResult(Status status) : this()
    {
        IsError = true;
        Result = default!;
        ResultCode = status.Code;
        Errors = [status];
    }
    public static implicit operator AlperResult<TValue>(TValue value)
    {
        return new AlperResult<TValue>(value);
    }

    public static implicit operator AlperResult<TValue>(List<Status> errors)
    {
        return new AlperResult<TValue>(errors);
    }

    public static implicit operator AlperResult<TValue>(string error)
    {
        return new AlperResult<TValue>(error);
    }

    public static implicit operator AlperResult<TValue>(Status error)
    {
        return new AlperResult<TValue>(error);
    }


    public override string ToString()
    {
        var builder = new StringBuilder("SuBilgiResult { IsError = " + IsError + ", Result = " + Result +
                                        ", ResultCode = " + ResultCode + ", Errors = [");
        foreach (var error in Errors)
        {
            builder.Append("{ ErrorId = " + error.ErrorId + ", Code = " + error.Code + ", Message = " + error.Message +
                           ",");

            builder.Append($"{Environment.NewLine}Error Details ={Environment.NewLine}");

            foreach (var trace in error.ErrorDetails())
            {
                builder.Append(
                    $"Class: {trace.ClassName} Method: {trace.MethodName}() Line: {trace.LineNumber}{Environment.NewLine}");
            }

            builder.Append('}');
        }

        builder.Append("]}");

        return builder.ToString();
    }
    public static AlperResult<TValue> Exception(string error)
    {
        var stackTrace = new StackTrace(true);
        var stackStr = stackTrace.ToString();

        var errorDetails = new List<ErrorDetail>();


        foreach (var trc in stackStr.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            if (trc.Contains("Alper") && !trc.Contains("AlperResult") &&
                (!trc.Contains('<') || !trc.Contains('>')))
            {
                var satir = trc.Replace("at ", "");

                var method = satir[..satir.IndexOf('(', StringComparison.Ordinal)];
                method = method[(method.LastIndexOf('.') + 1)..];

                var file = satir[(satir.LastIndexOf(" in ", StringComparison.Ordinal) + 4)..];
                var line = file[(file.LastIndexOf(":line ", StringComparison.Ordinal) + 6)..];
                file = file[..file.LastIndexOf(":line ", StringComparison.Ordinal)];
                var className = file[(file.LastIndexOf('/') + 1)..].Replace(".cs", "");
                var errorDetail = ErrorDetail.Olustur(className, method, line);
                errorDetails.Add(errorDetail);
            }
        }


        return Status.Olustur(ResultCodes.InternalServerError, error, errorDetails);
    }
    public static AlperResult<TValue> Validation(string error)
    {
        return Status.Olustur(ResultCodes.Forbidden, error);
    }

    public static AlperResult<TValue> Validation(List<ValidationFailure> errors)
    {
        return new AlperResult<TValue>(errors.ConvertAll(p =>
            Status.Olustur(ResultCodes.Forbidden, p.ErrorMessage)));
    }

    public static AlperResult<TValue> NotFound(string error)
    {
        return Status.Olustur(ResultCodes.NotFound, error);
    }

    public static AlperResult<TValue> Connection(string error)
    {
        return Status.Olustur(ResultCodes.BadGateway, error);
    }

    public static AlperResult<TValue> Success(TValue value)
    {
        return value;
    }

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<List<Status>, TResult> failure)
    {
        return !IsError ? success(Result!) : failure(Errors);
    }
}
