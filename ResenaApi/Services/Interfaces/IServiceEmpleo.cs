using ResenaApi.Modelos;

namespace ResenaApi.Services.Interfaces
{
    public interface IServiceEmpleo
    {
        Task<Empleos> CrearEmpleo(Empleos empleos);
        Task<Empleos> ObtenerEmpleoPorId(string id_empleo);

        Task<bool> ActualizarEmpleo(string id_empleo, Empleos updatedEmpleo);
        Task<IEnumerable<Empleos>> ObtenerTodosEmpleos();

        Task<bool> EliminarEmpleo(string id_empleo);
    }
}
