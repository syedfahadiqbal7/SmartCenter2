namespace AFFZ_API.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string to, string subject, string body, string toname);
    }
}
