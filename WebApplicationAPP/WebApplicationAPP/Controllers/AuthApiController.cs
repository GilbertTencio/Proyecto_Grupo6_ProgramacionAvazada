using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplicationAPP.Data;
using WebApplicationAPP.Services;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthApiController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GenerarToken(ApiTokenRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var comercio = await _context.Comercios
                .Include(c => c.ConfiguracionComercio)
                .FirstOrDefaultAsync(c => c.IdComercio == model.IdComercio);

            if (comercio is null || comercio.ConfiguracionComercio is null)
            {
                return Unauthorized(new { message = "Comercio no autorizado para consumir el API." });
            }

            var tipoConfiguracion = comercio.ConfiguracionComercio.TipoConfiguracion;
            var configuracionValida = comercio.ConfiguracionComercio.Estado
                && (tipoConfiguracion == 2 || tipoConfiguracion == 3);

            if (!configuracionValida)
            {
                return Unauthorized(new { message = "Comercio no autorizado para consumir el API." });
            }

            var token = _jwtTokenService.CrearTokenParaComercio(comercio.IdComercio);

            return Ok(new ApiTokenResponseViewModel
            {
                Token = token.Token,
                ExpiraEn = token.ExpiraEn,
                IdComercio = comercio.IdComercio
            });
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public IActionResult ValidarToken()
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token no proporcionado." });
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();
            var principal = _jwtTokenService.ValidarToken(token);

            if (principal is null)
            {
                return Unauthorized(new { message = "Token invalido o expirado." });
            }

            return Ok(new
            {
                autorizado = true,
                idComercio = principal.FindFirstValue("idComercio")
            });
        }
    }
}
