using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthApi.Models;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un JWT token
        /// </summary>
        /// <param name="loginModel">Informations de connexion</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Validation simple (à remplacer par une vraie vérification en base de données)
            if (ValidateUser(loginModel.Username, loginModel.Password))
            {
                var token = GenerateJwtToken(loginModel.Username);
                return Ok(token);
            }

            return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect" });
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur (endpoint de démonstration)
        /// </summary>
        /// <param name="loginModel">Informations de l'utilisateur</param>
        /// <returns>Message de confirmation</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginModel loginModel)
        {
            // Logique d'enregistrement simple (à remplacer par une vraie logique)
            if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Nom d'utilisateur et mot de passe requis" });
            }

            // Ici, vous devriez sauvegarder l'utilisateur dans une base de données
            // avec un mot de passe haché (BCrypt, PBKDF2, etc.)

            return Ok(new { message = "Utilisateur enregistré avec succès" });
        }

        private bool ValidateUser(string username, string password)
        {
            // Validation simple pour la démonstration
            // À remplacer par une vraie vérification en base de données
            // avec des mots de passe hachés
            return username == "admin" && password == "password";
        }

        private TokenResponse GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "VotreCleSecreteTresLongueEtSecurisee123456!";
            var issuer = jwtSettings["Issuer"] ?? "JwtAuthApi";
            var audience = jwtSettings["Audience"] ?? "JwtAuthApiClients";
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse
            {
                Token = tokenString,
                Expiration = expiration
            };
        }
    }
}
