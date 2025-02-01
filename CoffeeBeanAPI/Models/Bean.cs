using System.ComponentModel.DataAnnotations;

namespace CoffeeBeanAPI.Models
{
    public class Bean
    {
        [Key]
        public Guid Id { get; set; }

        public string _id { get; set; }

        public int index { get; set; }

        public bool isBOTD { get; set; }

        [Required]
        public string Cost { get; set; }

        public string Image { get; set; }

        public string colour { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }
    }

}


