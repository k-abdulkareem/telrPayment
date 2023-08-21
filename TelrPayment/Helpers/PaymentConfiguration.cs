namespace TelrPayment.Helpers
{
    public static class PaymentConfiguration
    {
        public const string Test = "1"; // 0 is live , 1 is test
        public const string Framed = "2";
        public const string AuthKey = "***********";
        public const string StoreId = "****";
        public const string HttpClientBaseAddress = "https://secure.telr.com/";
        public const string PostUrl = "gateway/order.json";
        public const string AuthorisedUrl = "https://yourwebsite/payment/success";
        public const string DeclinedUrl = "https://yourwebsite/payment/decline";
        public const string CancelledUrl = "https://yourwebsite/payment/canceled";
        public const string Currency = "AED";

        public enum PaymentStatusEnum
        {
            Pending = 1,    // Processing
            Authorized = 2, // Authorised (Transaction not captured, such as an AUTH transaction or a SALE transaction which has been placed on hold)
            Paid = 3,       // 	Paid (Transaction captured, SALE transaction, not placed on hold)
            Expired = -1,   // 	Expired Card
            Cancelled = -2, // 	Cancelled by the client
            Declined = -3, // Declined By Bank
        }

    }
}
