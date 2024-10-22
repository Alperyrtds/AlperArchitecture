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

public sealed class GetUserByEmailHnd(IProjectRepository<TblUsers> projectRepository) : IRequestHandler<GetUserByEmailQry, AlperResult<TblUsers>>
{
    public async Task<AlperResult<TblUsers>> Handle(GetUserByEmailQry request, CancellationToken cancellationToken)
    {
        try
        {
            var kullanici =
                await projectRepository.GetByEmailAsync(request.Eposta, cancellationToken);

            return kullanici == null ? AlperResult<TblUsers>.NotFound(UserErrors.KullaniciBulunamadi) : kullanici;
        }
        catch (AlperAppException ex)
        {
            return ex.Message;
        }
        catch (Exception e)
        {
            return AlperResult<TblUsers>.Exception(e.Message);
        }
    }
}

