using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RailWiki.Api.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        
    }
}
