namespace Windows.Security.Authentication.Web
{
    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationResult available in Win8.
    /// </summary>
    public sealed class WebAuthenticationResult
    {
        public string ResponseData
        {
            get;
            private set;
        }
        public WebAuthenticationStatus ResponseStatus
        {
            get;
            private set;
        }
        public uint ResponseErrorDetail
        {
            get;
            private set;
        }
        public WebAuthenticationResult(string data, WebAuthenticationStatus status, uint error)
        {
            this.ResponseData = data;
            this.ResponseStatus = status;
            this.ResponseErrorDetail = error;
        }
    }
}
