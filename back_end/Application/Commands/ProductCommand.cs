using back_end.Application.Interfaces;
using back_end.Domain;
using Microsoft.AspNetCore.Http;

namespace back_end.Application.Commands
{
    public class ProductCommand
    {
        private readonly IProductHandler productHandler;

        public ProductCommand(IProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }

        public async Task<bool> CreateProduct(ProductModel product, HttpRequest request)
        {
            ValidateProduct(product);
            await ProcessProductImage(product, request);
            return productHandler.CreateProduct(product);
        }

        private void ValidateProduct(ProductModel product)
        {
            if (product == null)
            {
                throw new ArgumentException("Datos de producto invalidos.");
            }
        }

        private async Task ProcessProductImage(ProductModel product, HttpRequest request)
        {
            if (request.Form.Files.Count > 0)
            {
                var file = request.Form.Files[0];

                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        product.ProductImage = memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
