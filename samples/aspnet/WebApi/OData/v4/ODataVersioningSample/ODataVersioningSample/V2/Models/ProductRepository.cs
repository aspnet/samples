using ODataVersioningSample.Models;
using ODataVersioningSample.V2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.OData;
using System.Data.Entity;
using ODataVersioningSample.Extensions;

namespace ODataVersioningSample.V2.Models
{
    public class ProductRepository
    {
        private DbProductsContext _db;

        public ProductRepository(DbProductsContext db)
        {
            _db = db;
        }

        public IQueryable<Product> Get()
        {
            return _db.Products.Project().To<Product>();
        }

        public Product GetByID(long id, HttpRequestMessage request)
        {
            var dbProduct = _db.Products.Find(id);
            if (dbProduct == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Mapper.Map<Product>(dbProduct);
        }

        public Product Create(Product product)
        {
            var dbProduct = Mapper.Map<DbProduct>(product);
            _db.Products.Add(dbProduct);
            _db.SaveChanges();

            return Mapper.Map<Product>(dbProduct);
        }

        public Product Patch(long id, Delta<Product> patch, HttpRequestMessage request)
        {
            var dbProduct = _db.Products.Find(id);
            if (dbProduct == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }

            var product = Mapper.Map<Product>(dbProduct);
            patch.Patch(product);
            if (product.ID != id)
            {
                throw new HttpResponseException(request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Changing key property is not allowed for PATCH method."));
            }

            dbProduct = Mapper.Map(product, dbProduct);
            _db.Entry(dbProduct).State = EntityState.Modified;
            _db.SaveChanges();

            return product;
        }

        public Product Update(long id, Product product, HttpRequestMessage request)
        {
            if (id != product.ID)
            {
                throw new HttpResponseException(request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Changing key property is not allowed for PUT method."));
            }

            var dbProduct = _db.Products.Find(id);
            if (dbProduct == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }

            dbProduct = Mapper.Map<DbProduct>(product);
            _db.Products.Attach(dbProduct);
            _db.Entry(dbProduct).State = EntityState.Modified;
            _db.SaveChanges();

            return product;
        }

        public void Delete(long id, HttpRequestMessage request)
        {
            var dbProduct = _db.Products.Find(id);
            if (dbProduct == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }
            _db.Products.Remove(dbProduct);
            _db.SaveChanges();
        }

        public ProductFamily GetFamily(long id, HttpRequestMessage request)
        {
            var dbProduct = _db.Products.Find(id);
            if (dbProduct == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }

            _db.Entry(dbProduct).Reference(p => p.Family).Load();

            return Mapper.Map<ProductFamily>(dbProduct.Family);
        }

        public void CreateLink(long id, string navigationProperty, Uri link, HttpRequestMessage request)
        {
            DbProduct dbProduct = _db.Products.Find(id);

            switch (navigationProperty)
            {
                case "Family":
                    // The utility method uses routing (ODataRoutes.GetById should match) to get the value of {id} parameter 
                    // which is the id of the ProductFamily.
                    int relatedKey = request.GetKeyValue<int>(link);
                    DbProductFamily dbFamily = _db.ProductFamilies.Find(relatedKey);
                    dbProduct.Family = dbFamily;
                    break;

                default:
                    throw new NotSupportedException("The property is not supported by creating link.");
            }
            _db.SaveChanges();
        }
    }
}