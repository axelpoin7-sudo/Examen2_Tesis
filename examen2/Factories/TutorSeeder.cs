using Bogus;
using examen2.Data;
using examen2.Models;

namespace examen2.Factories
{
    public static class TutorSeeder
    {
        public static void Seed(Examen2DbContext context)
        {
            if (context.Tutores.Any()) return;

            var especialidades = new[]
            {
                "Ingeniería de Software",
                "Inteligencia Artificial",
                "Redes y Ciberseguridad",
                "Base de Datos",
                "Sistemas Embebidos"
            };

            var faker = new Faker<Tutor>("es")
                .RuleFor(t => t.Id, _ => Guid.NewGuid())
                .RuleFor(t => t.Nombre, f => $"Ing. {f.Name.FirstName()} {f.Name.LastName()}")
                .RuleFor(t => t.Especialidad, f => f.PickRandom(especialidades))
                .RuleFor(t => t.Correo, (f, t) => f.Internet.Email(t.Nombre));

            context.Tutores.AddRange(faker.Generate(10));
            context.SaveChanges();
        }
    }
}
