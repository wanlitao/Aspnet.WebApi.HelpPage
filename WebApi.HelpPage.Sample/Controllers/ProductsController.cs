using System.Collections.Generic;
using System.Web.Http;
using WebApi.HelpPage.Sample.Models;
using System.Linq;

namespace WebApi.HelpPage.Sample.Controllers
{
    /// <summary>
    /// 产品接口
    /// </summary>
    public class ProductsController : ApiController
    {
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return new List<Product> 
            {
                new Product() { Id = 1, Name = "Gizmo 1", Price = 1.99M },
                new Product() { Id = 2, Name = "Gizmo 2", Price = 2.99M },
                new Product() { Id = 3, Name = "Gizmo 3", Price = 3.99M }
            };
        }

        /// <summary>
        /// 根据Id获取产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(int id)
        {
            if (id < 1 || id > 3)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return new Product()
            {
                Id = id,
                Name = "Gizmo " + id.ToString(),
                Price = id + 0.99M
            };
        }

        /// <summary>
        /// 产品查询列表
        /// </summary>
        /// <param name="queryProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Product> GetProducts([FromBody]ProductQuery queryProduct)
        {
            var resultProducts = GetAllProducts();

            if (queryProduct != null)
            {
                if (queryProduct.Id.HasValue)
                {
                    resultProducts = resultProducts.Where(m => m.Id == queryProduct.Id.Value);
                }
                if (!string.IsNullOrEmpty(queryProduct.Name))
                {
                    resultProducts = resultProducts.Where(m => m.Name.Contains(queryProduct.Name));
                }
                if (queryProduct.MinPrice.HasValue)
                {
                    resultProducts = resultProducts.Where(m => m.Price >= queryProduct.MinPrice.Value);
                }
                if (queryProduct.MaxPrice.HasValue)
                {
                    resultProducts = resultProducts.Where(m => m.Price <= queryProduct.MaxPrice.Value);
                }
            }

            return resultProducts.ToList();
        }
    }
}
