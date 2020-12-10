using System.Collections.Generic;
using Domain.Entities;

namespace WebApplication.ViewModels
{
    public class ShoppingListViewModel
    {
        public List<Order> Orders { get; set; }
        public int TotalPrice { get; set; }
        public bool IsUnconfOrdersAvailable { get; set; }
    }
}