using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;

namespace ODataContrainmentSample
{
    public class AccountsController : ODataController
    {
        private static IList<Account> _accounts = null;
        public AccountsController()
        {
            if (_accounts == null)
            {
                _accounts = InitAccounts();
            }
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_accounts.AsQueryable());
        }

        [EnableQuery]
        public IHttpActionResult GetPayinPIs(int key)
        {
            var payinPIs = _accounts.Single(a => a.AccountID == key).PayinPIs;
            return Ok(payinPIs);
        }

        [EnableQuery]
        [ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        public IHttpActionResult GetSinglePayinPI(int accountId, int paymentInstrumentId)
        {
            var payinPIs = _accounts.Single(a => a.AccountID == accountId).PayinPIs;
            var payinPI = payinPIs.Single(pi => pi.PaymentInstrumentID == paymentInstrumentId);
            return Ok(payinPI);
        }

        [EnableQuery]
        public IHttpActionResult GetPayoutPI(int key, int piKey)
        {
            var payoutPI = _accounts.Single(a => a.AccountID == key).PayoutPI;
            return Ok(payoutPI);
        }

        // POST ~/Accounts(100)/PayinPIs
        public IHttpActionResult PostToPayinPIsFromAccount(int key, PaymentInstrument pi)
        {
            var account = _accounts.Single(a => a.AccountID == key);
            pi.PaymentInstrumentID = account.PayinPIs.Max(p => p.PaymentInstrumentID) + 1;
            account.PayinPIs.Add(pi);
            return Created(pi);
        }

        // PUT ~/Accounts(100)/PayoutPI
        [ODataRoute("Accounts({accountId})/PayoutPI")]
        public IHttpActionResult PutToPayoutPIFromAccount(int accountId, [FromBody]PaymentInstrument paymentInstrument)
        {
            var account = _accounts.Single(a => a.AccountID == accountId);
            account.PayoutPI = paymentInstrument;
            return Ok(paymentInstrument);
        }

        // PUT ~/Accounts(100)/PayinPIs(101)
        [ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        public IHttpActionResult PutToPayinPI(int accountId, int paymentInstrumentId, [FromBody]PaymentInstrument paymentInstrument)
        {
            var account = _accounts.Single(a => a.AccountID == accountId);
            var originalPi = account.PayinPIs.Single(p => p.PaymentInstrumentID == paymentInstrumentId);
            originalPi.FriendlyName = paymentInstrument.FriendlyName;
            return Ok(paymentInstrument);
        }

        // DELETE ~/Accounts(100)/PayinPIs(101)
        [ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        public IHttpActionResult DeletePayinPIFromAccount(int accountId, int paymentInstrumentId)
        {
            var account = _accounts.Single(a => a.AccountID == accountId);
            var originalPi = account.PayinPIs.Single(p => p.PaymentInstrumentID == paymentInstrumentId);
            if (account.PayinPIs.Remove(originalPi))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE ~/Accounts(100)/PayinPIs(101)
        [ODataRoute("Accounts({accountId})/PayoutPI")]
        public IHttpActionResult DeletePayoutPIFromAccount(int accountId)
        {
            var account = _accounts.Single(a => a.AccountID == accountId);
            account.PayoutPI = null;
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET ~/Accounts(100)/PayinPIs/Namespace.GetCount)
        [ODataRoute("Accounts({accountId})/PayinPIs/ODataContrainmentSample.GetCount(NameContains={name})")]
        public IHttpActionResult GetPayinPIsCountWhoseNameContainsGivenValue(int accountId, [FromODataUri]string name)
        {
            var account = _accounts.Single(a => a.AccountID == accountId);
            var count = account.PayinPIs.Where(pi => pi.FriendlyName.Contains(name)).Count();

            return Ok(count);
        }

        [ODataRoute("ResetDataSource")]
        public IHttpActionResult ResetDataSource()
        {
            _accounts = InitAccounts();
            return StatusCode(HttpStatusCode.NoContent);
        }

        private string GetServiceRootUri()
        {
            var routeName = Request.ODataProperties().RouteName;
            ODataRoute odataRoute = Configuration.Routes[routeName] as ODataRoute;
            var prefixName = odataRoute.RoutePrefix;
            var requestUri = Request.RequestUri.ToString();
            var serviceRootUri = requestUri.Substring(0, requestUri.IndexOf(prefixName) + prefixName.Length);
            return serviceRootUri;
        }

        private static IList<Account> InitAccounts()
        {
            var accounts = new List<Account>()
            {
                new Account()
                {
                   AccountID = 100,
                   Name="Name100",
                   PayoutPI = new PaymentInstrument()
                   {
                       PaymentInstrumentID = 100,
                       FriendlyName = "Payout PI: Paypal",
                   },
                    PayinPIs = new List<PaymentInstrument>()
                    {
                        new PaymentInstrument()
                        {
                            PaymentInstrumentID = 101,
                            FriendlyName = "101 first PI",
                        },
                        new PaymentInstrument()
                        {
                            PaymentInstrumentID = 102,
                            FriendlyName = "102 second PI",
                        },
                    },
                },
            };
            return accounts;
        }
    }
}
