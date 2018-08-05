namespace BingTranslate.AzureMarketplace
{
    /// <summary>
    /// This class describes an Access Token from Azure Data Market 
    /// </summary>
    public class AdmAccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }
        
        public string expires_in { get; set; }
        
        public string scope { get; set; }
    }
}
