using Biblioteca_Examen2.DTOs;
using examen2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace examen2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TutoresController : ControllerBase
    {
        private readonly ITutorInterface _service;
        public TutoresController(ITutorInterface service) => _service = service;

        [HttpGet]
        public IActionResult Get() => Ok(_service.ObtenerTodos());

        [HttpPost]
        public IActionResult Post([FromBody] TutorDto dto)
        {
            try
            {
                _service.Crear(dto);
                return Ok(new { mensaje = "Tutor registrado correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { mensaje = ex.Message }); }
        }
    }
}