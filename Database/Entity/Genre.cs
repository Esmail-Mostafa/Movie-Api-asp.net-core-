

using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Database.Entity
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
         
    }
}
