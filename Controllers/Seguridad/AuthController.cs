using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_intiSoft.Models.Seguridad;
using intiSoft;

namespace api_intiSoft.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ConecDinamicaContext _context;

        public AuthController(IConfiguration config, ConecDinamicaContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            //var hashedPassword = HashHelper.ComputeSha256Hash(user.Password);

            var usuario = _context.SgUsuario
                .FirstOrDefault(u => u.NombreUsuario == user.Username && u.Contrasena == user.Password);

            if (usuario != null)
            {
                var token = GenerateJwtToken(usuario.UsuarioId, usuario.NombreUsuario);
                return Ok(new AuthResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(2) });
            }
            
            return Unauthorized();
        }

        private string GenerateJwtToken(int usuarioId,string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()), // ← CLAVE: Claim con el ID del usuario            
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin") // Se pueden agregar más roles o permisos                    

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
