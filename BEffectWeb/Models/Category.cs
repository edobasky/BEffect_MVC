using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BEffectWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [DisplayName("Categore Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}
