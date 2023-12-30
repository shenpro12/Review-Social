using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.Entities
{
    [Table("Province")]
    public class ProvinceEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Slug { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string CategoryThumb { get; set; }

        public ICollection<ProvinceCategoryEntity> ProvinceCategorys { get; set; }

        public ICollection<DestinationEntity> DestinationEntitys { get; set; }
    }
}
