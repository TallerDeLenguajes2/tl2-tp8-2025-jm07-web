namespace tl2_tp8_2025_jm07_web.Models;

public class Producto
{
    public int idProducto { get; set; }
    public string ?descripcion { get; set; }
    public double precio { get; set; }

    public Producto() { }

    public Producto(int id, string descripcion, double precio)
    {
        idProducto = id;
        this.descripcion = descripcion;
        this.precio = precio;
    }
}