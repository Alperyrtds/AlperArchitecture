using Alper.Domain.ErrorMessages;
using Alper.Domain.Exceptions;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Common.Utils;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alper.Application.Queries.UserQrys;

public sealed class GetAllUserHnd(IProjectRepository<TblUsers> projectRepository) : IRequestHandler<GetAllUserQry, AlperResult<IEnumerable<TblUsers>>>
{
    public async Task<AlperResult<IEnumerable<TblUsers>>> Handle(GetAllUserQry request, CancellationToken cancellationToken)
    {
        try
        {
            var userList =
                await projectRepository.GetAllAsync( cancellationToken);

            return userList == null ? AlperResult<IEnumerable<TblUsers>>.NotFound(UserErrors.KullaniciListesiBulunamadi) : AlperResult<IEnumerable<TblUsers>>.Success(userList);
        }
        catch (AlperAppException ex)
        {
            return ex.Message;
        }
        catch (Exception e)
        {
            return AlperResult<IEnumerable<TblUsers>>.Exception(e.Message);
        }
    }
}
