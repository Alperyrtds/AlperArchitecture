using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alper.Infrastructure.Models;
using Alper.Repository.Models;
using Common.Utils;

namespace Alper.Application.Queries.UserQrys;

public sealed record GetUserByEmailQry(string Eposta) : IRequest<AlperResult<TblUser>>;

