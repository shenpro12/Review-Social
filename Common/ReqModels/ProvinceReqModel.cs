using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class ProvinceReqModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; }
        [Required]
        public IFormFile CategoryThumb { get; set; }
    }
}
