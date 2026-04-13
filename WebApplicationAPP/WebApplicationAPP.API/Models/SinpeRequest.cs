using Microsoft.AspNetCore.Mvc;

public class SinpeRequest
{
    public int IdCaja { get; set; }
    public string TelefonoOrigen { get; set; } = "";
    public string TelefonoDestino { get; set; } = "";
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = "";
}