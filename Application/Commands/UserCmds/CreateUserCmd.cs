using Alper.Repository.Models;
using Application.Commands;
using Common.DTO.User;
using Common.Utils;
using MediatR;

namespace Alper.Application.Commands.UserCmds;

public sealed record CreateUserCmd(NewUserCmdDto NewUserCmdDto) : AlperCmd, IRequest<AlperResult<TblUsers>>;

