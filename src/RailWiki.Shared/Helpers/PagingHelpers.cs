namespace RailWiki.Shared.Helpers
{
    public static class PagingHelpers
    {
        public static int GetPageCount(int itemCount, int pageSize) =>
            itemCount % pageSize == 0
            ? itemCount / pageSize
            : (itemCount / pageSize) + 1;

        public static int CalculateSkip(int pageSize, int currentPage) =>
            pageSize * (currentPage - 1);        
    }
}
