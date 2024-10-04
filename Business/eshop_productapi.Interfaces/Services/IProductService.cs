using eshop_productapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_productapi.Interfaces.Services
{
    public interface IProductService : IBaseService<Product>
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetAsync(int id);

        Task<bool> AddAsync(Product model);

        Task<bool> UpdateAsync(Product model);

        Task<bool> DeleteAsync(int id);
    }
}