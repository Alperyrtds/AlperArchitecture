using Alper.Infrastructure.Models;
using Alper.Repository.Models;
using Common.DTO.User;
using Common.Utils;
using MediatR;

namespace Application.Commands.UserCmds;

public sealed record CreateUserCmd(NewUserCmdDto NewUserCmdDto) : AlperCmd, IRequest<AlperResult<TblUser>>;

