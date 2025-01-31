using System.ComponentModel.DataAnnotations;

namespace CoffeeBeanAPI.Models
{
    public class Bean
    {
        [Key]
        public Guid Id { get; set; }

        public int IndexNumber { get; set; }

        public bool IsBOTD { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public string ImageUrl { get; set; }

        public string Colour { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }
    }

}


