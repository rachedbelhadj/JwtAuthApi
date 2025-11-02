using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        /// <summary>
        /// Endpoint protégé - nécessite un token JWT valide
        /// </summary>
        /// <returns>Message de bienvenue personnalisé</returns>
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                message = $"Bienvenue {username}!",
                username = username,
                userId = userId,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        /// <summary>
        /// Endpoint protégé pour obtenir des données
        /// </summary>
        /// <returns>Liste de données</returns>
        [HttpGet("data")]
        public IActionResult GetData()
        {
            var data = new[]
            {
                new { Id = 1, Name = "Item 1", Description = "Description de l'item 1" },
                new { Id = 2, Name = "Item 2", Description = "Description de l'item 2" },
                new { Id = 3, Name = "Item 3", Description = "Description de l'item 3" }
            };

            return Ok(data);
        }

        /// <summary>
        /// Endpoint qui nécessite le rôle Admin
        /// </summary>
        /// <returns>Données sensibles</returns>
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok(new { message = "Ceci est une ressource protégée pour les admins uniquement" });
        }
    }
}
