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
		[Required]
		public string Telefono { get; set; }
		[Required]
		public string Direccion { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Lugar de Trabajo")]
		public string LugarTrabajo { get; set; }
		
		[Required]
		[Display(Name = "Nombre Garante")]
	
		public string NombreGarante { get; set; }

		[Display(Name = "Apellido Garante")]
		[Required]
		public string ApellidoGarante { get; set; }

		[Display(Name = "Dni Garante")]
		[Required]
		public string DniGarante { get; set; }

		[Display(Name = "Telefono Garante")]
		[Required]
		public string TelefonoGarante { get; set; }
		
		[Required]
		[Display(Name = "Dirección Garante")]
		public string DireccionGarante { get; set; }
		

	}
}
