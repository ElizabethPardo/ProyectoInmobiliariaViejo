using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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

        [Authorize]
        public ActionResult PorContrato(int id)
        {
            TempData["ContId"] = id;
            TempData["IdPago"] = id;
            var lista = repositorio.BuscarPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Pago/Details/5
        [Authorize]
        public ActionResult Details()
        {
            return View();
        }

        // GET: Pago/Create
        [Authorize]
        public ActionResult Create()
        {
            int id= (int)TempData["IdPago"];
            ViewBag.ContId = id;
            TempData["ContId"] = id;

            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago entidad)
        {
            int id = 0;

            id = (int)TempData["ContId"];


            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(entidad, id);
                    TempData["Id"] = entidad.Id;
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
        [Authorize]
        public ActionResult Edit(int id)
        {
            
            var entidad = repositorio.ObtenerPorId(id);
            TempData["IdCont"] = entidad.ContratoId;


            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago entidad)
        {
            try
            {   int idC=(int)TempData["IdCont"];
                entidad.ContratoId = idC;
                repositorio.Modificacion(entidad);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction("PorContrato", new { id = idC });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy = "Administrador")]
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago entidad)
        {
            try
            {
                ViewBag.IdCont = TempData["IdCont"];
                int idCont = ViewBag.IdCont;
                entidad = repositorio.ObtenerPorId(id);
                repositorio.Baja(id);

                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("PorContrato", new { id = idCont});
            }
            catch (Exception ex)
            {
                ViewBag.Error =" No se puede eliminar el Pago, ya que posee un Contrato asociado";
                return View(entidad);
            }
        }
    }
}
