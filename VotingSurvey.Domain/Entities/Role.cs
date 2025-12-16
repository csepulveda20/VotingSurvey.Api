namespace VotingSurvey.Domain.Entities
{
    public class Role
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; private set; }

        public Role() { }

        public Role(string roleId, string name, string description)
        {
            RoleId = roleId;
            Name = name;
            Description = description;
        }
    }
}
