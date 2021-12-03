using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Models
{
    [Table("Wishers")]
    public class Wisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WisherId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Wishes Made")] 
        public ICollection<Wish> WishesMade { get; set; }
    }
}
