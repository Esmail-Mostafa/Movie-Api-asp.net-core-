using Movie.Database.Entity;

namespace Movie.Repository.InterFace
{
    public interface IGerneInterface
    {
        Task<IEnumerable<Genre>> GetALL();
        Task<Genre>GetById(int Id);
        Task<Genre>Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        Task<bool> IsvalidGenre(int id);
    }
}
