using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeguridadNetCore.Models;

namespace SeguridadNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ///[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//using Microsoft.AspNetCore.Authorization;//using Microsoft.AspNetCore.Authentication.JwtBearer;
    public class CuentaController : ControllerBase
    {
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly SignInManager<UsuarioAplicacion> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentaController(UserManager<UsuarioAplicacion> userManager,
            SignInManager<UsuarioAplicacion> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }


        [HttpPost("Crear")]
        public async Task<ActionResult<UsuarioToken>> CrearUsuario ([FromBody] UsuarioInfo model)
        {
            var user = new UsuarioAplicacion { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return CrearToken(model,new List<string>());
            }
            else
            {
                return BadRequest("Usuario o contraseña invalida");
            }
        }
        public UsuarioToken CrearToken(UsuarioInfo userInfo,IList<string> roles)
        {
            var claims = new List<Claim>//using System.Security.Claims;
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.Email),//using System.IdentityModel.Tokens.Jwt;
                new Claim("miValor","Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,item));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));//using System.Text;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//using Microsoft.IdentityModel.Tokens;

            //Tiempo de expiracion del token
            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer:null,
                audience:null,
                claims:claims,
                expires:expiration,
                signingCredentials:creds
                );
            return new UsuarioToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
