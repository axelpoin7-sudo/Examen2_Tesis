using System.Security.Cryptography;
using System.Text;
using examen2.Data;
using examen2.Models;

namespace examen2.Factories
{
    public static class UsuarioSeeder
    {
        public static void Seed(Examen2DbContext context)
        {
            if (context.Usuarios.Any()) return;

            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("admin123")));

            context.Usuarios.Add(new Usuario
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "admin",
                PasswordHash = hash
            });
            context.SaveChanges();
        }
    }
}