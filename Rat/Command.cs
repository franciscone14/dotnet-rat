using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Rat
{
    public class Command
    {
        private readonly Socket _client;
        private readonly IPEndPoint _ipe;

        public Command()
        {
            _ipe = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 99);
            _client = new Socket(_ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public string Connect()
        {
            if (!_client.Connected)
            {
                _client.Connect(_ipe);
                return "Connected";
            }
            return "Already Connected";
        }

        public string CloseConnection()
        {
            if (_client.Connected)
            {
                _client.Close();
                return "Closed";
            }
            return "No Connection!";
        }
    }
}
