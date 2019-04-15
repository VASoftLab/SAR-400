using SAR.Control.Costume;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SAR.Control.Robot
{
    public class Robot : IDisposable
    {
        public bool Connected { get; private set; }

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
        private bool _wait;
        private string _retString;
        private CultureInfo _ci = new CultureInfo("en-US");

        public bool Connect()
        {
            if (Connected)
                return true;
            try
            {
                _client = new TcpClient();
                _client.Connect(IPAddress.Parse(_ip), (short)_port);
                _client.Client.DontFragment = true;
                _client.NoDelay = true;
                _client.Client.NoDelay = true;
                _client.ReceiveTimeout = 2000;

                _stream = _client.GetStream();
                _sr = new StreamReader((Stream)_stream);
                Connected = true;

                return true;
            }
            catch
            {
                Connected = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            Connected = false;

            try
            {
                _client.Client.Disconnect(true);
                _client.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public delegate void ErrorOccuredEventHandler(string message);
        public event ErrorOccuredEventHandler ErrorOccured;

        public RobotAnswer ExecuteCommand(List<CostumeJoint> joints)
        {
            // Если робот не покдлючен или количесвтво узлов не совпадает с количеством их конечных значений - прекратить операцию
            if (Connected == false)
                return RobotAnswer.NoConnection;

            try
            {
                // Составить строку команды для заданных узлов и конечных точек
                StringBuilder command = new StringBuilder();
                command.Append("robot:motors:");

                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Name};");
                command.Append(":posset:");

                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Value.ToString(_ci)};");

                // Отправить команду на робота
                return SendData(command.ToString(), 0);
            }
            catch
            {
                return RobotAnswer.ExceptionOccured;
            }
        }

        public RobotAnswer ExecuteCommand(List<CostumeJoint> joints, TimeSpan time)
        {
            // Если робот не покдлючен или количесвтво узлов не совпадает с количеством их конечных значений - прекратить операцию
            if (Connected == false)
                return RobotAnswer.NoConnection;

            try
            {
                // Составить строку команды для заданных узлов и конечных точек
                StringBuilder command = new StringBuilder();
                command.Append("robot:motors:");
                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Name};");

                command.Append(":GO:");
                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Value.ToString(_ci)};");

                float seconds = (float)time.TotalSeconds;

                command.Append($":{seconds}");

                // Отправить команду на робота
                return SendData(command.ToString(), seconds);
            }
            catch(Exception E)
            {
                ErrorOccured?.Invoke("Возникла ошибка при выполнении команды. "+ E.Message);
                return RobotAnswer.ExceptionOccured;
            }
        }

        private RobotAnswer SendData(string msg, float waitTime)
        {
            if (!_client.Connected)
            {
                Connected = false;
                return RobotAnswer.NoConnection;
            }

            _wait = true;
            // Переводим секунды в милисекунды
            int time = (int)(waitTime *1000);

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(msg.Trim() + Environment.NewLine);
                _stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception E)
            {
                Connected = false;
                _wait = false;
                throw new Exception($"Возникла ошибка при отправке пакета данных роботу. {E.Message}");
            }

            System.Threading.Thread.Sleep(time);

            try
            {
                //RobotAnswer exitCode = (RobotAnswer)_stream.ReadByte();
                RobotAnswer exitCode = RobotAnswer.CommandExecuted;
                Connected = true;
                return exitCode;
            }
            catch (Exception E)
            {
                Connected = false;
                _wait = false;
                throw new Exception($"Возникла ошибка при полученни пакета данных от робота. {E.Message}");
            }
        }

        private string GetData(string msg)
        {
            if (_wait)
                return null;

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(msg.Trim() + Environment.NewLine);
                _stream.Write(bytes, 0, bytes.Length);

                if (_stream.ReadByte() != 241)
                    return null;

                _retString = _sr.ReadLine();
                return _retString;
            }
            catch
            {
                return null;
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
                    if (Connected)
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
