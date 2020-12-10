using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApplication.ViewModels
{
    public class ShoppingListViewModel
    {
        public List<Order> Orders { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:#,###.00}", ApplyFormatInEditMode = true)]
        public int TotalPrice { get; set; }
    }
}