using AutoMapper;
using AutoMapper.QueryableExtensions;
using ODataVersioningSample.Models;
using ODataVersioningSample.V1.ViewModels;
using V2VM = ODataVersioningSample.V2.ViewModels;
using ODataVersioningSample.V2.Controller;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using ODataVersioningSample.V2.Models;

namespace ODataVersioningSample.V1.Controller
{
    public class ProductsV1Controller : ODataController
    {
        private ProductRepository _repository = new ProductRepository(new DbProductsContext());

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return _repository.Get().Project().To<Product>();
        }

        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var v2Product = _repository.GetByID((long)key, Request);
            return Ok(Mapper.Map<Product>(v2Product));
        }

        [HttpPost]
        public IHttpActionResult Post(Product entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var v2Product = _repository.Create(Mapper.Map<V2VM.Product>(entity));
            return Created(Mapper.Map<Product>(v2Product));
        }

        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Product> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Delta<V2VM.Product> v2Patch = new Delta<V2VM.Product>();
            foreach (string name in patch.GetChangedPropertyNames())
            {
                object value;
                if (patch.TryGetPropertyValue(name, out value))
                {
                    v2Patch.TrySetPropertyValue(name, value);
                }
            }
            var v2Product = _repository.Patch((long)key, v2Patch, Request);
            return Updated(Mapper.Map<Product>(v2Product));
        }

        [HttpPut]
        public IHttpActionResult Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var v2Product = _repository.Update((long)key, Mapper.Map<V2VM.Product>(update), Request);
            return Updated(Mapper.Map<Product>(v2Product));
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            _repository.Delete((long)key, Request);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}