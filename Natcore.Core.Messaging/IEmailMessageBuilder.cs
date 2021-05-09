namespace Natcore.Core.Messaging
{
    public interface IEmailMessageBuilder<in TOptions>
    {
        EmailMessage Build(TOptions options);
    }
}
