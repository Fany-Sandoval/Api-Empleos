namespace ResenaApi.Modelos
{
    public class Empleos
    {
        public string id_empleo { get; set; }
        public string Puesto { get; set; }

        public string Empresa { get; set; }

        public string Salario { get; set; }

        public string? Descripcion { get; set; }

        public string? Lugar { get; set; }
        public string? correo { get; set;}
    }
}
