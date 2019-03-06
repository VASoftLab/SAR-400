using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CostumeController.Robot
{
    public class Robot : IDisposable
    {
        public bool Initialized { get; private set; }

        public string Address
        {
            get
            {
                return $"{_ip}:{_port}";
            }
        }

        private string _ip = "127.0.0.1";
        private int _port = 10099;
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _sr;

        public void Connect()
        {
            if (Initialized)
                return;
            try
            {
                _client = new TcpClient();
                _client.Connect(IPAddress.Parse(_ip), _port);
                _client.Client.DontFragment = true;
                _client.NoDelay = true;
                _client.Client.NoDelay = true;
                _client.ReceiveTimeout = 2000;

                _stream = _client.GetStream();
                _sr = new StreamReader((System.IO.Stream)_stream);

                Initialized = true;
            }
            catch
            {
                Initialized = false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _client.Client.Disconnect(true);
                _client.Close();
                Initialized = false;
            }
            catch
            {
                Initialized = false;
            }
        }

        public void ExecuteCommand()
        {
            if (Initialized == false)
                return;

            try
            {
                string command = "robot:motors:R.Shoulder:posset:90";

                SendData(command);
            }
            catch
            {
                // Индикация ошибки
            }
        }

        private void SendData(string msg)
        {
            try
            {
                if (_client.Connected)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(msg.Trim() + Environment.NewLine);
                    _stream.Write(bytes, 0, bytes.Length);
                    int bytesRead = _stream.ReadByte();
                    return;
                }
            }
            catch
            {
                // TODO
            }
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Initialized)
                    {
                        try
                        {
                            Disconnect();
                        }
                        catch
                        {
                            // Индикация ошибки
                        }
                    }

                    disposedValue = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
