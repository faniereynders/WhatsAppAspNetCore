using System.Threading.Tasks;

namespace WhatsApp
{
    public interface IWhatsAppEventExecutor
    {
        Task OnScreenshotReceivedAsync(byte[] uiImage);

        Task OnMessagesReceived(UnreadMessage[] messages);
        Task OnStartedAsync();
        Task OnStoppedAsync();
        Task OnAuthenticatingAsync(string qrCodeBase64String);
        Task OnAuthenticatedAsync();
    }
}
