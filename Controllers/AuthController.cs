using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using garagebackend.Models;

namespace garagebackend.Controllers
{
    public class AuthController : Controller
    {
        private readonly GarageDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtHandler;

        public AuthController(GarageDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] Credentials model)
        {
            var user = _context.Credentials.SingleOrDefault(c => c.credentials == model.credentials);

            if (user == null || !SecretHasher.VerifySecret(model.secret, user.secret))
            {
                return Unauthorized("We can't seem to find your e-mail or passwords don't match :(");
            }


            var key = Encoding.ASCII.GetBytes(_configuration["JWT_SECRET"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.credentials),
                    new Claim(ClaimTypes.Name, user.credID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = _configuration["JWT_AUDIENCE"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString
            });

        }


    }
}
