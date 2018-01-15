using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : ApiController
    {
        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = new Product(id);
            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            product.Save();
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }
    }
}