using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace Modulo13.websockets
{
    /// <summary>
    /// Servicio web para el Chat de WebSockets
    /// </summary>
    public class chat : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(new ChatWebSocket());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ChatWebSocket : WebSocketHandler
    {
        private static WebSocketCollection conexiones = new WebSocketCollection();
        private string nick = "";

        public override void OnClose()
        {
            conexiones.Broadcast("Se ha desconectado " + nick);
            conexiones.Remove(this);
        }

        public override void OnError()
        {
            base.OnError();
        }

        public override void OnMessage(string message)
        {
            conexiones.Broadcast(nick.ToUpper()+": "+message);
            //this.Send("");  //mandaria un mensaje solo a ese mensaje
            //conexiones[1].Send(""); // algo asi para mandar un mensaje a otra persona
        }

        public override void OnOpen()
        {
            nick = WebSocketContext.QueryString["nick"];
            conexiones.Add(this);
            conexiones.Broadcast("Se ha conectado "+nick);
        }

    }
}