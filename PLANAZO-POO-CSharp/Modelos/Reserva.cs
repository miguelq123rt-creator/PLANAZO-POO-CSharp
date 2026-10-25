namespace PLANAZO_POO_CSharp.Modelos;

public enum EstadoReserva
{
    Activa,
    Cancelada
}

public class Reserva
{
    public int Id { get; }
    public Usuario Usuario { get; }
    public Evento Evento { get; }
    public int CantidadEntradas { get; }
    public decimal ValorTotal { get; }
    public EstadoReserva Estado { get; private set; }

    public Reserva(int id, Usuario usuario, Evento evento, int cantidadEntradas)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El identificador debe ser mayor que cero.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);

        if (cantidadEntradas <= 0)
        {
            throw new ArgumentException(
                "La cantidad de entradas debe ser mayor que cero.",
                nameof(cantidadEntradas));
        }

        Id = id;
        Usuario = usuario;
        Evento = evento;
        CantidadEntradas = cantidadEntradas;
        ValorTotal = evento.CalcularPrecioFinal() * cantidadEntradas;
        Estado = EstadoReserva.Activa;
    }

    internal void MarcarComoCancelada()
    {
        if (Estado == EstadoReserva.Cancelada)
        {
            throw new InvalidOperationException("Esta reserva ya fue cancelada.");
        }

        Estado = EstadoReserva.Cancelada;
    }
}
