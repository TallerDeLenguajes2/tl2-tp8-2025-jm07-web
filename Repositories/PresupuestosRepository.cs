using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using tl2_tp8_2025_jm07_web.Models;

namespace tl2_tp8_2025_jm07_web.Repositories;

public class PresupuestosRepository
{
    //Conexion con mi base de datos
    private string cadenaConexion = "Data Source=DB/Tienda.db;";

    public void CrearPresupuesto(Presupuestos presupuesto)
    {
        var sql = $"INSERT INTO presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            var comando = new SqliteCommand(sql, conexion);

            comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.nombreDestinatario));
            comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));

            comando.ExecuteNonQuery();

            conexion.Close();
        }
    }

    public List<Presupuestos> ListarTodosLosPresupuestos()
    {
        var presupuestos = new List<Presupuestos>();
        
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql1 = @"SELECT * FROM presupuestos;";
        using var comando1 = new SqliteCommand(sql1, conexion);

        using var lector1 = comando1.ExecuteReader();

        while (lector1.Read())
        {
            var p = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector1["IdPresupuesto"]),
                nombreDestinatario = lector1["NombreDestinatario"].ToString()!,
                FechaCreacion = lector1["FechaCreacion"].ToString()!,
                detalle = new List<PresupuestosDetalle>()
            };
            presupuestos.Add(p);
        }
        lector1.Close();

        foreach (var presupuesto in presupuestos)
        {
            string sql2 = @"SELECT pd.Cantidad, pr.IdProducto, pr.Descripcion, pr.Precio
                FROM presupuestosDetalle pd
                INNER JOIN productos pr ON pd.IdProducto = pr.IdProducto
                WHERE pd.IdPresupuesto = @IdPresupuesto";

            using var comando2 = new SqliteCommand(sql2, conexion);
            comando2.Parameters.Add(new SqliteParameter("@IdPresupuesto", presupuesto.IdPresupuesto));

            using var lector2 = comando2.ExecuteReader();

            while (lector2.Read())
            {
                var producto = new Producto
                {
                    idProducto = Convert.ToInt32(lector2["IdProducto"]),
                    descripcion = lector2["Descripcion"].ToString(),
                    precio = Convert.ToDouble(lector2["Precio"])
                };

                var detalle = new PresupuestosDetalle
                {
                    producto = producto,
                    cantidad = Convert.ToInt32(lector2["Cantidad"])
                };

                presupuesto.detalle.Add(detalle);
            }
            lector2.Close();
        }
        
        conexion.Close();
        return presupuestos;
    }

    public Presupuestos? ObtenerPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql1 = "SELECT * FROM presupuestos WHERE IdPresupuesto = @id";

        using var comando1 = new SqliteCommand(sql1, conexion);
        comando1.Parameters.Add(new SqliteParameter("@id", id));

        using var lector1 = comando1.ExecuteReader();

        if (!lector1.Read())
        {
            conexion.Close();
            return null;
        }

        var p = new Presupuestos
        {
            IdPresupuesto = Convert.ToInt32(lector1["IdPresupuesto"]),
            nombreDestinatario = lector1["NombreDestinatario"].ToString()!,
            FechaCreacion = lector1["FechaCreacion"].ToString()!,
            detalle = new List<PresupuestosDetalle>()
        };

        lector1.Close();

        string sql2= @"SELECT pd.Cantidad, pr.IdProducto, pr.Descripcion, pr.Precio
            FROM presupuestosDetalle pd
            INNER JOIN productos pr ON pd.IdProducto = pr.IdProducto
            WHERE pd.IdPresupuesto = @id";

        using var comando2 = new SqliteCommand(sql2, conexion);
        comando2.Parameters.Add(new SqliteParameter("@id", id));

        using var lector2 = comando2.ExecuteReader();

        while (lector2.Read())
        {
            var producto = new Producto
            {
                idProducto = Convert.ToInt32(lector2["IdProducto"]),
                descripcion = lector2["Descripcion"].ToString(),
                precio = Convert.ToDouble(lector2["Precio"])
            };

            var detalle = new PresupuestosDetalle
            {
                producto = producto,
                cantidad = Convert.ToInt32(lector2["Cantidad"])
            };

            p.detalle.Add(detalle);
        }
        lector2.Close();
        
        conexion.Close();
        return p;
    }

    public void ModificarPresupuesto(int id, Presupuestos presupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "UPDATE presupuestos SET NombreDestinatario = @NombreDestinatario, FechaCreacion = @FechaCreacion WHERE IdPresupuesto = @IdPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.nombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
        comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));

        comando.ExecuteNonQuery();

        conexion.Close();
    }

    public bool InsertarProductoYCantidadEnPresupuesto(int id, int idProd, int cantidad)
    {
        if (cantidad <= 0)
        {
            return false;
        }
        
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        bool existeProducto = false;
        bool existePresupuesto = false;

        //Verifico si existe el producto con el idProd
        string sql1 = "SELECT * FROM productos WHERE IdProducto = @idProd";

        using var comando1 = new SqliteCommand(sql1, conexion);
        comando1.Parameters.Add(new SqliteParameter("@idProd", idProd));

        using var lector1 = comando1.ExecuteReader();

        if (lector1.Read())
        {
            existeProducto = true;
        }

        //Verifico si existe el presupuesto con el id ingresado
        string sql2 = "SELECT * FROM presupuestos WHERE IdPresupuesto = @id";

        using var comando2 = new SqliteCommand(sql2, conexion);
        comando2.Parameters.Add(new SqliteParameter("@id", id));

        using var lector2 = comando2.ExecuteReader();

        if (lector2.Read())
        {
            existePresupuesto = true;
        }

        //S noi existe el producto y el presupuesto retorno falso
        if (!existeProducto || !existePresupuesto)
        {
            return false;
        }

        string sql3 = "INSERT INTO presupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @Cantidad)";
        using var comando3 = new SqliteCommand(sql3, conexion);

        comando3.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));
        comando3.Parameters.Add(new SqliteParameter("@IdProducto", idProd));
        comando3.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

        int filasAfectadas = comando3.ExecuteNonQuery();
        return filasAfectadas > 0;
    }

    public bool EliminarPresupuesto(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "DELETE FROM presupuestos WHERE IdPresupuesto = @id";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id", id));

        int filasAfectadas = comando.ExecuteNonQuery();

        conexion.Close();

        return filasAfectadas > 0;
    }
}