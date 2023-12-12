using ResenaApi.Modelos;

namespace ResenaApi.Services
{
    public interface IServiceAuth
    {
        Task<string> Register(Auth auth, Personas personas);
        Task<string> Login(Auth auth);
        Task<string> ForgotPassword(string email);
        Task<string> SuspendUser(string uid);
        Task<string> Eliminar(string uid);
        Task<string> HabilitarUser(string uid);
    }
}
