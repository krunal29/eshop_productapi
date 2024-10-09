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
    public class ProductController : BaseApiController
    {
        private readonly IHtmlLocalizer<ProductController> _localizer;
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService, IHtmlLocalizer<ProductController> localizer)
        {
            _ProductService = ProductService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<object> GetAll()
        {
            return await GetDataWithMessage(async () =>
            {
                var result = (await _ProductService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = await _ProductService.GetAsync(id);
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        public async Task<object> Post([FromBody] Product model)
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

        private async Task<Tuple<Product, string, DropMessageType>> AddAsync(Product model)
        {
            var flag = await _ProductService.AddAsync(model);
            if (flag)
            {
                return Response(model, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(model, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<Product, string, DropMessageType>> UpdateAsync(Product model)
        {
            var flag = await _ProductService.UpdateAsync(model);
            if (flag)
                return Response(model, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(model, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var flag = await _ProductService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
        }

        [HttpGet]
        public async Task GetProducts(string gradeLetter)
        {
            switch (gradeLetter)
            {
                case "A+":

                case "A": 




                default:
                    Console.WriteLine("Invalid grade letter!");
                    break;










                case "A-":
                    Console.WriteLine("Excellent");
                    break;
                case "B+":










                case "B":
                    Console.WriteLine("Very Good");
                    break;
                case "B-":
                case "C+":
                    Console.WriteLine("Good");
                    break;
                case "C":
                    Console.WriteLine("Pass");
                    break;
                case "F":
                    Console.WriteLine("Fail");
                    break;                
            }
        }
    }
}