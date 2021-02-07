Instalar
-Microsoft.AspNetCore.Identity.EntityFrameworkCore
-Microsoft.EntityFrameworkCore.SqlServer
-Microsoft.AspNetCore.Authentication

Luego de la instalacion y configuracuon de tokens
Agregar la informacion del Berer
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


---en el metodo tambien de configuracion implement
 public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();---Metodo agregado ---------------------------
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

Colocar en el Controlador a nivel de clase el siguiente Decorador
Autorice
 [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//using Microsoft.AspNetCore.Authorization;//using Microsoft.AspNetCore.Authentication.JwtBearer;



