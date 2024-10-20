using Application.Abstractions.Messaging;
using Common.DTO.User;
using Common.Enums;
using Common.Utils;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Commands.UserCmds;

public sealed class CreateUserHnd(IProjectRepository<TblEmployee> projectRepository, IUnitOfWork unitOfWork, IConfiguration configuration) : IRequestHandler<CreateUserCmd, AlperResult<TblEmployee>>
{
    public async Task<AlperResult<TblEmployee>> Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
        try
        {
            var key = configuration["Jwt:Key"];
            var user = new TblEmployee
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

            await projectRepository.InsertAsync(user);
            await unitOfWork.SaveChangesAsync();

            return new AlperResult<TblEmployee>(user);
        }
        catch (AlperAppException ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return ex.Message;
        }
        catch (Exception ex)
        {
            await unitOfWork.Rollback(cancellationToken);
            return AlperResult<TblEmployee>.Exception(ex.Message);
        }
       
    }
}
