namespace VotingSurvey.Domain.Entities;

public sealed class UserUnit
{
    public Guid UserId { get; }
    public Guid UnitId { get; }
    public DateOnly FromDate { get; }
    public DateOnly? ToDate { get; private set; }

    public UserUnit(Guid userId, Guid unitId, DateOnly fromDate, DateOnly? toDate)
    {
        if (toDate is not null && toDate < fromDate)
            throw new ArgumentException("ToDate must be >= FromDate");
        UserId = userId;
        UnitId = unitId;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public void Close(DateOnly toDate)
    {
        if (toDate < FromDate) throw new ArgumentException("ToDate must be >= FromDate");
        ToDate = toDate;
    }
}
