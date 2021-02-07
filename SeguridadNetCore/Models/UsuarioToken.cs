using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeguridadNetCore.Models
{
    public class UsuarioToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }//Toempo de duracion del Token
    }
}
