using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public interface IEmailClient
    {
        Task SendAsync(EmailMessage message);
    }
}
