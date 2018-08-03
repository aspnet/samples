using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using ODataCompositeKeySample.Extensions;
using ODataCompositeKeySample.Models;

namespace ODataCompositeKeySample.Controllers
{
    [ModelValidationFilter]
    public class PeopleController : ODataController
    {
        private PeopleRepository _repo = new PeopleRepository(); 

        [EnableQuery]
        public IEnumerable<Person> Get()
        {
            return _repo.Get();
        }

        [EnableQuery]
        public Person Get([FromODataUri] string firstName, [FromODataUri] string lastName)
        {
            Person person = _repo.Get(firstName, lastName);
            if (person == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return person;
        }

        public IHttpActionResult PutPerson([FromODataUri] string firstName, [FromODataUri] string lastName, Person person)
        {
            _repo.UpdateOrAdd(person);

            return Updated(person);
        }

        [AcceptVerbs("PATCH")]
        public IHttpActionResult PatchPerson([FromODataUri] string firstName, [FromODataUri] string lastName, Delta<Person> delta)
        {
            var person = _repo.Get(firstName, lastName);
            if (person == null)
            {
                return NotFound();
            }

            delta.Patch(person);

            person.FirstName = firstName;
            person.LastName = lastName;
            _repo.UpdateOrAdd(person);

            return Updated(person);
        }

        public IHttpActionResult PostPerson(Person person)
        {
            _repo.UpdateOrAdd(person);
            return Created(person);
        }

        public IHttpActionResult DeletePerson([FromODataUri] string firstName, [FromODataUri] string lastName)
        {
            var person = _repo.Remove(firstName, lastName);
            if (person == null)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
