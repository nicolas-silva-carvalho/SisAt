namespace SisAt.Repository.Persistence
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(string name, string mail, string subject, string body);
    }
}
