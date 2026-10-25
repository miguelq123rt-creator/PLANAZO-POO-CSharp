namespace PLANAZO_POO_CSharp.Modelos;

public class Usuario
{
    public int Id { get; }
    public string Nombre { get; }

    public Usuario(int id, string nombre)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El identificador debe ser mayor que cero.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
        }

        Id = id;
        Nombre = nombre.Trim();
    }
}
