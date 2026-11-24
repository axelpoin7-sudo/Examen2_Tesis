using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Biblioteca_Examen2.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vista_Examen2.Pages.Tutores
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _http;
        public IndexModel(IHttpClientFactory factory) => _http = factory.CreateClient("Api");

        public List<TutorDto> Tutores { get; set; } = new();

        [BindProperty]
        public TutorDto NuevoTutor { get; set; } = new();

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync("api/Tutores");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Auth/Login");
            }

            if (response.IsSuccessStatusCode)
                Tutores = await response.Content.ReadFromJsonAsync<List<TutorDto>>() ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            if (!ModelState.IsValid)
                return await OnGetAsync();

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync("api/Tutores", NuevoTutor);

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