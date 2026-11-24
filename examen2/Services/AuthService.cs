using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Biblioteca_Examen2.DTOs;
using examen2.Data;
using examen2.Interfaces;
using examen2.Models;
using Microsoft.IdentityModel.Tokens;

namespace examen2.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly Examen2DbContext _context;
        private readonly IConfiguration _config;

        public AuthService(Examen2DbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public void Registrar(LoginDto dto)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario.ToLower() == dto.NombreUsuario.Trim().ToLower()))
                throw new Exception("El usuario ya existe.");

            _context.Usuarios.Add(new Usuario
            {
                Id = Guid.NewGuid(),
                NombreUsuario = dto.NombreUsuario.Trim(),
                PasswordHash = Encriptar(dto.Password)
            });
            _context.SaveChanges();
        }

        public LoginRespuestaDto? Login(LoginDto dto)
        {
            var hash = Encriptar(dto.Password);
            var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == dto.NombreUsuario.Trim() && u.PasswordHash == hash);
            if (user == null) return null;

            return new LoginRespuestaDto
            {
                NombreUsuario = user.NombreUsuario,
                Mensaje = "Inicio de sesión exitoso",
                Token = GenerarToken(user)
            };
        }

        private string GenerarToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NombreUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string Encriptar(string texto)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(texto)));
        }
    }
}