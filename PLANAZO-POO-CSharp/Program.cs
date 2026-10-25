using PLANAZO_POO_CSharp.Modelos;
using PLANAZO_POO_CSharp.Pruebas;
using PLANAZO_POO_CSharp.Servicios;
using System.Globalization;

if (args.Length > 0 && args[0].Equals("--pruebas", StringComparison.OrdinalIgnoreCase))
{
    PruebasSistema.Ejecutar();
    return;
}

var gestor = new GestorReservas(CrearEventos());

while (true)
{
    MostrarMenu();
    int opcion = LeerEntero("Seleccione una opción: ");

    try
    {
        switch (opcion)
        {
            case 1:
                MostrarEventos(gestor);
                break;
            case 2:
                ConsultarEvento(gestor);
                break;
            case 3:
                ReservarEntradas(gestor);
                break;
            case 4:
                MostrarReservas(gestor);
                break;
            case 5:
                CancelarReserva(gestor);
                break;
            case 6:
                MostrarResumen(gestor);
                break;
            case 7:
                ConsultarReservasDeUsuario(gestor);
                break;
            case 0:
                Console.WriteLine("Gracias por usar PLANAZO. ¡Hasta pronto!");
                return;
            default:
                Console.WriteLine("Opción incorrecta. Elija un número del menú.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"No se pudo completar la operación: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"No se pudo completar la operación: {ex.Message}");
    }
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine();
}

static List<Evento> CrearEventos()
{
    DateTime hoy = DateTime.Today;
    return
    [
        new Cine(1, "El viaje espacial", hoy.AddDays(5), 18000m, 40),
        new Cine(2, "Misterio en la montaña", hoy.AddDays(8), 20000m, 35),
        new Concierto(3, "Noche de rock", hoy.AddDays(12), 50000m, 100),
        new Concierto(4, "Festival de pop", hoy.AddDays(20), 65000m, 120),
        new Taller(5, "Fotografía para principiantes", hoy.AddDays(7), 40000m, 25),
        new Taller(6, "Cocina fácil", hoy.AddDays(10), 35000m, 20),
        new EventoDeportivo(7, "Final de fútbol", hoy.AddDays(15), 30000m, 200)
    ];
}

static string FormatearPrecio(decimal valor) =>
    valor.ToString("C0", CultureInfo.GetCultureInfo("es-CO"));

static void MostrarMenu()
{
    Console.WriteLine("================================");
    Console.WriteLine("        PLANAZO - EVENTOS");
    Console.WriteLine("================================");
    Console.WriteLine("1. Ver eventos disponibles");
    Console.WriteLine("2. Consultar evento");
    Console.WriteLine("3. Reservar entradas");
    Console.WriteLine("4. Ver reservas realizadas");
    Console.WriteLine("5. Cancelar una reserva");
    Console.WriteLine("6. Ver resumen de reservas");
    Console.WriteLine("7. Consultar reservas de un usuario");
    Console.WriteLine("0. Salir");
}

static void MostrarEventos(GestorReservas gestor)
{
    Console.WriteLine("\nEVENTOS DISPONIBLES");
    foreach (Evento evento in gestor.Eventos)
    {
        Console.WriteLine(
            $"#{evento.Id} - {evento.Nombre} | {evento.GetType().Name} | " +
            $"{FormatearPrecio(evento.CalcularPrecioFinal())} | " +
            $"{evento.CuposDisponibles} cupos | {evento.Fecha:dd/MM/yyyy}");
    }
}

static void ConsultarEvento(GestorReservas gestor)
{
    int id = LeerEntero("Escriba el número del evento: ");
    Evento evento = gestor.BuscarEvento(id);
    Console.WriteLine($"\nEvento: {evento.Nombre}");
    Console.WriteLine($"Tipo: {evento.GetType().Name}");
    Console.WriteLine($"Fecha: {evento.Fecha:dd/MM/yyyy}");
    Console.WriteLine($"Precio final por entrada: {FormatearPrecio(evento.CalcularPrecioFinal())}");
    Console.WriteLine($"Cupos disponibles: {evento.CuposDisponibles}");
}

static void ReservarEntradas(GestorReservas gestor)
{
    int idEvento = LeerEntero("Número del evento: ");
    string nombre = LeerTexto("Nombre de la persona: ");
    int cantidad = LeerEntero("Cantidad de entradas: ");

    Reserva reserva = gestor.CrearReserva(idEvento, nombre, cantidad);
    Console.WriteLine(
        $"Reserva #{reserva.Id} confirmada para {reserva.Usuario.Nombre}. " +
        $"Total: {FormatearPrecio(reserva.ValorTotal)}");
}

static void MostrarReservas(GestorReservas gestor)
{
    Console.WriteLine("\nRESERVAS");
    if (gestor.Reservas.Count == 0)
    {
        Console.WriteLine("Todavía no hay reservas.");
        return;
    }

    foreach (Reserva reserva in gestor.Reservas)
    {
        Console.WriteLine(
            $"#{reserva.Id} - {reserva.Usuario.Nombre} - {reserva.Evento.Nombre} - " +
            $"{reserva.CantidadEntradas} entrada(s) - {FormatearPrecio(reserva.ValorTotal)} - " +
            $"{reserva.Estado}");
    }
}

static void CancelarReserva(GestorReservas gestor)
{
    int id = LeerEntero("Número de la reserva que desea cancelar: ");
    gestor.CancelarReserva(id);
    Console.WriteLine("Reserva cancelada. Los cupos fueron devueltos.");
}

static void MostrarResumen(GestorReservas gestor)
{
    (int reservas, int entradas, decimal valor) = gestor.ObtenerResumen();
    Console.WriteLine("\nRESUMEN DE RESERVAS ACTIVAS");
    Console.WriteLine($"Reservas activas: {reservas}");
    Console.WriteLine($"Entradas reservadas: {entradas}");
    Console.WriteLine($"Valor total: {FormatearPrecio(valor)}");
}

static void ConsultarReservasDeUsuario(GestorReservas gestor)
{
    string nombre = LeerTexto("Nombre de la persona: ");
    IReadOnlyList<Reserva> reservas = gestor.BuscarReservasDeUsuario(nombre);
    if (reservas.Count == 0)
    {
        Console.WriteLine("No se encontraron reservas para esa persona.");
        return;
    }

    foreach (Reserva reserva in reservas)
    {
        Console.WriteLine(
            $"Reserva #{reserva.Id} - {reserva.Evento.Nombre} - {reserva.CantidadEntradas} entrada(s) - " +
            $"{reserva.Estado} - {FormatearPrecio(reserva.ValorTotal)}");
    }
}

static int LeerEntero(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string? texto = Console.ReadLine();
        if (texto is null)
        {
            Console.WriteLine();
            return 0;
        }

        if (int.TryParse(texto, out int valor))
        {
            return valor;
        }

        Console.WriteLine("Escriba un número entero válido.");
    }
}

static string LeerTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string? texto = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(texto))
        {
            return texto.Trim();
        }

        Console.WriteLine("Este dato no puede quedar vacío.");
    }
}
