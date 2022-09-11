using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Database.Entity;
using Movie.Models;
using Movie.Repository.InterFace;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGerneInterface gerne;

        public GenreController(IGerneInterface gerne)
        {
            this.gerne = gerne;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await gerne.GetALL();

            return Ok(data);

        }

        [HttpPost("id")]

        public async Task<IActionResult>CreateAsync(GerneNameModel name)
        {
            if (name == null)
                return BadRequest("you have to add name ");
            var data = new Genre { Name = name.Name };

            await gerne.Add(data);
            return Ok(data);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var data = await gerne.GetById(id);

          if(data == null)
                return NotFound($"No genre was found with ID: {id}");

          gerne.Delete(data);

          return Ok(data);

        }

        [HttpPut(template: "{id}")]
        public async Task<IActionResult> UpdateASync(int id, [FromBody] GerneNameModel model) 
        {
            var data = await gerne.GetById(id);
            if (data == null)
                return NotFound($"No genre was found with ID: {id}");

            data.Name = model.Name;
             gerne.Update(data);

            return Ok(data);


        }


    }
}
