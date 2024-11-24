using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GestordeTareas.UI.Hubs
{
    public class ChatHub : Hub
    {
        // Método para enviar mensajes a todos los clientes conectados
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Método para notificar que alguien está escribiendo
        public async Task NotifyTyping(string user)
        {
            await Clients.Others.SendAsync("UserTyping", user);
        }
    }
}
