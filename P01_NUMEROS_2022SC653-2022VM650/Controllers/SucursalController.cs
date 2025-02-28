using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_NUMEROS_2022SC653_2022VM650.Models;

namespace P01_NUMEROS_2022SC653_2022VM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SucursalController(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas las sucursales
        [HttpGet]
        [Route("ListarSucursales")]
        public IActionResult ListarSucursales()
        {
            var sucursales = _context.Sucursales.ToList();
            return sucursales.Any() ? Ok(sucursales) : NotFound();
        }

        // Obtener una sucursal por ID
        [HttpGet]
        [Route("ObtenerSucursal/{id}")]
        public IActionResult ObtenerSucursal(int id)
        {
            var sucursal = _context.Sucursales.Find(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return Ok(sucursal);
        }

        //  Agregar nueva sucursal
        [HttpPost]
        [Route("AgregarSucursal")]
        public IActionResult AgregarSucursal([FromBody] Sucursale sucursal)
        {
            _context.Sucursales.Add(sucursal);
            _context.SaveChanges();
            return Ok(sucursal);
        }

        // Actualizar una sucursal
        [HttpPut]
        [Route("ActualizarSucursal/{id}")]
        public IActionResult ActualizarSucursal(int id, [FromBody] Sucursale sucursalActualizada)
        {
            var sucursal = _context.Sucursales.Find(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            sucursal.NombreSucursal = sucursalActualizada.NombreSucursal;
            sucursal.Direccion = sucursalActualizada.Direccion;
            sucursal.TelefonoSucursal = sucursalActualizada.TelefonoSucursal;
            sucursal.Administrador = sucursalActualizada.Administrador;
            sucursal.NumeroEspacios = sucursalActualizada.NumeroEspacios;

            _context.SaveChanges();
            return Ok(sucursal);
        }

        // Eliminar una sucursal
        [HttpDelete]
        [Route("EliminarSucursal/{id}")]
        public IActionResult EliminarSucursal(int id)
        {
            var sucursal = _context.Sucursales.Find(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            _context.Sucursales.Remove(sucursal);
            _context.SaveChanges();
            return Ok();
        }
    }
}
