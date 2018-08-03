using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UploadXDocumentSample.Models;

namespace UploadXDocumentSample.Controllers
{
    public class BookController : ApiController
    {
        private static object _thisLock = new object();
        private static List<Book> _books = new List<Book>
        {
            new Book 
            {
                 Author = "aaa",
                 Description = "ddd",
                 Genre = "ggg",
                 Price = 10.55,
                 PublishDate = new DateTime(2012, 10, 2),
                 Title = "ttt"
            }
        };

        public List<Book> Get()
        {
            lock (_thisLock)
            {
                return _books.ToList();
            }
        }

        public List<Book> Post(List<Book> books)
        {
            lock (_thisLock)
            {
                _books.AddRange(books);
                return _books.ToList();
            }
        }
    }
}
