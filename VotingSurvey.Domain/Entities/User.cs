using VotingSurvey.Domain.ValueObjects;

// Domain
namespace VotingSurvey.Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public PersonName Name { get; private set; } = default!;
        public EmailAddress Email { get; private set; } = default!;
        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        private User() { } // EF Core

        private User(Guid id, PersonName name, EmailAddress email, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            IsActive = isActive;
        }

        public static User Create(Guid id, PersonName name, EmailAddress email)
            => new(id, name, email, true);
    }
}
