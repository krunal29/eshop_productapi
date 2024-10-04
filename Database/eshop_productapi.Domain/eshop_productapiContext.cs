using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eshop_productapi.Domain.Models;

namespace eshop_productapi.Domain
{
    public partial class eshop_productapiContext : IdentityDbContext<ApplicationUser>
    {
        public eshop_productapiContext(DbContextOptions<eshop_productapiContext> options) : base(options)
        {
        }


        public virtual DbSet<Product>Product{ get; set; }

public virtual DbSet<ProductImage>ProductImage{ get; set; }

    }
}