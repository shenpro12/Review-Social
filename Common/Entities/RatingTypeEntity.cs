using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.Entities
{
    [Table("RatingType")]
    public class RatingTypeEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
    }
}
