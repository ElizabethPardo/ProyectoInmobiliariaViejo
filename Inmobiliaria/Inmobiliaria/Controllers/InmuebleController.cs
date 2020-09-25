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
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IConfiguration config;
    

        public InmuebleController(IRepositorioInmueble repositorio,IConfiguration config)
        {
            this.repositorio = repositorio;
            this.config = config;
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
     
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        [Authorize]
        public ActionResult PorPropietario(int id)
        {
              TempData["IdPro"] = id;
            

            var lista = repositorio.BuscarPorPropietario(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];

            return View(lista);
        }

        // GET: InmuebleController/Details/5
        [Authorize]
        public ActionResult Details()
        {
            return View();
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble entidad)
        {
            
            ViewBag.IdPro = TempData["IdPro"];
            int id = ViewBag.IdPro;

            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(entidad,id);
                    TempData["Id"] = entidad.IdInmueble;
                    return RedirectToAction("PorPropietario", new {id = id });
                }
                else
                {
                    ViewBag.Inmueble = repositorio.ObtenerTodos();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    return View(entidad);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                return View(entidad);
            }
        }

        // GET: InmuebleController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            TempData["IdPro"] = entidad.PropietarioId;
            //ViewBag.Propietarios = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            return View(entidad);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble entidad)
        {
            try
            {
                entidad.IdInmueble = id;
                ViewBag.IdPro = TempData["IdPro"];
                int idPro = ViewBag.IdPro;

                repositorio.Modificacion(entidad);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction("PorPropietario", new { id = idPro });
            }
            catch (Exception ex)
            {
                ViewBag.Propietarios = repositorio.ObtenerTodos();
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            TempData["IdPro"] = entidad.PropietarioId;
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble entidad)
        {
            try
            {
                ViewBag.IdPro = TempData["IdPro"];
                int idPro = ViewBag.IdPro;

                repositorio.Baja(id);
               
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("PorPropietario", new { id = idPro });
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
