namespace ResenaApi.Modelos
{
    public class Usuarios
    {
        public string? Id { get; set; }

        public string correo { get; set; }

        public byte[] passwordS { get; set; }

        public byte[] passwordH { get; set; }

        public string PersonaId { get; set; }

    }
}
