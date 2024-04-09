using Application.Queries.UserQrys;
using Common.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.Utils;
using WebApi.Helpers;


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


    }
}
