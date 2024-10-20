using Alper.Common.DTO;
using Alper.Repository.Cache;
using Common.DTO.User;
using Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace Alper.WebApi.Controllers
{
    public class SistemController : Controller
    {
        private readonly ICacheService _cacheService;

        public SistemController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("cleanRedisKeys")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(typeof(ApiResult<MessageDto>))]

        public async Task<IResult> CleanRedisKeys(CancellationToken cancellationToken)
        {
            var result = _cacheService.RemoveAll();
            return ApiResult<MessageDto>.GetHttpResult(result);
        }
    }
}
