using System;
using System.Collections.Generic;

namespace P01_NUMEROS_2022SC653_2022VM650.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Contrasena { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
