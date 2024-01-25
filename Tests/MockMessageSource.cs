using System.ServiceModel.Channels;
using System.Net;
using Itog.Abstracts;
using Itog.Services;
using Itog;

namespace Tests
{
    public class MockMessageSource : IMessageSource
    {
        private Queue<Itog.Message> messages = new();
        private Server server;
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

        public MockMessageSource()
        {
            messages.Enqueue(new Itog.Message { Command = Command.Register, NickNameFrom = "Вася" });
            messages.Enqueue(new Itog.Message { Command = Command.Register, NickNameFrom = "Юля" });
            messages.Enqueue(new Itog.Message { Command = Command.Message, NickNameFrom = "Юля", NickNameTo = "Вася", Text = "Ёу собака" });
            messages.Enqueue(new Itog.Message { Command = Command.Message, NickNameFrom = "Вася", NickNameTo = "Юля", Text = "Я Наруто узумака" });
        }

        public Itog.Message Receive(ref IPEndPoint ep)
        {
            ep = endPoint;
            if(messages.Count == 0)
            {
                server.Stop();
                return null;
            }
            return messages.Dequeue();
        }

        public void AddSever(Server serv)
        {
            server = serv;
        }

        public Task SendAsync(Itog.Message message, IPEndPoint iPEndPoint)
        {
            throw new NotImplementedException();
        }
    }
}
