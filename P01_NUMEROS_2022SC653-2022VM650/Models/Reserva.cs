using System;
using System.Collections.Generic;

namespace P01_NUMEROS_2022SC653_2022VM650.Models;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public int? UsuarioId { get; set; }

    public int? EspacioId { get; set; }

    public DateTime FechaReserva { get; set; }

    public DateTime HoraInicio { get; set; }

    public int CantidadHoras { get; set; }

    public string EstadoReserva { get; set; } = null!;
}
