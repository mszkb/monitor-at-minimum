namespace Mam.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Status { get; set; }
        public long LastChecked { get; set; }
        public int Ping { get; set; }
        public string? Type { get; set; }
    }
}