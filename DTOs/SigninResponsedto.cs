namespace JobTracker.API.DTOs
{
    public class SigninResponsedto
    {
        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
        public string Name { get; internal set; }
        public string Username { get; internal set; }
    }
}
