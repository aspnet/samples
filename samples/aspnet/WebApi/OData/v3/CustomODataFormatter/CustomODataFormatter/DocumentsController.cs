using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace CustomODataFormatter
{
    public class DocumentsController : ODataController
    {
        private static Random _random = new Random();
        private ODataValidationSettings _validationSettings = new ODataValidationSettings();

        private List<Document> _documents = new List<Document>
        {
            new Document { ID = 0, Name = "ReadMe.txt", Content = "Lorem ipsum dolor cat amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut " },
            new Document { ID = 1 , Name = "Another.txt", Content = "labore cat dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco " }
        };

        public IEnumerable<Document> GetDocuments(string search)
        {
            // Add a score to each document.
            var results = FindTerm(search);
            return results.Select(d => new Document(d) { Score = _random.NextDouble() });
        }

        public IHttpActionResult PostDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            document.ID = _documents.Count;
            _documents.Add(document);

            return Created(document);
        }

        private IEnumerable<Document> FindTerm(string term)
        {
            foreach (var doc in _documents)
            {
                if (doc.Content.Contains(term))
                {
                    yield return doc;
                }
            }
        }
    }
}
