using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repositorio;
        private readonly IConfiguration config;


        public PagoController(IRepositorioPago repositorio, IConfiguration config)
        {
            this.repositorio = repositorio;
            this.config = config;
        }

        // GET: Pago
        public ActionResult Index(int id)
        {
            TempData["IdPago"]=id;

            var lista = repositorio.BuscarPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        public ActionResult PorContrato(int id)
        {
            
            var lista = repositorio.BuscarPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Pago/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago entidad)
        {
            int id = 0;
            ViewBag.IdPago = TempData["IdPago"];
            if (ViewBag.IdPago != null) 
            {
                id = ViewBag.IdPago;
            }
                

            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(entidad, id);
                    TempData["Id"] = entidad.IdPago;
                    TempData["IdCont"] = entidad.ContratoId;
                    return RedirectToAction("PorContrato", new { id = id });
                }
                else
                {
                    ViewBag.Pago = repositorio.BuscarPorContrato(id);
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

        // GET: Pago/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            TempData["IdCont"] = entidad.ContratoId;
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago entidad)
        {
            try
            {
                ViewBag.IdCont = TempData["IdCont"];
                int idCont = ViewBag.IdCont;

                repositorio.Baja(id);

                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("PorContrato", new { id = idCont});
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}
