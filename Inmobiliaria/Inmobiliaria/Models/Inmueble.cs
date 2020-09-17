using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public int Uso { get; set; }
        [Required]
        public int Tipo { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public Boolean Estado { get; set; }
        [Required]

        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }

       



    }
}
