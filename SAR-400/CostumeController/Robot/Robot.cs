﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
                _client.Connect(IPAddress.Parse(_ip), (short) _port);
                _client.Client.DontFragment = true;
                _client.NoDelay = true;
                _client.Client.NoDelay = true;
                _client.ReceiveTimeout = 2000;

                _stream = _client.GetStream();
                _sr = new StreamReader((System.IO.Stream)_stream);
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

        public bool ExecuteCommand(CostumeJoint[] joints, double[] endPoints)
        {
            // Если робот не покдлючен или количесвтво узлов не совпадает с количеством конечных точек - прекратить операцию
            if (Connected == false || joints.Length != endPoints.Length)
                return false;

            try
            {
                // Составить строку команды для заданных узлов и конечных точек
                StringBuilder command = new StringBuilder();
                command.Append("robot:motors:");
                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Name};");
                command.Append(":posset:");
                foreach (double endPoint in endPoints)
                    command.Append($"{endPoint.ToString(_ci)};");

                Answer result;
                // Отправить команду на робота
                bool dataSended = SendData(command.ToString(), out result);

                return dataSended;
            }
            catch
            {
                return false;
            }
        }

        public bool ExecuteCommand(CostumeJoint[] joints, double[] endPoints, int time)
        {
            // Если робот не покдлючен или количесвтво узлов не совпадает с количеством их конечных значений - прекратить операцию
            if (Connected == false || joints.Length != endPoints.Length)
                return false;

            try
            {
                // Составить строку команды для заданных узлов и конечных точек
                StringBuilder command = new StringBuilder();
                command.Append("robot:motors:");
                foreach (CostumeJoint joint in joints)
                    command.Append($"{joint.Name};");
                command.Append(":GO:");
                foreach (double endPoint in endPoints)
                    command.Append($"{endPoint.ToString(_ci)};");
                command.Append($":{time}");

                Answer result;
                // Отправить команду на робота
                bool dataSended = SendData(command.ToString(), out result);

                return dataSended;
            }
            catch
            {
                return false;
            }
        }

        private bool SendData(string msg, out Answer exitCode)
        {
            try
            {
                _wait = true;
                if (_client.Connected)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(msg.Trim() + Environment.NewLine);
                    _stream.Write(bytes, 0, bytes.Length);
                    exitCode = (Answer)_stream.ReadByte();

                    Connected = true;
                    return true;
                }
                exitCode = Answer.ClientIsNotConnected;

                Connected = false;
                return false;
            }
            catch (Exception E)
            {
                string mes = E.Message;
                exitCode = Answer.ExceptionOccured;
                Connected = false;
                _wait = false;
                return false;
            }
        }

        private string GetData(string msg)
        {
            if (_wait)
                return null;

            byte[] bytes = Encoding.ASCII.GetBytes(msg.Trim() + Environment.NewLine);
            _stream.Write(bytes, 0, bytes.Length);

            if (_stream.ReadByte() != 241)
                return null;

            _retString = _sr.ReadLine();
            return _retString;
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

    enum Answer
    {
        ClientIsNotConnected = 0x00,
        ExceptionOccured = 0x01,
        SyntaxError = 0xFF,
        DeviceUnavailable = 0xFE,
        UnkonwnIndentifier = 0xFD,
        IncorrectInputValue = 0xFC,
        CommandExecuted = 0xF0,
        CommandExecutedWithReturnResult = 0xF1,
        CommandIsObsolete = 0xF2,
        CommandIsNotSupportedByDevice = 0xF3,
        CommandIsNotSupportedBySoftware = 0xF4,
        MaxAmountOfConnectionsAchieved = 0xFA
    }
}
