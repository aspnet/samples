using System.Web.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace DynamicEdmModelCreation.DataSource
{
    internal class MyDataSource : IDataSource
    {
        public void GetModel(EdmModel model, EdmEntityContainer container)
        {
            EdmEntityType product = new EdmEntityType("ns", "Product");
            product.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            EdmStructuralProperty key = product.AddStructuralProperty("ID", EdmPrimitiveTypeKind.Int32);
            product.AddKeys(key);
            model.AddElement(product);
            container.AddEntitySet("Products", product);
        }
        
        public void Get(IEdmEntityTypeReference entityType, EdmEntityObjectCollection collection)
        {
            EdmEntityObject entity = new EdmEntityObject(entityType);
            entity.TrySetPropertyValue("Name", "abc");
            entity.TrySetPropertyValue("ID", 1);
            collection.Add(entity);
            entity = new EdmEntityObject(entityType);
            entity.TrySetPropertyValue("Name", "def");
            entity.TrySetPropertyValue("ID", 2);
            collection.Add(entity);
        }

        public void Get(string key, EdmEntityObject entity)
        {
            entity.TrySetPropertyValue("Name", "abc");
            entity.TrySetPropertyValue("ID", int.Parse(key));
        }
    }
}
