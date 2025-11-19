using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp8_2025_jm07_web.Models;

namespace tl2_tp8_2025_jm07_web.Repositories;

public class ProductoRepository
{
    //Conexion con mi base de datos
    private string cadenaConexion = "Data Source=DB/Tienda.db;";

    public void CrearProducto(Producto producto)
    {
        var sql = $"INSERT INTO productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = new SqliteCommand(sql, conexion);

            comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.descripcion));
            comando.Parameters.Add(new SqliteParameter("@Precio", producto.precio));

            comando.ExecuteNonQuery();

            conexion.Close();
        }
    }

    public void ModificarProducto(int id, Producto producto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "UPDATE productos SET Descripcion = @Descripcion, Precio = @Precio WHERE IdProducto = @IdProducto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", producto.precio));
        comando.Parameters.Add(new SqliteParameter("@IdProducto", id));

        comando.ExecuteNonQuery();

        conexion.Close();
    }

    public List<Producto> ListarTodosLosProductos()
    {
        var productos = new List<Producto>();
        
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"SELECT * FROM productos;";
        using var comando = new SqliteCommand(sql, conexion);

        using var lector = comando.ExecuteReader();
            
        while (lector.Read())
        {
            var p = new Producto
            {
                idProducto = Convert.ToInt32(lector["IdProducto"]),
                descripcion = lector["Descripcion"].ToString(),
                precio = Convert.ToInt32(lector["Precio"]),
            };
            productos.Add(p);
        }

        conexion.Close();
        return productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "SELECT * FROM productos WHERE IdProducto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            var p = new Producto
            {
                idProducto = Convert.ToInt32(lector["IdProducto"]),
                descripcion = lector["Descripcion"].ToString(),
                precio = Convert.ToInt32(lector["Precio"]),
            };
            conexion.Close();
            return p;
        }

        conexion.Close();
        return null;
    }

    public bool EliminarProducto(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "DELETE FROM productos WHERE IdProducto = @id";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id", id));

        int filasAfectadas = comando.ExecuteNonQuery();

        conexion.Close();
        
        return filasAfectadas > 0;
    }
}