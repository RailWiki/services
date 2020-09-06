using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Helpers;

namespace RailWiki.Api.Models
{
    public class PagedResponse<TModel>
    {
        public IEnumerable<TModel> Data { get; set; } = new List<TModel>();

        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public PagedResponse(int pageSize, int page)
        {
            PageSize = pageSize;
            CurrentPage = page;
        }

        public async Task PaginateResultsAsync(IQueryable<TModel> data)
        {
            Total = await data.CountAsync();
            PageCount = PagingHelpers.GetPageCount(Total, PageSize);

            HasPrevious = CurrentPage > 1;
            HasNext = CurrentPage < PageCount;

            var skip = PagingHelpers.CalculateSkip(PageSize, CurrentPage);
            Data = await data.Skip(skip).Take(PageSize).ToListAsync();
        }
    }
}
