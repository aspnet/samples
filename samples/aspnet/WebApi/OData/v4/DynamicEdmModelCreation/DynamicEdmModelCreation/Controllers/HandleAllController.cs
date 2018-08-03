using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using DynamicEdmModelCreation.DataSource;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace DynamicEdmModelCreation.Controllers
{
    public class HandleAllController : ODataController
    {
        // Get entityset
        public EdmEntityObjectCollection Get()
        {
            // Get entity set's EDM type: A collection type.
            ODataPath path = Request.ODataProperties().Path;
            IEdmCollectionType collectionType = (IEdmCollectionType)path.EdmType;
            IEdmEntityTypeReference entityType = collectionType.ElementType.AsEntity();

            // Create an untyped collection with the EDM collection type.
            EdmEntityObjectCollection collection =
                new EdmEntityObjectCollection(new EdmCollectionTypeReference(collectionType));

            // Add untyped objects to collection.
            DataSourceProvider.Get((string)Request.Properties[Constants.ODataDataSource], entityType, collection);

            return collection;
        }

        // Get entityset(key)
        public IEdmEntityObject Get(string key)
        {
            // Get entity type from path.
            ODataPath path = Request.ODataProperties().Path;
            IEdmEntityType entityType = (IEdmEntityType)path.EdmType;

            // Create an untyped entity object with the entity type.
            EdmEntityObject entity = new EdmEntityObject(entityType);

            DataSourceProvider.Get((string)Request.Properties[Constants.ODataDataSource], key, entity);

            return entity;
        }
    }
}
