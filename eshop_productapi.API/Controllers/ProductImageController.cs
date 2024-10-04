using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using eshop_productapi.Business.Enums.General;
using eshop_productapi.Business.ViewModels.General;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_productapi.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProductImageController : BaseApiController
    {
        private readonly IHtmlLocalizer<ProductImageController> _localizer;
        private readonly IProductImageService _ProductImageService;

        public ProductImageController(IProductImageService ProductImageService, IHtmlLocalizer<ProductImageController> localizer)
        {
            _ProductImageService = ProductImageService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<object> GetAll()
        {
            return await GetDataWithMessage(async () =>
            {
                var result = (await _ProductImageService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = await _ProductImageService.GetAsync(id);
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        public async Task<object> Post([FromBody] ProductImage model)
        {
            return await GetDataWithMessage(async () =>
            {
                if (ModelState.IsValid && model != null)
                {
                    return model.Id <= 0 ? await AddAsync(model) : await UpdateAsync(model);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
                return Response(model, string.Join(",", errors), DropMessageType.Error);
            });
        }

        private async Task<Tuple<ProductImage, string, DropMessageType>> AddAsync(ProductImage model)
        {
            var flag = await _ProductImageService.AddAsync(model);
            if (flag)
            {
                return Response(model, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(model, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<ProductImage, string, DropMessageType>> UpdateAsync(ProductImage model)
        {
            var flag = await _ProductImageService.UpdateAsync(model);
            if (flag)
                return Response(model, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(model, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var flag = await _ProductImageService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
        }
    }
}