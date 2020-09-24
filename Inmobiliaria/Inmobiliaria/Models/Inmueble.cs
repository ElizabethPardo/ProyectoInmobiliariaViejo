using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public enum enTipo
    {
        Local = 1,
        Deposito = 2,
        Casa= 3,
        Departamento=4, 
    }

    public enum enUso
    {
        Comercial = 1,
        Residencial = 2,

    }
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

        public string TipoNombre => Tipo > 0 ? ((enTipo)Tipo).ToString() : "";
        public static IDictionary<int, string> ObtenerTipos()
        {
            SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
            Type tipoEnum = typeof(enTipo);
            foreach (var valor in Enum.GetValues(tipoEnum))
            {
                tipos.Add((int)valor, Enum.GetName(tipoEnum, valor));
            }
            return tipos;
        }

        public string UsoNombre => Uso > 0 ? ((enUso)Uso).ToString() : "";
        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type usoEnum = typeof(enUso);
            foreach (var valor in Enum.GetValues(usoEnum))
            {
                usos.Add((int)valor, Enum.GetName(usoEnum, valor));
            }
            return usos;
        }



    }
}
