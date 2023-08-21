namespace TelrPayment.Models.Telr.AddPayment.Response
{
    public class AddPaymentResponse
    {
        public string Method { get; set; }
        public Order Order { get; set; }
        public Error Error { get; set; }
    }
    public class Order
    {
        public string Ref { get; set; }
        public string Url { get; set; }
    }
    public class Error
    {
        public string Message { get; set; }
        public string Note { get; set; }
    }
}
