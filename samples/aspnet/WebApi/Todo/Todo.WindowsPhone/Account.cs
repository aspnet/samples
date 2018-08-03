using System.Collections.Generic;

namespace Todo.WindowsPhone
{
    public class Account
    {
        public static IDictionary<string, string> Icons { get; private set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
        public string Icon { get; set; }
        public string ProviderUri { get; set; }

        public static string GetAccountIcon(string provider)
        {
            switch(provider.ToLower())
            {
                case "facebook": return "/Assets/appbar.social.facebook.variant.png";
                case "twitter": return "/Assets/appbar.social.twitter.png";
                case "microsoft": return "/Assets/appbar.social.microsoft.png";
                case "google": return "/Assets/appbar.google.png";
                case "local": return "/Assets/appbar.interface.password.png";
                default: return null;
            }
        }
    }
}
