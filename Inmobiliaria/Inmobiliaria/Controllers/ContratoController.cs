using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IConfiguration config;

        public ContratoController(IRepositorioContrato repositorio, IRepositorioInmueble repoInmueble, IRepositorioInquilino repoInquilino, IConfiguration config)
        {
            this.repositorio = repositorio;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;

            this.config = config;
        }

        // GET: Contrato
        [Authorize]
        public ActionResult Index()
        {
            ViewData["Title"] = "CONTRATOS DE ALQUILER";
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            
                return View(lista);

        }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: Contrato/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.BuscarDisponibles();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = repositorio.Alta(entidad);

                    if (res == -1)
                    {
                        TempData["Error"] = "El inmueble se encuentra ocupado en las fechas seleccionadas";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        TempData["Id"] = entidad.Id;
                        return RedirectToAction("Index", "Pago", new { id = entidad.Id });
                    }

                }
                else
                {
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repoInmueble.BuscarDisponibles();
                    return View(entidad);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: Contrato/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato entidad)
        {
            try
            {
                entidad.Id = id;
                int res = repositorio.Modificacion(entidad);

                if (res == -1)
                {
                    TempData["Error"] = "El inmueble se encuentra ocupado en las fechas seleccionadas";
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                    ViewBag.Error = TempData["Error"];
                    return View(entidad);
                }
                else
                {

                    TempData["Id"] = entidad.Id;
                    TempData["Mensaje"] = "Datos guardados correctamente";
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: Contrato/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato entidad)
        {
            try
            {
                entidad = repositorio.ObtenerPorId(id);
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error ="No se puede eliminar el Contrato, ya que posee Pago/s asociados";
                
                return View(entidad);
            }
        }

        [Authorize]
        public ActionResult BuscarVigentes(BusquedaPorFechas busqueda)
        {
            try
            {
                IList<Contrato> entidad = repositorio.ContratosVigentes(busqueda.FechaInicio, busqueda.FechaFin);

                if (entidad != null)
                {
                    ViewData["Title"] = "CONTRATOS VIGENTES";
                    return View(nameof(Index), entidad);

                }
                else
                {
                    TempData["Mensaje"] = "El inmueble se encuentra ocupado en las fechas seleccionadas";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }

        }

        [Authorize]
        public ActionResult PorInmueble(int id)
        {
            TempData["IdInmueble"] = id;

            ViewData["Title"] = "CONTRATOS DE ALQUILER";
            IList<Contrato> lista = repoInmueble.BuscarPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];

            return View(lista);

        }

       


    }
}
