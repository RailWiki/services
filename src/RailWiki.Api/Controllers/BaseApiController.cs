using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailWiki.Api.Models;

namespace RailWiki.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        protected void AddPaginationResponseHeaders<TModel>(PagedResponse<TModel> pagedResponse)
        {
            Response.Headers.Add("X-Pagination-Total", pagedResponse.Total.ToString());
            Response.Headers.Add("X-Pagination-PageCount", pagedResponse.PageCount.ToString());
            Response.Headers.Add("X-Pagination-PageSize", pagedResponse.PageSize.ToString());
            Response.Headers.Add("X-Pagination-Page", pagedResponse.CurrentPage.ToString());
            Response.Headers.Add("X-Pagination-HasPrevious", pagedResponse.HasPrevious.ToString().ToLower());
            Response.Headers.Add("X-Pagination-HasNext", pagedResponse.HasNext.ToString().ToLower());
        }        
    }
}
