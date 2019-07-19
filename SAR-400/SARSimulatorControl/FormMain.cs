using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Globalization;

namespace SARSimulatorControl
{
    public partial class FormMain : Form
    {
        private bool isConnected = false;
        private TcpClient tcpClient = null;
        private NetworkStream networkStream = null;
        private StreamReader streamReader = null;
        private bool isReset = false;

        public FormMain()
        {
            InitializeComponent();            
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            buttonConnection.Enabled = false;

            String IP = textBoxIP.Text;
            Int32 Port = 10099;
            Int32.TryParse(textBoxPort.Text, out Port);
            
            try
            {
                if (!isConnected)
                {
                    // CONNECT
                    tcpClient = new TcpClient();
                    tcpClient.Connect(IPAddress.Parse(IP), (short)Port);
                    tcpClient.Client.DontFragment = true;
                    tcpClient.NoDelay = true;
                    tcpClient.Client.NoDelay = true;
                    tcpClient.ReceiveTimeout = 5000;

                    if (tcpClient.Connected)
                    {
                        networkStream = tcpClient.GetStream();
                        streamReader = new StreamReader((Stream)networkStream);
                        isConnected = true;
                    }                        
                    else
                    {
                        isConnected = false;
                        if (networkStream != null)
                            networkStream.Dispose();
                        if (streamReader != null)
                            streamReader.Dispose();
                    }                        
                }
                else
                {
                    // DISCONNECT
                    // TODO:
                    // 1. Разобраться какой из методов нужно использовать для закрытия соединения
                    // 2. Разобраться почему рвется соединение
                    // 3. Разобраться как передавать время выполения программы

                    // tcpClient.GetStream().Close();
                    // tcpClient.Client.Disconnect(true);

                    tcpClient.Client.Disconnect(true);
                    tcpClient.Close();

                    if (networkStream != null)
                        networkStream.Dispose();
                    if (streamReader != null)
                        streamReader.Dispose();
                }

                if ((tcpClient.Client != null) && (tcpClient.Connected))
                    isConnected = true;
                else
                    isConnected = false;

            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
                isConnected = false;
            }

            buttonSend.Enabled = isConnected;

            buttonConnection.Enabled = true;
            buttonConnection.Focus();
            
            Cursor.Current = Cursors.Default;

            if (isConnected)
            {
                toolStripStatusLabelConnectionStatus.Text = "SAR-400 Connected";
                toolStripStatusLabelConnectionStatus.ForeColor = Color.Green;
                buttonConnection.Text = "DISCONNECT";                
            }
            else
            {
                toolStripStatusLabelConnectionStatus.Text = "SAR-400 Disconnected";
                toolStripStatusLabelConnectionStatus.ForeColor = Color.Red;
                buttonConnection.Text = "CONNECT";
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelConnectionStatus.Text = "SAR-400 Disconnected";
            toolStripStatusLabelConnectionStatus.ForeColor = Color.Red;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            buttonSend.Enabled = false;

            try
            {
                String command = String.Empty;

                if (checkBoxReset.Checked)
                    command = "ROBOT:MOTORS:R.ShoulderF;R.Elbow:POSSET:0;0";
                else
                    command = textBoxCommand.Text;

                Int32 timeInSeconds = 0;
                Int32.TryParse(textBoxTime.Text, out timeInSeconds);
                

                TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
                float seconds = (float)time.TotalSeconds;
                CultureInfo cultureInfo = new CultureInfo("en-US");
                command = String.Format("{0}:{1}", command, seconds.ToString(cultureInfo));

                byte[] bytes = Encoding.ASCII.GetBytes(command.Trim() + Environment.NewLine);
                networkStream.Write(bytes, 0, bytes.Length);
                RobotAnswer robotAnswer = (RobotAnswer)networkStream.ReadByte();
                labelRobotAnswer.Text = RobotAnswerToString(robotAnswer);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }            

            buttonSend.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private String RobotAnswerToString(RobotAnswer robotAnswer)
        {
            return robotAnswer.ToString();
        }
    }
}
