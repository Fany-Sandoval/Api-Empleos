using FireSharp.Interfaces;
using FireSharp.Response;
using ResenaApi.Modelos;
using ResenaApi.Services.Interfaces;

namespace ResenaApi.Services
{
    public class ServiceEmpleos : IServiceEmpleo
    {
        private readonly IFirebaseClient _firebaseClient;

        public ServiceEmpleos(IFirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }

        public async Task<Empleos> CrearEmpleo(Empleos empleos)
        {
            try
            {
                if (empleos == null)
                {
                    throw new ArgumentNullException(nameof(empleos));
                }

                var idGuid = Guid.NewGuid();
                empleos.id_empleo = idGuid.ToString();

                SetResponse response = await _firebaseClient.SetAsync($"Empleos/{idGuid}", empleos);
                empleos.id_empleo = response.ResultAs<Empleos>().Puesto;

                if (response != null)
                {
                    return empleos;
                }
                else
                {
                    throw new Exception("Error al crear la persona.");
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al crear la persona: {ex.Message}");
            }
        }

        public async Task<Empleos> ObtenerEmpleoPorId(string id_empleo)
        {
            try
            {
                var response = await _firebaseClient.GetAsync($"Empleos/{id_empleo}");
                return response.ResultAs<Empleos>();
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al obtener el empleo por ID: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Empleos>> ObtenerTodosEmpleos()
        {
            try
            {
                var response = await _firebaseClient.GetAsync("Empleos");
                var empleos = response.ResultAs<Dictionary<string, Empleos>>();

                if (empleos == null || !empleos.Any())
                {
                    return Enumerable.Empty<Empleos>();
                }

                return empleos.Values.ToList();
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al obtener todos los empleos: {ex.Message}");
            }
        }

        public async Task<bool> ActualizarEmpleo(string id_empleo, Empleos updatedEmpleo)
        {
            try
            {
                var response = await _firebaseClient.GetAsync($"Empleos/{id_empleo}");
                var empleo = response.ResultAs<Personas>();

                if (empleo == null)
                {
                    return false;
                }

                updatedEmpleo.id_empleo = empleo.id;

                var updateResponse = await _firebaseClient.UpdateAsync($"Empleos/{id_empleo}", updatedEmpleo);

                return updateResponse.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al actualizar empleo: {ex.Message}");
            }
        }

        public async Task<bool> EliminarEmpleo(string id_empleo)
        {
            try
            {
                var response = await _firebaseClient.DeleteAsync($"Empleos/{id_empleo}");
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al eliminar empleo: {ex.Message}");
            }
        }
    }
}
