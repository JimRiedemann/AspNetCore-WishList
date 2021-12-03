using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Models
{
    [Table("Wishes")]
    public class Wish
    {
        #region Properties

        [Required]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string Description { get; set; }

        [Display(Name = "Wish Maker")]
        public Wisher Wisher { get; set; }

        [Required]
        public int WisherId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishId { get; set; }

        [Required]
        [Display(Name = "Wish Order")]
        public int WishOrder { get; set; }

        #endregion Properties
    }
}
