using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeguridadNetCore.Contexto
{
    //using Microsoft.AspNetCore.Identity.EntityFrameworkCore; Instalar para utilizar
    // =>IdentityDbContext
    public class ApplicationDBContext: IdentityDbContext<Models.UsuarioAplicacion>
    {
        //using Microsoft.EntityFrameworkCore; =>DbContextOptions
        //Inyeccion de la configuracion
        //Realizamos el super
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }
    }
}
