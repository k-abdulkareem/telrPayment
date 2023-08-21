namespace TelrPayment.Models.Telr.CheckPayment.Request
{
    public class CheckPaymentRequest
    {
        public CheckPaymentRequest()
        {
            Order = new Order();
        }
        public string Method { get; set; } // set it to 'check' value

        public string Store { get; set; }

        public string AuthKey { get; set; }

        public Order Order { get; set; }
    }
    public class Order
    {
        public string Ref { get; set; }
    }
}
