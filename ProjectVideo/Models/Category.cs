using System.ComponentModel.DataAnnotations;

namespace ProjectVideo.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Display Order")]
        [Range(1,int.MaxValue, ErrorMessage = "Display Order for category must be great than 0")]
        public int DisplayOrder { get; set; }
    }
}
