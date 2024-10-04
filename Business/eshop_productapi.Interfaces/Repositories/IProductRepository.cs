using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Repository;

namespace eshop_productapi.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
    }
}