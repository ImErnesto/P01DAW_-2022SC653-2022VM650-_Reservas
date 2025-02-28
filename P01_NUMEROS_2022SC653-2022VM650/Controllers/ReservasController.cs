using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_NUMEROS_2022SC653_2022VM650.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P01_NUMEROS_2022SC653_2022VM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("ReservarEspacio")]
        public async Task<IActionResult> ReservarEspacio([FromBody] Reserva reserva)
        {
            List<Reserva> reservasExistentes = await _context.Reservas
                .Where(r => r.EspacioId == reserva.EspacioId && r.FechaReserva == reserva.FechaReserva)
                .ToListAsync();

            if (reservasExistentes.Count > 0)
            {
                return BadRequest("El espacio ya está reservado en la fecha y hora especificadas.");
            }

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return Ok(reserva);
        }

        [HttpGet]
        [Route("ReservasActivas")]
        public async Task<IActionResult> ObtenerReservasActivas()
        {
            var reservasActivas = await (from re in _context.Reservas where re.EstadoReserva== "Activa" select re).ToListAsync();

            return Ok(reservasActivas);
        }



        [HttpPut]
        [Route("ActualizarReserva/{reservaId}")]
        public async Task<IActionResult> ActualizarReserva(int reservaId, [FromBody] Reserva reservaActualizada)
        {
            List<Reserva> reservas = await _context.Reservas
                .Where(r => r.ReservaId == reservaId)
                .ToListAsync();

            if (reservas.Count == 0)
            {
                return NotFound("No se encontró la reserva.");
            }

            Reserva reserva = reservas[0];


            reserva.FechaReserva = reservaActualizada.FechaReserva;
            reserva.HoraInicio = reservaActualizada.HoraInicio;
            reserva.CantidadHoras = reservaActualizada.CantidadHoras;
            reserva.EspacioId = reservaActualizada.EspacioId;

            await _context.SaveChangesAsync();

            return Ok("Reserva actualizada con éxito.");
        }



        [HttpPut]
        [Route("CancelarReserva/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            Reserva reserva = await _context.Reservas
                .Where(r => r.ReservaId == reservaId && r.FechaReserva > DateTime.Now)
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                return NotFound("No se encontró la reserva o ya ha pasado la fecha de uso.");
            }

            reserva.EstadoReserva = "Cancelado"; 
            await _context.SaveChangesAsync();
            return Ok("Reserva cancelada con éxito.");
        }

    }
}
