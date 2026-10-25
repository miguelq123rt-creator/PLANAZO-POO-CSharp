using PLANAZO_POO_CSharp.Modelos;

namespace PLANAZO_POO_CSharp.Servicios;

public class GestorReservas
{
    private readonly List<Evento> _eventos;
    private readonly List<Reserva> _reservas = [];
    private readonly List<Usuario> _usuarios = [];
    private int _siguienteIdReserva = 1;

    public IReadOnlyList<Evento> Eventos => _eventos.AsReadOnly();
    public IReadOnlyList<Reserva> Reservas => _reservas.AsReadOnly();

    public GestorReservas(IEnumerable<Evento> eventos)
    {
        ArgumentNullException.ThrowIfNull(eventos);
        _eventos = eventos.ToList();

        if (_eventos.Select(evento => evento.Id).Distinct().Count() != _eventos.Count)
        {
            throw new ArgumentException("Los identificadores de los eventos deben ser únicos.", nameof(eventos));
        }
    }

    public Evento BuscarEvento(int id)
    {
        return _eventos.FirstOrDefault(evento => evento.Id == id)
            ?? throw new KeyNotFoundException($"No existe un evento con el número {id}.");
    }

    public Reserva CrearReserva(int idEvento, string nombreUsuario, int cantidadEntradas)
    {
        if (cantidadEntradas <= 0)
        {
            throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.", nameof(cantidadEntradas));
        }

        Evento evento = BuscarEvento(idEvento);
        Usuario usuario = BuscarOCrearUsuario(nombreUsuario);
        var reserva = new Reserva(_siguienteIdReserva, usuario, evento, cantidadEntradas);
        evento.ReservarCupos(cantidadEntradas);
        _reservas.Add(reserva);
        _siguienteIdReserva++;
        return reserva;
    }

    public void CancelarReserva(int idReserva)
    {
        Reserva reserva = _reservas.FirstOrDefault(item => item.Id == idReserva)
            ?? throw new KeyNotFoundException($"No existe una reserva con el número {idReserva}.");

        if (reserva.Estado == EstadoReserva.Cancelada)
        {
            throw new InvalidOperationException("Esta reserva ya fue cancelada.");
        }

        reserva.Evento.DevolverCupos(reserva.CantidadEntradas);
        reserva.MarcarComoCancelada();
    }

    public IReadOnlyList<Reserva> BuscarReservasDeUsuario(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("Escriba el nombre de la persona.", nameof(nombre));
        }

        Usuario? usuario = _usuarios.FirstOrDefault(
            item => item.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase));

        if (usuario is null)
        {
            return Array.Empty<Reserva>();
        }

        return _reservas.Where(reserva => reserva.Usuario.Id == usuario.Id).ToList().AsReadOnly();
    }

    public (int ReservasActivas, int EntradasReservadas, decimal ValorTotal) ObtenerResumen()
    {
        List<Reserva> activas = _reservas
            .Where(reserva => reserva.Estado == EstadoReserva.Activa)
            .ToList();

        return (
            activas.Count,
            activas.Sum(reserva => reserva.CantidadEntradas),
            activas.Sum(reserva => reserva.ValorTotal));
    }

    private Usuario BuscarOCrearUsuario(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("Escriba el nombre de la persona.", nameof(nombre));
        }

        string nombreLimpio = nombre.Trim();
        Usuario? usuario = _usuarios.FirstOrDefault(
            item => item.Nombre.Equals(nombreLimpio, StringComparison.OrdinalIgnoreCase));

        if (usuario is not null)
        {
            return usuario;
        }

        usuario = new Usuario(_usuarios.Count + 1, nombreLimpio);
        _usuarios.Add(usuario);
        return usuario;
    }
}
