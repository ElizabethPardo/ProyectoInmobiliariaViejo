using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        int ValidarDisponibilidad(DateTime fechaDesde, DateTime fechaHasta, int inmuebleId);
        IList<Contrato> ContratosVigentes();
       
    }
}
