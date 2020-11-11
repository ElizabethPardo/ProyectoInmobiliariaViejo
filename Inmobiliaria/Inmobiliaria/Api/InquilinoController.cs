using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext contexto;
     

        public InquilinoController(DataContext contexto)
        {
            this.contexto = contexto;
            
        }

        // GET: api/<InquilinoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                //var res = contexto.Inquilino.Select(x => new { x.Nombre, x.Apellido, x.Email }).SingleOrDefault(x => x.Email == inquilino);
                var res = contexto.Contrato.Include(x => x.Inquilino)
                         .Include(e => e.Inmueble.Duenio)
                         .Where(e => e.Inmueble.Duenio.Email == usuario)
                         .Select(x => x.Inquilino).Distinct()
                         .ToList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<InquilinoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return NotFound();
            var res = contexto.Inquilino.FirstOrDefault(x => x.Id == id);

            if (res != null)
                return Ok(res);
            else
                return NotFound();


        }


        // POST api/<InquilinoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Inquilino inquilino)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    contexto.Inquilino.Add(inquilino);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = inquilino.Id }, inquilino);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<InquilinoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Inquilino.Update(inquilino);
                    contexto.SaveChanges();
                    return Ok(inquilino);
                }

                return BadRequest();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquilinoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool InquilinoExists(int id)
        {
            return contexto.Inquilino.Any(e => e.Id == id);
        }

        // DELETE api/<InquilinoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var p = contexto.Inquilino.Find(id);
                    if (p == null)
                        return NotFound();
                    contexto.Inquilino.Remove(p);
                    contexto.SaveChanges();
                    return Ok(p);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
