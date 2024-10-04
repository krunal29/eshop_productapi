using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_productapi.Domain.Models
{
    [Table("Product")]
    public partial class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}