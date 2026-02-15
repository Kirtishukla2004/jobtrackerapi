namespace JobTracker.API.DTOs
{
    public class ResetPasswordRequestdto
    {
        public string?Token { get; set; }
        public string? Password { get; set; }
    }
}
