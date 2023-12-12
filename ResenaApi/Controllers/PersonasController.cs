using Microsoft.AspNetCore.Mvc;
using ResenaApi.Modelos;
using ResenaApi.Services.Interfaces;


namespace ResenaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IServicePersonas _personasService;

        public PersonasController(IServicePersonas personasService)
        {
            _personasService = personasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonas()
        {
            try
            {
                var personas = await _personasService.ObtenerTodasPersonas();

                if (personas != null && personas.Any())
                {
                    return Ok(personas);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las personas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonaById(string id)
        {
            try
            {
                var persona = await _personasService.ObtenerPersonaPorId(id);

                if (persona != null)
                {
                    return Ok(persona);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener la persona por ID: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePersona(Personas persona)
        {
            try
            {
                var nuevaPersona = await _personasService.CrearPersona(persona);
                return CreatedAtAction(nameof(GetPersonaById), new { id = nuevaPersona.id }, nuevaPersona);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la persona: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersona(string id, Personas updatedPersona)
        {
            try
            {
                var resultado = await _personasService.ActualizarPersona(id, updatedPersona);

                if (resultado)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la persona: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(string id)
        {
            try
            {
                var resultado = await _personasService.EliminarPersona(id);

                if (resultado)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la persona: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                // Validar que se proporcionen el email y el password en la solicitud
                if (string.IsNullOrEmpty(loginRequest.email) || string.IsNullOrEmpty(loginRequest.password))
                {
                    return BadRequest("El email y el password son obligatorios.");
                }

                // Lógica de autenticación
                var persona = await _personasService.LoginPersona(loginRequest.email, loginRequest.password);

                if (persona != null)
                {
                    // Aquí podrías generar un token JWT u otro mecanismo de autenticación
                    // y devolverlo en la respuesta si la autenticación es exitosa.

                    // Ejemplo con un objeto anónimo que contiene información del usuario autenticado
                    var usuarioAutenticado = new
                    {
                        Id = persona.id,
                        Email = persona.email,
                        // Otras propiedades que desees devolver
                    };

                    return Ok(usuarioAutenticado);
                }

                return Unauthorized("Credenciales inválidas");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al iniciar sesión: {ex.Message}");
            }
        }




    }
}