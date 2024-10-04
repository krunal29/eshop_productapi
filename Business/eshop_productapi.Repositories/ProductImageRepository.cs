using eshop_productapi.Domain;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Repositories;

namespace eshop_productapi.Repositories
{
    public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(eshop_productapiContext context) : base(context)
        {
        }
    }
}