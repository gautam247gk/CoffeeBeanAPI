using CoffeeBeanAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CoffeeBeanAPI.Models
{
    public class BeanOfTheDay
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BeanId { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime SelectedDate { get; set; }

        [ForeignKey("BeanId")]
        public Bean Bean { get; set; }
    }

}


