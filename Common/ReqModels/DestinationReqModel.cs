using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class DestinationReqModel
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }
        [MaxLength(12)]
        public string? Phone { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }

        public DateTime? Open { get; set; }
        public DateTime? Closed { get; set; }

        [Required]
        public IFormFile Thumb { get; set; }
        [MaxLength(50)]
        public string? Lat { get; set; }
        [MaxLength(50)]
        public string? Long { get; set; }
        
        [Required]
        public string ProvinceID { get; set; }
    }
}
