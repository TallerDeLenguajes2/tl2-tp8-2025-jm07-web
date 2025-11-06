namespace tl2_tp8_2025_jm07_web.Models;

public class Presupuestos
{
    public int IdPresupuesto { get; set; }
    public string? nombreDestinatario { get; set; }
    public string? FechaCreacion { get; set; }
    public List<PresupuestosDetalle> detalle { get; set; }

    public Presupuestos() { }

    public Presupuestos(int id, string nombre, string fecha)
    {
        IdPresupuesto = id;
        nombreDestinatario = nombre;
        FechaCreacion = fecha;
        detalle = new List<PresupuestosDetalle>();
    }

    //Metodos
    public double MontoPresupuesto()
    {
        double monto = 0;
        foreach (var item in detalle)
        {
            monto += item.producto.precio * item.cantidad;
        }
        return monto;
    }

    public double MontoPresupuestoConIva()
    {
        double montoConIVA = 0;
        foreach (var item in detalle)
        {
            montoConIVA += item.producto.precio * item.cantidad;
        }
        return montoConIVA * 1.21;
    }

    public int CantidadProductos()
    {
        int cantidad = 0;
        foreach (var item in detalle)
        {
            cantidad += item.cantidad;
        }
        return cantidad;
    }
}