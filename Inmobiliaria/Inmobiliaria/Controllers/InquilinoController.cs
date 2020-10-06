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
    public class InquilinoController : Controller
    {

        private readonly IRepositorioInquilino repositorio;
        private readonly IRepositorioUsuario repoUsuario;
        private readonly IRepositorioPropietario repoPropietario;
        private readonly IConfiguration config;

        public InquilinoController(IRepositorioInquilino repositorio,IConfiguration config, IRepositorioUsuario repoUsuario, IRepositorioPropietario repoPropietario)
        {
            this.repositorio = repositorio;
            this.config = config;
            this.repoUsuario = repoUsuario;
            this.repoPropietario = repoPropietario;
        }

        // GET: InquilinoController
        [Authorize]
        public ActionResult Index()
        {
            
            var lista = repositorio.ObtenerTodos();
            ViewBag.Id = TempData["Id"];      
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }


        // GET: InquilinoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
            
        }

        [Authorize]
        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var inqui = repositorio.ObtenerPorEmail(inquilino.Email);
                    var user = repoUsuario.ObtenerPorEmail(inquilino.Email);
                    var prop = repoPropietario.ObtenerPorEmail(inquilino.Email);

                    if (user == null && prop == null && (inqui == null || inqui.Email == inquilino.Email))
                    {
                        repositorio.Alta(inquilino);
                        TempData["Id"] = inquilino.IdInquilino;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "El Email ingresado ya se encuentra registrado en el sistema! ";
                        ViewBag.Error = TempData["Error"];
                        return View(inquilino);
                    }
                }
                else
                {
                    ViewBag.Propietarios = repositorio.ObtenerTodos();
                    return View(inquilino);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inquilino);
            }
        }


        // GET: InquilinoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino i)
        {
            try
            {
                var inqui = repositorio.ObtenerPorEmail(i.Email);
                var user = repoUsuario.ObtenerPorEmail(i.Email);
                var prop = repoPropietario.ObtenerPorEmail(i.Email);

                if (user == null && prop == null && (inqui == null || inqui.Email == i.Email))
                {
                    repositorio.Modificacion(i);
                    TempData["Mensaje"] = "Datos guardados correctamente";
                     return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "El Email ingresado ya se encuentra registrado en el sistema! ";
                    ViewBag.Error = TempData["Error"];
                    return View(i);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }


        // GET: InquilinoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {  entidad= repositorio.ObtenerPorId(id);
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se puede eliminar el Inquilino, ya que posee Contratos asociados";
                return View(entidad);
            }
        }
    }
}
