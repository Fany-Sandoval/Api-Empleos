using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using ResenaApi.Modelos;
using ResenaApi.Services.Interfaces;

namespace ResenaApi.Services
{
    public class ServicePersonas: IServicePersonas
    {
        private readonly IFirebaseClient _firebaseClient;

        public ServicePersonas(IFirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }
        public async Task<IEnumerable<Personas>> ObtenerTodasPersonas()
        {
            try
            {
                var response = await _firebaseClient.GetAsync("Personas");
                var personas = response.ResultAs<Dictionary<string, Personas>>();

                if (personas == null || !personas.Any())
                {
                    return Enumerable.Empty<Personas>();
                }

                return personas.Values.ToList();
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al obtener todas las personas: {ex.Message}");
            }
        }

        public async Task<Personas> ObtenerPersonaPorId(string id)
        {
            try
            {
                var response = await _firebaseClient.GetAsync($"Personas/{id}");
                return response.ResultAs<Personas>();
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al obtener la persona por ID: {ex.Message}");
            }
        }

        public async Task<Personas> CrearPersona(Personas persona)
        {
            try
            {
                if (persona == null)
                {
                    throw new ArgumentNullException(nameof(persona));
                }

                var idGuid = Guid.NewGuid();
                persona.id= idGuid.ToString();

                SetResponse response = await _firebaseClient.SetAsync($"Personas/{idGuid}", persona);
                persona.id = response.ResultAs<Personas>().Nombre;

                if (response != null)
                {
                    return persona;
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

        public async Task<bool> ActualizarPersona(string id, Personas updatedPersona)
        {
            try
            {
                var response = await _firebaseClient.GetAsync($"Personas/{id}");
                var persona = response.ResultAs<Personas>();

                if (persona == null)
                {
                    return false;
                }

                updatedPersona.id = persona.id;

                var updateResponse = await _firebaseClient.UpdateAsync($"Personas/{id}", updatedPersona);

                return updateResponse.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al actualizar la persona: {ex.Message}");
            }
        }

        public async Task<bool> EliminarPersona(string id)
        {
            try
            {
                var response = await _firebaseClient.DeleteAsync($"Personas/{id}");
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                throw new Exception($"Error al eliminar la persona: {ex.Message}");
            }
        }



        public async Task<Personas> LoginPersona(string email, string password)
        {
            try
            {
                // Verificar si el usuario con el correo electrónico proporcionado existe
                var response = await _firebaseClient
                .GetAsync("Personas",
                 QueryBuilder.New("Email")
                );

                var personas = response.ResultAs<Dictionary<string, Personas>>();

                if (personas == null || !personas.Any())
                {
                    return null; // El usuario no existe
                }

                // Obtener la primera persona que coincida con el correo electrónico
                var usuario = personas.Values.FirstOrDefault();

                // Verificar la contraseña (esto debe ser una lógica segura en una aplicación real)
                if (usuario.password == password)
                {
                    return usuario; // Autenticación exitosa
                }

                return null; // Contraseña incorrecta
                 }
                    catch (Exception ex)
                        {
                // Manejar la excepción aquí si es necesario
                   throw new Exception($"Error al iniciar sesión: {ex.Message}");
            }
        }




    }
}