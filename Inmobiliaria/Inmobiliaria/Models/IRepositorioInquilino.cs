using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        Inquilino ObtenerPorEmail(string email);
        IList<Inquilino> BuscarPorNombre(string nombre);
    }
}
