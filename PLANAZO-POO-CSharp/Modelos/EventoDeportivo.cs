namespace PLANAZO_POO_CSharp.Modelos;

public class EventoDeportivo : Evento
{
    public EventoDeportivo(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima)
    {
    }

    public override decimal CalcularPrecioFinal() => PrecioBase * 1.20m;
}
