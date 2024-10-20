using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alper.Domain.ErrorMessages;
using Alper.Domain.Exceptions;
using Alper.Infrastructure.Models;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Common.Utils;


namespace Alper.Application.Queries.UserQrys;

public sealed class GetUserByEmailHnd(IProjectRepository<TblUser> projectRepository) : IRequestHandler<GetUserByEmailQry, AlperResult<TblUser>>
{
    public async Task<AlperResult<TblUser>> Handle(GetUserByEmailQry request, CancellationToken cancellationToken)
    {
        try
        {
            var kullanici =
                await projectRepository.GetByEmailAsync(request.Eposta, cancellationToken);

            return kullanici == null ? AlperResult<TblUser>.NotFound(UserErrors.KullaniciBulunamadi) : kullanici;
        }
        catch (AlperAppException ex)
        {
            return ex.Message;
        }
        catch (Exception e)
        {
            return AlperResult<TblUser>.Exception(e.Message);
        }
    }
}

