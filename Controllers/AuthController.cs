
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository.InterFace;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface authinterface;

        public AuthController(IAuthInterface authinterface )
        {
            this.authinterface = authinterface;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authinterface.RegisterAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TkoenRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authinterface.GetTokenasync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);

        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> Addroleasunc([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authinterface.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);

        }


    }
}
