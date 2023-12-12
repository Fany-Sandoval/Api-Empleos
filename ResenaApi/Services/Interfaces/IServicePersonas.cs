using ResenaApi.Modelos;

namespace ResenaApi.Services.Interfaces
{
    public interface IServicePersonas
    {
        Task<IEnumerable<Personas>> ObtenerTodasPersonas();
        Task<Personas> ObtenerPersonaPorId(string id);
       
        Task<Personas> CrearPersona(Personas persona);
        Task<bool> ActualizarPersona(string id, Personas updatedPersona);
        Task<bool> EliminarPersona(string id);
        Task<Personas> LoginPersona(string email, string password);
    }
}
