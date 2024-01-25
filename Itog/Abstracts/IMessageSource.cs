using NetMQ;
using System.Net;

namespace Itog.Abstracts
{
    public interface IMessageSource
    {
        Task SendAsync(Message message, IPEndPoint iPEndPoint);
        Message Receive(ref IPEndPoint endPoint);
    }
}
