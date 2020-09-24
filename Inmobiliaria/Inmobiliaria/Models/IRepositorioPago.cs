using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> BuscarPorContrato(int id);
        int Alta(Pago p,int id);
    }
}
