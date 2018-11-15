using LabActiveDirectory.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tisal.Cryptography;

namespace Clases
{
    public class ChatHub : Hub
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        public void SendToAll(string name, string message)
        {
            String fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Clients.All.SendAsync("sendToAll", name, message,fecha);
        }

        public void Login(string dominio, string usuario,string clave)
        {
            string claveEncriptada = "";
            
            string secret = Environment.GetEnvironmentVariable("ENCRIPT_SECRET");

            claveEncriptada = Helper.Instance.Encripta(clave, secret);
            var plainTextBytes = Encoding.UTF8.GetBytes(claveEncriptada);
            claveEncriptada = Convert.ToBase64String(plainTextBytes);

            ValidacionUsuario respuesta = new ValidacionUsuario();

            try
            {
                string urlBase = Environment.GetEnvironmentVariable("LOGIN_BASEURL");
                string urlRelativa = Environment.GetEnvironmentVariable("LOGIN_URLRELATIVA");

                if (String.IsNullOrEmpty(urlBase) || String.IsNullOrEmpty(urlRelativa))
                {
                    Console.WriteLine("Error, debe definir variables LOGIN_BASEURL y LOGIN_URLRELATIVA");
                    throw new Exception("Error configuración");
                }

                Uri uriBase = new Uri(urlBase);
                var client = new ApiClient(uriBase);

                string parametros = "{\"dominio\" : \"" + dominio + "\",\"usuario\" : \"" + usuario + "\", \"clave\" : \"" + claveEncriptada + "\"}";
                StringContent json = new StringContent(parametros, Encoding.UTF8, "application/json");
                    
                string url = urlBase + urlRelativa;

                respuesta = client.Post(url, json);
            }
            catch (Exception ex)
            {
                respuesta.EstadoValidacion = 0;
                respuesta.Mensaje = "Ocurrió un error inesperado." + ex.Message;
            }

            if (respuesta.EstadoValidacion == 1)
            {
                Clients.Caller.SendAsync("loginResult", 1, respuesta.UsuarioAD.Nombre,respuesta.UsuarioAD.Cuenta , respuesta.Token );
              
            }
            else
            {
                Clients.Caller.SendAsync("loginResult", 0,respuesta.Mensaje );
            }
                       
        }
        
    }
}
