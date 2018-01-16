using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Route]
        [HttpGet]
      public Products GetAll()
      {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          Products products = new Products();
          if (products.Items.Count == 0)
          {
            string message = "No products found";
            throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
          }
          return products;
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          Products products = new Products(name);
          if (products.Items.Count == 0)
          {
            string message = "No products found";
            throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
          }
          return products;
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          var product = new Product(id);
            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          product.Save();
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

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
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          var product = new Product(id);
          product.Delete();
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          ProductOptions productOptions = new ProductOptions(productId);
          if (productOptions.Items.Count == 0)
          {
            string message = "No product options found";
            throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
          }
          return productOptions;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

            var option = new ProductOption(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);
           
          option.ProductId = productId;
          option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          var orig = new ProductOption(id)
          {
              Name = option.Name,
              Description = option.Description
          };

          if (!orig.IsNew)
              orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
          if (!ModelState.IsValid)
            throw new HttpResponseException(HttpStatusCode.BadRequest);

          var opt = new ProductOption(id);
          opt.Delete();
        }
    }
}
