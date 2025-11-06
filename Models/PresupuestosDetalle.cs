namespace tl2_tp8_2025_jm07_web.Models;

public class PresupuestosDetalle
{
    public Producto producto { get; set; }
    public int cantidad { get; set; }

    public PresupuestosDetalle() { }

    public PresupuestosDetalle(Producto produc, int cant)
    {
        producto = produc;
        cantidad = cant;
    }
}