using System.ComponentModel;

namespace meusite.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        [DisplayName("Platform")]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; }

    }
}
