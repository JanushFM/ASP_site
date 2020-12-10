using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        [Phone] public string PhoneNumber { get; set; }
        public string AppUserId { get; set; }
        public int PaintingId { get; set; }
        public Painting Painting { get; set; }
        public int Amount { get; set; }
        public string ShippingAddress { get; set; }
        public bool IsConfirmedByUser { get; set; }  
        public bool IsReviewedBySailor { get; set; }
    }
}