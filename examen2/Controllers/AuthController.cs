using Biblioteca_Examen2.DTOs;
using examen2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace examen2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _auth;
        public AuthController(IAuthInterface auth) => _auth = auth;

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] LoginDto dto)
        {
            try
            {
                _auth.Registrar(dto);
                return Ok(new { mensaje = "Usuario registrado" });
            }
            catch (Exception ex) { return BadRequest(new { mensaje = ex.Message }); }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var res = _auth.Login(dto);
            return res == null ? Unauthorized(new { mensaje = "Credenciales incorrectas" }) : Ok(res);
        }
    }
}