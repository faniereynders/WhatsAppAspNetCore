using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WhatsApp
{

    public class WhatsAppConnector : IWhatsAppConnector
    {
        private ChromeDriver driver;
        private readonly IWhatsAppEventExecutor eventExecutor;
        private readonly WhatsAppOptions options;

        public WhatsAppConnector(IOptions<WhatsAppOptions> options, IWhatsAppEventExecutor eventExecutor)
        {
            
            this.options = options.Value;
            this.eventExecutor = eventExecutor;
        }

        private void OnStopped()
        {
            this.eventExecutor?.OnStoppedAsync();

        }

        private void OnStarted()
        {
            this.eventExecutor?.OnStartedAsync();

        }

        private void OnAuthenticated()
        {
            this.eventExecutor?.OnAuthenticatedAsync();

        }

        private void OnAuthenticating(string qrCodeBase64String)
        {
            this.eventExecutor?.OnAuthenticatingAsync(qrCodeBase64String);

        }

        private void OnUIReceived(byte[] ui)
        {
            this.eventExecutor?.OnScreenshotReceivedAsync(ui);
        }

        private void OnMessagesReceived(UnreadMessage[] unreadMessages)
        {
            this.eventExecutor?.OnMessagesReceived(unreadMessages);
        }

       
        public async Task SendMessageAsync(string number, string message)
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                
                var javascriptDriver = driver as IJavaScriptExecutor;
                var sendScript = Scripting.SendMessageByNumber(number, message);
                javascriptDriver?.ExecuteScript(sendScript);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private IWebElement FindElement(ChromeDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
        private IEnumerable<IWebElement> FindElementsCss(ChromeDriver driver, string css)
        {
            try
            {
                return driver.FindElementsByCssSelector(css);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        private Task WaitForMessages(int interval)
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            while (true)
            {
                var mode = (string)js.ExecuteScript(@"return Store.Stream.mode");

                if (mode != "MAIN")
                {

                    break;

                }
                var messagesFromUsers = GetUnreadMessages();
                if (messagesFromUsers.Any())
                {
                    OnMessagesReceived(messagesFromUsers.ToArray());
                }

                Thread.Sleep(interval);
            }

            return Task.CompletedTask;


        }



        private IEnumerable<UnreadMessage> GetUnreadMessages()
        {
            // driver.GetScreenshot().SaveAsFile(@"c:\whatsapp\screen.jpg");
            var javascriptDriver = (IJavaScriptExecutor)this.driver;// as IJavaScriptExecutor;
            var response = javascriptDriver.ExecuteScript(Scripting.GetUnReadMessages);
            var json = JsonConvert.SerializeObject(response);
            JArray messages = JArray.Parse(json);
            var unreadMessages = new List<UnreadMessage>();
            foreach (var message in messages)
            {
                JObject o = JObject.Parse(message.ToString());
                var unread = o.ToObject<UnreadMessage>();
                unreadMessages.Add(unread);
            }
            return unreadMessages;
        }

        private Task StreamUI(int interval)
        {
            Task.Run(() =>
            {
                while (true)
                {

                    OnUIReceived(driver.GetScreenshot().AsByteArray);
                    Thread.Sleep(interval);
                }
            });

            return Task.CompletedTask;
           
          
        }


        public async Task StartAsync()
        {
            var chromeOption = new ChromeOptions();
            if (options.Incognito)
            {
                chromeOption.AddArgument("--incognito");
            }
            if (options.Headless)
            {
                chromeOption.AddArgument("--headless");
            }
            if (options.DisableGPU)
            {
                chromeOption.AddArgument("--disable-gpu");
            }
            if (!string.IsNullOrEmpty(options.UserAgent))
            {
                chromeOption.AddArgument($"--user-agent={options.UserAgent}");
            }

            var driverLocation = AppContext.BaseDirectory;
            if (!string.IsNullOrEmpty(options.ChromeDriverDirectory))
            {
                driverLocation = options.ChromeDriverDirectory;
            }

            driver = new ChromeDriver(driverLocation, chromeOption);

            driver.Navigate().GoToUrl(options.WhatsappUrl);

            if (options.TakeScreenshot)
            {
                StreamUI(options.ScreenshotInterval);
            }

            OnStarted();

            while (true)
            {
                await Authenticate();

                OnAuthenticated();

                await WaitForMessages(options.CheckForNewMessagesInterval);
            }

            

        }

        public async Task StopAsync()
        {
            this.driver.Quit();

            OnStopped();
            await Task.CompletedTask;
        }

        private Task Authenticate()
        {




           
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;



            while (true)
            {

                var mode = (string)js.ExecuteScript(@"return Store.Stream.mode");

                if (mode == "MAIN")
                {
                    
                    break;
                    
                }
                else
                {
                    var element = FindElement(driver, By.ClassName("_2EZ_m"));
                    var reload = FindElement(driver, By.ClassName("HnNfm"));
                    if (reload != null)
                    {

                        reload.Click();

                    }
                    else

                    {
                        try
                        {
                            var img = element.FindElement(By.TagName("img"));
                            var qr = img.GetAttribute("src");
                            OnAuthenticating(qr);
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                }
                Thread.Sleep(100);
            }




            return Task.CompletedTask;


        }

    }
}
