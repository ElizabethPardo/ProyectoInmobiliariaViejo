using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorPropietario(int idPropietario);
        int Alta(Inmueble e, int id);
    }
}
