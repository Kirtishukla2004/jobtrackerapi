namespace JobTracker.API.DTOs
{
    public class Tokendto
    {

        public string ?Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
