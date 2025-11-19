using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_jm07_web.Models;
using tl2_tp8_2025_jm07_web.Repositories;

namespace tl2_tp8_2025_jm07_web.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = productoRepository.ListarTodosLosProductos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto nuevoProducto)
    {
        productoRepository.CrearProducto(nuevoProducto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto = productoRepository.ObtenerPorId(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        productoRepository.ModificarProducto(producto.idProducto, producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        return View(id);
    }
        
    [HttpGet]
    public IActionResult ConfirmarEliminarProducto(int id)
    {
        if (productoRepository.ObtenerPorId(id) == null) return NotFound();
        productoRepository.EliminarProducto(id);
        return RedirectToAction("Index");     
    }
}