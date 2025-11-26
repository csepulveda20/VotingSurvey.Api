using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Domain.Entities;

public sealed class Voting
{
    private readonly List<Vote> _votes = new();
    private readonly HashSet<Guid> _recipients = new();

    public Guid Id { get; }
    public Guid CommunityId { get; }
    public Guid CreatedByUserId { get; }
    public string Title { get; private set; }
    public string QuestionDescription { get; private set; }    
    public VotingWindow Window { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Guid> Recipients => _recipients;
    public IReadOnlyCollection<Vote> Votes => _votes;

    private Voting(Guid id, Guid communityId, Guid createdByUserId, string title, string questionDescription, VotingWindow window, DateTimeOffset createdAt)
    {
        Id = id;
        CommunityId = communityId;
        CreatedByUserId = createdByUserId;
        Title = ValidateTitle(title);
        QuestionDescription = ValidateQuestion(questionDescription);
        Window = window;
        CreatedAt = createdAt;
        IsPublished = true; // per requirement publish on creation
    }

    public static Voting Create(Guid id, Guid communityId, Guid createdByUserId, string title, string questionDescription, VotingWindow window, DateTimeOffset now)
        => new(id, communityId, createdByUserId, title, questionDescription, window, now);

    private static string ValidateTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Title required", nameof(value));
        if (value.Length > 200) throw new ArgumentOutOfRangeException(nameof(value));
        return value.Trim();
    }
    private static string ValidateQuestion(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Question description required", nameof(value));
        if (value.Length > 1000) throw new ArgumentOutOfRangeException(nameof(value));
        return value.Trim();
    }

    public void AddRecipient(Guid userId)
    {
        _recipients.Add(userId);
    }

    public Vote SelectOption(Guid userId, VoteOption option, DateTimeOffset now)
    {
        if (!_recipients.Contains(userId)) throw new InvalidOperationException("User not a recipient");
        if (!Window.HasStarted(now)) throw new InvalidOperationException("Voting not started yet");
        if (Window.HasEnded(now)) throw new InvalidOperationException("Voting already closed");
        if (_votes.Any(v => v.UserId == userId)) throw new InvalidOperationException("User already voted");

        var vote = Vote.Select(Guid.NewGuid(), Id, userId, option, now);
        _votes.Add(vote);
        return vote;
    }

    public void ConfirmVote(Guid userId, DateTimeOffset now)
    {
        var vote = _votes.SingleOrDefault(v => v.UserId == userId) ?? throw new InvalidOperationException("Vote not found");
        vote.Confirm(now);
    }

    public void EditBeforeStart(string newTitle, string newQuestion, VotingWindow newWindow, DateTimeOffset now)
    {
        if (Window.HasStarted(now)) throw new InvalidOperationException("Cannot edit after start except end date");
        Title = ValidateTitle(newTitle);
        QuestionDescription = ValidateQuestion(newQuestion);
        Window = newWindow;
        UpdatedAt = now;
    }

    public void ExtendEnd(DateTimeOffset newEnd, DateTimeOffset now)
    {
        if (!Window.HasStarted(now)) throw new InvalidOperationException("Use EditBeforeStart for pre-start edits");
        if (Window.HasEnded(now)) throw new InvalidOperationException("Voting already closed");
        if (newEnd <= Window.StartsAt) throw new ArgumentException("End must be after start");
        if (newEnd <= Window.EndsAt) throw new ArgumentException("New end must be later than current end");
        Window = VotingWindow.Create(Window.StartsAt, newEnd);
        UpdatedAt = now;
    }

    public void CloseEarly(DateTimeOffset now)
    {
        if (Window.HasEnded(now)) return;
        Window = VotingWindow.Create(Window.StartsAt, now);
        UpdatedAt = now;
        // Could auto-confirm pending selections here depending on design
    }

    public (int yes, int no) TallyConfirmed()
        => (_votes.Count(v => v.Confirmed && v.Option == VoteOption.Yes), _votes.Count(v => v.Confirmed && v.Option == VoteOption.No));
}
