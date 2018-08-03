using System.Web.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace DynamicEdmModelCreation.DataSource
{
    internal interface IDataSource
    {
        void GetModel(EdmModel model, EdmEntityContainer container);

        void Get(IEdmEntityTypeReference entityType, EdmEntityObjectCollection collection);

        void Get(string key, EdmEntityObject entity);
    }
}
