namespace Biblioteca_Examen2.DTOs
{
    public class LoginRespuestaDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}