using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class ContratoController : ControllerBase
    {
        private readonly DataContext contexto;

        public ContratoController(DataContext contexto)
        {
            this.contexto = contexto;
        }
        // GET: api/<ContratoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var res=contexto.Contrato.Include(x => x.Inquilino).Include(x => x.Inmueble).ThenInclude(x => x.Duenio).Where(c => c.Inmueble.Duenio.Email == usuario);

                return Ok(res);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }

        }

        // GET api/<ContratoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var res = contexto.Contrato.Include(x => x.Inquilino)
                    .Include(x => x.Inmueble)
                    .ThenInclude(x => x.Duenio)
                    .Where(c => c.Inmueble.Duenio.Email == usuario)
                    .Single(e => e.Id == id);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<ContratoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contrato.InmuebleId = contexto.Inmueble.FirstOrDefault(e => e.Duenio.Email == User.Identity.Name).Id;
                    contrato.InquilinoId = contexto.Inquilino.Single(e => e.Id == contrato.InquilinoId).Id;
                    contexto.Contrato.Add(contrato);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id =contrato.Id }, contrato);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<ContratoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid && contexto.Contrato.AsNoTracking().Include(e => e.Inmueble).ThenInclude(x => x.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name) != null)
                {
                   
                    contrato.Id = id;
                    contexto.Contrato.Update(contrato);
                    contexto.SaveChanges();
                    return Ok(contrato);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    

        // DELETE api/<ContratoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Contrato.Include(e => e.Inmueble.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name); ;
                if (entidad != null)
                {
                    contexto.Contrato.Remove(entidad);
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

        public async Task<IActionResult> GetPropietariosVigentes() 
        {

            return Ok();
        }

    }
}
