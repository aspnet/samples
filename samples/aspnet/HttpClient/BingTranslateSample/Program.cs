using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using BingTranslate.AzureMarketplace;

namespace BingTranslate
{
    /// <summary>
    /// Sample illustrating using the BING translator API. The API requires an OAuth token which we obtain
    /// by sending a request to the Azure token server each time we send a request to the translator service. 
    /// The result from that request is fed into the request sent to the translation service itself.
    /// <remarks>
    /// Before you can run this sample you must obtain an application key from Azure Marketplace, and 
    /// fill in the information in the AccessTokenMessageHandler class, see 
    /// http://msdn.microsoft.com/en-us/library/hh454950.aspx for details.
    /// </remarks>
    /// </summary>
    class Program
    {
        static string _address = "http://api.microsofttranslator.com/V2/Http.svc/GetTranslations?text=Hello%20World&from=en&to=es&maxTranslations=5";

        static async void RunClient()
        {
            try
            {
                Console.WriteLine("Translating English phrase {0} (might take some seconds)...", "Hello World");

                // Create client and insert a message handler that obtains an Azure Marketplace Access Token in the message path
                HttpClient client = new HttpClient(new AccessTokenMessageHandler(new HttpClientHandler()));

                // Create BING translate options data which we send along with the HTTP request as part of the HTTP request body.
                // TranslateOptions is a type provided by BING translation service.
                TranslateOptions options = new TranslateOptions("", "defaultUser");

                // Send asynchronous request to BING translation API
                MediaTypeFormatter xmlFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                HttpResponseMessage response = await client.PostAsync<TranslateOptions>(_address, options, xmlFormatter);

                // Check that response was successful or throw exception
                response.EnsureSuccessStatusCode();

                // Read response asynchronously as GetTranslationsResponse which is a type defined by BING translation service
                GetTranslationsResponse translation = await response.Content.ReadAsAsync<GetTranslationsResponse>(new[] { xmlFormatter });

                Console.WriteLine();
                Console.WriteLine("...{0}", translation.Translations[0].TranslatedText);
            }
            catch (Exception e)
            {
                Console.WriteLine("Request caused exception: {0}", e.Message);
            }
        }

        static void Main(string[] args)
        {
            RunClient();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}