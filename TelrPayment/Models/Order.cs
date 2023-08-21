using System.ComponentModel.DataAnnotations.Schema;

namespace TelrPayment.Models
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        //Payment Fields
        public int? PaymentStatus { get; set; } 
        public string? PaymentOrderRef { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
