using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LabActiveDirectory.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ServicioValidaAD_OpenShift.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public class AutenthicationInfo
        {
            public string dominio { get; set; }
            public string usuario { get; set; }
            public string clave { get; set; }
        }

        //VALIDAR  -> https://localhost:44395/api/login/login?usuario=lpetruzzella&clave=UTBSWmkwSTczVDgyY3hGTThna2liQnh4RElHR3dVMXVHMlUrNXMxQ1dncz0=&dominio=TISAL
        //URL AD LOCAL -> http://labwsad.tisal.cl/UsuarioActiveDirectory/api/v1/

        [Route("auth")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(AutenthicationInfo dataLogin)
        {
            ValidacionUsuario respuesta = new ValidacionUsuario();

            try
            {

            IActionResult response = Unauthorized();

            string urlBase = Environment.GetEnvironmentVariable("LOGIN_BASEURL");
            string urlRelativa = Environment.GetEnvironmentVariable("LOGIN_URLRELATIVA");

            if (String.IsNullOrEmpty(urlBase) || String.IsNullOrEmpty(urlRelativa))
            {
                Console.WriteLine("Error, debe definir variables LOGIN_BASEURL y LOGIN_URLRELATIVA");
                throw new Exception("Error configuración"); 
            }

            Uri uriBase = new Uri(urlBase);
            var client = new ApiClient(uriBase);

            var parametros = new Dictionary<string, string>
            {
                { "dominio", dataLogin.dominio  },
                { "usuario", dataLogin.usuario  },
                { "clave", dataLogin.clave  }
            }; 

            var request = new FormUrlEncodedContent(parametros);
            string url = urlBase + urlRelativa;

            respuesta = client.Post(url, request);
            
            if (respuesta.EstadoValidacion == 1)
            {
                respuesta.Token = GenerateJSONWebToken(respuesta);
            }


            }
            catch(Exception ex) {
                respuesta.EstadoValidacion = 0;
                respuesta.Mensaje = "Ocurrió un error inesperado";
            }

            return Ok(respuesta);
        }

        private string GenerateJSONWebToken(ValidacionUsuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Nombre" , usuario.UsuarioAD.Nombre ),
                new Claim("Intentos_Login_Fallidos", usuario.UsuarioAD.Intentos_Login_Fallidos.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}