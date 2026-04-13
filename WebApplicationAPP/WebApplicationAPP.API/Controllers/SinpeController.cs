using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;
using WebApplicationAPP.Bussines;

namespace WebApplicationAPP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SinpeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SinpeBusiness _business;

        public SinpeController(AppDbContext context, SinpeBusiness business)
        {
            _context = context;
            _business = business;
        }

        // ============================================
        // 🔥 1. CONSULTAR SINPE
        // ============================================
        [HttpGet("consultar/{telefonoCaja}")]
        public IActionResult Consultar(string telefonoCaja)
        {
            try
            {
                var caja = _context.Cajas
                    .Include(c => c.Comercio)
                    .FirstOrDefault(c => c.Telefono == telefonoCaja);

                if (caja == null)
                {
                    return NotFound(new
                    {
                        EsValido = false,
                        Mensaje = "Caja no encontrada"
                    });
                }

                var config = _context.ConfiguracionComercio
                    .FirstOrDefault(c => c.IdComercio == caja.IdComercio && c.Estado);

                if (config == null)
                {
                    return BadRequest(new
                    {
                        EsValido = false,
                        Mensaje = "El comercio no tiene configuración activa"
                    });
                }

                if (config.TipoConfiguracion != 2 && config.TipoConfiguracion != 3)
                {
                    return BadRequest(new
                    {
                        EsValido = false,
                        Mensaje = "Comercio no autorizado"
                    });
                }

                var sinpes = _context.Sinpes
                    .Where(s => s.IdCaja == caja.IdCaja)
                    .OrderByDescending(s => s.FechaDeRegistro)
                    .Select(s => new
                    {
                        s.IdSinpe,
                        s.TelefonoOrigen,
                        TelefonoDestinatario = s.TelefonoDestino,
                        NombreDestinatario = caja.Comercio.Nombre,
                        s.Monto,
                        s.Descripcion,
                        Fecha = s.FechaDeRegistro,
                        s.Estado
                    })
                    .ToList();

                return Ok(sinpes);
            }
            catch
            {
                return StatusCode(500, new
                {
                    EsValido = false,
                    Mensaje = "Error al consultar SINPE"
                });
            }
        }

        // ============================================
        // 🔥 2. SINCRONIZAR SINPE
        // ============================================
        [HttpPost("sincronizar/{idSinpe}")]
        public IActionResult Sincronizar(int idSinpe)
        {
            try
            {
                var sinpe = _context.Sinpes.FirstOrDefault(s => s.IdSinpe == idSinpe);

                if (sinpe == null)
                {
                    return NotFound(new
                    {
                        EsValido = false,
                        Mensaje = "SINPE no encontrado"
                    });
                }

                sinpe.Estado = true;
                _context.SaveChanges();

                return Ok(new
                {
                    EsValido = true,
                    Mensaje = "SINPE sincronizado correctamente"
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    EsValido = false,
                    Mensaje = "Error al sincronizar SINPE"
                });
            }
        }

        // ============================================
        // 🔥 3. RECIBIR SINPE
        // ============================================
        [HttpPost("recibir")]
        public IActionResult Recibir([FromBody] SinpeRequest request)
        {
            try
            {
                var caja = _context.Cajas
                    .FirstOrDefault(c => c.IdCaja == request.IdCaja && c.Estado);

                if (caja == null)
                {
                    return BadRequest(new
                    {
                        EsValido = false,
                        Mensaje = "Caja no válida"
                    });
                }

                var sinpe = new Sinpe
                {
                    IdCaja = request.IdCaja,
                    TelefonoOrigen = request.TelefonoOrigen,
                    TelefonoDestino = caja.Telefono, // 🔥 FIX REAL
                    Monto = request.Monto,
                    Descripcion = request.Descripcion
                };

                var resultado = _business.Registrar(sinpe);

                if (!resultado)
                {
                    return BadRequest(new
                    {
                        EsValido = false,
                        Mensaje = "No se pudo registrar el SINPE"
                    });
                }

                return Ok(new
                {
                    EsValido = true,
                    Mensaje = "SINPE recibido correctamente"
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    EsValido = false,
                    Mensaje = "Error al recibir SINPE"
                });
            }
        }
    }
}