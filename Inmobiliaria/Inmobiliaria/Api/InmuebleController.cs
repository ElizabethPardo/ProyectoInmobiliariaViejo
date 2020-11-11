using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext contexto;

        public InmuebleController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<InmuebleController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var res = contexto.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<InmuebleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario).Single(e => e.Id == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<InmuebleController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.PropietarioId = contexto.Propietario.Single(e => e.Email == User.Identity.Name).Id;
                    contexto.Inmueble.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<InmuebleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid && contexto.Inmueble.AsNoTracking().Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name) != null)
                {
                    entidad.Id = id;
                    contexto.Inmueble.Update(entidad);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<InmuebleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    contexto.Inmueble.Remove(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("BajaLogica/{id}")]
        public async Task<IActionResult> BajaLogica(int id)
        {
            try
            {
                var entidad = contexto.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    entidad.Tipo = -1;//cambiar por estado = 0
                    contexto.Inmueble.Update(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("disponibles")]
        public async Task<IActionResult> InmueblesDisponibles()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario && e.Estado).ToList());

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("porFechas")]
        public async Task<IActionResult> BuscarInmueblesPorFecha([FromForm]BusquedaPorFechas busqueda)
        {
            try
            {
                var usuario = User.Identity.Name;
                var res = contexto.Contrato.Include(e => e.Inmueble.Duenio)
                    .Where(e => e.Inmueble.Duenio.Email == usuario && (busqueda.FechaInicio <= e.FechaDesde || busqueda.FechaInicio <=  e.FechaHasta) && (busqueda.FechaFin >= e.FechaHasta || busqueda.FechaFin >= e.FechaHasta))
                    .Select(x => new {x.Inmueble.Direccion, x.Inmueble.Ambientes,x.Inmueble.UsoNombre,x.Inmueble.TipoNombre,x.Inquilino.Nombre })
                    .ToList();
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
