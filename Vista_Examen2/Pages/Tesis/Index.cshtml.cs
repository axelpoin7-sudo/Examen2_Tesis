using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Biblioteca_Examen2.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vista_Examen2.Pages.Tesis
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public IndexModel(IHttpClientFactory factory, IConfiguration config)
        {
            _http = factory.CreateClient("Api");
            _config = config;
        }

        public List<TesisDto> TesisLista { get; set; } = new();
        public List<TutorDto> Tutores { get; set; } = new();
        public string UrlBaseApi { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        [BindProperty]
        public CrearTesisDto NuevaTesis { get; set; } = new();

        [BindProperty]
        public IFormFile? ArchivoImagen { get; set; }

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            UrlBaseApi = (_config["ApiSettings:BaseUrl"] ?? "https://localhost:7211/").TrimEnd('/');
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string endpoint = string.IsNullOrWhiteSpace(Filtro)
                ? "api/Tesis"
                : $"api/Tesis/buscar?filtro={Uri.EscapeDataString(Filtro)}";

            var resTesis = await _http.GetAsync(endpoint);
            if (resTesis.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Auth/Login");
            }

            if (resTesis.IsSuccessStatusCode)
                TesisLista = await resTesis.Content.ReadFromJsonAsync<List<TesisDto>>() ?? new();

            Tutores = await _http.GetFromJsonAsync<List<TutorDto>>("api/Tutores") ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            if (ArchivoImagen == null || ArchivoImagen.Length == 0)
            {
                MensajeError = "Debe seleccionar una imagen para la portada de la tesis.";
                return await OnGetAsync();
            }

            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(NuevaTesis.Titulo ?? ""), "Titulo");
            form.Add(new StringContent(NuevaTesis.Estudiante ?? ""), "Estudiante");
            form.Add(new StringContent(NuevaTesis.Estado ?? "En Proceso"), "Estado");
            form.Add(new StringContent(NuevaTesis.NombreTutor ?? ""), "NombreTutor");

            var fileContent = new StreamContent(ArchivoImagen.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(ArchivoImagen.ContentType);
            form.Add(fileContent, "archivoImagen", ArchivoImagen.FileName);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsync("api/Tesis", form);

            if (response.IsSuccessStatusCode)
                return RedirectToPage();

            var body = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(body);
                MensajeError = doc.RootElement.GetProperty("mensaje").GetString();
            }
            catch
            {
                MensajeError = body;
            }

            return await OnGetAsync();
        }
    }
}