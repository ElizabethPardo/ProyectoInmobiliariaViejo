using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorPropietario(int idPropietario);
        IList<Contrato> BuscarPorContrato(int idInmueble);
        int Alta(Inmueble e, int id);
        IList<Inmueble> BuscarDisponibles();
        IList<Inmueble> BuscarInmueblesDisponibles(DateTime inicio, DateTime fin);
    }
}
