using System.Web.Http.OData;
using System.Web.Http.OData.Formatter.Serialization;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace CustomODataFormatter
{
    // A custom serializer provider to inject the AnnotatingEntitySerializer.
    public class CustomODataSerializerProvider : DefaultODataSerializerProvider
    {
        private AnnotatingEntitySerializer _annotatingEntitySerializer;

        public CustomODataSerializerProvider()
        {
            _annotatingEntitySerializer = new AnnotatingEntitySerializer(this);
        }

        public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsEntity())
            {
                return _annotatingEntitySerializer;
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }


    // A custom entity serializer that adds the score annotation to document entries.
    public class AnnotatingEntitySerializer : ODataEntityTypeSerializer
    {
        public AnnotatingEntitySerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        {
        }

        public override ODataEntry CreateEntry(SelectExpandNode selectExpandNode, EntityInstanceContext entityInstanceContext)
        {
            ODataEntry entry = base.CreateEntry(selectExpandNode, entityInstanceContext);

            Document document = entityInstanceContext.EntityInstance as Document;
            if (entry != null && document != null)
            {
                // annotate the document with the score.
                entry.InstanceAnnotations.Add(new ODataInstanceAnnotation("org.northwind.search.score", new ODataPrimitiveValue(document.Score)));
            }

            return entry;
        }
    }
}
