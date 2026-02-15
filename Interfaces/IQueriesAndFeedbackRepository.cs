namespace JobTracker.API.Interfaces
{
    public interface IQueriesAndFeedbackRepository
    {
        Task SaveFeedbackAsync(int userid, string Comment);
    }
}
