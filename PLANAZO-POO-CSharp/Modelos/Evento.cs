namespace PLANAZO_POO_CSharp.Modelos;

public abstract class Evento
{
    public int Id { get; }
    public string Nombre { get; }
    public DateTime Fecha { get; }
    public decimal PrecioBase { get; }
    public int CapacidadMaxima { get; }
    public int CuposDisponibles { get; private set; }

    protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El identificador debe ser mayor que cero.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del evento no puede estar vacío.", nameof(nombre));
        }

        if (precioBase < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.", nameof(precioBase));
        }

        if (capacidadMaxima <= 0)
        {
            throw new ArgumentException("La capacidad debe ser mayor que cero.", nameof(capacidadMaxima));
        }

        Id = id;
        Nombre = nombre.Trim();
        Fecha = fecha;
        PrecioBase = precioBase;
        CapacidadMaxima = capacidadMaxima;
        CuposDisponibles = capacidadMaxima;
    }

    public abstract decimal CalcularPrecioFinal();

    internal void ReservarCupos(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.", nameof(cantidad));
        }

        if (cantidad > CuposDisponibles)
        {
            throw new InvalidOperationException(
                $"Solo quedan {CuposDisponibles} cupos disponibles para este evento.");
        }

        CuposDisponibles -= cantidad;
    }

    internal void DevolverCupos(int cantidad)
    {
        if (cantidad <= 0 || CuposDisponibles + cantidad > CapacidadMaxima)
        {
            throw new InvalidOperationException("No se pueden devolver esos cupos al evento.");
        }

        CuposDisponibles += cantidad;
    }
}
