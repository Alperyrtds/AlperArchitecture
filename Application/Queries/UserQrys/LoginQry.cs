using MediatR;
using System.Text;
using Common.DTO;
using Common.Utils;

namespace Application.Queries.UserQrys;

public sealed record LoginQry(string Email, string Password) : IRequest<AlperResult<LoginDto>>
{
    public override string ToString()
    {
        var builder = new StringBuilder();

        (this with { Password = "*****" }).PrintMembers(builder);

        return builder.ToString();
    }
}

