using AutoMapper;
using AutoMapper.QueryableExtensions;
using ODataVersioningSample.Models;
using ODataVersioningSample.V2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace ODataVersioningSample.V2.Controller
{
    public class ProductFamiliesV2Controller : ODataController
    {
        private DbProductsContext _db = new DbProductsContext();

        public IQueryable<ProductFamily> Get()
        {
            return _db.ProductFamilies.Project().To<ProductFamily>();
        }

        public IHttpActionResult Get([FromODataUri] int key)
        {
            var dbProductFamily = _db.ProductFamilies.Find(key);
            if (dbProductFamily == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Ok(Mapper.Map<ProductFamily>(dbProductFamily));
        }
    }
}
