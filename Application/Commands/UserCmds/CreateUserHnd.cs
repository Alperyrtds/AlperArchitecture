using Alper.Application.Commands.UserCmds;
using Alper.Domain.Exceptions;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Common.Enums;
using Common.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Commands.UserCmds;

public sealed class CreateUserHnd(IProjectRepository<TblUsers> projectRepository, IUnitOfWork unitOfWork, IConfiguration configuration) : IRequestHandler<CreateUserCmd, AlperResult<TblUsers>>
{
    public async Task<AlperResult<TblUsers>> Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
        try
        {
            var key = configuration["Jwt:Key"];
            var user = new TblUsers
            {
                Id = Genarate.IdGenerator(),
                Email = request.NewUserCmdDto.Email,
                Password = Genarate.PasswordHash(request.NewUserCmdDto.Password, key!),
                Name = request.NewUserCmdDto.Name,
                Surname = request.NewUserCmdDto.Surname,
                PhoneNumber = request.NewUserCmdDto.PhoneNumber,
                BirthDate = request.NewUserCmdDto.BirthDate,
                StartJobDate = DateTime.Now,
                TransactionUser = request.NewUserCmdDto.TransactionUser,
                Status = UserStatus.DogrulamaBekliyor.GetHashCode(),
            };

            await projectRepository.InsertAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            return new AlperResult<TblUsers>(user);
        }
        catch (AlperAppException ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return ex.Message;
        }
        catch (Exception ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return AlperResult<TblUsers>.Exception(ex.Message);
        }
       
    }
}
