using System;
using System.Collections.Generic;

namespace P01_NUMEROS_2022SC653_2022VM650.Models;

public partial class Sucursale
{
    public int SucursalId { get; set; }

    public string NombreSucursal { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? TelefonoSucursal { get; set; }

    public string? Administrador { get; set; }

    public int NumeroEspacios { get; set; }
}
