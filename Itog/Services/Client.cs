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
    internal class Client
    {
        string name;
        string address;
        int port;
        readonly IMessageSource messageSource;
        IPEndPoint remoteEndPoint;

        public Client(string name, string address, int port)
        {
            this.name = name;
            this.address = address;
            this.port = port;

            messageSource = new UdpMessageSource();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), 12345);
        }

        UdpClient udpClient = new UdpClient();
        async Task ClientListener()
        {
            

            while(true)
            {
                try
                {
                    var messageReceived = messageSource.Receive(ref remoteEndPoint);

                    Console.WriteLine($"Получено сообщение от {messageReceived.NickNameFrom}:");
                    Console.WriteLine(messageReceived.Text);

                    await Confirm(messageReceived, remoteEndPoint);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Ошибка при получении сообщения: " + ex.Message);
                }
            }
        }

        async Task Confirm(Message mess, IPEndPoint remoteEndPoint)
        {
            mess.Command = Command.Confirnation;
            messageSource.SendAsync(mess, remoteEndPoint);
        }

        void Register(IPEndPoint remoteEndPoint)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            var message = new Message() { NickNameFrom = name, NickNameTo = null, Text = null, Command = Command.Register, EndPoint = ep };
            messageSource.SendAsync(message, remoteEndPoint);
        }

        async Task ClientSender()
        {
            Register(remoteEndPoint);
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите имя получателя: ");
                    var nameTo = Console.ReadLine();
                    Console.WriteLine("UDP Клиент ожидает ввода сообщения");


                    Console.WriteLine("Введите сообщение и нажмите Enter: ");
                    var messageText = Console.ReadLine();
                    var message = new Message()
                    {
                        Command = Command.Message,
                        NickNameFrom = name,
                        NickNameTo = nameTo,
                        Text = messageText
                    };

                    await messageSource.SendAsync(message, remoteEndPoint);
                    Console.WriteLine("Сообщение отправлено.");
                }                
                catch(Exception ex) 
                {
                    Console.WriteLine("Ошибка при отправке сообщение: "+ex.Message);
                }
            }
        }
        public async Task Start()
        {
            await ClientListener();
            await ClientSender();
        }
    }
}
