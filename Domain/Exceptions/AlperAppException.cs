using Common.Utils;

namespace Alper.Domain.Exceptions;

public sealed class AlperAppException : Exception
{
    public AlperAppException(string message) : base(message)
    {
    }

    public AlperAppException(IEnumerable<Status> errors) : base(errors.FlatAll())
    {
    }
}

