using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klassy_Test.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        [NotMapped,Required]
        public IFormFile Photo { get; set; }
    }
}
