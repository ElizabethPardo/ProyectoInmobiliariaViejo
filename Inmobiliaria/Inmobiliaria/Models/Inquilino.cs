using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inquilino
    {
		[Key]
		[Display(Name = "Código")]
		public int IdInquilino { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		public string Telefono { get; set; }
		public string Direccion { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
		public string LugarTrabajo { get; set; }
		public string NombreGarante { get; set; }
		public string ApellidoGarante { get; set; }
		public string DniGarante { get; set; }
		public string TelefonoGarante { get; set; }
		public string DireccionGarante { get; set; }

	}
}
