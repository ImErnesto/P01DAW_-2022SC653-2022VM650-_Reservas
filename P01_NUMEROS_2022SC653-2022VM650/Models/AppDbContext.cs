using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P01_NUMEROS_2022SC653_2022VM650.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EspaciosDeParqueo> EspaciosDeParqueos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EspaciosDeParqueo>(entity =>
        {
            entity.HasKey(e => e.EspacioId).HasName("PK__Espacios__EF75FDF9CE577959");

            entity.ToTable("EspaciosDeParqueo");

            entity.Property(e => e.EspacioId).HasColumnName("EspacioID");
            entity.Property(e => e.CostoPorHora).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.SucursalId).HasColumnName("SucursalID");
            entity.Property(e => e.Ubicacion).HasMaxLength(100);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reservas__C3993703017A2605");

            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.EspacioId).HasColumnName("EspacioID");
            entity.Property(e => e.EstadoReserva).HasMaxLength(50);
            entity.Property(e => e.FechaReserva).HasColumnType("datetime");
            entity.Property(e => e.HoraInicio).HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.SucursalId).HasName("PK__Sucursal__6CB48281073C6C98");

            entity.Property(e => e.SucursalId).HasColumnName("SucursalID");
            entity.Property(e => e.Administrador).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.NombreSucursal).HasMaxLength(100);
            entity.Property(e => e.TelefonoSucursal).HasMaxLength(15);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798220A8588");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contrasena).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
