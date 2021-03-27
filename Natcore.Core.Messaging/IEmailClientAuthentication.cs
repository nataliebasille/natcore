using MailKit;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public interface IEmailClientAuthentication
    {
        internal Task AuthenticateAsync(IMailTransport transport);
    }
}
