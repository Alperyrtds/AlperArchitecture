using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alper.Repository.Models;
using Common.Utils;
using MediatR;

namespace Alper.Application.Queries.UserQrys;

public sealed record GetAllUserQry : IRequest<AlperResult<IEnumerable<TblUsers>>>;
