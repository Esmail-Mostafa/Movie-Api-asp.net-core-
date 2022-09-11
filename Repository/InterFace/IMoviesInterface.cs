using Movie.Database.Entity;

namespace Movie.Repository.InterFace
{
    public interface IMoviesInterface
    {
        Task<IEnumerable<Movies>> GetALL(int gerneId = 0);
        Task<Movies> GetById(int Id);
        Task<Movies> Add(Movies movie);
        Movies Update(Movies movie);
        Movies Delete(Movies movie);
    }
}
