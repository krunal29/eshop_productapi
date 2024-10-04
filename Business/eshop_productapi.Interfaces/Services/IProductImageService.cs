using eshop_productapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_productapi.Interfaces.Services
{
    public interface IProductImageService : IBaseService<ProductImage>
    {
        Task<List<ProductImage>> GetAllAsync();

        Task<ProductImage> GetAsync(int id);

        Task<bool> AddAsync(ProductImage model);

        Task<bool> UpdateAsync(ProductImage model);

        Task<bool> DeleteAsync(int id);
    }
}