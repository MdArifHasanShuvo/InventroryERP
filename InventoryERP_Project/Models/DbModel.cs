using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryERP_Project.Models
{
    public class ProductType
    {
        public ProductType()
        {
            this.Categories = new List<Category>();
        }
        public int Id { get; set; }
        [Required, StringLength(40), ]
        public string Name { get; set; }
        [Required, StringLength(150),]
        public string Description { get; set; }
        [StringLength(32)]
        public string _Key { get; set; }
        [Column(TypeName ="tinyint")]
        public int Is_Deleted { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }
        public int Id { get; set; }
        [Required, StringLength(40),]
        public string Name { get; set; }
        [Required, StringLength(150),]
        public string Description { get; set; }
        [StringLength(32)]
        public string _Key { get; set; }
        [Column(TypeName = "tinyint")]
        public int Is_Deleted { get; set; }
        [Required]
        public int Product_Type_Id { get; set; }
        [ForeignKey("Product_Type_Id")]
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(40),]
        public string Name { get; set; }
        [Required, StringLength(150),]
        public string Description { get; set; }
        [StringLength(32)]
        public string _Key { get; set; }
        [ Column(TypeName = "tinyint")]
        public int Is_Deleted { get; set; }
        [Required]
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public virtual Category Category { get; set; }
    }
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
