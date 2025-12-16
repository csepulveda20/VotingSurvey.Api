namespace VotingSurvey.Domain.Entities
{
    public class UserRole
    {
        public string RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        private UserRole() { }
        public UserRole(string roleId, Guid userId)
        {
            RoleId = roleId;
            UserId = userId;
        }
    }

}
