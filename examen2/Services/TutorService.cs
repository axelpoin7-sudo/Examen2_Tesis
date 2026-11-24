using Biblioteca_Examen2.DTOs;
using examen2.Data;
using examen2.Interfaces;
using examen2.Models;

namespace examen2.Services
{
    public class TutorService : ITutorInterface
    {
        private readonly Examen2DbContext _context;
        public TutorService(Examen2DbContext context) => _context = context;

        public List<TutorDto> ObtenerTodos() =>
            _context.Tutores.Select(t => new TutorDto
            {
                Nombre = t.Nombre,
                Especialidad = t.Especialidad,
                Correo = t.Correo
            }).ToList();

        public void Crear(TutorDto dto)
        {
            var nombreLimpio = dto.Nombre.Trim();
            var correoLimpio = dto.Correo.Trim().ToLower();

            bool existe = _context.Tutores.Any(t =>
                t.Nombre.ToLower() == nombreLimpio.ToLower() ||
                t.Correo.ToLower() == correoLimpio);

            if (existe)
                throw new Exception("Ya existe un tutor registrado con ese nombre o correo electrónico.");

            _context.Tutores.Add(new Tutor
            {
                Id = Guid.NewGuid(),
                Nombre = nombreLimpio,
                Especialidad = dto.Especialidad.Trim(),
                Correo = dto.Correo.Trim()
            });
            _context.SaveChanges();
        }
    }
}