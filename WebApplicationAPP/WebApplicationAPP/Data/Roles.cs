namespace WebApplicationAPP.Data
{
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Cajero = "Cajero";
        public const string Contador = "Contador";
        public const string CajeroAutorizado = Cajero + "," + Contador;
    }
}
