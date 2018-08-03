using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using System;
using System.Data.Services.Common;
using System.Xml;

namespace ODataClient.MSProducts
{
    /// <summary>
    /// Create an override for the generated Container (i.e. ctx) for the ODataService.Sample
    /// that:
    /// 1) Uses V3 of the protocol
    /// 2) Converts 404's into empty enumerations (i.e. ctx.Products.Where(p => p.ID == missingID) doesn't throw exception).
    /// </summary>
    public partial class Container
    {
        public Container()
            : this(new Uri("http://localhost:50231"))
        {
            SendingRequest += Container_SendingRequest;
            IgnoreResourceNotFoundException = true;
            ResolveName = new global::System.Func<global::System.Type, string>(ResolveNameFromType);
            ResolveType = new global::System.Func<string, global::System.Type>(ResolveTypeFromName);
            OnContextCreated();
            this.Format.LoadServiceModel = LoadModel;
            this.Format.UseJson();
        }

        private IEdmModel LoadModel()
        {
            var xmlTextReader = new XmlTextReader(GetMetadataUri().ToString());
            IEdmModel model = EdmxReader.Parse(xmlTextReader);
            return model;
        }

        void Container_SendingRequest(object sender, System.Data.Services.Client.SendingRequestEventArgs e)
        {
            Console.WriteLine("\t{0} {1}", e.Request.Method, e.Request.RequestUri.ToString());
        }
    }
}
