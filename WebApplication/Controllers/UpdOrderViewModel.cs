using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApplication.Controllers
{
    public class UpdOrderViewModel : BaseEntity
    {
        [Phone] 
        public string PhoneNumber { get; set; }
        public Painting Painting { get; set; }
        public int Amount { get; set; }
        public int MaxAmount { get; set; }
        public string ShippingAddress { get; set; }
    }
}