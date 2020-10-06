using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IConfiguration config;


        public InmuebleController(IRepositorioInmueble repositorio, IConfiguration config)
        {
            this.repositorio = repositorio;
            this.config = config;
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            //String dis = (string)TempData["DisInmueble"];
            //IList<Inmueble> dis = TempData["Disponibles"] as IList<Inmueble>;
            /*if (dis != null)
            {
                var lista = JsonConvert.DeserializeObject<List<Inmueble>>((string)TempData["Disponibles"]);
                ViewData["Title"] = "INMUEBLES DISPONIBLES";
                return View(lista);
            }
            else 
            {}*/

            var lista = repositorio.ObtenerTodos();
            ViewData["Title"] = "INMUEBLES";
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

            TempData["pro"] = id;

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
            ViewBag.IdPro = TempData["IdPro"];
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
                    repositorio.Alta(entidad, id);
                    TempData["Id"] = entidad.IdInmueble;
                    return RedirectToAction("PorPropietario", new { id = id });
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
            ViewBag.Pro = TempData["pro"];
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
                 ViewBag.IdPro = TempData["IdPro"];
               
                int idPro = ViewBag.IdPro;
               
                    entidad.IdInmueble = id;
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
            ViewBag.Pro = TempData["pro"];
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
                entidad = repositorio.ObtenerPorId(id);
                int idPro = ViewBag.IdPro;

                repositorio.Baja(id);

                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("PorPropietario", new { id = idPro });
            }
            catch (Exception ex)
            {
                ViewBag.Error =" No se puede eliminar el Inmueble, ya que posee Contratos asociados";


                return View(entidad);
            }
        }

        [Authorize]
        public ActionResult InmueblesDisponibles()
     {
            try
            {
                IList<Inmueble> lista =repositorio.BuscarDisponibles();

                ViewData["Title"] = "INMUEBLES DISPONIBLES";
                //TempData["Disponibles"] = JsonConvert.SerializeObject(dis);
                //TempData["DisInmueble"] = "Activo";
                return View(nameof(Index), lista);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return RedirectToAction(nameof(Index)); 
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult BuscarInmueblesPorFecha(BusquedaPorFechas busqueda)
        {

            var lista = repositorio.BuscarInmueblesDisponibles(busqueda.FechaInicio, busqueda.FechaFin);
            ViewData["Title"] = "INMUEBLES DISPONIBLES";
            ViewData["Title2"] = "Periodo: "+ busqueda.FechaInicio.ToShortDateString() +"-"+ busqueda.FechaFin.ToShortDateString();

            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];


            return View(nameof(Index), lista);

        }





    }
}
