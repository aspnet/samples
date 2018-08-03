CustomODataFormatterSample
------------------

This sample illustrates how to use extensiblity points in the ODataFormatter and plugin custom OData 
serializers/deserializers. The sample extends the ODataFormatter to add support of OData instance annotations.

The sample models a Search service where you can query a set of documents. You can POST a document to the 'Documents' 
entityset to add it to the index. You can search for documents by sending the search query. The results of the search 
query are documents that matched. Just returning the matched documents is not that useful. Giving a score (rank) for the 
match for each document would be more useful. As the score is dependant on the incoming query, it cannot be modelled as 
a property on the document. It should instead be modelled as an annotation on the document. We use the instance annotation 
"org.northwind.search.score" to capture this.

The sample has a custom entity type serializer, AnnotatingEntitySerializer, that adds the instance annotation to ODataEntry 
by overriding the CreateEntry method. It defines a custom ODataSerializerProvider (CustomODataSerializerProvider) to 
provide AnnotatingEntitySerializer instead of ODataEntityTypeSerializer. It then creates the OData formatters using this 
serializer provider and uses those formatters in the configuration.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487

