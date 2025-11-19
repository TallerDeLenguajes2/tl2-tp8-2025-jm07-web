using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_jm07_web.Models;
using tl2_tp8_2025_jm07_web.Repositories;

namespace tl2_tp8_2025_jm07_web.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;
    public PresupuestosController()
    {
        presupuestosRepository = new PresupuestosRepository();
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presupuestosRepository.ListarTodosLosPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult CrearPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos nuevoPresupuesto)
    {
        presupuestosRepository.CrearPresupuesto(nuevoPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DetallePresupuesto(int id)
    {
        var presupuesto = presupuestosRepository.ObtenerPorId(id);
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        var presupuesto = presupuestosRepository.ObtenerPorId(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuestos presupuesto)
    {
        presupuestosRepository.ModificarPresupuesto(presupuesto.IdPresupuesto, presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        return View(id);
    }
        
    [HttpGet]
    public IActionResult ConfirmarEliminarPresupuesto(int id)
    {
        if (presupuestosRepository.ObtenerPorId(id) == null) return NotFound();
        presupuestosRepository.EliminarPresupuesto(id);
        return RedirectToAction("Index");     
    }
}