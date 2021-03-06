using MochaCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MochaCoffee.Web.Serialization
{
    public static class ProductMapper
    {
        public static ProductModel SerializeProductModel(Data.Models.Product product)
        {
            return new ProductModel
            {
                Id = product.Id,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                IsTaxable = product.IsTaxable,
                IsArchived = product.IsTaxable,
            };
        }

        public static Data.Models.Product SerializeProductModel(ProductModel product)
        {
            return new Data.Models.Product
            {
                Id = product.Id,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                IsTaxable = product.IsTaxable,
                IsArchived = product.IsTaxable,
            };
        }
    }
}
