namespace JobTracker.API.DTOs
{
    public class PagedDatadto<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }

}
