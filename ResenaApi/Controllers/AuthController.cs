using Microsoft.AspNetCore.Mvc;
using ResenaApi.Modelos;
using ResenaApi.Services;

namespace ResenaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceAuth _authService;

        public AuthController(IServiceAuth authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UsuariosDto usuariosDto)
        {
            try
            {
                var uid = await _authService.Register(usuariosDto.auth, usuariosDto.personas);
                return Ok(uid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Auth auth)
        {
            try
            {
                var localId = await _authService.Login(auth);
                return Ok(localId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var result = await _authService.ForgotPassword(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("suspend-user")]
        public async Task<IActionResult> SuspendUser(string uid)
        {
            try
            {
                var result = await _authService.SuspendUser(uid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string uid)
        {
            try
            {
                var result = await _authService.Eliminar(uid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("habilitar-user")]
        public async Task<IActionResult> HabilitarUser(string uid)
        {
            try
            {
                var result = await _authService.HabilitarUser(uid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}