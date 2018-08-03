using Microsoft.Phone.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using WebAuthenticationBrokerPhone;

namespace Windows.Security.Authentication.Web
{
    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationBroker available in Win8.
    /// </summary>
    public sealed class WebAuthenticationBroker
    {  
        /// <summary>
        /// Mimics the WebAuthenticationBroker's AuthenticateAsync method.
        /// </summary>
        public static async Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, Uri startUri, Uri endUri)
        {
            if (options != WebAuthenticationOptions.None)
            {
                throw new NotImplementedException("This method does not support authentication options other than 'None'.");
            }
            PhoneApplicationFrame phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneApplicationFrame == null)
            {
                throw new InvalidOperationException();
            }
            string loginPageUri = "/WebAuthenticationBroker;component/loginpage.xaml" 
                + "?startUri=" + Uri.EscapeDataString(startUri.AbsoluteUri)
                + "&endUri=" + Uri.EscapeDataString(endUri.AbsoluteUri);
            if (LoginPage.WebAuthenticationResultSource != null)
            {
                LoginPage.WebAuthenticationResultSource.TrySetCanceled();
            }
            LoginPage.WebAuthenticationResultSource = new TaskCompletionSource<WebAuthenticationResult>();
            phoneApplicationFrame.Navigate(new Uri(loginPageUri, UriKind.Relative));
            return await LoginPage.WebAuthenticationResultSource.Task;
        }
    }
}
