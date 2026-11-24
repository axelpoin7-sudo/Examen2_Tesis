using Biblioteca_Examen2.DTOs;

namespace examen2.Interfaces
{
    public interface IAuthInterface
    {
        void Registrar(LoginDto dto);
        LoginRespuestaDto? Login(LoginDto dto);
    }
}