using PLANAZO_POO_CSharp.Modelos;
using PLANAZO_POO_CSharp.Servicios;

namespace PLANAZO_POO_CSharp.Pruebas;

public static class PruebasSistema
{
    public static void Ejecutar()
    {
        int aprobadas = 0;
        var cine = new Cine(1, "Película de prueba", DateTime.Today, 20000m, 5);
        var concierto = new Concierto(2, "Concierto de prueba", DateTime.Today, 50000m, 5);
        var taller = new Taller(3, "Taller de prueba", DateTime.Today, 40000m, 5);
        var deporte = new EventoDeportivo(4, "Partido de prueba", DateTime.Today, 30000m, 5);
        var gestor = new GestorReservas([cine, concierto, taller, deporte]);

        Verificar("Cine mantiene el precio base", cine.CalcularPrecioFinal() == 20000m, ref aprobadas);
        Verificar("Concierto aumenta el precio un 15 %", concierto.CalcularPrecioFinal() == 57500m, ref aprobadas);
        Verificar("Taller aplica descuento del 10 %", taller.CalcularPrecioFinal() == 36000m, ref aprobadas);
        Verificar("Evento deportivo aplica su regla del 20 %", deporte.CalcularPrecioFinal() == 36000m, ref aprobadas);

        Reserva reserva = gestor.CrearReserva(1, "Ana", 2);
        Verificar("Reserva válida", reserva.Estado == EstadoReserva.Activa, ref aprobadas);
        Verificar("La reserva descuenta cupos", cine.CuposDisponibles == 3, ref aprobadas);
        Verificar(
            "No permite reservar más cupos disponibles",
            Lanza<InvalidOperationException>(() => gestor.CrearReserva(1, "Ana", 4)),
            ref aprobadas);
        Verificar(
            "No permite cantidades negativas",
            Lanza<ArgumentException>(() => gestor.CrearReserva(1, "Ana", -1)),
            ref aprobadas);

        gestor.CancelarReserva(reserva.Id);
        Verificar("Cancelar devuelve los cupos", cine.CuposDisponibles == 5, ref aprobadas);
        Verificar(
            "No permite cancelar dos veces",
            Lanza<InvalidOperationException>(() => gestor.CancelarReserva(reserva.Id)),
            ref aprobadas);
        Verificar(
            "No permite consultar un evento inexistente",
            Lanza<KeyNotFoundException>(() => gestor.BuscarEvento(999)),
            ref aprobadas);

        gestor.CrearReserva(2, "Ana", 2);
        Verificar(
            "Permite consultar las reservas de una persona",
            gestor.BuscarReservasDeUsuario("ana").Count == 2,
            ref aprobadas);
        (int reservas, int entradas, decimal total) = gestor.ObtenerResumen();
        Verificar(
            "El resumen solo cuenta reservas activas",
            reservas == 1 && entradas == 2 && total == 115000m,
            ref aprobadas);

        Console.WriteLine($"\nPruebas aprobadas: {aprobadas}/13");
        if (aprobadas != 13)
        {
            throw new InvalidOperationException("Una o más pruebas no fueron aprobadas.");
        }
    }

    private static void Verificar(string nombre, bool resultado, ref int aprobadas)
    {
        Console.WriteLine($"{(resultado ? "[OK]" : "[FALLO]")} {nombre}");
        if (!resultado)
        {
            throw new InvalidOperationException($"Falló la prueba: {nombre}");
        }

        aprobadas++;
    }

    private static bool Lanza<TExcepcion>(Action accion) where TExcepcion : Exception
    {
        try
        {
            accion();
            return false;
        }
        catch (TExcepcion)
        {
            return true;
        }
    }
}
