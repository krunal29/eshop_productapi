using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using eshop_productapi.Domain;

namespace eshop_productapi.UoW
{
    public class UnitOfWorkHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UnitOfWork GetUnitOfWork()
        {
            var optionBuilder = new DbContextOptionsBuilder<eshop_productapiContext>();
            optionBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            var _context = new eshop_productapiContext(optionBuilder.Options);
            return new UnitOfWork(_context);
        }
    }
}
