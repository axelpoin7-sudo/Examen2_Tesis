using examen2.Models;
using Microsoft.EntityFrameworkCore;

namespace examen2.Data
{
    public class Examen2DbContext : DbContext
    {
        public Examen2DbContext(DbContextOptions<Examen2DbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Tesis> Tesis { get; set; }
    }
}