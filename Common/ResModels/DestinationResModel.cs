using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.ResModels
{
    public class DestinationResModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public DateTime? Open { get; set; }
        public DateTime? Closed { get; set; }
        public string? Thumb { get; set; }
        public string? Lat { get; set; }
        public string? Long { get; set; }
        public int IsAdmin { get; set; }
        public string ProvinceID { get; set; }
        public string UserID { get; set; }
    }
}
