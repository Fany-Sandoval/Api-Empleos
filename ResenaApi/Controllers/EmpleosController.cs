using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResenaApi.Modelos;
using ResenaApi.Services;
using ResenaApi.Services.Interfaces;

namespace ResenaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleosController : ControllerBase
    {
        private readonly IServiceEmpleo _empleosService;

        public EmpleosController(IServiceEmpleo empleosService)
        {
            _empleosService = empleosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleos()
        {
            try
            {
                var empleos = await _empleosService.ObtenerTodosEmpleos();

                if (empleos != null && empleos.Any())
                {
                    return Ok(empleos);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener empleos: {ex.Message}");
            }
        }

        [HttpGet("{id_empleo}")]
        public async Task<IActionResult> GetEmpleoById(string id_empleo)
        {
            try
            {
                var empleos = await _empleosService.ObtenerEmpleoPorId(id_empleo);

                if (empleos != null)
                {
                    return Ok(empleos);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el empleo por ID: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmpleo(Empleos empleos)
        {
            try
            {
                var nuevaEmpleo = await _empleosService.CrearEmpleo(empleos);
                return CreatedAtAction(nameof(GetEmpleoById), new { id_empleo = nuevaEmpleo.id_empleo }, nuevaEmpleo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear empleo: {ex.Message}");
            }
        }

        [HttpPut("{id_empleo}")]
        public async Task<IActionResult> UpdateEmpleo(string id_empleo, Empleos updatedEmpleo)
        {
            try
            {
                var resultado = await _empleosService.ActualizarEmpleo(id_empleo, updatedEmpleo);

                if (resultado)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar empleo: {ex.Message}");
            }
        }

        [HttpDelete("{id_empleo}")]
        public async Task<IActionResult> DeleteEmpleo(string id_empleo)
        {
            try
            {
                var resultado = await _empleosService.EliminarEmpleo(id_empleo);

                if (resultado)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar empleo: {ex.Message}");
            }
        }
    }
}
