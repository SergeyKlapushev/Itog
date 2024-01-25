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
    internal class UdpMessageSource : IMessageSource
    {
        private readonly UdpClient _unpClient;

        public UdpMessageSource()
        {
            _unpClient = new UdpClient();
        }
        public Message Receive(ref IPEndPoint endPoint)
        {
            byte[] data = _unpClient.Receive(ref endPoint);
            string str = Encoding.UTF8.GetString(data);
            return Message.DeserializeFromJson(str) ?? new Message();
        }

        public async Task SendAsync(Message message, IPEndPoint iPEndPoint)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message.SerializeToJson());

            await _unpClient.SendAsync(buffer, buffer.Length, iPEndPoint);
        }
    }
}
