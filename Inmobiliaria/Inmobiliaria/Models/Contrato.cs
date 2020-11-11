using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaDesde { get; set; }

        [Required]
        [Display(Name = "Fecha Salida")]
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }

        [Required]
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]
        public Inquilino Inquilino { get; set; }

        [Required]
        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [ForeignKey("InmuebleId")]

        public Inmueble Inmueble { get; set; }

       
    }
}
