using Biblioteca_Examen2.DTOs;
using examen2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace examen2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TesisController : ControllerBase
    {
        private readonly ITesisInterface _service;
        public TesisController(ITesisInterface service) => _service = service;

        [HttpGet]
        public IActionResult Get() => Ok(_service.ObtenerTodas());

        [HttpGet("buscar")]
        public IActionResult Buscar([FromQuery] string? filtro) => Ok(_service.Buscar(filtro));

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CrearTesisDto dto, IFormFile? archivoImagen)
        {
            var archivo = archivoImagen ?? Request.Form.Files.FirstOrDefault();
            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "Debe adjuntar una imagen para la tesis." });

            try
            {
                await _service.CrearAsync(dto, archivo);
                return Ok(new { mensaje = "Tesis registrada correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { mensaje = ex.Message }); }
        }
    }
}