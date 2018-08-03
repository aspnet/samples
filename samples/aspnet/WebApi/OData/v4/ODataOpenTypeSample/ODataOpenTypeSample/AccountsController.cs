using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataOpenTypeSample
{
    public class AccountsController : ODataController
    {
        public AccountsController()
        {
            if (null == Accounts)
            {
                InitAccounts();
            }
        }

        /// <summary>
        /// static so that the data is shared among requests.
        /// </summary>
        public static IList<Account> Accounts = null;

        private static void InitAccounts()
        {
            Accounts = new List<Account>
            {
                new Account()
                {
                    Id = 1,
                    Name = "Name1",
                    Address = new Address()
                    {
                        City = "Redmond",
                        Street = "1 Microsoft Way"
                    },
                },
            };

            Account account = Accounts.Single(a => a.Id == 1);
            account.Address.DynamicProperties.Add("Country", "US");
            account.DynamicProperties.Add("Gender", Gender.Male);
            account.DynamicProperties.Add("Emails", new List<string>() { "a@a.com", "b@b.com" });
        }

        [EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
        public IHttpActionResult Get()
        {
            return Ok(Accounts.AsQueryable());
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            IQueryable<Account> accounts = Accounts.Where(e => e.Id == key).AsQueryable();
            return Ok(SingleResult.Create<Account>(accounts));
        }

        [HttpGet]
        [ODataRoute("Accounts({key})/Address")]
        public IHttpActionResult GetAddress(int key)
        {
            return Ok(Accounts.SingleOrDefault(e => e.Id == key).Address);
        }

        [HttpPut]
        public IHttpActionResult Put(int key, Account account)
        {
            if (key != account.Id)
            {
                return BadRequest("The ID of account is not matched with the key");
            }

            Account originalAccount = Accounts.Where(a => a.Id == account.Id).Single();
            Accounts.Remove(originalAccount);
            Accounts.Add(account);
            return Ok(account);
        }

        [EnableQuery]
        public IHttpActionResult Patch(int key, Delta<Account> patch)
        {
            IEnumerable<Account> appliedAccounts = Accounts.Where(a => a.Id == key);

            if (appliedAccounts.Count() == 0)
            {
                return BadRequest(string.Format("The entry with Id {0} doesn't exist", key));
            }

            Account account = appliedAccounts.Single();
            patch.Patch(account);

            return Ok(account);
        }

        [HttpPost]
        public IHttpActionResult Post(Account account)
        {
            account.Id = Accounts.Count + 1;
            Accounts.Add(account);

            return Created(account);
        }
    }
}
