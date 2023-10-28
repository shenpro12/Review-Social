using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.Entities
{
    [Table("Destination")]
    public class DestinationEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string? Phone { get; set; }

        public float? MinPrice { get; set; }

        public float? MaxPrice { get; set; }

        public DateTime? Open { get; set; }

        public DateTime? Closed { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Thumb { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Lat { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string? Long { get; set; }
        public int IsAdmin { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ProvinceID { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string UserID { get; set; }
        public ProfileEntity Profile { get; set; }

        public ProvinceEntity Province { get; set; }

    }
}
