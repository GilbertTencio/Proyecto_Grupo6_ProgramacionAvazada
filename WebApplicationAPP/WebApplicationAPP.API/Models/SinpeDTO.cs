using Microsoft.AspNetCore.Mvc;

public class SinpeDTO
{
    public int IdSinpe { get; set; }
    public string TelefonoOrigen { get; set; } = "";
    public string TelefonoDestino { get; set; } = "";
    public decimal Monto { get; set; }
    public string Descripcion { get; set; } = "";
    public DateTime Fecha { get; set; }
    public bool Estado { get; set; }
}