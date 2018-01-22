using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WhatsApp
{
    public class WhatsAppHostedService : IHostedService
    {
     //   private readonly IHubContext<WhatsappHub> hubcontext;
        private readonly IWhatsAppConnector whatsappConnector;
        // private Whatsapp.WhatsappConnector whatsapp;
        public WhatsAppHostedService(IWhatsAppConnector whatsappConnector)
        {
            this.whatsappConnector = whatsappConnector;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {

          //  whatsappConnector.OnQRCodeChanged += Whatsapp_QRCodeChanged;
           // whatsappConnector.OnMessagesReceived += Whatsapp_ReceivedMessages;
          //  whatsappConnector.OnUIReceived += Whatsapp_OnUIReceived;

            await whatsappConnector.StartAsync();





        }

        //private void Whatsapp_OnUIReceived(object sender, string ui)
        //{
        //    hubcontext.Clients.All.InvokeAsync("UI", ui);
        //}

        //private void Whatsapp_ReceivedMessages(object sender, UnreadMessage[] unreadMessages)
        //{
        //    var messages = new List<Message>();


        //    foreach (var user in unreadMessages)
        //    {
        //        var i = user.contact.Id.IndexOf("@");
        //        var msisdn = user.contact.Id.Substring(0, i);

        //        foreach (var userMessage in user.messages.OrderBy(z => z.timestamp))
        //        {
        //            messages.Add(userMessage);
        //            //   await this.incomingMessagesPublisher.SendMessage(new { Id = msisdn, Message = userMessage.message, CompanyCode = this.companyCode.ToUpper() });
        //        }
        //    }

        //    var message = new
        //    {
        //        messages = messages
        //    };
        //    hubcontext.Clients.All.InvokeAsync("Start", message);
        //}

        //private void Whatsapp_QRCodeChanged(object sender, string e)
        //{
        //    hubcontext.Clients.All.InvokeAsync("Login", e);
        //}

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await whatsappConnector.StopAsync();

        }
    }
}
