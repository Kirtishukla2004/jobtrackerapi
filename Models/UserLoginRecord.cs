namespace JobTracker.API.Models
{
    public class UsersLoginRecord
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiry { get; set; }
        public bool IsActive { get; set; }
    }

}

