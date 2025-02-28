using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_NUMEROS_2022SC653_2022VM650.Models;

namespace P01_NUMEROS_2022SC653_2022VM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParqueoController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ParqueoController(AppDbContext context)
        {
            _context = context;
        }

        // Registrar un nuevo espacio de parqueo
        [HttpPost]
        [Route("RegistrarEspacio")]
        public IActionResult RegistrarEspacio([FromBody] EspaciosDeParqueo espacio)
        {
            _context.EspaciosDeParqueos.Add(espacio);
            _context.SaveChanges();
            return Ok(espacio);
        }

        // Mostrar lista de espacios disponibles para reservar por día
        [HttpGet]
        [Route("EspaciosDisponibles")]
        public IActionResult EspaciosDisponibles()
        {
            var espaciosDisponibles = _context.EspaciosDeParqueos
                .Where(e => e.Estado == "Disponible")
                .ToList();

            return espaciosDisponibles.Any() ? Ok(espaciosDisponibles) : NotFound("No hay espacios disponibles.");
        }

        //Actualizar información de un espacio de parqueo
        [HttpPut]
        [Route("ActualizarEspacio/{id}")]
        public IActionResult ActualizarEspacio(int id, [FromBody] EspaciosDeParqueo espacioActualizado)
        {
            var espacio = _context.EspaciosDeParqueos.Find(id);
            if (espacio == null)
            {
                return NotFound("Espacio de parqueo no encontrado.");
            }

            espacio.Numero = espacioActualizado.Numero;
            espacio.Ubicacion = espacioActualizado.Ubicacion;
            espacio.CostoPorHora = espacioActualizado.CostoPorHora;
            espacio.Estado = espacioActualizado.Estado;

            _context.SaveChanges();
            return Ok(espacio);
        }

        // Eliminar un espacio de parqueo
        [HttpDelete]
        [Route("EliminarEspacio/{id}")]
        public IActionResult EliminarEspacio(int id)
        {
            var espacio = _context.EspaciosDeParqueos.Find(id);
            if (espacio == null)
            {
                return NotFound("Espacio de parqueo no encontrado.");
            }

            _context.EspaciosDeParqueos.Remove(espacio);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("EspaciosReservadosPorDia/{fecha}")]
        public IActionResult EspaciosReservadosPorDia(DateTime fecha)
        {
            var reservas = _context.Reservas
                .Where(r => r.FechaReserva.Date == fecha.Date && r.EstadoReserva == "Activa")
                .Join(_context.EspaciosDeParqueos,
                    reserva => reserva.EspacioId,
                    espacio => espacio.EspacioId,
                    (reserva, espacio) => new
                    {
                        reserva.ReservaId,
                        reserva.EspacioId,
                        Numero = espacio.Numero,
                        Ubicacion = espacio.Ubicacion,
                        CostoPorHora = espacio.CostoPorHora,
                        SucursalId = espacio.SucursalId,
                        Estado = espacio.Estado,
                        reserva.UsuarioId,
                        reserva.HoraInicio,
                        reserva.CantidadHoras,
                        reserva.EstadoReserva
                    })
                .Join(_context.Sucursales,
                    espacioReserva => espacioReserva.SucursalId,
                    sucursal => sucursal.SucursalId,
                    (espacioReserva, sucursal) => new
                    {
                        sucursal.SucursalId,
                        sucursal.NombreSucursal,
                        espacioReserva.ReservaId,
                        espacioReserva.EspacioId,
                        espacioReserva.Numero,
                        espacioReserva.Ubicacion,
                        espacioReserva.CostoPorHora,
                        espacioReserva.Estado,
                        espacioReserva.UsuarioId,
                        espacioReserva.HoraInicio,
                        espacioReserva.CantidadHoras,
                        espacioReserva.EstadoReserva
                    })
                .GroupBy(r => new { r.SucursalId, r.NombreSucursal })
                .Select(grupo => new
                {
                    SucursalId = grupo.Key.SucursalId,
                    NombreSucursal = grupo.Key.NombreSucursal,
                    EspaciosReservados = grupo.ToList()
                })
                .ToList();

            return reservas.Any() ? Ok(reservas) : NotFound("No hay espacios reservados en esta fecha.");
        }

        // Mostrar una lista de los espacios reservados entre dos fechas de una sucursal específica
        [HttpGet]
        [Route("EspaciosReservadosEntreFechas/{sucursalId}/{fechaInicio}/{fechaFin}")]
        public IActionResult EspaciosReservadosEntreFechas(int sucursalId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservas = _context.Reservas
                .Where(r => r.EspacioId != null &&
                            r.FechaReserva.Date >= fechaInicio.Date &&
                            r.FechaReserva.Date <= fechaFin.Date &&
                            r.EstadoReserva == "Activa")
                .Join(_context.EspaciosDeParqueos,
                    reserva => reserva.EspacioId,
                    espacio => espacio.EspacioId,
                    (reserva, espacio) => new
                    {
                        reserva.ReservaId,
                        reserva.EspacioId,
                        Numero = espacio.Numero,
                        Ubicacion = espacio.Ubicacion,
                        CostoPorHora = espacio.CostoPorHora,
                        Estado = espacio.Estado,
                        SucursalId = espacio.SucursalId,
                        reserva.UsuarioId,
                        reserva.HoraInicio,
                        reserva.CantidadHoras,
                        reserva.EstadoReserva
                    })
                .Where(r => r.SucursalId == sucursalId)
                .Join(_context.Sucursales,
                    espacioReserva => espacioReserva.SucursalId,
                    sucursal => sucursal.SucursalId,
                    (espacioReserva, sucursal) => new
                    {
                        sucursal.SucursalId,
                        sucursal.NombreSucursal,
                        espacioReserva.ReservaId,
                        espacioReserva.EspacioId,
                        espacioReserva.Numero,
                        espacioReserva.Ubicacion,
                        espacioReserva.CostoPorHora,
                        espacioReserva.Estado,
                        espacioReserva.UsuarioId,
                        espacioReserva.HoraInicio,
                        espacioReserva.CantidadHoras,
                        espacioReserva.EstadoReserva
                    })
                .GroupBy(r => new { r.SucursalId, r.NombreSucursal })
                .Select(grupo => new
                {
                    SucursalId = grupo.Key.SucursalId,
                    NombreSucursal = grupo.Key.NombreSucursal,
                    EspaciosReservados = grupo.ToList()
                })
                .ToList();

            return reservas.Any() ? Ok(reservas) : NotFound("No hay reservas en este rango de fechas para esta sucursal.");
        }



    }
}
