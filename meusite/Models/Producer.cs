using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace meusite.Models
{
    public class Producer
    {
        public int ProducerId { get; set; }
        [DisplayName("Producer")]
        public string Name { get; set; }
    }
}
