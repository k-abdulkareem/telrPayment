using System.ComponentModel.DataAnnotations.Schema;

namespace TelrPayment.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public string City { get; set; }
        public string Address { get; set; }
    }
}
