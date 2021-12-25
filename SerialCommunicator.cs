using System;
using System.IO.Ports;
using System.Text;

namespace raspai_blazor
{
    public class SerialCommunicator
    {
        SerialPort serialPort = new SerialPort();
        
        public bool IsOpen => serialPort.IsOpen;

        public string selectComport { get; private set; } = "";
        
        public int selectBaudrate { get; private set; } = 0;

        public event EventHandler StateHasChanged;

        public event EventHandler<SerialCommunicatorDataReceivedEventArgs> DataReceived;

        public void Connect(string comport, int baudrate)
        {
            lock (this)
            {
                if (serialPort.IsOpen) return;
                
                selectComport = comport;
                selectBaudrate = baudrate;

                serialPort.PortName = selectComport;
                serialPort.BaudRate = selectBaudrate;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.WriteTimeout = 1000;
                serialPort.ReadTimeout = 1000;
                serialPort.Encoding = Encoding.UTF8;

                serialPort.Open();
                serialPort.DataReceived += OnReceived;

                StateHasChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Disconnect()
        {
            lock (this)
            {
                if(!serialPort.IsOpen) return;
                serialPort.Close();
                StateHasChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Send(string sendData)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(sendData);
            }
        }

        private void OnReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var recieveData = serialPort.ReadExisting();
            DataReceived?.Invoke(this, new SerialCommunicatorDataReceivedEventArgs(recieveData));
        }
    }
}
