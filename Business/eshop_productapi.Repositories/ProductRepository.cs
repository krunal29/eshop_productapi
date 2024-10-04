using eshop_productapi.Domain;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Repositories;

namespace eshop_productapi.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(eshop_productapiContext context) : base(context)
        {
        }
    }
}