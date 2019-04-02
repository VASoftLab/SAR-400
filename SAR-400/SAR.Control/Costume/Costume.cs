using SAR.Control.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAR.Control.Costume
{
    public class Costume : IDisposable
    {
        const int MaxPPS = 500;
        const int BufSZ = 310;

        #region Публичные переменные
        public string ID { get; private set; }
        public bool Initialized { get; private set; }
        public List<CostumeJoint> Joints
        {
            get
            {
                return _joints;
            }
        }
        /// <summary>
        /// Количество пакетов данных с робота, принимаемых в секунду.
        /// </summary>
        public int PackPerSecond
        {
            get
            {
                return _pps;
            }
            set
            {
                _pps = value > MaxPPS ? MaxPPS : value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Сетевой адрес робота в сети.
        /// </summary>
        public string Address
        {
            get
            {
                return $"{_ip}:{_port}";
            }
        }

        public Quaternion Rotation { get; private set; }
        public CostumeVoltage Voltage { get; } = new CostumeVoltage();
        #endregion

        #region События
        public delegate void DataChangedEventHandler();
        public event DataChangedEventHandler DataChanged;
        #endregion

        protected Encoding idEncoding = ASCIIEncoding.ASCII;
        protected object bufLockTX = new object();
        protected byte[] bufTX = new byte[BufSZ];

        #region Приватные переменные
        private string _ip = "127.0.0.1";
        private int _port = 10003;
        private int _pps;

        private List<CostumeJoint> _joints = new List<CostumeJoint>();
        private EventWaitHandle exchangeDone = new EventWaitHandle(false, EventResetMode.AutoReset);
        #endregion

        public void Initialize(string pathToConfig)
        {
            if (Initialized)
                return;

            _joints.Clear();

            if (!System.IO.File.Exists(pathToConfig))
                throw new Exception($"Файл {pathToConfig} не найден.");

            XDocument xDoc;
            xDoc = XDocument.Load(pathToConfig);

            try
            {
                var element = XMLReader.XElementByName(xDoc, "costume");

                if (element == null)
                    return;

                ID = XMLReader.XElementAttributeValueByName(element, "id");
                _ip = XMLReader.XElementAttributeValueByName(element, "ip");

                int.TryParse(XMLReader.XElementAttributeValueByName(element, "port"), out _port);
                int.TryParse(XMLReader.XElementAttributeValueByName(element, "pps"), out _pps);

                float ofs = 0;
                RawLoad(XMLReader.XElementByName(xDoc, "voltage"), Voltage.Raw, ref Voltage.valueScaler, ref ofs);

                element = XMLReader.XElementByName(xDoc, "joints");
                if (element != null)
                {
                    foreach (var item in XMLReader.XElementsByName(element, "joint"))
                    {
                        var joint = new CostumeJoint() { Name = XMLReader.XElementAttributeValueByName(item, "name"), };

                        RawLoad(item, joint.Raw, ref joint.valueScaler, ref joint.rawValueOffset);
                        _joints.Add(joint);
                    }
                }

                // Инициализируем контроллеры на линии
                for (int i = 0; i < 7; i++)
                {
                    byte idx = (byte)(i + 1);
                    bufTX[16 * i] = idx;
                    bufTX[16 * i + 128] = idx;
                }

                Initialized = true;

                new Thread(Exchange).Start();
            }
            finally
            {
                xDoc = null;
            }
        }

        private void RawLoad(XElement element, PackedData pack, ref float scaler, ref float offset)
        {
            if (element == null)
                return;

            scaler = Utility.ConvertToFloat(XMLReader.XElementAttributeValueByName(element, "scaler"));
            var item = XMLReader.XElementByName(element, "raw");
            if (item == null)
                return;

            offset = Utility.ConvertToFloat(XMLReader.XElementAttributeValueByName(item, "offset"));
            int.TryParse(XMLReader.XElementAttributeValueByName(item, "index"), out pack.index);
            switch (XMLReader.XElementAttributeValueByName(item, "type"))
            {
                case "_byte": pack.type = DataType._byte; break;
                case "_int16": pack.type = DataType._int16; break;
                case "_int32": pack.type = DataType._int32; break;
                case "_float": pack.type = DataType._float; break;
            }
        }

        public void Exchange()
        {
            long lastTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                UdpClient server = new UdpClient(_port);
                server.BeginReceive(new AsyncCallback(ReceiveCallback), server);

                UdpClient client = new UdpClient();
                try
                {
                    while (Initialized)
                    {
                        var currentPPS = _pps;
                        if (currentPPS <= 0)
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        lock (bufLockTX)
                        {
                            client.Send(bufTX, bufTX.Length, _ip, _port);
                        }

                        int dt = Utility.Round(1000f / currentPPS) - (int)(sw.ElapsedMilliseconds - lastTime);

                        if (dt > 0)
                            Thread.Sleep(dt);

                        lastTime = sw.ElapsedMilliseconds;
                    }
                }
                finally
                {
                    server.Close();
                }
            }
            finally
            {
                exchangeDone.Set();
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var server = (UdpClient)(ar.AsyncState);
            if (server == null || server.Client == null)
                return;

            var addr = new IPEndPoint(0, 0);
            var receiveBytes = server.EndReceive(ar, ref addr);
            if (receiveBytes == null)
                return;

            if (receiveBytes.Length == BufSZ)
                OnData(receiveBytes);
            else
                throw new Exception(string.Format("Unexpected recive bytes length ({0} of {1}).", receiveBytes.Length, BufSZ));

            if (Initialized)
                server.BeginReceive(new AsyncCallback(ReceiveCallback), server);
        }

        protected virtual void OnData(byte[] buffer)
        {
            // Update rotation data
            //Rotation = new Quaternion(BitConverter.ToSingle(buffer, 292), BitConverter.ToSingle(buffer, 296), BitConverter.ToSingle(buffer, 300), BitConverter.ToSingle(buffer, 288));
            //var e = Quaternion.ToEulerAngles(Rotation);

            MathNet.Spatial.Euclidean.Quaternion rotation = new MathNet.Spatial.Euclidean.Quaternion(BitConverter.ToSingle(buffer, 288), BitConverter.ToSingle(buffer, 292), BitConverter.ToSingle(buffer, 296), BitConverter.ToSingle(buffer, 300));
            var e = rotation.ToEulerAngles();
            // Update joints value
            foreach (var item in _joints)
            {
                if (item == null)
                    continue;

                //Проверяем на предустановленные служебные индексы
                switch (item.Raw.index)
                {
                    //Индекс вращение вокруг оси x
                    case -10:
                        //var v = e.x;
                        //item.Raw.Value = v > 180 ? v - 360 : v;
                        item.Value = (float)e.Alpha.Degrees;
                        continue;
                    //Индекс вращение вокруг оси y
                    case -11:
                        //v = e.y;
                        //item.Raw.Value = v > 180 ? v - 360 : v;
                        item.Value = (float)e.Beta.Degrees;
                        continue;
                    //Индекс вращение вокруг оси z
                    case -12:
                        //v = e.z;
                        //item.Raw.Value = v > 180 ? v - 360 : v;
                        item.Value = (float)e.Gamma.Degrees;
                        continue;

                }

                item.Raw.ReadValue(buffer);
                item.Update();
            }

            // Update voltage value
            Voltage.Raw.ReadValue(buffer);
            Voltage.Update();

            DataChanged();
        }

        public float GetValue(string jointName)
        {
            // TODO: Исправить на другое значение
            if (Joints == null || Joints.Count == 0)
                return 0;

            try
            {
                return Joints.Where(x => x.Name == jointName).FirstOrDefault().Value;
            }
            catch
            {
                return 0;
            }
        }

        public string GetCostumeSnapshot()
        {
            string snapshot = string.Empty;

            float[] values = _joints.GetValuesArray();

            snapshot = string.Join(";", values);

            return snapshot;
        }

        public string GetJointNames()
        {
            string names = string.Empty;

            string[] jointNames = _joints.GetNamesArray();

            names = string.Join(";", jointNames);

            return names;
        }

        #region IDisposable
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Initialized = false;
                    // ожидаем завершение потока
                    exchangeDone.WaitOne(1500);
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~Costume() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
