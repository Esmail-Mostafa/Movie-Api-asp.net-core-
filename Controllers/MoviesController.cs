using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Database.ApplecationContext;
using Movie.Database.Entity;
using Movie.Models;
using Movie.Repository.InterFace;
using System.IO;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
    
        private readonly List<string> AllowExtentions = new List<string> { ".jpg", ".png" };
        private readonly IMoviesInterface moviesinterface;
        private readonly IMapper mapper;
        private readonly IGerneInterface gerneInterface;
        private long _MaxAllowPosterSize = 3145728;
        public MoviesController(IMoviesInterface moviesinterface , IMapper mapper , IGerneInterface gerneInterface)
        {
            this.moviesinterface = moviesinterface;
            this.mapper = mapper;
            this.gerneInterface = gerneInterface;
        }


        [HttpGet]
        public async Task<IActionResult> GatAllAsync(int gerneId = 0)
        {
            var data = await moviesinterface.GetALL();
            var movies = mapper.Map<IEnumerable<MoviesDetalisDto>>(data);
            return Ok(movies);    


        }

        [HttpGet(template:"{id}")]
        public async Task<IActionResult> GeyByIdAsync(int id)
        {
            var movie = await moviesinterface.GetById(id);
            if (movie == null)
                return NotFound();
            var data = mapper.Map<MoviesDetalisDto>(movie);

            return Ok(data);
        }
        [HttpGet("GetByGerneId")]
        public async Task<IActionResult> GetByGerneIdAsync(int genreId)
        {

            var data = await moviesinterface.GetALL(genreId);
            var movies = mapper.Map<IEnumerable<MoviesDetalisDto>>(data);
            return Ok(movies);
        }


        [HttpPost]
        public async Task<IActionResult>CreateAsync([FromForm] MoviesDto model)
        {
            if (model.Poster == null)
                return BadRequest("poster is required");
            if (!AllowExtentions.Contains(Path.GetExtension(model.Poster.FileName).ToLower()))
                return BadRequest("only jpg anf png imges are allowed");
            if (model.Poster.Length > _MaxAllowPosterSize)
                return BadRequest("MAx allowed length for poster is 3MB");

            var isvalidgernes = await gerneInterface.IsvalidGenre(model.GenreId);
            if (!isvalidgernes)
                return BadRequest("InValid Id");

            using var datastream = new MemoryStream();

            await model.Poster.CopyToAsync(datastream);

            var data = mapper.Map<Movies>(model);
            data.Poster = datastream.ToArray();

            await moviesinterface.Add(data);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsynce(int id)
        {
            var data = await moviesinterface.GetById(id);
            if (data == null)
                return NotFound("no movies was found whis this id");
            moviesinterface.Delete(data);
            return Ok(data);


        }
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAsynce(int id , [FromForm] MoviesDto model)
        {

            var movie = await moviesinterface.GetById(id);
            if(movie == null)
                return BadRequest("no movies is not found");
           
            var isVAleidganraID = await gerneInterface.IsvalidGenre(model.GenreId);
            if (!isVAleidganraID)
                return BadRequest("invalid genere id");
            if (model.Poster != null)
            {
                if (!AllowExtentions.Contains(Path.GetExtension(model.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (model.Poster.Length > _MaxAllowPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await model.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }
            movie.Title = model.Title;
            movie.StreLine =model.Storeline;
            movie.Rate = model.Rate;
            movie.Year = model.Year;
            movie.GenreId = model.GenreId;

            moviesinterface.Update(movie);

            return Ok(movie);
        }
    }
}
