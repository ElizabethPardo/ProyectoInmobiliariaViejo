using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorEmail(string email);
        int ModificarPass(Usuario u);
    }
}
