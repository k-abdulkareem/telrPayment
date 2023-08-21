namespace TelrPayment.Models.Telr.CheckPayment.Response
{
    public class CheckPaymentResponse
    {
        public string Method { get; set; }
        public Order Order { get; set; }
        public Error Error { get; set; }
    }
    public class Order
    {
        public string Ref { get; set; }
        public Status Status { get; set; }
    }
    public class Status
    {
        public string Code { get; set; }
        public string Text { get; set; }
    }
    public class Error
    {
        public string Message { get; set; }
        public string Note { get; set; }
    }
}