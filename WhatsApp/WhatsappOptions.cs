using System;

namespace WhatsApp
{
    public class WhatsAppOptions
    {
        public bool Incognito { get; set; } = true;
        public bool Headless { get; set; } = true;
        public bool DisableGPU { get; set; } = true;
        public string UserAgent { get; set; } = $"Mozilla/5.0 ({Environment.OSVersion.Platform}; x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";

        public string ChromeDriverDirectory { get; set; }
        public string WhatsappUrl { get; set; } = "https://web.whatsapp.com";
        public bool TakeScreenshot { get; set; } = false;
        public int ScreenshotInterval { get; set; } = 1000;
        public int CheckForNewMessagesInterval { get; set; } = 500;
    }
}
