using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace meusite.Models
{
    public class Item
    {
        [ScaffoldColumn(false)]
        public int ItemId { get; set; }
        [DisplayName("Platform")]
        public int CategoryId { get; set; }
        [DisplayName("Producer")]
        public int ProducerId { get; set; }
        [Required(ErrorMessage = "An Item title is required")]
        [StringLength(160)]
        [DisplayName("Game Name")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, 1000000, ErrorMessage = "price Must be between 0.1 and 100")]
        public decimal Price { get; set; }
        [DisplayName("Item Art Url")]
        [StringLength(1024)]
        public string ItemArtUrl { get; set; }
        public Category Category { get; set; }
        public Producer Producer { get; set; }
    }
}
