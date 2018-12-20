using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;
using Modulo13.Models;
using Newtonsoft.Json;

namespace Modulo13.websockets
{
    /// <summary>
    /// Summary description for ejercicio
    /// </summary>
    public class ejercicio : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.AcceptWebSocketRequest(new Ejercicio());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class Ejercicio: WebSocketHandler
    {
        private ModelNorthwind db = new ModelNorthwind();
        private static WebSocketCollection conexiones = new WebSocketCollection();
        private int id;

        public override void OnClose()
        {
            conexiones.Remove(this);
        }

        public override void OnError()
        {
            base.OnError();
        }

        public override void OnMessage(string message)
        {
            db.Configuration.LazyLoadingEnabled = false;
            id = Convert.ToInt32(message);
            var result = db.Products.Where(r => r.ProductID == id).FirstOrDefault();
            //Comprobar si result es vacio o no
            this.Send(JsonConvert.SerializeObject(result));
        }

        public override void OnOpen()
        {
            conexiones.Add(this);
        }
    }
}