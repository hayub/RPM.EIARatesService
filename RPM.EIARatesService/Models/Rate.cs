using System;

namespace RPM.EIARatesService.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FormattedDate { get; set; }
        public decimal Price { get; set; }
    }
}
