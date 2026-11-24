using Bogus;
using examen2.Data;
using examen2.Models;

namespace examen2.Factories
{
    public static class TesisSeeder
    {
        public static void Seed(Examen2DbContext context)
        {
            if (context.Tesis.Any()) return;

            var tutores = context.Tutores.Take(10).ToList();
            if (!tutores.Any()) return;

            var estados = new[] { "Aprobada", "En Revisión", "En Desarrollo" };
            var faker = new Faker("es");
            var listaTesis = new List<Tesis>();

            for (int i = 0; i < 10; i++)
            {
                var tutorAsignado = tutores[i % tutores.Count];

                listaTesis.Add(new Tesis
                {
                    Id = Guid.NewGuid(),
                    Titulo = $"Sistema de {faker.Hacker.Verb()} para {faker.Company.CompanyName()}",
                    Estudiante = faker.Name.FullName(),
                    Estado = faker.PickRandom(estados),
                    Imagen = faker.Image.PicsumUrl(),
                    IdTutor = tutorAsignado.Id
                });
            }

            context.Tesis.AddRange(listaTesis);
            context.SaveChanges();
        }
    }
}