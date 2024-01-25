using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Itog
{
    public enum Command
    {
        Register,
        Message,
        Confirnation
    }
    public class Message
    {
        public int? MessageId { get; set; }
        public string? Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool isSent { get; set; }
        public int? UserToId { get; set; }
        public int? UserFromId { get; set; }     
        public string? NickNameFrom { get; set; }
        public string? NickNameTo { get; set; }
        public IPEndPoint? EndPoint { get; set; }
        public virtual User UserTo {  get; set; }
        public virtual User UserFrom { get; set; }

        public Command Command { get; set; }
        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public static Message? DeserializeFromJson(string message)
        {
            return JsonSerializer.Deserialize<Message>(message);
        }
        public void Print()
        {
            Console.WriteLine($"Cообщение от: {this.UserToId}");
            for (int i = 0; i < Text.Length; i++)
            {
                Console.Write($"{Text[i]}");
                Thread.Sleep(100);
            }
            Console.Write("\n");
        }
    }
}
