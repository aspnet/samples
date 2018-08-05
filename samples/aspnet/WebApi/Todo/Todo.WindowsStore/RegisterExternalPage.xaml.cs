using Account.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.WindowsStore.Common;
using WebApi.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Todo.WindowsStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RegisterExternalPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        TaskCompletionSource<string> accessTokenSource;
        string externalLoginUri;


        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public RegisterExternalPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            RegisterExternalPageParameters parameters = (RegisterExternalPageParameters)e.NavigationParameter;
            this.accessTokenSource = parameters.AccessTokenSource;
            this.externalLoginUri = parameters.ExternalLoginUri;
            UserInfo userInfo = parameters.UserInfo;
            string text = SuccessfulExternalLoginTextBlock.Text;
            SuccessfulExternalLoginTextBlock.Text =
                text.Substring(0, text.LastIndexOf(' ') + 1) + userInfo.LoginProvider + '.';
            this.UsernameTextBox.Text = userInfo.UserName;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            RegisterExternalUser registerExternalUser = new RegisterExternalUser()
            {
                UserName = this.UsernameTextBox.Text,
            };

            HttpResult result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.RegisterExternalAsync(registerExternalUser);
            }
            if (result.Succeeded)
            {
                // Need to login again now that we are registered - should happen with the existing cookie
                ExternalLoginResult externalLoginResult = await ExternalLoginManager.GetExternalAccessTokenAsync(externalLoginUri, silentMode: true);
                if (externalLoginResult.AccessToken != null)
                {
                    bool completed = accessTokenSource.TrySetResult(externalLoginResult.AccessToken);
                    this.Frame.Navigate(typeof(TodoPage));
                }
                else
                {
                    await ErrorDialog.ShowErrorAsync("Failed to register external account");
                }
            }
            else
            {
                DisplayErrors(result.Errors);
            }
        }


        void DisplayErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                this.ErrorList.Items.Add(error);
            }
        }

        void ClearErrors()
        {
            this.ErrorList.Items.Clear();
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
