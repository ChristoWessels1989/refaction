using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{
    [RoutePrefix("ProductOptions")]
    public class ProductOptionsController : ApiController
    {
        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }
    }
}