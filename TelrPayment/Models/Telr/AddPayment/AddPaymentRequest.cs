namespace TelrPayment.Models.Telr.AddPayment.Request
{
    public class AddPaymentRequest
    {
        public string Method { get; set; } // set it to 'create' value
        public string Store { get; set; }
        public string Authkey { get; set; }
        public string Framed { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public Return Return { get; set; }

        public AddPaymentRequest()
        {
            Order = new Order();
            Customer = new Customer();
            Return = new Return();
        }
    }

    public class Order
    {
        public string CartId { get; set; }
        public string Description { get; set; }
        public string Test { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            Address = new Address();
        }
        public string Ref { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Return
    {
        public string authorised { get; set; }
        public string declined { get; set; }
        public string cancelled { get; set; }
    }
}
