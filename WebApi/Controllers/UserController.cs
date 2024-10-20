using Alper.Application.Queries.UserQrys;
using Alper.Repository.Models;
using Application.Queries.UserQrys;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.Utils;
using WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebApi.Filters;
using Common.DTO.User;
using Application.Commands.UserCmds;
using WebApi.Validators.Employees;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/login")]
        [Produces(typeof(AlperResult<LoginDto>))]
        public async Task<IResult> Login([FromBody] LoginQry qry, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(qry, cancellationToken);

            return ApiResult<LoginDto>.GetHttpResult(result); 

        }

        [HttpPost("/createUser")]
        [Produces(typeof(AlperResult<TblUser>))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AlperCmdFilter<CreateUserCmd, CreateUserVld>))]
        public async Task<IResult> CreateUser([FromBody] CreateUserCmd qry, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(qry, cancellationToken);

            return ApiResult<TblUser>.GetHttpResult(result);

        }
        [HttpPost("/getUserByEmail")]
        [Produces(typeof(AlperResult<TblUser>))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IResult> GetUserByEmail([FromBody] GetUserByEmailQry qry , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(qry, cancellationToken);

            return ApiResult<TblUser>.GetHttpResult(result);

        }
    }
}
