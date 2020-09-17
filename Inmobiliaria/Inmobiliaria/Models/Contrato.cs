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

        [Display(Name = "Código")]
        public int IdContrato { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime FechaDesde { get; set; }

        [Display(Name = "Fecha Salida")]
        public DateTime FechaHasta { get; set; }

        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]

        public Inquilino Inquilino { get; set; }

        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [ForeignKey("InmuebleId")]

        public Inmueble Inmueble { get; set; }

       
    }
}
