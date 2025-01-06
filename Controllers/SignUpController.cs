using garagebackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace garagebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {
        private readonly GarageDbContext _context;

        public SignUpController(GarageDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> NewUser([FromBody] SignUp model)
        {
            if (model == null)
            {
                return BadRequest("Oopsie! Bad request");
            }

            if (model.secret != model.ConfirmPassword)
            {
                return BadRequest("Ai caramba! The passwords don't match");
            }

            string hashedSecret = SecretHasher.HashSecret(model.secret);

            var user = new Credentials
            {
                credentials = model.credentials,
                secret = hashedSecret,
                createdAt = DateTime.UtcNow
            };

            _context.Credentials.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Hooray! Welcome to the community!" });
        }
    }
}
