using System.Security.Claims;
using System.Text;

namespace Common.DTO;

public sealed record LoginDto(string Token, IEnumerable<Claim> Claims)
{
    public override string ToString()
    {
        var builder = new StringBuilder();

        (this with { Token = "*****" }).PrintMembers(builder);

        return builder.ToString();
    }
}

