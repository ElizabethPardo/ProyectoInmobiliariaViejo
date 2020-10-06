using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        

        private readonly IRepositorioUsuario repoUsuario;
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorioPropietario repositorio;
        private readonly IConfiguration config;

        public PropietarioController(IRepositorioPropietario repositorio, IConfiguration config, IRepositorioUsuario repoUsuario, IRepositorioInquilino repoInquilino, IRepositorioInmueble repoInmueble)
        {
            this.repositorio = repositorio;
            this.config = config;
            this.repoUsuario = repoUsuario;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
        }

        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PropietarioController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var prop = repositorio.ObtenerPorEmail(propietario.Email);
                    var user = repoUsuario.ObtenerPorEmail(propietario.Email);
                    var inqui = repoInquilino.ObtenerPorEmail(propietario.Email);

                    if (user == null && inqui == null && prop == null)
                    {

                        repositorio.Alta(propietario);
                        TempData["Id"] = propietario.IdPropietario;
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        TempData["Error"] = "El Email ingresado ya se encuentra registrado en el sistema! ";
                        ViewBag.Error = TempData["Error"];
                        return View();
                    }
                }
                else
                    return View(propietario);
                 }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(propietario);
            }
        }

        // GET: PropietarioController/Edit/5
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

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult Edit(int id, Propietario p)
        {
            try
            {
                var prop = repositorio.ObtenerPorEmail(p.Email);
                var user = repoUsuario.ObtenerPorEmail(p.Email);
                var inqui = repoInquilino.ObtenerPorEmail(p.Email);

                if (user == null && inqui == null && (prop == null || prop.Email == p.Email))
                {
                    repositorio.Modificacion(p);
                    TempData["Mensaje"] = "Datos guardados correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "El Email ingresado ya se encuentra registrado en el sistema! ";
                    ViewBag.Error = TempData["Error"];
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }
        
        // GET: PropietarioController/Delete/5
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

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                var inmuebles = repoInmueble.BuscarPorPropietario(id);
                entidad = repositorio.ObtenerPorId(id);
                if (inmuebles.Count() == 0)
                {
                    repositorio.Baja(id);
                    TempData["Mensaje"] = "Eliminación realizada correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    ViewBag.Error = "No se puede eliminar el propietario ya que posee inmuebles registrados";
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

        /* [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
       public ActionResult CambiarPass(int id, CambioClaveView cambio)
        {
            Propietario propietario = null;
            try
            {
                propietario = repositorio.ObtenerPorId(id);
                // verificar clave antigüa
                var pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: cambio.ClaveVieja ?? "",
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                if (propietario.Clave != pass)
                {
                    TempData["Error"] = "Clave incorrecta";
                    //se rederige porque no hay vista de cambio de pass, está compartida con Edit
                    return RedirectToAction("Edit", new { id = id });
                }
                if (ModelState.IsValid)
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: cambio.ClaveNueva,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Modificacion(propietario);
                    TempData["Mensaje"] = "Contraseña actualizada correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["Error"] += error.ErrorMessage + "\n";
                        }
                    }
                    return RedirectToAction("Edit", new { id = id });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToAction("Edit", new { id = id });
            }
        }*/

    }
}
