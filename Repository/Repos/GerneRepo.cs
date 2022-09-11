using Microsoft.EntityFrameworkCore;
using Movie.Database.ApplecationContext;
using Movie.Database.Entity;
using Movie.Repository.InterFace;

namespace Movie.Repository.Repos
{
    public class GerneRepo : IGerneInterface
    {
        private readonly ApplecationDbContext db;

        public GerneRepo(ApplecationDbContext Db )
        {
            db = Db;
        }
        public async Task<Genre> Add(Genre genre)
        {
            await db.AddAsync(genre);
            db.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            db.Remove(genre);
            db.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetALL()
        {
            return await db.Genre.ToListAsync();
        }

        public async Task<Genre> GetById(int Id)
        {
         return  await db.Genre.FirstOrDefaultAsync(x => x.Id == Id); 

        }

        public Task<bool> IsvalidGenre(int id)
        {
            return db.Genre.AnyAsync(x => x.Id == id);
        }

        public Genre Update(Genre genre)
        {
            db.Update(genre);
            db.SaveChanges();
            return genre;


        }
    }
}
