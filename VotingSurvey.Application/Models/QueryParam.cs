namespace VotingSurvey.Application.Models
{
    public class QueryParam
    {
        public string? Search { get; set; } = string.Empty;
        public string? OrderBy { get; set; }
        public bool IsAscending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastDate { get; set; } = null;
    }
}