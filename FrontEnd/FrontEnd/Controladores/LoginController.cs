using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LabActiveDirectory.Entidades;
using FrontEnd.Modelos;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontEnd.Controladores
{
    public class LoginController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            ValidacionUsuario respuesta = new ValidacionUsuario();

            try
            {

                IActionResult response = Unauthorized();

                string urlBase = Environment.GetEnvironmentVariable("SERVICIOAD_BASEURL");
                string urlRelativa = Environment.GetEnvironmentVariable("SERVICIOAD_URLRELATIVA");

                if (String.IsNullOrEmpty(urlBase) || String.IsNullOrEmpty(urlRelativa))
                {
                    Console.WriteLine("Error, debe definir variables SERVICIOAD_BASEURL y SERVICIOAD_URLRELATIVA");
                    throw new Exception("Error configuración");
                }

                Uri uriBase = new Uri(urlBase);
                var client = new ApiClient(uriBase);

                var parametros = new Dictionary<string, string>
                {
                    { "dominio", dominio },
                    { "usuario", usuario },
                          { "clave", clave }
                };

                var request = new FormUrlEncodedContent(parametros);
                string url = urlBase + urlRelativa;

                respuesta = client.Post(url, request);

                if (respuesta.EstadoValidacion == 1)
                {
                    respuesta.Token = GenerateJSONWebToken(respuesta);
                }   


            }
            catch (Exception)
            {
                respuesta.EstadoValidacion = 0;
                respuesta.Mensaje = "Ocurrió un error inesperado";
            }

            return Ok(respuesta);



        }
    }
}
