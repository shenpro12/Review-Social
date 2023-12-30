using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class CategoryReqModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Slug { get; set; }
    }
}
