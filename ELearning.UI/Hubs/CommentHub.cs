using ELearning.Bl.Models;
using ELearning.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace ELearning.UI.Hubs
{
    public class CommentHub:Hub
    {
        private AppDbContext db;
        public CommentHub(AppDbContext _db)
        {
           db = _db;
        }
        public async Task SendMessage( string message)
        {
            var senderName = Context.User!.Identity!.Name;
            if (message!="")
            {
                var chatMessage = new Message
                {
                    SenderName = senderName,
                    Messages = message,
                    Timestamp = DateTime.UtcNow
                };
                db.Messagess.Add(chatMessage);
                db.SaveChanges();
                await Clients.All.SendAsync("ReceiveMessage", senderName, message);
            }



              
        }
      
    }
}
