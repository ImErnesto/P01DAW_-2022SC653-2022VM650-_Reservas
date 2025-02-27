using System;
using System.Collections.Generic;

namespace P01_NUMEROS_2022SC653_2022VM650.Models;

public partial class EspaciosDeParqueo
{
    public int EspacioId { get; set; }

    public int? SucursalId { get; set; }

    public int Numero { get; set; }

    public string? Ubicacion { get; set; }

    public decimal CostoPorHora { get; set; }

    public string Estado { get; set; } = null!;
}
