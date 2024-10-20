using Common.DTO.User;
using Common.Utils;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCmds;

public sealed record CreateUserCmd(NewUserCmdDto NewUserCmdDto) : AlperCmd, IRequest<AlperResult<TblEmployee>>;

