namespace Movie.Database.Entity
{
    public class Movies
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StreLine { get; set; }
        public byte[] Poster { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

    }
}
