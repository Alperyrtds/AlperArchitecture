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
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;


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
        [Produces(typeof(AlperResult<TblEmployee>))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AlperCmdFilter<CreateUserCmd, CreateUserVld>))]
        public async Task<IResult> CreateUser([FromBody] CreateUserCmd qry, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(qry, cancellationToken);

            return ApiResult<TblEmployee>.GetHttpResult(result);

        }

    }
}
