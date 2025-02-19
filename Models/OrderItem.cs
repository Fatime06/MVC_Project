using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }
        public int Count { get; set; }

    }
}
