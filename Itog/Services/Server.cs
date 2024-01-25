using Itog.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Itog.Services
{
    public class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();

        private  readonly IMessageSource messageSource;
        IPEndPoint ep;

        public Server()
        {
            messageSource = new UdpMessageSource();
            ep = new IPEndPoint(IPAddress.Any, 0);
        }

        async Task Register(Message mess)
        {
            Console.WriteLine($" Message Register name = {mess.UserFrom.FullName}");

            if(clients.TryAdd(mess.NickNameFrom, mess.EndPoint))
            {
                using(ChatContext context = new ChatContext())
                {
                    context.Users.Add(new User(){FullName = mess.NickNameFrom});
                    await context.SaveChangesAsync();
                }
            }
        }

        async Task RelyMessage(Message message)
        {
            
            if (clients.TryGetValue(message.NickNameTo, out IPEndPoint IPEnd))
            {
                int? id = 0;
                using (var ctx = new ChatContext())
                {
                    var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                    var toUser = ctx.Users.First(x=>x.FullName == message.NickNameFrom);
                    var msg = new Message { UserFrom = fromUser, UserTo = toUser, isSent = false, Text=message.Text };
                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();
                    id = msg.MessageId;
                }

                message.MessageId = id;
                await messageSource.SendAsync(message, IPEnd);

                Console.WriteLine($"Message Relied, from = {message.NickNameFrom} to = {message.NickNameTo}");

            }
            else
            {
                Console.WriteLine("Пользоавтель не найден");
            }
        }

        async Task ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);
            using (var ctx = new ChatContext())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.MessageId == id);

                if(msg != null)
                {
                    msg.isSent = true;
                    await ctx.SaveChangesAsync();
                }
            }
        }

        async Task ProccessMessage(Message message)
        {
            switch (message.Command)
            {
                case Command.Register:await Register(message); break;
                case Command.Message:await RelyMessage(message); break;
                case Command.Confirnation:await ConfirmMessageReceived(message.MessageId); break;
            }
        }

        public async Task Start() 
        {
            Console.WriteLine("Сервер ожидает сообщения");

            while (true)
            {
                try
                {
                    var message = messageSource.Receive(ref ep);
                    Console.WriteLine(message.ToString());
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void Stop()
        {

        }
    }
}
