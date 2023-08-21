namespace TelrPayment.Helpers
{
    public class ValidationResult
    {
        public string? Field { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}