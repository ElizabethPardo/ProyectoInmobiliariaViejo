using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int IdPago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Importe { get; set; }

        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }
        [ForeignKey("ContratoId")]
        public Contrato Contrato { get; set; }
    }
}
