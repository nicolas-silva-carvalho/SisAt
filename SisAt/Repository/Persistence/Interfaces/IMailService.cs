namespace SisAt.Repository.Persistence.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(string name, string mail, string subject, string body);
    }
}
