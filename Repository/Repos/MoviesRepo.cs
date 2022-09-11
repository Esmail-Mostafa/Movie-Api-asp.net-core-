using Microsoft.EntityFrameworkCore;
using Movie.Database.ApplecationContext;
using Movie.Database.Entity;
using Movie.Models;
using Movie.Repository.InterFace;

namespace Movie.Repository.Repos
{
    public class MoviesRepo : IMoviesInterface
    {
        private readonly ApplecationDbContext db;

        public MoviesRepo(ApplecationDbContext db )
        {
            this.db = db;
        }
        public async Task<Movies> Add(Movies movie)
        {
          await  db.Movies.AddAsync(movie);
            db.SaveChanges();
            return (movie);
        }

        public Movies Delete(Movies movie)
        {
            db.Remove(movie);
            db.SaveChanges();
            return movie;


        }

        public async Task<IEnumerable<Movies>> GetALL(int gerneId = 0)
        {
                     return await db.Movies
                     .Where(m=>m.GenreId == gerneId || gerneId == 0) 
                    .Include(m => m.Genre)
                    .ToListAsync();

        }

        public async Task<Movies> GetById(int Id)
        {
            return await db.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == Id);
        }

        public Movies Update(Movies movie)
        {
           db.Update(movie);
            db.SaveChanges();

            return movie;
        }
    }
}
