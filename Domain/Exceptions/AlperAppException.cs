using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;

namespace Domain.Exceptions;

public sealed class AlperAppException : Exception
{
    public AlperAppException(string message) : base(message)
    {
    }

    public AlperAppException(IEnumerable<Status> errors) : base(errors.FlatAll())
    {
    }
}

