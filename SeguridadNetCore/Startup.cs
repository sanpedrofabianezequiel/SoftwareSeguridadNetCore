using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SeguridadNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //UseSqlServer => Instalar  Microsoft.EntityFrameworkCore.SqlServer
            services.AddDbContext<Contexto.ApplicationDBContext>( options  => options.UseSqlServer(Configuration.GetConnectionString("SQLCONEX")));
            //using Microsoft.AspNetCore.Identity; Servicio de Identiti que se asocia para trabajar con TOKENS
            services.AddIdentity<Models.UsuarioAplicacion, IdentityRole>()//using Microsoft.AspNetCore.Identity;
                   .AddEntityFrameworkStores<Contexto.ApplicationDBContext>()
                   .AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//using Microsoft.AspNetCore.Authentication.JwtBearer;
                    .AddJwtBearer(
                    options =>
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer=false,
                        ValidateAudience=false,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey= new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                        ClockSkew=TimeSpan.Zero
                    }
                );
                
                
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
