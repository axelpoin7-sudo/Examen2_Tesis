using System.Text.Json;
using Biblioteca_Examen2.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vista_Examen2.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _http;
        public LoginModel(IHttpClientFactory factory) => _http = factory.CreateClient("Api");

        [BindProperty]
        public LoginDto Credenciales { get; set; } = new();

        public string? MensajeError { get; set; }
        public string? MensajeExito { get; set; }

        public void OnGet(bool logout = false)
        {
            if (logout)
            {
                HttpContext.Session.Clear();
                MensajeExito = "Sesión cerrada correctamente.";
            }
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/login", Credenciales);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var datos = JsonSerializer.Deserialize<LoginRespuestaDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    HttpContext.Session.SetString("UsuarioLogueado", datos?.NombreUsuario ?? Credenciales.NombreUsuario);
                    HttpContext.Session.SetString("JwtToken", datos?.Token ?? string.Empty);

                    return RedirectToPage("/Tesis/Index");
                }

                MensajeError = "Usuario o contraseña incorrectos.";
                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = "Error de conexión con la API: " + ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRegistrarAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/registrar", Credenciales);
                if (response.IsSuccessStatusCode)
                {
                    MensajeExito = $"¡Usuario '{Credenciales.NombreUsuario}' registrado! Ahora inicia sesión.";
                    return Page();
                }

                MensajeError = await response.Content.ReadAsStringAsync();
                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = "Error de conexión: " + ex.Message;
                return Page();
            }
        }
    }
}