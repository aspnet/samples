using System.Web.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace DynamicEdmModelCreation.DataSource
{
    internal class AnotherDataSource : IDataSource
    {
        public void GetModel(EdmModel model, EdmEntityContainer container)
        {
            EdmEntityType product = new EdmEntityType("ns", "Student");
            product.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            EdmStructuralProperty key = product.AddStructuralProperty("ID", EdmPrimitiveTypeKind.Int32);
            product.AddKeys(key);
            model.AddElement(product);
            container.AddEntitySet("Students", product);
        }

        public void Get(IEdmEntityTypeReference entityType, EdmEntityObjectCollection collection)
        {
            EdmEntityObject entity = new EdmEntityObject(entityType);
            entity.TrySetPropertyValue("Name", "Foo");
            entity.TrySetPropertyValue("ID", 100);
            collection.Add(entity);
            entity = new EdmEntityObject(entityType);
            entity.TrySetPropertyValue("Name", "Bar");
            entity.TrySetPropertyValue("ID", 101);
            collection.Add(entity);
        }

        public void Get(string key, EdmEntityObject entity)
        {
            entity.TrySetPropertyValue("Name", "Foo");
            entity.TrySetPropertyValue("ID", int.Parse(key));
        }
    }
}
