using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace review.Common.ResModels
{
    public class ProvinceCategoryResModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string ProvinceID { get; set; }
    }
    public class ProvinceCategoryGroupResModel
    {
        
        public string CategoryThumb { get; set; }
        public string CategorySlug { get; set; }
        public IEnumerable<ProvinceCategoryResModel> Items { get; set; }
    }

}
