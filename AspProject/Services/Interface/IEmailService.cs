

using System.Threading.Tasks;

namespace AspProject.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmail(string emailTO, string html, string content, string userName);
    }
}
