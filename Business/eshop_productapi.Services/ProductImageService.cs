using AutoMapper;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Services;
using eshop_productapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_productapi.Services
{
    public class ProductImageService : ServiceBase, IProductImageService
    {
        public ProductImageService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<ProductImage>> GetAllAsync()
        {
            var result = mapper.Map<List<ProductImage>>(await unitOfWork.ProductImageRepository.GetAllAsync());
            return result.ToList();
        }

        public async Task<ProductImage> GetAsync(int id)
        {
            return mapper.Map<ProductImage>(await unitOfWork.ProductImageRepository.GetAsync(id));
        }

        public async Task<bool> AddAsync(ProductImage model)
        {
            await unitOfWork.ProductImageRepository.AddAsync(model);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(ProductImage model)
        {
            var data = await unitOfWork.ProductImageRepository.GetAsync(model.Id);
            if (data != null)
            {
                data.Id = model.Id;
                //MAP other fields
                await unitOfWork.ProductImageRepository.UpdateAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = unitOfWork.ProductImageRepository.GetAsync(id).Result;
            if (data != null)
            {
                await unitOfWork.ProductImageRepository.DeleteAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}