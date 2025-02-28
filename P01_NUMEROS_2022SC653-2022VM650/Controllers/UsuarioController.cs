using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_NUMEROS_2022SC653_2022VM650.Models;

namespace P01_NUMEROS_2022SC653_2022VM650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var Usuario = await (from u in _context.Usuarios select u).ToListAsync();
            return Ok(Usuario);
        }

        [HttpPost("Validar")]
        public async Task<IActionResult> Validar(string usuario, string contraseña)
        {
            var usuarioValidar = await (from u in _context.Usuarios where u.Correo==usuario && u.Contrasena == contraseña select u).ToListAsync();
            if (usuarioValidar == null)
            {
                return NotFound();
            }
            return Ok("Validacion exitosa \n Bienvenido: " + usuarioValidar[0].Nombre);
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        [HttpPost]
        [Route("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            var usuarioActualizar = await _context.Usuarios.FirstOrDefaultAsync(c => c.UsuarioId == id);
            if (usuarioActualizar == null)
            {
                return NotFound();
            }
            usuarioActualizar.Nombre = usuario.Nombre;
            usuarioActualizar.Correo = usuario.Correo;
            usuarioActualizar.Telefono = usuario.Telefono;
            usuarioActualizar.Contrasena = usuario.Contrasena;
            usuarioActualizar.Rol = usuario.Rol;

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [HttpDelete]
        [Route("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
