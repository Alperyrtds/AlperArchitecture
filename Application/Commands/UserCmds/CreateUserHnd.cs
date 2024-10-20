using Alper.Domain.Exceptions;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Common.Enums;
using Common.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Commands.UserCmds;

public sealed class CreateUserHnd(IProjectRepository<TblUser> projectRepository, IUnitOfWork unitOfWork, IConfiguration configuration) : IRequestHandler<CreateUserCmd, AlperResult<TblUser>>
{
    public async Task<AlperResult<TblUser>> Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
        try
        {
            var key = configuration["Jwt:Key"];
            var user = new TblUser
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

            return new AlperResult<TblUser>(user);
        }
        catch (AlperAppException ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return ex.Message;
        }
        catch (Exception ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return AlperResult<TblUser>.Exception(ex.Message);
        }
       
    }
}
