using AutoMapper;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Services;
using eshop_productapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_productapi.Services
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var result = mapper.Map<List<Product>>(await unitOfWork.ProductRepository.GetAllAsync());
            return result.ToList();
        }

        public async Task<Product> GetAsync(int id)
        {
            return mapper.Map<Product>(await unitOfWork.ProductRepository.GetAsync(id));
        }

        public async Task<bool> AddAsync(Product model)
        {
            await unitOfWork.ProductRepository.AddAsync(model);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Product model)
        {
            var data = await unitOfWork.ProductRepository.GetAsync(model.Id);
            if (data != null)
            {
                data.Id = model.Id;
                //MAP other fields
                await unitOfWork.ProductRepository.UpdateAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = unitOfWork.ProductRepository.GetAsync(id).Result;
            if (data != null)
            {
                await unitOfWork.ProductRepository.DeleteAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}