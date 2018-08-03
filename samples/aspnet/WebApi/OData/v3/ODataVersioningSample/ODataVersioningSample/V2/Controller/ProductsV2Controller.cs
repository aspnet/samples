using AutoMapper;
using AutoMapper.QueryableExtensions;
using ODataVersioningSample.Models;
using ODataVersioningSample.V2.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Data.Entity;
using ODataVersioningSample.V2.Models;
using ODataVersioningSample.Extensions;

namespace ODataVersioningSample.V2.Controller
{
    public class ProductsV2Controller : ODataController
    {
        private ProductRepository _repository = new ProductRepository(new DbProductsContext());

        [EnableQuery]
        public  IQueryable<Product> Get()
        {
            return _repository.Get();
        }

        public IHttpActionResult Get([FromODataUri] long key)
        {
            return Ok(_repository.GetByID(key, Request));
        }

        public IHttpActionResult Post(Product entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _repository.Create(entity) ;
            return Created(product);
        }

        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Product> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Updated(_repository.Patch(key, patch, Request));
        }

        public IHttpActionResult Put([FromODataUri] long key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ID)
            {
                return BadRequest();
            }
            return Updated(_repository.Update(key, update, Request));
        }

        public IHttpActionResult Delete([FromODataUri] long key)
        {
            _repository.Delete(key, Request);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public ProductFamily GetFamily([FromODataUri] long key)
        {
            return _repository.GetFamily(key, Request);
        }

        [AcceptVerbs("POST", "PUT")]
        public IHttpActionResult CreateLink([FromODataUri] long key, string navigationProperty, [FromBody] Uri link)
        {
            _repository.CreateLink(key, navigationProperty, link, Request);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}