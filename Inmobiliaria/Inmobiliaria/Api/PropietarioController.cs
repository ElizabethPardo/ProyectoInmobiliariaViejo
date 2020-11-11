using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietarioController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        public PropietarioController(DataContext contexto, IConfiguration config) 
        {
            this.contexto = contexto;
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propietario>>> Get()
        {
            try
            {
                /*contexto.Inmuebles
                    .Include(x => x.Duenio)
                    .Where(x => x.Duenio.Nombre == "")//.ToList() => lista de inmuebles
                    .Select(x => x.Duenio)
                    .ToList();//lista de propietarios
                var usuario = User.Identity.Name;
                var res = contexto.Propietario.Select(x => new { x.Nombre, x.Apellido, x.Email }).SingleOrDefault(x => x.Email == usuario);
                return Ok(res); */
                // return contexto.Propietario;

                var usuario = User.Identity.Name;
                var res = contexto.Propietario.Select(x => new { x.Nombre, x.Apellido, x.Dni, x.Direccion, x.Telefono, x.Email }).SingleOrDefault(x => x.Email == usuario);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    

        // GET api/<PropietarioController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return NotFound();
            var res = contexto.Propietario.FirstOrDefault(x => x.Id == id);
            if (res != null)
                return Ok(res);
            else
                return NotFound();


        }

        // POST api/<PropietarioController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]Propietario propietario)
        {
            /*contexto.Propietario.Add(propietario);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetPropietario", new { id = propietario.IdPropietario }, propietario);*/
            try
            {

                if (ModelState.IsValid)
                {
                    contexto.Propietario.Add(propietario);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = propietario.Id }, propietario);
                }

                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }



        }

        // PUT api/<PropietarioController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]Propietario propietario)
        {
            /*if (id != propietario.Id) 
            {
                return BadRequest();
            }*/

            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Propietario.Update(propietario);
                    contexto.SaveChanges();
                    return Ok(propietario);
                }

                return BadRequest();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (!PropietarioExists(id))
                {
                    return NotFound();
                }
                else 
                {
                    throw;
                }
            }

            
        }

        private bool PropietarioExists(int id)
        {
            return contexto.Propietario.Any(e=>e.Id == id);
        }

        // DELETE api/<PropietarioController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    var p = contexto.Propietario.Find(id);
                    if (p == null)
                        return NotFound();
                    contexto.Propietario.Remove(p);
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


        // GET api/<controller>/5
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = contexto.Propietario.FirstOrDefault(x => x.Email == loginView.Usuario);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
