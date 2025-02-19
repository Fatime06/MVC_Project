using Juan_Mvc_Project.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Models
{
    public class Order :BaseEntity
    {
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser AppUser { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string TownCity { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        [ValidateNever]
        public List<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }
}
