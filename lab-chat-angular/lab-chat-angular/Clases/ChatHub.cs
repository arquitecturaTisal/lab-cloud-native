using LabActiveDirectory.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Tisal.Cryptography;

namespace Clases
{
    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            String fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Clients.All.SendAsync("sendToAll", name, message,fecha);
        }

        public void Login(string dominio, string usuario,string clave)
        {
            string claveEncriptada = "";
            
            //string iv = "tisal.2018_security-PrivateKey#p";
            string secret = "tisal.2018_security+PublicKey!#d";

            claveEncriptada = Tisal.Cryptography.Helper.Instance.Encripta(clave, secret);

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

                var parametros = new Dictionary<string, string>
                {
                    { "dominio", dominio  },
                    { "usuario", usuario  },
                    { "clave", claveEncriptada  }
                };

                var request = new FormUrlEncodedContent(parametros);
                string url = urlBase + urlRelativa;

                respuesta = client.Post(url, request);
            }
            catch (Exception ex)
            {
                respuesta.EstadoValidacion = 0;
                respuesta.Mensaje = "Ocurrió un error inesperado." + ex.Message;
            }

            if (respuesta.EstadoValidacion == 1)
            {
                Clients.Caller.SendAsync("loginResult", 1, "OK");
            }
            else
            {
                Clients.Caller.SendAsync("loginResult", 0, "OK");
            }
                       
        }
        
    }
}
