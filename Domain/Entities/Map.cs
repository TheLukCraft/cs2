using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Maps")]
    public class Map
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string? Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public Map()
        {
        }

        public Map(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}