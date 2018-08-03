using Account.Client;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Todo.WindowsPhone
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        public static TaskCompletionSource<string> AccessTokenSource;

        Frame frame;

        void Navigate(Uri source)
        {
            if (frame == null)
            {
                frame = (Frame)Application.Current.RootVisual;
            }
            frame.Navigate(source);
        }

        public async Task<string> GetTokenAsync()
        {
            if (AccessTokenProvider.AccessTokenSource != null)
            {
                AccessTokenProvider.AccessTokenSource.TrySetCanceled();
            }
            AccessTokenSource = new TaskCompletionSource<string>(); 
            Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            return await AccessTokenSource.Task;
        }
    }
}
