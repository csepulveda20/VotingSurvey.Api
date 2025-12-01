namespace VotingSurvey.Application.Models
{
    public class PaginationResponse<T>
    {
        public int? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public IReadOnlyList<T> Items { get; init; } = [];
        
        public static Task<PaginationResponse<T>> Create(IQueryable<T> source, int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(1, pageNumber);
            var totalCount = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return Task.FromResult(new PaginationResponse<T>([.. items], totalCount, pageNumber, pageSize));
        }

        public PaginationResponse(IReadOnlyList<T> items, int totalCount, int? pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalCount = totalCount;
            PageSize = items.Any() ? pageSize : 0;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items = items;
        }
    }
}