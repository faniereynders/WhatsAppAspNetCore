using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsApp;

namespace WebApplication1
{

    public class MyWhatsappEvents : IWhatsAppEventExecutor
    {
        private readonly IHubContext<WhatsappHub> hub;

        public MyWhatsappEvents(IHubContext<WhatsappHub> hub)
        {
            this.hub = hub;
        }

        public async Task OnAuthenticatedAsync()
        {
            await hub.Clients.All.InvokeAsync("Authenticated");
        }

        public async Task OnAuthenticatingAsync(string qrCodeBase64String)
        {
            await hub.Clients.All.InvokeAsync("Authenticating", qrCodeBase64String);
        }

        public async Task OnMessagesReceived(UnreadMessage[] messages)
        {

            await hub.Clients.All.InvokeAsync("MessagesReceived", messages);
        }

       

        public async Task OnScreenshotReceivedAsync(byte[] uiImage)
        {
            var filename = @"c:\whatsapp\screen.jpg";
            await File.WriteAllBytesAsync(filename, uiImage);
            await hub.Clients.All.InvokeAsync("UI");

        }

        public async Task OnStartedAsync()
        {
            await hub.Clients.All.InvokeAsync("Started");
        }

        public async Task OnStoppedAsync()
        {
            await hub.Clients.All.InvokeAsync("Stopped");
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSignalR();

            services.AddWhatsApp<MyWhatsappEvents>(o=>
            {
                o.ScreenshotInterval = 100;
                o.TakeScreenshot = true;
                
            });



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          
        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<WhatsappHub>("whatsapp");
            });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void OnShutdown()
        {
            
        }
    }
}
