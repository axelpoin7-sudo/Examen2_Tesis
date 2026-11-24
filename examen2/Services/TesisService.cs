using Biblioteca_Examen2.DTOs;
using examen2.Data;
using examen2.Interfaces;
using examen2.Models;
using Microsoft.EntityFrameworkCore;

namespace examen2.Services
{
    public class TesisService : ITesisInterface
    {
        private readonly Examen2DbContext _context;
        private readonly IWebHostEnvironment _env;

        public TesisService(Examen2DbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<TesisDto> ObtenerTodas() => Buscar(null);

        public List<TesisDto> Buscar(string? filtro)
        {
            var query = _context.Tesis.Include(t => t.Tutor).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var term = filtro.Trim().ToLower();
                query = query.Where(t =>
                    t.Titulo.ToLower().Contains(term) ||
                    t.Estudiante.ToLower().Contains(term) ||
                    (t.Tutor != null && t.Tutor.Nombre.ToLower().Contains(term)));
            }

            return query.Select(t => new TesisDto
            {
                Titulo = t.Titulo,
                Estudiante = t.Estudiante,
                Estado = t.Estado,
                Imagen = t.Imagen,
                TutorNombre = t.Tutor != null ? t.Tutor.Nombre : "",
                TutorEspecialidad = t.Tutor != null ? t.Tutor.Especialidad : "",
                TutorCorreo = t.Tutor != null ? t.Tutor.Correo : ""
            }).ToList();
        }

        public async Task CrearAsync(CrearTesisDto dto, IFormFile archivoImagen)
        {
            var tituloLimpio = dto.Titulo.Trim();


            bool tesisDuplicada = _context.Tesis.Any(t => t.Titulo.ToLower() == tituloLimpio.ToLower());
            if (tesisDuplicada)
                throw new Exception("Ya existe una tesis registrada con ese mismo título.");


            var tutor = _context.Tutores
                .Include(t => t.Tesis)
                .FirstOrDefault(t => t.Nombre.ToLower() == dto.NombreTutor.Trim().ToLower());

            if (tutor == null)
                throw new Exception("El tutor seleccionado no existe.");

            if (tutor.Tesis.Count >= 3)
                throw new Exception($"El tutor '{tutor.Nombre}' ya alcanzó el límite máximo de 3 tesis asignadas.");

            string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string carpetaDestino = Path.Combine(webRoot, "imagenes");
            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            string nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(archivoImagen.FileName)}";
            string rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivoImagen.CopyToAsync(stream);
            }

            _context.Tesis.Add(new Tesis
            {
                Id = Guid.NewGuid(),
                Titulo = tituloLimpio,
                Estudiante = dto.Estudiante.Trim(),
                Estado = dto.Estado.Trim(),
                Imagen = $"/imagenes/{nombreArchivo}",
                IdTutor = tutor.Id
            });
            await _context.SaveChangesAsync();
        }
    }
}