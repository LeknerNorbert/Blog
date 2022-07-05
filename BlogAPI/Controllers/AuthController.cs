using BlogAPI.Models;
using BlogAPI.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([FromBody] AccountDto account)
        {
            Account registeredAccount = new Account();
            // Alapból mindenki "User" jogosultságot kap regisztráláskor
            Role? defaultRole = await _db.Roles.FindAsync(2);
            bool usernameExist = _db.Accounts.Any(a => a.Username == account.Username);

            if (!ModelState.IsValid)
            {
                return BadRequest("Account validation failed");
            }

            if (usernameExist)
            {
                return BadRequest("Already someone registered with this username");
            }

            CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);

            registeredAccount.Username = account.Username;
            registeredAccount.PasswordHash = passwordHash;
            registeredAccount.PasswordSalt = passwordSalt;
            registeredAccount.Role = defaultRole;

            await _db.Accounts.AddAsync(registeredAccount);
            await _db.SaveChangesAsync();

            return Ok("Successful registration");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AccountDto account)
        {
            Account? loggedAccount = _db.Accounts
                .Include(a => a.Role)
                .FirstOrDefault(a => a.Username == account.Username);

            if (loggedAccount == null)
            {
                return BadRequest("There is no user with this username");
            }

            if (!VerifyPasswordHash(account.Password, loggedAccount.PasswordHash, loggedAccount.PasswordSalt))
            {
                return BadRequest("Password is incorrect");
            }

            string token = CreateToken(loggedAccount);

            return Ok(token);
        }

        private string CreateToken(Account loggedAccount)
        {
            // Az adatok a felhasználóról, amik szerepelnek majd a visszaküldött tokenben
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loggedAccount.Id.ToString()),
                new Claim(ClaimTypes.Name, loggedAccount.Username),
                new Claim(ClaimTypes.Role, loggedAccount.Role.Name)
            };

            // Kulcs, amiből később generáljuk az aláírást
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value
                ));

            // Aláírás
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // Kell majd írni egy jelszó validálás függvényt is
    }
}
