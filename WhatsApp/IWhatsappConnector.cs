using System.Threading.Tasks;

namespace WhatsApp
{
    public interface IWhatsAppConnector
    {
        Task SendMessageAsync(string number, string message);
        Task StartAsync();
        Task StopAsync();
    }
}