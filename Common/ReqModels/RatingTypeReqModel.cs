using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class RatingTypeReqModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
